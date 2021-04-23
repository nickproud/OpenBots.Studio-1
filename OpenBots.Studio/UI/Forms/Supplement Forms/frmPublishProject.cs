using NuGet;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using OpenBots.Core.Enums;
using OpenBots.Core.IO;
using OpenBots.Core.Project;
using OpenBots.Core.Server.API_Methods;
using OpenBots.Core.Server.Models;
using OpenBots.Core.UI.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using OBNuget = OpenBots.Nuget;
using File = System.IO.File;
using OpenBots.Core.Script;
using System.Linq;

namespace OpenBots.UI.Supplement_Forms
{
    public partial class frmPublishProject : UIForm
    {
        public string NotificationMessage { get; set; }
        private string _projectPath;
        private string _projectName;
        private string _automationEngine;
        private List<ProjectArgument> _projectArguments;
        private Dictionary<string, string> _projectDependencies { get; set; }
        
        public frmPublishProject(string projectPath, Project project)
        {
            _projectPath = projectPath;
            _projectName = project.ProjectName;
            _projectDependencies = project.Dependencies;
            _automationEngine = project.ProjectType.ToString();
            _projectArguments = project.ProjectArguments;

            InitializeComponent();           
        }

        private void frmPublishProject_Load(object sender, EventArgs e)
        { 
            txtLocation.Text = Folders.GetFolder(FolderType.PublishedFolder);
            lblPublish.Text = $"publish {_projectName}";
            Text = $"publish {_projectName}";
            cbxLocation.SelectedIndex = 0;
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;         

            if (!PublishProject())
                return;

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private bool PublishProject()
        {
            try
            {
                string[] scriptFiles = Directory.GetFiles(_projectPath, "*.*", SearchOption.AllDirectories);
                List<ManifestContentFiles> manifestFiles = new List<ManifestContentFiles>();
                foreach (string file in scriptFiles)
                {
                    ManifestContentFiles manifestFile = new ManifestContentFiles
                    {
                        Include = file.Replace(_projectPath, "")
                    };
                    manifestFiles.Add(manifestFile);
                }

                ManifestMetadata metadata = new ManifestMetadata()
                {
                    Id = _projectName.Replace(" ", "_"),
                    Title = _projectName,
                    Authors = txtAuthorName.Text.Trim(),
                    Version = txtVersion.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    RequireLicenseAcceptance = false,
                    IconUrl = "https://openbots.ai/wp-content/uploads/2020/11/Studio-Icon-256px.png",
                    DependencySets = new List<ManifestDependencySet>()
                {
                    new ManifestDependencySet()
                    {
                        Dependencies = new List<ManifestDependency>()
                        {
                            
                            new ManifestDependency()
                            {
                                Id = "OpenBots.Studio",
                                Version = new Version(Application.ProductVersion).ToString()
                            },
                        }
                    }
                },
                    ContentFiles = manifestFiles,
                };

                foreach (var dependency in _projectDependencies)
                {
                    var dep = new ManifestDependency
                    {
                        Id = dependency.Key,
                        Version = dependency.Value
                    };
                    metadata.DependencySets[0].Dependencies.Add(dep);
                }

                PackageBuilder builder = new PackageBuilder();
                builder.PopulateFiles(_projectPath, new[] { new ManifestFile() { Source = "**" } });
                builder.Populate(metadata);

                if (!Directory.Exists(txtLocation.Text))
                    Directory.CreateDirectory(txtLocation.Text);

                string nugetFilePath = Path.Combine(txtLocation.Text.Trim(), $"{_projectName}_{txtVersion.Text.Trim()}.nupkg");
                using (FileStream stream = File.Open(nugetFilePath, FileMode.OpenOrCreate))
                    builder.Save(stream);

                NotificationMessage = $"'{_projectName}' published successfully";

                if (cbxLocation.Text == "Local Only")
                    return true;

                try {
                    lblError.Text = $"Publishing {_projectName} to the server...";

                    var client = AuthMethods.GetAuthToken();
                    var automation = AutomationMethods.UploadAutomation(client, _projectName, nugetFilePath, _automationEngine);

                    IEnumerable<AutomationParameter> automationParameters = _projectArguments.Select(arg => new AutomationParameter()
                    {
                        Name = arg.ArgumentName,
                        DataType = GetServerType(arg.ArgumentType),
                        Value = arg.ArgumentValue?.ToString(),
                        AutomationId = automation.Id
                    });

                    AutomationMethods.UpdateParameters(client, automation.Id, automationParameters);
                }
                catch (Exception)
                {
                    NotificationMessage = $"'{_projectName}' was published locally. To publish to an OpenBots Server please install and connect the OpenBots Agent.";
                }

                return true;             
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                return false;
            }           
        }

        private string GetServerType(Type studioType)
        {
            switch (studioType.ToString())
            {
                case "System.String":
                    return "Text";
                case "System.Int32":
                    return "Number";
                default:
                    return null;
            }
        }

        private void btnFolderManager_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtLocation.Text = fbd.SelectedPath;
                txtLocation.Focus();
            }
        }  
        
        private bool ValidateForm()
        {           
            if (string.IsNullOrEmpty(txtVersion.Text.Trim()))
            {
                lblError.Text = "Please provide a valid version";
                return false;
            }
            else if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            {
                lblError.Text = "Please provide a valid description";
                return false;
            }
            else if (string.IsNullOrEmpty(txtLocation.Text.Trim()))
            {
                lblError.Text = "Please provide a valid output path";
                return false;
            }
            else return true;
        }

        private async void txtLocation_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(txtLocation.Text))
                {
                    return;
                }
                else
                {
                    lblError.Text = "";
                }
                List<IPackageSearchMetadata> packageList = await OBNuget.NugetPackageManager.SearchPackages(_projectName, txtLocation.Text, false);
                if (packageList.Count > 0)
                {
                    NuGetVersion newestVersion = packageList[0].Identity.Version;
                    for (int i = 1; i < packageList.Count; i++)
                    {
                        if (packageList[i].Identity.Version.Major > newestVersion.Major)
                        {
                            newestVersion = packageList[i].Identity.Version;
                        }
                        else if (packageList[i].Identity.Version.Major == newestVersion.Major && packageList[i].Identity.Version.Minor > newestVersion.Minor)
                        {
                            newestVersion = packageList[i].Identity.Version;
                        }
                        else if (packageList[i].Identity.Version.Major == newestVersion.Major && packageList[i].Identity.Version.Minor == newestVersion.Minor && packageList[i].Identity.Version.Revision > newestVersion.Revision)
                        {
                            newestVersion = packageList[i].Identity.Version;
                        }
                    }
                    NuGetVersion currentVersion = newestVersion;
                    string newVersion = currentVersion.Major + "." + (currentVersion.Minor + 1) + "." + currentVersion.Revision;
                    txtVersion.Text = newVersion;
                }
                else
                {
                    txtVersion.Text = "1.0.0";
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}