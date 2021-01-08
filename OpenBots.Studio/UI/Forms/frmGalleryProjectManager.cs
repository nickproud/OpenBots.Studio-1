using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using OpenBots.Nuget;
using OpenBots.Properties;
using OpenBots.Core.UI.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace OpenBots.UI.Forms
{
    public partial class frmGalleryProjectManager : UIForm
    {
        private string _projectLocation;
        private string _projectName;

        private List<IPackageSearchMetadata> _searchresults;
        private IPackageSearchMetadata _catalog;
        private List<NuGetVersion> _projectVersions;
        private List<IPackageSearchMetadata> _selectedPackageMetaData;
        private string _galleryAutomationsSourceUrl = "https://gallery.openbots.io/v3/automation.json";
        private string _gallerySamplesSourceUrl = "https://gallery.openbots.io/v3/sample.json";
        private string _galleryTemplatesSourceUrl = "https://gallery.openbots.io/v3/template.json";

        public frmGalleryProjectManager(string projectLocation, string projectName)
        {
            InitializeComponent();
            _projectLocation = projectLocation;
            _projectName = projectName;
            _searchresults = new List<IPackageSearchMetadata>();
            _projectVersions = new List<NuGetVersion>();
        }

        private async void frmGalleryProject_LoadAsync(object sender, EventArgs e)
        {
            pnlProjectVersion.Hide();
            pnlProjectDetails.Hide();
            uiBtnOpen.Enabled = false;
            try
            {           
                _searchresults = await NugetPackageManager.SearchPackages("", _galleryAutomationsSourceUrl, true);
                PopulateListBox(_searchresults);
            }
            catch (Exception ex)
            {
                //not connected to internet
                lblError.Text = "Error: " + ex.Message;
            }
        }
       
        private void PopulateListBox(List<IPackageSearchMetadata> searchresults)
        {
            lbxGalleryProjects.Visible = false;
            tpbLoadingSpinner.Visible = true;

            lbxGalleryProjects.Clear();
            foreach (var result in searchresults)
            {
                Image img;

                try
                {
                    WebClient wc = new WebClient();
                    byte[] bytes = wc.DownloadData(result.IconUrl);
                    MemoryStream ms = new MemoryStream(bytes);
                    img = Image.FromStream(ms);
                }
                catch (Exception)
                {
                    img = Resources.nuget_icon;
                }

                lbxGalleryProjects.Add(result.Identity.Id, result.Identity.Id, result.Description, img, result.Identity.Version.ToString());
            }

            tpbLoadingSpinner.Visible = false;
            lbxGalleryProjects.Visible = true;
        }

        private async void lbxGalleryProjects_ItemClick(object sender, int index)
        {
            try
            {
                string projectId = lbxGalleryProjects.ClickedItem.Id;
                List<IPackageSearchMetadata> metadata = new List<IPackageSearchMetadata>();

                if (lblGalleryProjects.Text == "Gallery Automations")
                    metadata.AddRange(await NugetPackageManager.GetPackageMetadata(projectId, _galleryAutomationsSourceUrl, true));
                else if (lblGalleryProjects.Text == "Gallery Samples")
                    metadata.AddRange(await NugetPackageManager.GetPackageMetadata(projectId, _gallerySamplesSourceUrl, true));
                else if (lblGalleryProjects.Text == "Gallery Templates")
                    metadata.AddRange(await NugetPackageManager.GetPackageMetadata(projectId, _galleryTemplatesSourceUrl, true));

                string latestVersion = metadata.LastOrDefault().Identity.Version.ToString();

                _projectVersions.Clear();
                if (lblGalleryProjects.Text == "Gallery Automations")
                    _projectVersions.AddRange(await NugetPackageManager.GetPackageVersions(projectId, _galleryAutomationsSourceUrl, true));
                else if (lblGalleryProjects.Text == "Gallery Samples")
                    _projectVersions.AddRange(await NugetPackageManager.GetPackageVersions(projectId, _gallerySamplesSourceUrl, true));
                else if (lblGalleryProjects.Text == "Gallery Templates")
                    _projectVersions.AddRange(await NugetPackageManager.GetPackageVersions(projectId, _galleryTemplatesSourceUrl, true));

                List<string> versionList = _projectVersions.Select(x => x.ToString()).ToList();
                versionList.Reverse();

                cbxVersion.Items.Clear();
                foreach (var version in versionList)
                    cbxVersion.Items.Add(version);
              
                _selectedPackageMetaData = metadata;

                pnlProjectVersion.Show();
                pnlProjectDetails.Show();
                uiBtnOpen.Enabled = true;
                cbxVersion.SelectedItem = latestVersion;
            }
            catch (Exception)
            {
                pnlProjectVersion.Hide();
                pnlProjectDetails.Hide();
                uiBtnOpen.Enabled = false;
            }
        }

        private void PopulateProjectDetails(string version)
        {
            _catalog = _selectedPackageMetaData.Where(x => x.Identity.Version.ToString() == version).SingleOrDefault();

            if (_catalog != null)
            {
                try
                {
                    WebClient wc = new WebClient();
                    byte[] bytes = wc.DownloadData(_catalog.IconUrl);
                    MemoryStream ms = new MemoryStream(bytes);
                    pbxOBStudio.Image = Image.FromStream(ms);
                }
                catch (Exception)
                {
                    pbxOBStudio.Image = Resources.OpenBots_icon;
                }

                lblTitle.Text = _catalog.Title;
                lblAuthors.Text = _catalog.Authors;
                lblDescription.Text = _catalog.Description;
                lblDownloads.Text = _catalog.DownloadCount.ToString();
                lblVersion.Text = _catalog.Identity.Version.ToString();
                lblPublishDate.Text = DateTime.Parse(_catalog.Published.ToString()).ToString("g");
                llblProjectURL.LinkVisited = false;
                llblLicenseURL.LinkVisited = false;

                lvDependencies.Items.Clear();
                if (_catalog.DependencySets.ToList().Count > 0)
                {
                    foreach (var dependency in _catalog.DependencySets.FirstOrDefault().Packages)
                        lvDependencies.Items.Add(new ListViewItem(new string[] { dependency.Id, dependency.VersionRange.ToString() }));
                }
            }
            else
            {
                _catalog = _selectedPackageMetaData.Last();
                try
                {
                    WebClient wc = new WebClient();
                    byte[] bytes = wc.DownloadData(_catalog.IconUrl);
                    MemoryStream ms = new MemoryStream(bytes);
                    pbxOBStudio.Image = Image.FromStream(ms);
                }
                catch (Exception)
                {
                    pbxOBStudio.Image = Resources.nuget_icon;
                }

                lblTitle.Text = _catalog.Title;
                lblAuthors.Text = "Unknown";
                lblDescription.Text = "Unknown";
                lblDownloads.Text = "Unknown";
                lblVersion.Text = version;
                lblPublishDate.Text = "Unknown";
                llblProjectURL.LinkVisited = false;
                llblLicenseURL.LinkVisited = false;

                lvDependencies.Items.Clear();
            }
        }

        private void llblLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_catalog.LicenseUrl != null)
            {
                llblLicenseURL.LinkVisited = true;
                Process.Start(_catalog.LicenseUrl.ToString());
            }
        }

        private void llblProjectURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_catalog.ProjectUrl != null)
            {
                llblProjectURL.LinkVisited = true;
                Process.Start(_catalog.ProjectUrl.ToString());
            }
        }

        private void cbxVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateProjectDetails(cbxVersion.SelectedItem.ToString());
        }

        private async void DownloadAndOpenProject(string projectId, string version)
        {
            try
            {
                string packageName = $"{projectId}.{version}";
                Cursor.Current = Cursors.WaitCursor;
                lblError.Text = $"Downloading {packageName}";

                if (lblGalleryProjects.Text == "Gallery Automations")
                    await NugetPackageManager.DownloadPackage(projectId, version, _projectLocation, _projectName, _galleryAutomationsSourceUrl);
                else if (lblGalleryProjects.Text == "Gallery Samples")
                    await NugetPackageManager.DownloadPackage(projectId, version, _projectLocation, _projectName, _gallerySamplesSourceUrl);
                else if (lblGalleryProjects.Text == "Gallery Templates")
                    await NugetPackageManager.DownloadPackage(projectId, version, _projectLocation, _projectName, _galleryTemplatesSourceUrl);

                lblError.Text = string.Empty;
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                lblError.Text = "Error: " + ex.Message;
            }
        }

        private void uiBtnOpen_Click(object sender, EventArgs e)
        {
            DownloadAndOpenProject(_catalog.Identity.Id, cbxVersion.SelectedItem.ToString());
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private async void tvProjectFeeds_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            lblError.Text = "";
            try
            {
                if (tvProjectFeeds.SelectedNode != null)
                {
                    lbxGalleryProjects.Clear();
                    lbxGalleryProjects.Visible = false;
                    tpbLoadingSpinner.Visible = true;
                   
                    if (tvProjectFeeds.SelectedNode.Name == "Automations")
                    {
                        lblGalleryProjects.Text = "Gallery Automations";
                        var sourceResults = await NugetPackageManager.SearchPackages("", _galleryAutomationsSourceUrl, false);
                        PopulateListBox(sourceResults);
                    }
                    else if (tvProjectFeeds.SelectedNode.Name == "Samples")
                    {
                        lblGalleryProjects.Text = "Gallery Samples";
                        var sourceResults = await NugetPackageManager.SearchPackages("", _gallerySamplesSourceUrl, false);
                        PopulateListBox(sourceResults);
                    }
                    else if (tvProjectFeeds.SelectedNode.Name == "Templates")
                    {
                        lblGalleryProjects.Text = "Gallery Templates";
                        var sourceResults = await NugetPackageManager.SearchPackages("", _galleryTemplatesSourceUrl, false);
                        PopulateListBox(sourceResults);
                    }
                }
            }
            catch (Exception)
            {
                tpbLoadingSpinner.Visible = false;
            }
        }      

        private async void txtSampleSearch_KeyDown(object sender, KeyEventArgs e)
        {
            lblError.Text = "";
            var searchResults = new List<IPackageSearchMetadata>();

            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (lblGalleryProjects.Text == "Gallery Automations")
                        searchResults.AddRange(await NugetPackageManager.SearchPackages(txtSampleSearch.Text, _galleryAutomationsSourceUrl, false));
                    else if (lblGalleryProjects.Text == "Gallery Samples")
                        searchResults.AddRange(await NugetPackageManager.SearchPackages(txtSampleSearch.Text, _gallerySamplesSourceUrl, false));
                    else if (lblGalleryProjects.Text == "Gallery Templates")
                        searchResults.AddRange(await NugetPackageManager.SearchPackages(txtSampleSearch.Text, _galleryTemplatesSourceUrl, false));
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error: " + ex.Message;
                }

                PopulateListBox(searchResults);
                e.Handled = true;
            }
        }

        private void txtSampleSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                e.Handled = true;
        }

        private void tvProjectFeeds_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
