using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace OpenBots.Core.Settings
{
    /// <summary>
    /// Defines application/client-level settings which can be managed by the user
    /// </summary>
    [Serializable]
    public class ClientSettings
    {
        public bool AntiIdleWhileOpen { get; set; }
        public string RootFolder2 { get; set; }
        public string ScriptsFolder { get; set; }
        public bool InsertCommandsInline { get; set; }
        public bool EnableSequenceDragDrop { get; set; }
        public bool MinimizeToTray { get; set; }
        public bool CloseToTray { get; set; }
        public string StartupMode { get; set; }
        public bool UseSlimActionBar { get; set; }
        public DataTable PackageSourceDT { get; set; }
        public bool IsRestarting { get; set; }
        public bool IsInstallingPackages { get; set; }
        public List<string> RecentProjects { get; set; }
        public List<string> DefaultPackages { get; set; }

        public ClientSettings()
        {
            MinimizeToTray = false;
            CloseToTray = false;
            AntiIdleWhileOpen = false;
            InsertCommandsInline = true;
            RootFolder2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "OpenBots Studio");
            ScriptsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "OpenBots Studio", "OB Scripts");
            StartupMode = "Builder Mode";
            EnableSequenceDragDrop = true;
            UseSlimActionBar = true;
            IsRestarting = false;
            IsInstallingPackages = false;
        }
    }
}
