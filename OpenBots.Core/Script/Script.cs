﻿//Copyright (c) 2019 Jason Bayldon
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

using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenBots.Core.Command;
using OpenBots.Core.Model.EngineModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;
using Formatting = Newtonsoft.Json.Formatting;

namespace OpenBots.Core.Script
{
    public class Script
    {
        /// <summary>
        /// Contains user-defined variables
        /// </summary>
        public List<ScriptVariable> Variables { get; set; }
        /// <summary>
        /// Contains user-defined arguments
        /// </summary>
        public List<ScriptArgument> Arguments { get; set; }
        /// <summary>
        /// Contains user-defined elements
        /// </summary>
        public List<ScriptElement> Elements { get; set; }
        /// <summary>
        /// Contains user-selected assemblies
        /// </summary>
        public Dictionary<string, List<AssemblyReference>> ImportedNamespaces { get; set; }
        /// <summary>
        /// Contains user-selected commands
        /// </summary>
        public List<ScriptAction> Commands;        
        public string Version { get; set; }

        public Script()
        {
            Variables = new List<ScriptVariable>();
            Arguments = new List<ScriptArgument>();
            Elements = new List<ScriptElement>();
            Commands = new List<ScriptAction>();
            ImportedNamespaces = new Dictionary<string, List<AssemblyReference>>(ScriptDefaultNamespaces.DefaultNamespaces);
        }

        /// <summary>
        /// Returns a new 'Top-Level' command.
        /// </summary>
        public ScriptAction AddNewParentCommand(ScriptCommand scriptCommand)
        {
            ScriptAction newExecutionCommand = new ScriptAction() { ScriptCommand = scriptCommand };
            Commands.Add(newExecutionCommand);
            return newExecutionCommand;
        }

        /// <summary>
        /// Converts and serializes the user-defined commands into an JSON file
        /// </summary>
        public static Script SerializeScript(ListView.ListViewItemCollection scriptCommands, EngineContext engineContext)
        {
            var script = new Script();

            script.Variables = engineContext.Variables;
            script.Arguments = engineContext.Arguments;
            script.Elements = engineContext.Elements;
            script.ImportedNamespaces = engineContext.ImportedNamespaces;

            //set version to current application version
            script.Version = Application.ProductVersion;

            //save listview tags to command list
            int lineNumber = 1;
            List<ScriptAction> subCommands = new List<ScriptAction>();

            foreach (ListViewItem commandItem in scriptCommands)
            {
                var command = (ScriptCommand)commandItem.Tag;
                command.LineNumber = lineNumber;

                if ((command.CommandName == "LoopNumberOfTimesCommand") || (command.CommandName == "LoopContinuouslyCommand") ||
                    (command.CommandName == "BeginForEachCommand") || (command.CommandName == "BeginIfCommand") ||
                    (command.CommandName == "BeginMultiIfCommand") || (command.CommandName == "BeginTryCommand") ||
                    (command.CommandName == "BeginWhileCommand") || (command.CommandName == "BeginMultiWhileCommand") ||
                    (command.CommandName == "BeginDoWhileCommand") || (command.CommandName == "BeginRetryCommand") || 
                    (command.CommandName == "BeginSwitchCommand"))
                {
                    //if this is the first loop
                    if (subCommands.Count == 0)
                    {
                        //add command to root node
                        var newCommand = script.AddNewParentCommand(command);
                        //add to tracking for additional commands
                        subCommands.Add(newCommand);
                    }
                    else  //we are already looping so add to sub item
                    {
                        //get reference to previous node
                        var parentCommand = subCommands[subCommands.Count - 1];
                        //add as new node to previous node
                        var nextNodeParent = parentCommand.AddAdditionalAction(command);
                        //add to tracking for additional commands
                        subCommands.Add(nextNodeParent);
                    }
                }
                //if current loop scenario is ending
                else if ((command.CommandName == "EndLoopCommand") || (command.CommandName == "EndIfCommand") ||
                         (command.CommandName == "EndTryCommand") || (command.CommandName == "EndSwitchCommand"))
                {
                    //get reference to previous node
                    var parentCommand = subCommands[subCommands.Count - 1];
                    //add to end command // DECIDE WHETHER TO ADD WITHIN LOOP NODE OR PREVIOUS NODE
                    parentCommand.AddAdditionalAction(command);
                    //remove last command since loop is ending
                    subCommands.RemoveAt(subCommands.Count - 1);
                }
                else if (subCommands.Count == 0) //add command as a root item
                    script.AddNewParentCommand(command);
                else //we are within a loop so add to the latest tracked loop item
                {
                    var parentCommand = subCommands[subCommands.Count - 1];
                    parentCommand.AddAdditionalAction(command);
                }

                //increment line number
                lineNumber++;
            }

            var serializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects,
                Error = HandleDeserializationError,
                ContractResolver = new ScriptAutofacContractResolver(engineContext.Container),
            };

