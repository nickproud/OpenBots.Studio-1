//Copyright (c) 2019 Jason Bayldon
//Modifications - Copyright (c) 2020 OpenBots Inc.
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
using OpenBots.Core.Enums;
using OpenBots.Core.IO;
using OpenBots.Core.Project;
using OpenBots.Core.Settings;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.UI.Forms;
using OpenBots.UI.Forms.ScriptBuilder_Forms;
using OpenBots.UI.Forms.Supplement_Forms;
using OpenBots.Utilities;
using Serilog.Core;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OpenBots
{
    static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
        public static frmSplash SplashForm { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //exception handler
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            //get app settings
            var appSettings = new ApplicationSettings();
            appSettings = appSettings.GetOrCreateApplicationSettings();

            //if the exe was passed a filename argument then run the script
            if (args.Length > 0)
            {
                string configPath = args[0];

                if (!File.Exists(configPath))
                {
                    using (EventLog eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "Application";
                        eventLog.WriteEntry($"An attempt was made to run an OpenBots project from '{new DirectoryInfo(configPath).Parent}'" +
                            "but the 'project.obconfig' file was not found. Please verify that the file exists at the path indicated.",
                            EventLogEntryType.Error, 101, 1);
                    }

                    Application.Exit();
                    return;
                }

                //initialize Logger
                string engineLoggerFilePath = Path.Combine(Folders.GetFolder(FolderType.LogFolder), "OpenBots Engine Logs.txt");
                Logger engineLogger = new Logging().CreateFileLogger(engineLoggerFilePath, Serilog.RollingInterval.Day);

                ProjectType projectType = Project.OpenProject(configPath).ProjectType;
                switch (projectType)
                {
                    case ProjectType.OpenBots:
                        Application.Run(new frmScriptEngine(configPath, engineLogger));
                        break;
                    case ProjectType.Python:
                    case ProjectType.TagUI:
                    case ProjectType.CSScript:
                        ExecutionManager.RunTextEditorProject(configPath, Project.OpenProject(configPath).ProjectArguments);
                        break;
                }                
            }
            else if (appSettings.ClientSettings.StartupMode == "Builder Mode" && appSettings.ClientSettings.IsRestarting)
            {
                Application.DoEvents();
                Application.Run(new frmScriptBuilder(appSettings.ClientSettings.RecentProjects[0]));
            }
            else if (appSettings.ClientSettings.StartupMode == "Builder Mode")
            {
                SplashForm = new frmSplash();
                SplashForm.Show();

                Application.DoEvents();
                Application.Run(new frmScriptBuilder(null));
            }           
            else
            {
                SplashForm = new frmSplash();
                SplashForm.Show();

                Application.DoEvents();
                Application.Run(new frmAttendedMode());
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception occured: " + (e.ExceptionObject as Exception).ToString(), "Oops");
        }
    }
}