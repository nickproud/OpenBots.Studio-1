using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using CSScriptLibrary;
using System.IO;
using System.Runtime.InteropServices;
using OpenBots.Core.Server.User;

namespace OpenBots.Utilities
{
    public class ExecutionManager
    {
        private const int MAX_PATH = 260;

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, SetLastError = false)]
        public static extern bool PathFindOnPath([In, Out] StringBuilder pszFile, [In] string[] ppszOtherDirs);

        public static void RunPythonAutomation(string scriptPath, object[] scriptArgs)
        {
            string pythonExecutable = GetPythonPath(Environment.UserName, "");

            if (!string.IsNullOrEmpty(pythonExecutable))
            {
                Process scriptProc = new Process();

                scriptProc.StartInfo = new ProcessStartInfo()
                {
                    FileName = pythonExecutable,
                    Arguments = $"\"{scriptPath}\" ", //+ scriptArgs,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                scriptProc.Start();
                scriptProc.WaitForExit();

                scriptProc.StandardOutput.ReadToEnd();
                scriptProc.StandardError.ReadToEnd();
                scriptProc.Close();
            }
        }

        private static string GetPythonPath(string username, string requiredVersion = "")
        {

            var possiblePythonLocations = new List<string>()
            {
                @"HKLM\SOFTWARE\Python\PythonCore\",
                @"HKLM\SOFTWARE\Wow6432Node\Python\PythonCore\"
            };

            try
            {
                NTAccount f = new NTAccount(username);
                SecurityIdentifier s = (SecurityIdentifier)f.Translate(typeof(SecurityIdentifier));
                string sidString = s.ToString();
                possiblePythonLocations.Add($@"HKU\{sidString}\SOFTWARE\Python\PythonCore\");
            }
            catch
            {
                throw new Exception("Unabled to retrieve SID for provided user credentials.");
            }

            Version requestedVersion = new Version(requiredVersion == "" ? "0.0.1" : requiredVersion);

            //Version number, install path
            Dictionary<Version, string> pythonLocations = new Dictionary<Version, string>();

            foreach (string possibleLocation in possiblePythonLocations)
            {
                var regVals = possibleLocation.Split(new[] { '\\' }, 2);
                var rootKey = (regVals[0] == "HKLM" ? RegistryHive.LocalMachine : RegistryHive.Users);
                var regView = (Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);
                var hklm = RegistryKey.OpenBaseKey(rootKey, regView);
                RegistryKey theValue = hklm.OpenSubKey(regVals[1]);

                if (theValue == null)
                    continue;

                foreach (var version in theValue.GetSubKeyNames())
                {
                    RegistryKey productKey = theValue.OpenSubKey(version);
                    if (productKey != null)
                    {
                        try
                        {
                            string pythonExePath = productKey.OpenSubKey("InstallPath").GetValue("ExecutablePath").ToString();
                            if (pythonExePath != null && pythonExePath != "")
                            {
                                pythonLocations.Add(Version.Parse(version), pythonExePath);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }

            if (pythonLocations.Count == 0)
                throw new Exception("No installed Python versions found.");

            int max = pythonLocations.Max(x => x.Key.CompareTo(requestedVersion));
            requestedVersion = pythonLocations.First(x => x.Key.CompareTo(requestedVersion) == max).Key;

            if (pythonLocations.ContainsKey(requestedVersion))
            {
                return pythonLocations[requestedVersion];
            }
            else
            {
                throw new Exception($"Required Python version [{requiredVersion}] or higher was not found on the machine.");
            }
        }

        public static void RunCSharpAutomation(string scriptPath, object[] scriptArgs)
        {
            string code = File.ReadAllText(scriptPath);
            dynamic script = CSScript.LoadCode(code).CreateObject("*");
            script.Main(scriptArgs);

            //string exePath = GetFullPathFromWindows("cscs.exe");
            //if (exePath == null)
            //    throw new Exception("CS-Script installation was not detected on the machine. Please perform the installation as outlined in the official documentation.");

        }

        private static string GetFullPathFromWindows(string exeName)
        {
            if (exeName.Length >= MAX_PATH)
                throw new ArgumentException($"The executable name '{exeName}' must have less than {MAX_PATH} characters.",
                    nameof(exeName));

            StringBuilder sb = new StringBuilder(exeName, MAX_PATH);
            var exePath = PathFindOnPath(sb, null) ? sb.ToString() : null;

            if (exePath != null)
                return exePath;

            // Get User Environment Variable "Path"
            var envPathValue = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User);

            exePath = FindAppPath(envPathValue, exeName);
            if (!string.IsNullOrEmpty(exePath))
                return exePath;

            // Get System Environment Variable "Path"
            envPathValue = Environment.GetEnvironmentVariable("Path");
            exePath = FindAppPath(envPathValue, exeName);
            if (!string.IsNullOrEmpty(exePath))
                return exePath;
            else
                return null;

        }

        private static string FindAppPath(string envPathValue, string exeName)
        {
            string appFullPath = string.Empty;
            if (envPathValue != null)
            {
                var pathValues = envPathValue.ToString().Split(Path.PathSeparator);

                foreach (var path in pathValues)
                {
                    if (File.Exists(Path.Combine(path, exeName)))
                    {
                        appFullPath = Path.Combine(path, exeName);
                        break;
                    }
                }
            }

            return appFullPath;
        }
    }
}
