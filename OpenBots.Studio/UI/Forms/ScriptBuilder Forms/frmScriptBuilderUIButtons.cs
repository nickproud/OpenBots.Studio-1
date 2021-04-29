using Autofac;
using Newtonsoft.Json;
using NuGet;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.IO;
using OpenBots.Core.Model.EngineModel;
using OpenBots.Core.Project;
using OpenBots.Core.Script;
using OpenBots.Core.Settings;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Core.Utilities.FormsUtilities;
using OpenBots.Nuget;
using OpenBots.Studio.Utilities;
using OpenBots.UI.CustomControls.CustomUIControls;
using OpenBots.UI.Forms.Supplement_Forms;
using OpenBots.UI.Supplement_Forms;
using OpenBots.Utilities;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.ScriptBuilder_Forms
{
    public partial class frmScriptBuilder : Form
    {
        #region UI Buttons
        #region File Actions Tool Strip and Buttons
        private void uiBtnNew_Click(object sender, EventArgs e)
        {
            NewFile();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFile();
        }

        private void NewFile()
        {
            ScriptFilePath = null;
            _scriptFileExtension = null;
            _isMainScript = false;

            string title = $"New Tab {(uiScriptTabControl.TabCount + 1)} *";
            TabPage newTabPage = new TabPage(title)
            {
                Name = title,
                ToolTipText = ""
            };
            uiScriptTabControl.Controls.Add(newTabPage);

            switch (ScriptProject.ProjectType)
            {
                case ProjectType.OpenBots:
                    newTabPage.Tag = new ScriptObject();                   
                    newTabPage.Controls.Add(NewLstScriptActions(title));
                    newTabPage.Controls.Add(pnlCommandHelper);

                    uiScriptTabControl.SelectedTab = newTabPage;

                    _selectedTabScriptActions = (UIListView)uiScriptTabControl.SelectedTab.Controls[0];
                    _selectedTabScriptActions.Items.Clear();
                    HideSearchInfo();

                    _scriptVariables = new List<ScriptVariable>();

                    //assign ProjectPath variable
                    var projectPathVariable = new ScriptVariable
                    {
                        VariableName = "ProjectPath",
                        VariableType = typeof(string),
                        VariableValue = "Value Provided at Runtime"
                    };
                    _scriptVariables.Add(projectPathVariable);

                    _scriptArguments = new List<ScriptArgument>();
                    _scriptElements = new List<ScriptElement>();
                    _importedNamespaces = new Dictionary<string, AssemblyReference>(ScriptDefaultNamespaces.DefaultNamespaces);

                    ResetVariableArgumentBindings();

                    GenerateRecentProjects();
                    newTabPage.Controls[0].Hide();
                    pnlCommandHelper.Show();
                    break;
                case ProjectType.Python:
                    newTabPage.Controls.Add(NewTextEditorActions(ProjectType.Python, title));
                    newTabPage.Tag = new ScriptObject();
                    uiScriptTabControl.SelectedTab = newTabPage;
                    _scriptArguments = new List<ScriptArgument>();

                    //assign pythonVersion and mainFunction arguments
                    var mainFunctionArgument = new ScriptArgument
                    {
                        ArgumentName = "--MainFunction",
                        ArgumentType = typeof(string),
                        Direction = ScriptArgumentDirection.In,
                        ArgumentValue = "main"
                    };
                    _scriptArguments.Add(mainFunctionArgument);

                    var pythonVersionArgument = new ScriptArgument
                    {
                        ArgumentName = "--PythonVersion",
                        ArgumentType = typeof(string),
                        Direction = ScriptArgumentDirection.In                      
                    };
                    _scriptArguments.Add(pythonVersionArgument);

                    SetVarArgTabControlSettings(ScriptProject.ProjectType);
                    ResetVariableArgumentBindings();
                    break;
                case ProjectType.TagUI:
                    newTabPage.Controls.Add(NewTextEditorActions(ProjectType.TagUI, title));
                    newTabPage.Tag = new ScriptObject();
                    uiScriptTabControl.SelectedTab = newTabPage;
                    _scriptArguments = new List<ScriptArgument>();

                    var reportArgument = new ProjectArgument
                    {
                        ArgumentName = "-report",
                        ArgumentType = typeof(string),
                    };
                    ScriptProject.ProjectArguments.Add(reportArgument);

                    SetVarArgTabControlSettings(ScriptProject.ProjectType);
                    ResetVariableArgumentBindings();
                    break;
                case ProjectType.CSScript:
                    newTabPage.Controls.Add(NewTextEditorActions(ProjectType.CSScript, title));
                    newTabPage.Tag = new ScriptObject();
                    uiScriptTabControl.SelectedTab = newTabPage;
                    _scriptArguments = new List<ScriptArgument>();

                    SetVarArgTabControlSettings(ScriptProject.ProjectType);
                    ResetVariableArgumentBindings();
                    break;
            }
            
        }

        private void uiBtnOpen_Click(object sender, EventArgs e)
        {
            //show ofd
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = ScriptProjectPath,
                RestoreDirectory = true,
            };

            //if user selected file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string extension = Path.GetExtension(openFileDialog.FileName);

                //open file
                switch (extension.ToLower())
                {
                    case ".obscript":
                        OpenOpenBotsFile(openFileDialog.FileName);
                        break;
                    case ".py":
                        OpenTextEditorFile(openFileDialog.FileName, ProjectType.Python);
                        break;
                    case ".tag":
                        OpenTextEditorFile(openFileDialog.FileName, ProjectType.TagUI);
                        break;
                    case ".cs":
                        OpenTextEditorFile(openFileDialog.FileName, ProjectType.CSScript);
                        break;
                    default:
                        Process.Start(openFileDialog.FileName);
                        break;
                }
                
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uiBtnOpen_Click(sender, e);
        }

        public delegate void OpenFileDelegate(string filepath, bool isRunTaskCommand);
        public void OpenOpenBotsFile(string filePath, bool isRunTaskCommand = false)
        {
            if (InvokeRequired)
            {
                var d = new OpenFileDelegate(OpenOpenBotsFile);
                Invoke(d, new object[] { filePath, isRunTaskCommand });
            }
            else
            {
                try
                {
                    _isRunTaskCommand = isRunTaskCommand;

                    //create or switch to TabPage
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    var foundTab = uiScriptTabControl.TabPages.Cast<TabPage>().Where(t => t.ToolTipText == filePath)
                                                                              .FirstOrDefault();
                    if (foundTab == null)
                    {
                        TabPage newtabPage = new TabPage(fileName)
                        {
                            Name = fileName,
                            ToolTipText = filePath
                        };

                        uiScriptTabControl.TabPages.Add(newtabPage);
                        newtabPage.Controls.Add(NewLstScriptActions(fileName));
                        uiScriptTabControl.SelectedTab = newtabPage;
                        _isRunTaskCommand = false;      
                    }
                    else
                    {
                        uiScriptTabControl.SelectedTab = foundTab;
                        _isRunTaskCommand = false;
                        return;
                    }

                    _selectedTabScriptActions = (UIListView)uiScriptTabControl.SelectedTab.Controls[0];

                    //get deserialized script
                    EngineContext engineContext = new EngineContext()
                    {
                        FilePath = filePath,
                        Container = AContainer
                    };
                    Script deserializedScript = Script.DeserializeFile(engineContext);

                    //reinitialize
                    _selectedTabScriptActions.Items.Clear();
                    _scriptVariables = new List<ScriptVariable>();
                    _scriptArguments = new List<ScriptArgument>();
                    _scriptElements = new List<ScriptElement>();
                    _importedNamespaces = new Dictionary<string, AssemblyReference>();

                    if (deserializedScript.Commands.Count == 0)
                    {
                        Notify("Error Parsing File: Commands not found!", Color.Red);
                    }

                    //update file path and reflect in title bar
                    ScriptFilePath = filePath;
                    _scriptFileExtension = Path.GetExtension(ScriptFilePath).ToLower();
                    _isMainScript = Path.Combine(ScriptProjectPath, ScriptProject.Main) == ScriptFilePath;

                    string scriptFileName = Path.GetFileNameWithoutExtension(ScriptFilePath);
                    _selectedTabScriptActions.Name = $"{scriptFileName}ScriptActions";

                    //assign variables
                    _scriptVariables.AddRange(deserializedScript.Variables);
                    _scriptElements.AddRange(deserializedScript.Elements);
                    _scriptArguments.AddRange(deserializedScript.Arguments);
                    _importedNamespaces.AddRange(deserializedScript.ImportedNamespaces);

                    uiScriptTabControl.SelectedTab.Tag = new ScriptObject(_scriptVariables, _scriptArguments, _scriptElements, _importedNamespaces);                  

                    //populate commands
                    PopulateExecutionCommands(deserializedScript.Commands);

                    uiScriptTabControl.SelectedTab.Text = scriptFileName;

                    if (!isRunTaskCommand)
                    {
                        SetVarArgTabControlSettings(ProjectType.OpenBots);
                        ResetVariableArgumentBindings();

                        Notify("Script Loaded Successfully!", Color.White);
                    }
                    else
                        _selectedTabScriptActions.Enabled = false;
                }
                catch (Exception ex)
                {
                    //signal an error has happened
                    Notify("An Error Occurred: " + ex.Message, Color.Red);
                }
            }           
        }

        //helper method for RunTaskCommand
        public void OpenScriptFile(string scriptFilePath, bool isRunTaskCommand = true)
        {
            OpenOpenBotsFile(scriptFilePath, isRunTaskCommand);
        }

        public void OpenTextEditorFile(string filePath, ProjectType projectType)
        {
            try
            {
                //create or switch to TabPage
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                var foundTab = uiScriptTabControl.TabPages.Cast<TabPage>().Where(t => t.ToolTipText == filePath)
                                                                          .FirstOrDefault();
                if (foundTab == null)
                {
                    TabPage newtabPage = new TabPage(fileName)
                    {
                        Name = fileName,
                        ToolTipText = filePath
                    };

                    uiScriptTabControl.TabPages.Add(newtabPage);

                    newtabPage.Controls.Add(NewTextEditorActions(projectType, fileName));
                 
                    uiScriptTabControl.SelectedTab = newtabPage;
                    ((Scintilla)uiScriptTabControl.SelectedTab.Controls[0]).Text = File.ReadAllText(filePath);
                }
                else
                {
                    uiScriptTabControl.SelectedTab = foundTab;
                    return;
                }

                _selectedTabScriptActions = (Scintilla)uiScriptTabControl.SelectedTab.Controls[0];

                //reinitialize
                _scriptVariables = new List<ScriptVariable>();
                _scriptArguments = new List<ScriptArgument>();
                _scriptElements = new List<ScriptElement>();
                _importedNamespaces = new Dictionary<string, AssemblyReference>();

                //update file path and reflect in title bar
                ScriptFilePath = filePath;
                _scriptFileExtension = Path.GetExtension(ScriptFilePath).ToLower();
                _isMainScript = Path.Combine(ScriptProjectPath, ScriptProject.Main) == ScriptFilePath;

                string scriptFileName = Path.GetFileNameWithoutExtension(ScriptFilePath);
                _selectedTabScriptActions.Name = $"{scriptFileName}ScriptActions";

                //assign project arguments
                _scriptArguments.AddRange(ScriptProject.ProjectArguments.Select(arg => new ScriptArgument 
                                                                                        { 
                                                                                            ArgumentName = arg.ArgumentName,
                                                                                            ArgumentType = arg.ArgumentType,
                                                                                            ArgumentValue = arg.ArgumentValue,
                                                                                            Direction = arg.Direction,
                                                                                        })
                                                                        .ToList());

                uiScriptTabControl.SelectedTab.Tag = new ScriptObject(_scriptVariables, _scriptArguments, _scriptElements, _importedNamespaces);
                uiScriptTabControl.SelectedTab.Text = scriptFileName;

                SetVarArgTabControlSettings(ProjectType.Python);
                ResetVariableArgumentBindings();
            }
            catch (Exception ex)
            {
                //signal an error has happened
                Notify("An Error Occurred: " + ex.Message, Color.Red);
            }
        }

        private void uiBtnSave_Click(object sender, EventArgs e)
        {
            if (_selectedTabScriptActions is ListView)
            {
                //clear selected items
                ClearSelectedListViewItems();
                SaveToOpenBotsFile(false);
            }
            else
                SaveToTextEditorFile(false);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uiBtnSave_Click(sender, e);
        }

        private void uiBtnSaveAs_Click(object sender, EventArgs e)
        {
            if (_selectedTabScriptActions is ListView)
            {
                //clear selected items
                ClearSelectedListViewItems();
                SaveToOpenBotsFile(true);
            }
            else
                SaveToTextEditorFile(true);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uiBtnSaveAs_Click(sender, e);
        }

        private bool SaveToTextEditorFile(bool saveAs)
        {
            bool isSuccessfulSave = false;
            try
            {
                //define default output path
                if (string.IsNullOrEmpty(ScriptFilePath) || saveAs)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        InitialDirectory = ScriptProjectPath,
                        RestoreDirectory = true,
                    };

                    if (saveFileDialog.ShowDialog() != DialogResult.OK)
                        return isSuccessfulSave;

                    if (!saveFileDialog.FileName.Contains(ScriptProjectPath))
                    {
                        Notify("An Error Occurred: Attempted to save script outside of project directory", Color.Red);
                        return isSuccessfulSave;
                    }

                    ScriptFilePath = saveFileDialog.FileName;
                    _scriptFileExtension = Path.GetExtension(ScriptFilePath).ToLower();
                    _isMainScript = Path.Combine(ScriptProjectPath, ScriptProject.Main) == ScriptFilePath;

                    string scriptFileName = Path.GetFileNameWithoutExtension(ScriptFilePath);
                    if (uiScriptTabControl.SelectedTab.Text != scriptFileName)
                        UpdateTabPage(uiScriptTabControl.SelectedTab, ScriptFilePath);
                }

                File.WriteAllText(ScriptFilePath, ((Scintilla)_selectedTabScriptActions).Text);
                uiScriptTabControl.SelectedTab.Text = uiScriptTabControl.SelectedTab.Text.Replace(" *", "");

                Notify("File has been saved successfully!", Color.White);
                isSuccessfulSave = true;

                try
                {
                    if (_isMainScript)
                    {
                        ScriptProject.ProjectArguments.Clear();
                        ScriptProject.ProjectArguments.AddRange(_scriptArguments.Select(arg => new ProjectArgument()
                        {
                            ArgumentName = arg.ArgumentName,
                            ArgumentType = arg.ArgumentType,
                            ArgumentValue = arg.ArgumentValue
                        })
                                                                                .ToList());
                    }

                    ScriptProject.SaveProject(ScriptFilePath);
                }
                catch (Exception ex)
                {
                    Notify("An Error Occured: " + ex.Message, Color.Red);
                } 
            }
            catch (Exception ex)
            {
                Notify("An Error Occurred: " + ex.Message, Color.Red);
            }

            return isSuccessfulSave;
        }

        private bool SaveToOpenBotsFile(bool saveAs)
        {
            bool isSuccessfulSave = false;

            dgvVariables.EndEdit();
            dgvArguments.EndEdit();

            if (_selectedTabScriptActions.Items.Count == 0)
            {
                Notify("You must have at least 1 automation command to save.", Color.Yellow);
                return isSuccessfulSave;
            }

            int beginLoopValidationCount = 0;
            int beginIfValidationCount = 0;
            int tryCatchValidationCount = 0;
            int retryValidationCount = 0;
            int beginSwitchValidationCount = 0;

            foreach (ListViewItem item in _selectedTabScriptActions.Items)
            {
                if(item.Tag is BrokenCodeCommentCommand)
                {
                    Notify("Please verify that all broken code has been removed or replaced.", Color.Yellow);
                    return isSuccessfulSave;
                }
                else if ((item.Tag.GetType().Name == "LoopCollectionCommand") || (item.Tag.GetType().Name == "LoopContinuouslyCommand") ||
                    (item.Tag.GetType().Name == "LoopNumberOfTimesCommand") || (item.Tag.GetType().Name == "BeginLoopCommand") ||
                    (item.Tag.GetType().Name == "BeginMultiLoopCommand"))
                {
                    beginLoopValidationCount++;
                }
                else if (item.Tag.GetType().Name == "EndLoopCommand")
                {
                    beginLoopValidationCount--;
                }
                else if ((item.Tag.GetType().Name == "BeginIfCommand") || (item.Tag.GetType().Name == "BeginMultiIfCommand"))
                {
                    beginIfValidationCount++;
                }
                else if (item.Tag.GetType().Name == "EndIfCommand")
                {
                    beginIfValidationCount--;
                }
                else if (item.Tag.GetType().Name == "BeginTryCommand")
                {
                    tryCatchValidationCount++;
                }
                else if (item.Tag.GetType().Name == "EndTryCommand")
                {
                    tryCatchValidationCount--;
                }
                else if (item.Tag.GetType().Name == "BeginRetryCommand")
                {
                    retryValidationCount++;
                }
                else if (item.Tag.GetType().Name == "EndRetryCommand")
                {
                    retryValidationCount--;
                }
                else if(item.Tag.GetType().Name == "BeginSwitchCommand")
                {
                    beginSwitchValidationCount++;
                }
                else if (item.Tag.GetType().Name == "EndSwitchCommand")
                {
                    beginSwitchValidationCount--;
                }

                //end loop was found first
                if (beginLoopValidationCount < 0)
                {
                    Notify("Please verify the ordering of your loops.", Color.Yellow);
                    return isSuccessfulSave;
                }

                //end if was found first
                if (beginIfValidationCount < 0)
                {
                    Notify("Please verify the ordering of your ifs.", Color.Yellow);
                    return isSuccessfulSave;
                }

                if (tryCatchValidationCount < 0)
                {
                    Notify("Please verify the ordering of your try/catch blocks.", Color.Yellow);
                    return isSuccessfulSave;
                }

                if (retryValidationCount < 0)
                {
                    Notify("Please verify the ordering of your retry blocks.", Color.Yellow);
                    return isSuccessfulSave;
                }

                if (beginSwitchValidationCount < 0)
                {
                    Notify("Please verify the ordering of your switch/case blocks.", Color.Yellow);
                    return isSuccessfulSave;
                }
            }

            //extras were found
            if (beginLoopValidationCount != 0)
            {
                Notify("Please verify the ordering of your loops.", Color.Yellow);
                return isSuccessfulSave;
            }

            //extras were found
            if (beginIfValidationCount != 0)
            {
                Notify("Please verify the ordering of your ifs.", Color.Yellow);
                return isSuccessfulSave;
            }

            if (tryCatchValidationCount != 0)
            {
                Notify("Please verify the ordering of your try/catch blocks.", Color.Yellow);
                return isSuccessfulSave;
            }

            if (retryValidationCount != 0)
            {
                Notify("Please verify the ordering of your retry blocks.", Color.Yellow);
                return isSuccessfulSave;
            }

            if (beginSwitchValidationCount != 0)
            {
                Notify("Please verify the ordering of your switch/case blocks.", Color.Yellow);
                return isSuccessfulSave;
            }

            //define default output path
            if (string.IsNullOrEmpty(ScriptFilePath) || saveAs)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    InitialDirectory = ScriptProjectPath,
                    RestoreDirectory = true,
                };

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return isSuccessfulSave;

                if (!saveFileDialog.FileName.ToString().Contains(ScriptProjectPath))
                {
                    Notify("An Error Occurred: Attempted to save script outside of project directory", Color.Red);
                    return isSuccessfulSave;
                }

                ScriptFilePath = saveFileDialog.FileName;
                _scriptFileExtension = Path.GetExtension(ScriptFilePath).ToLower();
                _isMainScript = Path.Combine(ScriptProjectPath, ScriptProject.Main) == ScriptFilePath;

                string scriptFileName = Path.GetFileNameWithoutExtension(ScriptFilePath);
                if (uiScriptTabControl.SelectedTab.Text != scriptFileName)
                    UpdateTabPage(uiScriptTabControl.SelectedTab, ScriptFilePath);
            }

            //serialize script
            try
            {
                EngineContext engineContext = new EngineContext
                {
                    Variables = _scriptVariables.Where(x => !string.IsNullOrEmpty(x.VariableName)).ToList(),
                    Arguments = _scriptArguments.Where(x => !string.IsNullOrEmpty(x.ArgumentName)).ToList(),
                    Elements = _scriptElements.Where(x => !string.IsNullOrEmpty(x.ElementName)).ToList(),
                    ImportedNamespaces = _importedNamespaces,
                    FilePath = ScriptFilePath,
                    Container = AContainer
                };

                var exportedScript = Script.SerializeScript(_selectedTabScriptActions.Items, engineContext);
                uiScriptTabControl.SelectedTab.Text = uiScriptTabControl.SelectedTab.Text.Replace(" *", "");

                Notify("File has been saved successfully!", Color.White);
                isSuccessfulSave = true;
                try
                {
                    if (_isMainScript)
                    {
                        ScriptProject.ProjectArguments.Clear();
                        ScriptProject.ProjectArguments.AddRange(_scriptArguments.Select(arg => new ProjectArgument()
                                                                                    {
                                                                                        ArgumentName = arg.ArgumentName,
                                                                                        ArgumentType = arg.ArgumentType,
                                                                                        ArgumentValue = arg.ArgumentValue
                                                                                    })
                                                                                .ToList());
                    }

                    ScriptProject.SaveProject(ScriptFilePath);
                }
                catch (Exception ex)
                {
                    Notify("An Error Occurred: " + ex.Message, Color.Red);
                }              
            }
            catch (Exception ex)
            {
                Notify("An Error Occurred: " + ex.Message, Color.Red);
            }
            return isSuccessfulSave;
        }

        private bool SaveAllFiles()
        {
            bool isSuccessfulSaveAll;
            TabPage currentTab = uiScriptTabControl.SelectedTab;
            foreach (TabPage openTab in uiScriptTabControl.TabPages)
            {
                uiScriptTabControl.SelectedTab = openTab;
                //clear selected items
                ClearSelectedListViewItems();

                if (_selectedTabScriptActions is ListView)
                    isSuccessfulSaveAll = SaveToOpenBotsFile(false);
                else
                    isSuccessfulSaveAll = SaveToTextEditorFile(false);

                if (!isSuccessfulSaveAll)
                    return isSuccessfulSaveAll;
            }
            uiScriptTabControl.SelectedTab = currentTab;
            Thread.Sleep(100);
            isSuccessfulSaveAll = true;

            return isSuccessfulSaveAll;
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAllFiles();
        }

        private void uiBtnSaveAll_Click(object sender, EventArgs e)
        {
            SaveAllFiles();
        }

        private void ClearSelectedListViewItems()
        {
            if (_selectedTabScriptActions is ListView)
            {
                _selectedTabScriptActions.SelectedItems.Clear();
                _selectedTabScriptActions.Invalidate();
            }               
        }

        private void publishProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (ScriptProject.ProjectType)
            {
                case ProjectType.OpenBots:
                    if (!SaveAllFiles())
                        return;
                    break;
            }
            
            frmPublishProject publishProject = new frmPublishProject(ScriptProjectPath, ScriptProject);
            publishProject.ShowDialog();

            if (publishProject.DialogResult == DialogResult.OK)
                Notify(publishProject.NotificationMessage, Color.White);

            publishProject.Dispose();
        }

        private void uiBtnPublishProject_Click(object sender, EventArgs e)
        {
            publishProjectToolStripMenuItem_Click(sender, e);
        }

        private void uiBtnImport_Click(object sender, EventArgs e)
        {
            BeginImportProcess();
        }

        private void importFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeginImportProcess();
        }

        private void BeginImportProcess()
        {
            if (!(_selectedTabScriptActions is ListView))
                return;

            //show ofd
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = ScriptProjectPath,
                RestoreDirectory = true,
                Filter = "obscript (*.obscript)|*.obscript"
            };

            //if user selected file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //import
                Cursor.Current = Cursors.WaitCursor;
                Import(openFileDialog.FileName);
                Cursor.Current = Cursors.Default;
            }
        }

        private void Import(string filePath)
        {
            try
            {              
                //deserialize file
                EngineContext engineContext = new EngineContext()
                {
                    FilePath = filePath,
                    Container = AContainer
                };
                Script deserializedScript = Script.DeserializeFile(engineContext);

                if (deserializedScript.Commands.Count == 0)
                    Notify("Error Parsing File: Commands not found!", Color.Red);

                //variables for comments
                var fileName = new FileInfo(filePath).Name;
                var dateTimeNow = DateTime.Now.ToString();

                CreateUndoSnapshot();

                //comment
                dynamic addCodeCommentCommand = TypeMethods.CreateTypeInstance(AContainer, "AddCodeCommentCommand");
                addCodeCommentCommand.v_Comment = "Imported From " + fileName + " @ " + dateTimeNow;
                _selectedTabScriptActions.Items.Add(CreateScriptCommandListViewItem(addCodeCommentCommand));

                //import
                PopulateExecutionCommands(deserializedScript.Commands);
                foreach (ScriptVariable newVar in deserializedScript.Variables)
                {
                    var existingVar = _scriptVariables.Find(alreadyExists => alreadyExists.VariableName == newVar.VariableName);
                    if (existingVar != null)
                        _scriptVariables.Remove(existingVar);

                    _scriptVariables.Add(newVar);                        
                }

                foreach (ScriptArgument newArg in deserializedScript.Arguments)
                {
                    var existingArg = _scriptArguments.Find(alreadyExists => alreadyExists.ArgumentName == newArg.ArgumentName);
                    if (existingArg != null)
                        _scriptArguments.Remove(existingArg);

                    _scriptArguments.Add(newArg);
                }

                foreach (ScriptElement newElem in deserializedScript.Elements)
                {
                    var existingElem = _scriptElements.Find(alreadyExists => alreadyExists.ElementName == newElem.ElementName);
                    if (existingElem != null)
                        _scriptElements.Remove(newElem);

                    _scriptElements.Add(newElem);
                }

                foreach (var nsp in deserializedScript.ImportedNamespaces)
                {
                    if (!_importedNamespaces.ContainsKey(nsp.Key))
                        _importedNamespaces.Add(nsp.Key, nsp.Value);
                    else
                        _importedNamespaces[nsp.Key] = nsp.Value;
                }

                ResetVariableArgumentBindings();

                //comment
                dynamic codeCommentCommand = TypeMethods.CreateTypeInstance(AContainer, "AddCodeCommentCommand");
                codeCommentCommand.v_Comment = "End Import From " + fileName + " @ " + dateTimeNow;
                _selectedTabScriptActions.Items.Add(CreateScriptCommandListViewItem(codeCommentCommand));

                Notify("Script Imported Successfully!", Color.White);
            }
            catch (Exception ex)
            {
                //signal an error has happened
                Notify("An Error Occurred: " + ex.Message, Color.Red);
            }
        }

        public void PopulateExecutionCommands(List<ScriptAction> commandDetails)
        {

            foreach (ScriptAction item in commandDetails)
            {
                if (item.ScriptCommand != null)
                    _selectedTabScriptActions.Items.Add(CreateScriptCommandListViewItem(item.ScriptCommand));
                else
                {
                    var brokenCodeCommentCommand = new BrokenCodeCommentCommand();
                    brokenCodeCommentCommand.v_Comment = item.SerializationError;
                    _selectedTabScriptActions.Items.Add(CreateScriptCommandListViewItem(brokenCodeCommentCommand));
                }
                if (item.AdditionalScriptCommands?.Count > 0)
                    PopulateExecutionCommands(item.AdditionalScriptCommands);
            }

            if (pnlCommandHelper.Visible)
            {
                uiScriptTabControl.SelectedTab.Controls.Remove(pnlCommandHelper);
                uiScriptTabControl.SelectedTab.Controls[0].Show();
            }
            else if (!uiScriptTabControl.SelectedTab.Controls[0].Visible)
                uiScriptTabControl.SelectedTab.Controls[0].Show();
        }
        #region Restart And Close Buttons
        
       
        private void uiBtnRestart_Click(object sender, EventArgs e)
        {
            _appSettings.ClientSettings.IsRestarting = true;
            _appSettings.Save(_appSettings);
            Application.Restart();
        }

        private void restartApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uiBtnRestart_Click(sender, e);
        }

        private void uiBtnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void closeApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uiBtnClose_Click(sender, e);
        }
        #endregion
        #endregion

        #region Options Tool Strip and Buttons
        private void variablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenVariableManager();
        }

        private void uiBtnAddVariable_Click(object sender, EventArgs e)
        {
            OpenVariableManager();
        }

        private void OpenVariableManager()
        {
            if (!(_selectedTabScriptActions is ListView))
                return;

            frmScriptVariables scriptVariableEditor = new frmScriptVariables(_typeContext)
            {
                ScriptName = uiScriptTabControl.SelectedTab.Name,
                ScriptVariables = new List<ScriptVariable>(_scriptVariables),
                ScriptArguments = new List<ScriptArgument>(_scriptArguments)
            };

            if (scriptVariableEditor.ShowDialog() == DialogResult.OK)
            {
                Invalidate();
                _scriptVariables = scriptVariableEditor.ScriptVariables;
                uiScriptTabControl.SelectedTab.Tag = new ScriptObject(_scriptVariables, _scriptArguments, _scriptElements, _importedNamespaces);

                if (!uiScriptTabControl.SelectedTab.Text.Contains(" *"))
                    uiScriptTabControl.SelectedTab.Text += " *"; 
            }

            ResetVariableArgumentBindings();
            scriptVariableEditor.Dispose();
        }

        private void argumentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenArgumentManager();
        }

        private void uiBtnAddArgument_Click(object sender, EventArgs e)
        {
            OpenArgumentManager();
        }

        private void OpenArgumentManager()
        {
            if (!(_selectedTabScriptActions is ListView))
                return;

            frmScriptArguments scriptArgumentEditor = new frmScriptArguments(_typeContext)
            {
                ScriptName = uiScriptTabControl.SelectedTab.Name,
                ScriptVariables = new List<ScriptVariable>(_scriptVariables),
                ScriptArguments = new List<ScriptArgument>(_scriptArguments)
            };

            if (scriptArgumentEditor.ShowDialog() == DialogResult.OK)
            {
                _scriptArguments = scriptArgumentEditor.ScriptArguments;
                uiScriptTabControl.SelectedTab.Tag = new ScriptObject(_scriptVariables, _scriptArguments, _scriptElements, _importedNamespaces);

                if (!uiScriptTabControl.SelectedTab.Text.Contains(" *"))
                    uiScriptTabControl.SelectedTab.Text += " *";
            }

            ResetVariableArgumentBindings();
            scriptArgumentEditor.Dispose();
        }

        private void elementManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenElementManager();
        }

        private void uiBtnAddElement_Click(object sender, EventArgs e)
        {
            OpenElementManager();
        }

        private void OpenElementManager()
        {
            if (!(_selectedTabScriptActions is ListView))
                return;

            frmScriptElements scriptElementEditor = new frmScriptElements
            {
                ScriptName = uiScriptTabControl.SelectedTab.Name,
                ScriptElements = new List<ScriptElement>(_scriptElements)
            };

            if (scriptElementEditor.ShowDialog() == DialogResult.OK)
            {
                CreateUndoSnapshot();
                _scriptElements = scriptElementEditor.ScriptElements;
                uiScriptTabControl.SelectedTab.Tag = new ScriptObject(_scriptVariables, _scriptArguments, _scriptElements, _importedNamespaces);
            }

            scriptElementEditor.Dispose();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSettingsManager();
        }

        private void uiBtnSettings_Click(object sender, EventArgs e)
        {
            OpenSettingsManager();
        }

        private void OpenSettingsManager()
        {
            //show settings dialog
            frmSettings newSettings = new frmSettings(AContainer);
            newSettings.ShowDialog();

            //reload app settings
            _appSettings = new ApplicationSettings().GetOrCreateApplicationSettings();

            newSettings.Dispose();

            LoadActionBarPreference();
        }

        private void showSearchBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //set to empty
            tsSearchResult.Text = "";
            tsSearchBox.Text = "";

            //show or hide
            tsSearchBox.Visible = !tsSearchBox.Visible;
            tsSearchButton.Visible = !tsSearchButton.Visible;
            tsSearchResult.Visible = !tsSearchResult.Visible;

            //update verbiage
            if (tsSearchBox.Visible)
            {
                showSearchBarToolStripMenuItem.Text = "Hide Search Bar";
            }
            else
            {
                showSearchBarToolStripMenuItem.Text = "Show Search Bar";
            }
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uiBtnClearAll_Click(sender, e);
        }

        private void uiBtnClearAll_Click(object sender, EventArgs e)
        {
            if (!(_selectedTabScriptActions is ListView))
                return;

            CreateUndoSnapshot();
            HideSearchInfo();
            _selectedTabScriptActions.Items.Clear();
        }

        private void aboutOpenBotsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout frmAboutForm = new frmAbout();
            frmAboutForm.Show();
        }

        private void packageManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsScriptRunning)
            {
                Notify("Package Manager cannot be opened while a script is running.", Color.Yellow);
                return;
            }

            string configPath = Path.Combine(ScriptProjectPath, "project.obconfig");
            frmGalleryPackageManager frmManager = new frmGalleryPackageManager(ScriptProject.Dependencies);
            frmManager.ShowDialog();

            if (frmManager.DialogResult == DialogResult.OK)
            {
                ScriptProject.Dependencies = ScriptProject.Dependencies.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                File.WriteAllText(configPath, JsonConvert.SerializeObject(ScriptProject));

                if (frmManager.ShowRestartWarning)
                {
                    var result = MessageBox.Show("OpenBots Studio must restart in order for certain changes to take effect.\n" + 
                                                 "Would you like to restart? Not doing so could cause unexpected behavior.", 
                                                 "Restart", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        _appSettings.ClientSettings.IsRestarting = true;
                        _appSettings.Save(_appSettings);
                        Application.Restart();
                        return;
                    }
                }

                NotifySync("Loading package assemblies...", Color.White);

                var assemblyList = NugetPackageManager.LoadPackageAssemblies(configPath);
                _builder = AppDomainSetupManager.LoadBuilder(assemblyList, _typeContext.GroupedTypes, _allNamespaces, _importedNamespaces);
                AContainer = _builder.Build();

                LoadCommands(this);
                ReloadAllFiles();
            }

            frmManager.Dispose();
        }

        private void uiBtnPackageManager_Click(object sender, EventArgs e)
        {
            packageManagerToolStripMenuItem_Click(sender, e);
        }

        private async void installDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string localPackagesPath = Folders.GetFolder(FolderType.LocalAppDataPackagesFolder);

                if (Directory.Exists(localPackagesPath) && Directory.GetDirectories(localPackagesPath).Length > 0)
                {
                    MessageBox.Show("Close OpenBots and delete all packages first.", "Delete Packages");
                    return;
                }

                //show spinner and disable package manager related buttons
                NotifySync("Installing and loading package assemblies...", Color.White);

                installDefaultToolStripMenuItem.Enabled = false;
                packageManagerToolStripMenuItem.Enabled = false;
                uiBtnPackageManager.Enabled = false;

                Directory.CreateDirectory(localPackagesPath);

                //require admin access to move/download packages and their dependency .nupkg files to Program Files
                await NugetPackageManager.DownloadCommandDependencyPackages();

                //unpack commands using Program Files as the source repository
                var commandVersion = Regex.Matches(Application.ProductVersion, @"\d+\.\d+\.\d+")[0].ToString();
                Dictionary<string, string> dependencies = Project.DefaultCommandGroups.ToDictionary(x => $"OpenBots.Commands.{x}", x => commandVersion);

                foreach (var dep in dependencies)
                    await NugetPackageManager.InstallPackage(dep.Key, dep.Value, new Dictionary<string, string>(), 
                        Folders.GetFolder(FolderType.ProgramFilesPackagesFolder));

                //load existing command assemblies
                string configPath = Path.Combine(ScriptProjectPath, "project.obconfig");
                var assemblyList = NugetPackageManager.LoadPackageAssemblies(configPath);
                _builder = AppDomainSetupManager.LoadBuilder(assemblyList, _typeContext.GroupedTypes, _allNamespaces, _importedNamespaces);
                AContainer = _builder.Build();

                LoadCommands(this);
                ReloadAllFiles();
            }
            catch (Exception ex)
            {
                if (ex is UnauthorizedAccessException)
                    MessageBox.Show("Close Visual Studio and run as Admin to install default packages.", "Unauthorized");
                else
                    Notify("An Error Occurred: " + ex.Message, Color.Red);
            }

            //hide spinner and enable package manager related buttons
            installDefaultToolStripMenuItem.Enabled = true;
            packageManagerToolStripMenuItem.Enabled = true;
            uiBtnPackageManager.Enabled = true;
        }
        #endregion

        #region Script Events Tool Strip and Buttons


        private void scheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmScheduleManagement scheduleManager = new frmScheduleManagement();
            scheduleManager.Show();
        }

        private void uiBtnScheduleManagement_Click(object sender, EventArgs e)
        {
            frmScheduleManagement scheduleManager = new frmScheduleManagement();
            scheduleManager.Show();
        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(_selectedTabScriptActions is ListView) || IsScriptRunning)
                return;
           
            _isDebugMode = true;
            RunOBScript();
        }

        private void uiBtnDebugScript_Click(object sender, EventArgs e)
        {
            debugToolStripMenuItem_Click(sender, e);
        }

        private void RunOBScript(int startLineNumber = 1)
        {
            if (_selectedTabScriptActions.Items.Count == 0)
            {
                Notify("You must first build the script by adding commands!", Color.Yellow);
                return;
            }

            if (ScriptFilePath == null)
            {
                Notify("You must first save your script before you can run it!", Color.Yellow);
                return;
            }

            if (!SaveAllFiles())
                return;

            Notify("Running Script...", Color.White);

            try
            {
                if (CurrentEngine != null)
                {
                    ((Form)CurrentEngine).Close();
                    ((Form)CurrentEngine).Dispose();
                    CurrentEngine = null;
                }
            }
            catch(Exception ex)
            {
                //failed to close engine form
                Console.WriteLine(ex);
            }

            GC.Collect();
            //initialize Logger
            switch (_appSettings.EngineSettings.LoggingSinkType)
            {
                case SinkType.File:
                    if (string.IsNullOrEmpty(_appSettings.EngineSettings.LoggingValue1.Trim()))
                        _appSettings.EngineSettings.LoggingValue1 = Path.Combine(Folders.GetFolder(FolderType.LogFolder), "OpenBots Engine Logs.txt");

                    EngineLogger = new Logging().CreateFileLogger(_appSettings.EngineSettings.LoggingValue1, Serilog.RollingInterval.Day,
                        _appSettings.EngineSettings.MinLogLevel);
                    break;
                case SinkType.HTTP:
                    EngineLogger = new Logging().CreateHTTPLogger(ScriptProject.ProjectName, _appSettings.EngineSettings.LoggingValue1, _appSettings.EngineSettings.MinLogLevel);
                    break;
            }

            EngineContext engineContext = new EngineContext(ScriptFilePath, ScriptProjectPath, AContainer, this, EngineLogger, null, null, null, null, null, null, startLineNumber, _isDebugMode, false);

            //initialize Engine
            CurrentEngine = new frmScriptEngine(engineContext, false, _isDebugMode);

            CurrentEngine.ScriptEngineContext.ScriptBuilder = this;
            IsScriptRunning = true;
            ((frmScriptEngine)CurrentEngine).Show();

            Notify("", Color.White);

            if (!_isDebugMode)
                FormsHelper.HideAllForms();
        }

        private void RunFromThisCommand()
        {
            if (_selectedTabScriptActions is ListView)
            {
                SaveToOpenBotsFile(false);
                var commandLineNumber = ((ScriptCommand)_selectedTabScriptActions.SelectedItems[0].Tag).LineNumber;
                RunOBScript(commandLineNumber);
            }
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {         
            if (IsScriptRunning)
                return;

            _isDebugMode = false;

            switch (_scriptFileExtension)
            {
                case ".obscript":
                    RunOBScript();
                    break;
                default:
                    if (!SaveAllFiles())
                        return;

                    if (_isMainScript)
                    {
                        try
                        {
                            //arguments and outputs not yet implemented
                            switch (_scriptFileExtension)
                            {
                                case ".py":
                                    ExecutionManager.RunPythonAutomation(ScriptFilePath, ScriptProject.ProjectArguments);
                                    break;
                                case ".tag":
                                    ExecutionManager.RunTagUIAutomation(ScriptFilePath, ScriptProjectPath, ScriptProject.ProjectArguments);
                                    break;
                                case ".cs":
                                    ExecutionManager.RunCSharpAutomation(ScriptFilePath, ScriptProject.ProjectArguments);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            frmDialog errorMessageBox = new frmDialog(ex.Message, "Error", DialogType.OkOnly, 0);
                            errorMessageBox.ShowDialog();
                            errorMessageBox.Dispose();
                        }
                    }
                    else
                    {
                        frmDialog errorMessageBox = new frmDialog("Unable to run a script that isn't 'Main'", "Error", DialogType.OkOnly, 0);
                        errorMessageBox.ShowDialog();
                        errorMessageBox.Dispose();
                    }
                    
                    break; 
            }          
        }

        private void uiBtnRunScript_Click(object sender, EventArgs e)
        {
            runToolStripMenuItem_Click(sender, e);
        }

        private void breakpointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uiBtnBreakpoint_Click(sender, e);
        }

        private void uiBtnBreakpoint_Click(object sender, EventArgs e)
        {
            AddRemoveBreakpoint();
        }
        #endregion

        #region Recorder Buttons
        private void elementRecorderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(_selectedTabScriptActions is ListView))
                return;

            frmWebElementRecorder elementRecorder = new frmWebElementRecorder(AContainer, HTMLElementRecorderURL)
            {
                CallBackForm = this,
                IsRecordingSequence = true,
                ScriptElements = _scriptElements
            };
            elementRecorder.chkStopOnClick.Visible = false;
            elementRecorder.IsCommandItemSelected = _selectedTabScriptActions.SelectedItems.Count > 0;

            CreateUndoSnapshot();

            elementRecorder.ShowDialog();

            HTMLElementRecorderURL = elementRecorder.StartURL;
            _scriptElements = elementRecorder.ScriptElements;

            elementRecorder.Dispose();
        }

        private void uiBtnRecordElementSequence_Click(object sender, EventArgs e)
        {
            elementRecorderToolStripMenuItem_Click(sender, e);
        }

        private void uiRecorderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RecordSequence();
        }

        private void uiBtnRecordUISequence_Click(object sender, EventArgs e)
        {
            RecordSequence();
        }

        private void RecordSequence()
        {
            if (!(_selectedTabScriptActions is ListView))
                return;

            Hide();
            frmScreenRecorder sequenceRecorder = new frmScreenRecorder(AContainer)
            {
                CallBackForm = this,
                IsCommandItemSelected = _selectedTabScriptActions.SelectedItems.Count > 0,               
            };

            sequenceRecorder.ShowDialog();
            sequenceRecorder.Dispose();
            uiScriptTabControl.SelectedTab.Controls.Remove(pnlCommandHelper);
            uiScriptTabControl.SelectedTab.Controls[0].Show();

            Show();
            BringToFront();
        }

        private void uiAdvancedRecorderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(_selectedTabScriptActions is ListView))
                return;

            Hide();

            frmAdvancedUIElementRecorder appElementRecorder = new frmAdvancedUIElementRecorder(AContainer)
            {
                CallBackForm = this,
                IsRecordingSequence = true
            };
            appElementRecorder.chkStopOnClick.Visible = false;
            appElementRecorder.IsCommandItemSelected = _selectedTabScriptActions.SelectedItems.Count > 0;

            CreateUndoSnapshot();

            appElementRecorder.ShowDialog();
            appElementRecorder.Dispose();

            Show();
            BringToFront();
        }

        private void uiBtnRecordAdvancedUISequence_Click(object sender, EventArgs e)
        {
            uiAdvancedRecorderToolStripMenuItem_Click(sender, e);
        }

        private void shortcutMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShortcutMenu shortcutMenuForm = new frmShortcutMenu();
            shortcutMenuForm.Show();
        }

        private void openShortcutMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            shortcutMenuToolStripMenuItem_Click(sender, e);
        }
        #endregion
        #endregion
    }
}
