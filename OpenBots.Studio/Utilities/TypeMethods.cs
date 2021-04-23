using Autofac;
using NuGet;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Script;
using OpenBots.Core.Settings;
using OpenBots.Core.UI.Controls;
using OpenBots.Core.Utilities.CommandUtilities;
using OpenBots.UI.Forms.Supplement_Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AContainer = Autofac.IContainer;

namespace OpenBots.Studio.Utilities
{
    public static class TypeMethods
    {       
        public static List<AutomationCommand> GenerateAutomationCommands(AContainer container)
        {          
            var commandList = new List<AutomationCommand>();
            var commandClasses = new List<Type>();
                               
            using (var scope = container.BeginLifetimeScope())
            {
                    var types = scope.ComponentRegistry.Registrations
                                .Where(r => typeof(ScriptCommand).IsAssignableFrom(r.Activator.LimitType))
                                .Select(r => r.Activator.LimitType).ToList();

                    commandClasses.AddRange(types);
            }

            var userPrefs = new ApplicationSettings().GetOrCreateApplicationSettings();

            //Loop through each class
            foreach (var commandClass in commandClasses)
            {
                var newAutomationCommand = CommandsHelper.ConvertToAutomationCommand(commandClass);

                //If command is enabled, pull for display and configuration
                if (newAutomationCommand != null)
                    commandList.Add(newAutomationCommand);
            }

            return commandList.Distinct().ToList();
        }

        public static List<Type> GenerateCommandTypes(AContainer container)
        {
            var commandList = new List<AutomationCommand>();
            var commandClasses = new List<Type>();

            using (var scope = container.BeginLifetimeScope())
            {
                var types = scope.ComponentRegistry.Registrations
                            .Where(r => typeof(ScriptCommand).IsAssignableFrom(r.Activator.LimitType))
                            .Select(r => r.Activator.LimitType).ToList();

                commandClasses.AddRange(types);
            }
            
            return commandClasses;
        }

        public static void GenerateAllVariableTypes(List<Assembly> assemblyList, Dictionary<string, List<Type>> groupedTypes)
        {
            groupedTypes.Clear();

            foreach (var assem in assemblyList)
            {
                try
                {
                    var newTypes = assem.GetTypes()?
                                        .Where(x => x.IsVisible && x.IsPublic && !x.IsInterface && !x.IsAbstract &&
                                                    x.Namespace != null && !x.Namespace.StartsWith("OpenBots"))
                                        .ToList();

                    //special inclusion for MailItem and IWebElement interface types
                    newTypes.AddRange(assem.GetTypes()?
                                           .Where(x => x.Name == "MailItem" || x.Name == "IWebElement")
                                           .ToList());

                    if (newTypes.Count > 0 && !groupedTypes.ContainsKey($"{assem.GetName().Name} [{assem.GetName().Version}]"))
                        groupedTypes.Add($"{assem.GetName().Name} [{assem.GetName().Version}]", newTypes);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public static void GenerateAllNamespaces(List<Assembly> assemblyList, Dictionary<string, AssemblyReference> allNamespaces)
        {
            allNamespaces.Clear();

            try
            {
                allNamespaces.AddRange(assemblyList
                             .SelectMany(a => 
                             {
                                 try 
                                 {
                                     return a.GetTypes();
                                 } 
                                 catch (Exception) 
                                 {
                                     return new Type[] { };
                                 } 
                             })?
                             .Select(t => new 
                             { 
                                 Key = t?.Namespace, 
                                 Value = new AssemblyReference(t.Assembly.GetName().Name, t.Assembly.GetName().Version.ToString()) 
                             })
                             .Where(n => !string.IsNullOrEmpty(n.Key) && !n.Key.StartsWith("<"))
                             .GroupBy(x => x.Key)
                             .OrderBy(n => n.Key)
                             .ToDictionary(x => x.Key, x => x.First().Value));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }           
        }

        public static Type GetTypeByName(AContainer container, string typeName)
        {
            using (var scope = container.BeginLifetimeScope())
            {
                var types = scope.ComponentRegistry.Registrations
                            .Where(r => typeof(ScriptCommand).IsAssignableFrom(r.Activator.LimitType))
                            .Select(r => r.Activator.LimitType).ToList();

                var commandType = types.Where(x => x.Name == typeName).FirstOrDefault();

                if (commandType == null)
                {
                    var packageName = GetPackageName(typeName);
                    ShowErrorDialog($"Missing {packageName}, please download the package from Package Manager and retry.");
                }

                return commandType;
            }
        }

        public static object CreateTypeInstance(AContainer container, string typeName)
        {
            var commandType = GetTypeByName(container, typeName);
            return Activator.CreateInstance(commandType);
        }

        private static void ShowErrorDialog(string message)
        {
            var confirmationForm = new frmDialog(message, "MessageBox", DialogType.OkOnly, 10);
            confirmationForm.ShowDialog();
            confirmationForm.Dispose();
        }

        private static string GetPackageName(string typeName)
        {
            string packageName = string.Empty;
            switch (typeName)
            {
                case "ExecuteDLLCommand":
                    packageName = "API Commands Package";
                    break;
                case "PauseScriptCommand":
                    packageName = "Engine Commands Package";
                    break;
                case "SendMouseMoveCommand":
                case "SendKeystrokesCommand":
                case "SendAdvancedKeystrokesCommand":
                case "UIAutomationCommand":
                    packageName = "Input Commands Package";
                    break;
                case "BeginSwitchCommand":
                case "CaseCommand":
                case "EndSwitchCommand":
                    packageName = "Switch Commands Package";
                    break;
                case "ActivateWindowCommand":
                case "MoveWindowCommand":
                case "ResizeWindowCommand":
                    packageName = "Window Commands Package";
                    break;
                case "SequenceCommand":
                case "AddCodeCommentCommand":
                case "BrokenCodeCommentCommand":
                case "ShowMessageCommand":
                    packageName = "Misc Commands Package";
                    break;
                case "SeleniumCreateBrowserCommand":
                case "SeleniumElementActionCommand":
                case "SeleniumRefreshCommand":
                case "SeleniumNavigateBackCommand":
                case "SeleniumNavigateForwardCommand":
                case "SeleniumNavigateToURLCommand":
                case "SeleniumCloseBrowserCommand":
                    packageName = "Web Browser Commands Package";
                    break;
                case "BeginIfCommand":
                case "EndIfCommand":
                    packageName = "If Commands Package";
                    break;
                case "EndLoopCommand":
                    packageName = "Loop Commands Package";
                    break;
                case "CatchCommand":
                case "EndTryCommand":
                case "EndRetryCommand":
                    packageName = "Error Handling Commands Package";
                    break;
            }
            return packageName;
        }
    }
}