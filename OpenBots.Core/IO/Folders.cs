using OpenBots.Core.Enums;
using OpenBots.Core.Settings;
using System;
using System.IO;
using System.Windows.Forms;

namespace OpenBots.Core.IO
{
    public static class Folders
    {
        public static string GetFolder(FolderType folderType, bool createFolder = true)
        {
            string folderPath;
            switch (folderType)
            {
                case FolderType.RootFolder:
                    //return root folder from settings
                    var rootSettings = new ApplicationSettings().GetOrCreateApplicationSettings();
                    folderPath = rootSettings.ClientSettings.RootFolder;
                    break;
                case FolderType.AttendedTasksFolder:
                    //return attended tasks folder from settings
                    var attendedSettings = new ApplicationSettings().GetOrCreateApplicationSettings();
                    folderPath = attendedSettings.ClientSettings.AttendedTasksFolder;
                    break;
                case FolderType.SettingsFolder:
                    //return app data OpenBots folder
                    folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OpenBots Inc");
                    break;
                case FolderType.StudioFolder:
                    //return app data OpenBots.Studio folder
                    folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OpenBots Inc", "OpenBots.Studio");
                    break;
                case FolderType.ScriptsFolder:
                    //return scripts folder
                    folderPath = Path.Combine(GetFolder(FolderType.RootFolder), "My Scripts");
                    break;
                case FolderType.PublishedFolder:
                    //return scripts folder
                    folderPath = Path.Combine(GetFolder(FolderType.RootFolder), "Published");
                    break;
                case FolderType.LogFolder:
                    //return logs folder
                    folderPath = Path.Combine(GetFolder(FolderType.RootFolder), "Logs");
                    break;
                case FolderType.TempFolder:
                    //return temp folder
                    folderPath = Path.Combine(Path.GetTempPath(), "OpenBotsStudio");
                    break;
                case FolderType.LocalAppDataPackagesFolder:
                    //return local app data packages folder
                    folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OpenBots Inc", "packages");
                    break;
                case FolderType.ProgramFilesPackagesFolder:
                    //return program files packages folder
                    folderPath = Path.Combine(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName, "packages", Application.ProductVersion);
                    break;
                default:
                    //enum is not implemented
                    throw new NotImplementedException("FolderType " + folderType.ToString() + " Not Supported");
            }

            if (!Directory.Exists(folderPath) && createFolder)
                Directory.CreateDirectory(folderPath);

            return folderPath;
        }
    }
}
