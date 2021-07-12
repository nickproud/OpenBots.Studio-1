using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using OpenBots.Core.Enums;
using OpenBots.Core.Script;
using OpenBots.Core.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ZipFile = ICSharpCode.SharpZipLib.Zip.ZipFile;

namespace OpenBots.Core.Project
{
    public class Project
    {
        public Guid ProjectID { get; set; }
        public string ProjectName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ProjectType ProjectType { get; set; }
        public string Main { get; set; }
        public string Version { get; set; }
        public List<ProjectArgument> ProjectArguments { get; set; }
        public Dictionary<string, string> Dependencies { get; set; }

        public Project(string projectName, ProjectType projectType)
        {      
            ProjectID = Guid.NewGuid();
            ProjectName = projectName;
            ProjectType = projectType;
            Version = Application.ProductVersion;
            ProjectArguments = new List<ProjectArgument>();

            var commandVersion = Regex.Matches(Application.ProductVersion, @"\d+\.\d+\.\d+")[0].ToString();
            
            switch (ProjectType)
            {
                case ProjectType.OpenBots:
                    Main = "Main.obscript";
                    var appSettings = new ApplicationSettings().GetOrCreateApplicationSettings();
                    Dependencies = appSettings.ClientSettings.DefaultPackages.ToDictionary(x => x, x => commandVersion);
                    break;
                case ProjectType.Python:
                    Main = "Main.py";
                    Dependencies = new Dictionary<string, string>();
                    break;
                case ProjectType.TagUI:
                    Main = "Main.tag";
                    Dependencies = new Dictionary<string, string>();
                    break;
                case ProjectType.CSScript:
                    Main = "Main.cs";
                    Dependencies = new Dictionary<string, string>();
                    break;
                case ProjectType.PowerShell:
                    Main = "Main.ps1";
                    Dependencies = new Dictionary<string, string>();
                    break;
            }
        }

        public static void SerializeProjectConfig(Project project, string configPath)
        {
            JsonSerializer serializer = JsonSerializer.Create();
            using (StreamWriter sw = new StreamWriter(configPath))
            using (JsonWriter writer = new JsonTextWriter(sw) { Formatting = Formatting.Indented })
                serializer.Serialize(writer, project, typeof(Project));
        }

        public void SaveProject(string scriptPath)
        {
            //Looks through sequential parent directories to find one that matches the script's ProjectName and contains a Main
            string projectPath;
            string dirName;
            string configPath;

            try
            {
                do
                {
                    projectPath = Path.GetDirectoryName(scriptPath);
                    DirectoryInfo dirInfo = new DirectoryInfo(projectPath);
                    dirName = dirInfo.Name;
                    configPath = Path.Combine(projectPath, "project.obconfig");
                    scriptPath = projectPath;
                } while (dirName != ProjectName || !File.Exists(configPath));

                //If requirements are met, a project.obconfig is created/updated
                if (dirName == ProjectName && File.Exists(configPath))
                {
                    Version = Application.ProductVersion;
                    SerializeProjectConfig(this, configPath);
                }
            }
            catch (Exception)
            {
                throw new Exception("Project Directory Not Found. File Saved Externally");
            }
        }

        public static void RenameProject(Project newProject, string newProjectPath)
        {
            string configPath = Path.Combine(newProjectPath, "project.obconfig");

            if (File.Exists(configPath))
                SerializeProjectConfig(newProject, configPath);
            else
                throw new FileNotFoundException("project.obconfig not found. Unable to save project.");
        }

        public static Project OpenProject(string configFilePath)
        {
            //Loads project from project.obconfig
            if (File.Exists(configFilePath))
            {
                string projectJSONString = File.ReadAllText(configFilePath);
                var project = JsonConvert.DeserializeObject<Project>(projectJSONString);

                if (project.ProjectType == ProjectType.OpenBots)
                {
                    string dialogMessageFirstLine = "";

                    if (!projectJSONString.Contains("Version"))
                    {
                        dialogMessageFirstLine = $"Attempting to open a 'project.obconfig' from a version of OpenBots Studio older than 1.2.0.0.";
                    }
                    //if project version is lower than the Studio version
                    else if (new Version(project.Version).CompareTo(new Version(Application.ProductVersion)) < 0)
                    {                      
                        dialogMessageFirstLine = $"Attempting to open a 'project.obconfig' from OpenBots Studio version {project.Version}.";

                        var dialogResult = MessageBox.Show($"{dialogMessageFirstLine} " +
                                                       $"Would you like to attempt to convert this config file to {Application.ProductVersion}? " +
                                                       "\n\nWarning: Once a 'project.obconfig' has been converted, it cannot be undone.",
                                                       "Convert 'project.obconfig'", MessageBoxButtons.YesNo);

                        if (dialogResult == DialogResult.Yes)
                        {
                            if (new Version(project.Version).CompareTo(new Version("1.4.0.0")) < 0)
                                configFilePath = ConvertProjectTo1400(configFilePath, project);

                            project.Version = Application.ProductVersion;
                            var commandVersion = Regex.Matches(Application.ProductVersion, @"\d+\.\d+\.\d+")[0].ToString();

                            var appSettings = new ApplicationSettings().GetOrCreateApplicationSettings();
                            project.Dependencies = appSettings.ClientSettings.DefaultPackages.ToDictionary(x => x, x => commandVersion);
                            SerializeProjectConfig(project, configFilePath);
                        }
                    }
                }
                                  
                return project;
            }
            else
            {
                throw new Exception("project.obconfig Not Found");
            }
        }