            JsonSerializer serializer = JsonSerializer.Create(serializerSettings);

            //output to json file
            //if output path was provided
            if (engineContext.FilePath != "")
            {
                //write to file
                using (StreamWriter sw = new StreamWriter(engineContext.FilePath))
                using (JsonWriter writer = new JsonTextWriter(sw){ Formatting = Formatting.Indented })
                    serializer.Serialize(writer, script, typeof(Script));
            }

            return script;
        }

        /// <summary>
        /// Deserializes a valid JSON file back into user-defined commands
        /// </summary>
        public static Script DeserializeFile(EngineContext engineContext, bool isDialogResultYes = false)
        {
            var serializerSettings = new JsonSerializerSettings();
            if (engineContext.IsTest)
            {
                serializerSettings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                };
            }
            else
            {
                serializerSettings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                    Error = HandleDeserializationError,
                    ContractResolver = new ScriptAutofacContractResolver(engineContext.Container),
                };
            }

            Script deserializedData = JsonConvert.DeserializeObject<Script>(File.ReadAllText(engineContext.FilePath), serializerSettings);
            Version deserializedScriptVersion;

            deserializedData.Commands.ForEach(x => { if (x.ScriptCommand != null) { x.ScriptCommand.CommandIcon = null; }});
            GC.Collect();

            if (deserializedData != null)
            {
                if (deserializedData.Version == null)
                    deserializedData.Version = "0.0.0.0";

                deserializedScriptVersion = new Version(deserializedData.Version);
            }
            else
                deserializedScriptVersion = new Version("0.0.0.0");

            ////if deserialized Script version is lower than than the current application version
            //if (!engineContext.IsTest && deserializedScriptVersion.CompareTo(new Version(Application.ProductVersion)) < 0 && !isDialogResultYes)
            //{
            //    var dialogResult = MessageBox.Show($"Attempting to open a Script file from OpenBots Studio {deserializedScriptVersion}. " +
            //                                       $"Would you like to attempt to convert this Script to {Application.ProductVersion}? " + 
            //                                       "\n\nWarning: Once a Script has been converted, it cannot be undone.", 
            //                                       "Convert Script", MessageBoxButtons.YesNo);

            //    if (dialogResult == DialogResult.Yes)
            //        deserializedData = ConvertScriptToLatestVersion(engineContext.FilePath, engineContext.Container, deserializedScriptVersion.ToString());
            //}
            //else if(!engineContext.IsTest && deserializedScriptVersion.CompareTo(new Version(Application.ProductVersion)) < 0 && isDialogResultYes)
            //    deserializedData = ConvertScriptToLatestVersion(engineContext.FilePath, engineContext.Container, deserializedScriptVersion.ToString());

            //update ProjectPath variable
            var projectPathVariable = deserializedData.Variables.Where(v => v.VariableName == "ProjectPath").SingleOrDefault();

            if (projectPathVariable != null)
                deserializedData.Variables.Remove(projectPathVariable);

            projectPathVariable = new ScriptVariable
            {
                VariableName = "ProjectPath",
                VariableType = typeof(string),
                VariableValue = "\"Value Provided at Runtime\""
            };
            deserializedData.Variables.Add(projectPathVariable);

            return deserializedData;
        }

        public static void HandleDeserializationError(object sender, ErrorEventArgs e)
        {
            var deserializationError = e.ErrorContext.Error.Message;
            var commandNameMatch = Regex.Match(deserializationError, @"OpenBots\.Commands\.\w+\.\w+Command");
            
            Dictionary<string, string> newCommandGroupMapping = new Dictionary<string, string>()
            {
                { "Data", "DataManipulation" },
                { "DataTable", "DataManipulation" },
                { "Dictionary", "DataManipulation" },
                { "List", "DataManipulation" },
                { "RegEx", "DataManipulation" },
                { "Email", "SystemAutomation" },
                { "File", "SystemAutomation" },
                { "Folder", "SystemAutomation" },
                { "TextFile", "SystemAutomation" },
                { "System", "SystemAutomation" },
                { "Image", "UIAutomation" },
                { "Input", "UIAutomation" },
                { "Process", "UIAutomation" },
                { "WebBrowser", "UIAutomation" },
                { "Window", "UIAutomation" },
                { "Excel", "Microsoft" },
                { "Outlook", "Microsoft" },
                { "Word", "Microsoft" },
                { "Engine", "Core" },
                { "ErrorHandling", "Core" },
                { "If", "Core" },
                { "Loop", "Core" },
                { "Misc", "Core" },
                { "SecureData", "Core" },
                { "Switch", "Core" },
                { "Task", "Core" },
                { "Variable", "Core" },
                { "Asset", "Server" },
                { "Credential", "Server" },
                { "QueueItem", "Server" },
                { "ServerEmail", "Server" },
            };

            if (commandNameMatch.Success)
            {
                var commandGroupMatch = Regex.Match(commandNameMatch.Value, @"OpenBots\.Commands\.\w+");
                string commandGroupFullName = commandGroupMatch.Value;
                string commandGroupShortName = commandGroupFullName.Split('.').Last();

                if (newCommandGroupMapping.ContainsKey(commandGroupShortName))
                    commandGroupFullName = commandGroupFullName.Replace(commandGroupShortName, newCommandGroupMapping[commandGroupShortName]);

                deserializationError = $"Unable to load '{commandNameMatch.Value}'. Please install '{commandGroupFullName}'" +
                                        " from the Package Manager.";
            }
                
            if (e.CurrentObject is ScriptAction)
                ((ScriptAction)e.CurrentObject).SerializationError = deserializationError;
            else if (e.CurrentObject is ScriptVariable)
            {
                var splitErrorMessage = e.ErrorContext.Error.Message.Split(new char[] { '"', ',' });
                if (splitErrorMessage.Length > 1)
                    ((ScriptVariable)e.CurrentObject).VariableType = GetTypeByFullName(splitErrorMessage[1]);
                else
                    ((ScriptVariable)e.CurrentObject).VariableType = typeof(object);
            }
            else if (e.CurrentObject is ScriptArgument)
            {
                var splitErrorMessage = e.ErrorContext.Error.Message.Split(new char[] { '"', ',' });
                if (splitErrorMessage.Length > 1)
                    ((ScriptArgument)e.CurrentObject).ArgumentType = GetTypeByFullName(splitErrorMessage[1]);
                else
                    ((ScriptArgument)e.CurrentObject).ArgumentType = typeof(object);
            }
        
            e.ErrorContext.Handled = true;
        }

        /// <summary>
        /// Deserializes an json string into user-defined commands (server sends a string to the client)
        /// </summary>
        public static Script DeserializeJsonString(string jsonScript)
        {
            return JsonConvert.DeserializeObject<Script>(jsonScript);
        }

        public static Script ConvertScriptToLatestVersion(string filePath, IContainer container, string version)
        {
            string scriptText = File.ReadAllText(filePath);

            if (version == "0.0.0.0")
                scriptText = scriptText.Insert(scriptText.LastIndexOf('\r'), ",\r\n  \"Version\": \"1.1.0.0\"");

            var conversionFilePath = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, 
                                                  "Supplementary Files", "Script Conversion Files", version + ".json");

            string conversionFileText = File.ReadAllText(conversionFilePath);
            JObject conversionObject = JObject.Parse(conversionFileText);

            foreach (var x in conversionObject["Replace"])
            {
                if (((JProperty)x).Name.StartsWith("__comment"))
                    continue;

                scriptText = Regex.Replace(scriptText, ((JProperty)x).Name, ((JProperty)x).Value.ToString());
            }
                               
            File.WriteAllText(filePath, scriptText);

            EngineContext engineContext = new EngineContext()
            {
                FilePath = filePath,
                Container = container
            };

            return DeserializeFile(engineContext, true);
        }

        public static Type GetTypeByFullName(string typeFullName)
        {
            var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a =>
                {
                    try
                    {
                        return a.GetTypes();
                    }
                    catch (Exception)
                    {
                        return new Type[] { };
                    }
                });

            Type type = allTypes.FirstOrDefault(t => t.FullName == typeFullName);

            return type == null ? typeof(object) : type;                  
        }          
    }
}