        private static string ConvertProjectTo1400(string configFilePath, Project project)
        {
            //convert project to 1.4.0.0
            string configExtension = Path.GetExtension(configFilePath);
            if (configExtension == ".config")
            {
                FileInfo oldConfig = new FileInfo(configFilePath);
                File.Move(configFilePath, configFilePath.Replace(".config", ".obconfig"));
                configFilePath = Path.Combine(oldConfig.DirectoryName, "project.obconfig");

                string projectJSONString = File.ReadAllText(configFilePath);
                project.Main = project.Main.Replace(".json", ".obscript");

                var projectScriptFiles = Directory.GetFiles(oldConfig.DirectoryName, "*.json", SearchOption.AllDirectories).ToList();
                projectScriptFiles.ForEach(x => File.Move(x, x.Replace(".json", ".obscript")));
            }

            return configFilePath;
        }

        public static string ExtractGalleryProject(string projectDirectory)
        {
            if (!Directory.Exists(projectDirectory))
                Directory.CreateDirectory(projectDirectory);

            var processNugetFilePath = projectDirectory + ".nupkg";
            var processZipFilePath = projectDirectory + ".zip";

            // Create .zip file
            File.Copy(processNugetFilePath, processZipFilePath, true);

            // Extract Files/Folders from (.zip) file
            DecompressFile(processZipFilePath, projectDirectory);

            // Delete .zip File
            File.Delete(processZipFilePath);
            File.Delete(processNugetFilePath);

            //get config file and rename project
            string configFilePath = Directory.GetFiles(projectDirectory, "project.obconfig", SearchOption.AllDirectories).First();
            var config = JObject.Parse(File.ReadAllText(configFilePath));
            config["ProjectName"] = new DirectoryInfo(projectDirectory).Name;

            JsonSerializer serializer = JsonSerializer.Create();
            using (StreamWriter sw = new StreamWriter(configFilePath))
            using (JsonWriter writer = new JsonTextWriter(sw) { Formatting = Formatting.Indented })
                serializer.Serialize(writer, config);

            // Return "Main" Script File Path of the Process
            return configFilePath;
        }

        public static void DecompressFile(string projectFilePath, string targetDirectory)
        {
            FileStream fs = File.OpenRead(projectFilePath);
            ZipFile file = new ZipFile(fs);

            foreach (ZipEntry zipEntry in file)
            {
                if (!zipEntry.IsFile)
                {
                    // Ignore directories but create them in case they're empty
                    Directory.CreateDirectory(Path.Combine(targetDirectory, zipEntry.Name));
                    continue;
                }

                //exclude nuget metadata files
                string[] excludedFiles = { ".nuspec", ".xml", ".rels", ".psmdcp" };
                if (excludedFiles.Any(e => Path.GetExtension(zipEntry.Name) == e))
                    continue;

                string entryFileName = Uri.UnescapeDataString(zipEntry.Name);

                // 4K is optimum
                byte[] buffer = new byte[4096];
                Stream zipStream = file.GetInputStream(zipEntry);

                // Manipulate the output filename here as desired.
                string fullZipToPath = Path.Combine(targetDirectory, entryFileName);
                string directoryName = Path.GetDirectoryName(fullZipToPath);

                if (directoryName.Length > 0)
                    Directory.CreateDirectory(directoryName);

                // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                // of the file, but does not waste memory.
                // The "using" will close the stream even if an exception occurs.
                using (FileStream streamWriter = File.Create(fullZipToPath))
                    StreamUtils.Copy(zipStream, streamWriter, buffer);
            }

            if (file != null)
            {
                file.IsStreamOwner = true;
                file.Close();
            }
        }       
    }
}
