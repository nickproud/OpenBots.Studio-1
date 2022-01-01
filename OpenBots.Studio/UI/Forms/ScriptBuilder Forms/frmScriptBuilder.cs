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
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.IO;
using OpenBots.Core.Project;
using OpenBots.Core.Script;
using OpenBots.Core.Settings;
using OpenBots.Core.UI.Controls;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Core.Utilities.FormsUtilities;
using OpenBots.Nuget;
using OpenBots.Studio.Utilities;
using OpenBots.UI.Forms.Supplement_Forms;
using OpenBots.Utilities;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using AContainer = Autofac.IContainer;
using CoreResources = OpenBots.Properties.Resources;
using Point = System.Drawing.Point;

namespace OpenBots.UI.Forms.ScriptBuilder_Forms
{
    public partial class frmScriptBuilder : Form, IfrmScriptBuilder
    //form tracks the overall configuration and enables script editing, saving, and running
    //features ability to add, drag/drop reorder commands
    {
        #region Instance Variables
        //engine context variables
        private ScriptContext _scriptContext;
        private List<ListViewItem> _rowsSelectedForCopy;
        private Dictionary<string, List<AssemblyReference>> _allNamespaces;
        private string _scriptFilePath;
        public string ScriptFilePath
        {
            get
            {
                return _scriptFilePath;
            }
            set
            {
                _scriptFilePath = value;
                UpdateWindowTitle();
            }
        }
        public Project ScriptProject { get; set; }
        public string ScriptProjectPath { get; private set; }
        private string _mainFileName;
        public Logger EngineLogger { get; set; }
        public IfrmScriptEngine CurrentEngine { get; set; }

        //notification variables
        private List<Tuple<string, Color>> _notificationList = new List<Tuple<string, Color>>();
        private DateTime _notificationExpires;
        private string _notificationText;
        private Color _notificationColor;
        private bool _isNotificationListEmpty;

        //debug variables
        private int _debugLine;
        public int DebugLine
        {
            get
            {
                return _debugLine;
            }
            set
            {
                _debugLine = value;
                if (_debugLine > 0)
                {
                    try
                    {
                        IsScriptRunning = true;
                        _selectedTabScriptActions.EnsureVisible(_debugLine - 1);
                    }
                    catch (Exception)
                    {
                        //log exception?
                    }
                }
                else if (_debugLine == 0)
                {
                    IsScriptRunning = false;
                    IsScriptSteppedOver = false;
                    IsScriptSteppedInto = false;
                    RemoveDebugTab();
                }

                _selectedTabScriptActions.Invalidate();

                if (IsScriptSteppedInto || IsScriptSteppedOver)
                    CreateDebugTab();
            }
        }
        private bool _isScriptRunning;
        public bool IsScriptRunning
        {
            get
            {
                return _isScriptRunning;
            }
            set
            {
                _isScriptRunning = value;
                if (_isScriptRunning)
                {
                    try
                    {
                        uiVariableArgumentTabs.Visible = false;
                        splitContainerScript.Panel2Collapsed = true;
                        tpProject.Controls[0].Enabled = false;
                        tpCommands.Controls[0].Enabled = false;
                        tlpControls.Controls[0].Enabled = false;

                        foreach (TabPage tab in uiScriptTabControl.TabPages)
                            tab.Controls[0].Enabled = false;

                        uiScriptTabControl.AllowDrop = false;
                    }
                    catch (Exception)
                    {
                        //DragDrop registration did not succeed
                    }
                }
                else
                {
                    try
                    {
                        //do after execution stops and right before the variable/argument tabs are made visible
                        if (!uiVariableArgumentTabs.Visible)
                        {
                            ResetVariableArgumentBindings();

                            if (_selectedTabScriptActions is ListView)
                                SetVarArgTabControlSettings(ProjectType.OpenBots);
                            else
                                SetVarArgTabControlSettings(ProjectType.Python);
                                                           
                            tpProject.Controls[0].Enabled = true;
                            tpCommands.Controls[0].Enabled = true;
                            tlpControls.Controls[0].Enabled = true;

                            foreach (TabPage tab in uiScriptTabControl.TabPages)
                                tab.Controls[0].Enabled = true;
                        }

                        uiVariableArgumentTabs.Visible = true;
                        uiScriptTabControl.AllowDrop = true;
                    }
                    catch (Exception)
                    {
                        //DragDrop registration did not succeed
                    }
                }                    
            }
        }
        public bool IsScriptPaused { get; set; }
        public bool IsScriptSteppedOver { get; set; }
        public bool IsScriptSteppedInto { get; set; }
        public bool IsUnhandledException { get; set; }       
        private bool _isDebugMode;
        private bool _isRunTaskCommand;

        //command search variables
        private TreeView _tvCommandsCopy;
        private string _txtCommandWatermark = "Type Here to Search";       

        //package manager variables
        public AContainer AContainer { get; private set; }
        private ContainerBuilder _builder;
        private bool _isFirstTimeSetupComplete = true;
        private bool _studioFormShownEarly = false;

        //variable/argument tab variables
        private List<string> _existingVarArgSearchList;
        private string _preEditVarArgName;
        private Type _preEditVarArgType;
        private TypeContext _typeContext;

        //other scriptbuilder form variables 
        public string HTMLElementRecorderURL { get; set; }
        private List<AutomationCommand> _automationCommands;
        private ImageList _uiImages;
        private ApplicationSettings _appSettings;
        private DateTime _lastAntiIdleEvent;
        private int _reqdIndex;
        private List<int> _matchingSearchIndex = new List<int>();
        private int _currentIndex = -1;
        private dynamic _selectedTabScriptActions;
        private Point _lastClickPosition;
        private float _slimBarHeight;
        private float _thickBarHeight;

        //hello world
        private string _helloWorldTextPython = CoreResources.DefaultPythonScript;
        private string _helloWorldTextTagUI = CoreResources.DefaultTagUIScript;
        private string _helloWorldTextCSScript = CoreResources.DefaultCSScript;
        private string _helloWorldTextPowerShell = CoreResources.DefaultPowerShellScript;
        #endregion

        #region Form Events
        public frmScriptBuilder(string projectPath)
        {
            ScriptProjectPath = projectPath;
            _scriptContext = new ScriptContext();

            _selectedTabScriptActions = NewLstScriptActions();
            InitializeComponent();

            //rendering for variable/argument tabs
            dgvVariables.AutoGenerateColumns = false;
            dgvArguments.AutoGenerateColumns = false;

            //vertical control splitter default location
            splitContainerScript.SplitterDistance = (int)(splitContainerScript.Size.Height * 0.7);
            //horizontal control splitter default location
            splitContainerStudioControls.SplitterDistance = (int)(splitContainerStudioControls.Size.Width * 0.2);

            direction.DataSource = Enum.GetValues(typeof(ScriptArgumentDirection));

            Folders.GetFolder(FolderType.LocalAppDataPackagesFolder);

            _builder = new ContainerBuilder();
            var groupedTypes = new Dictionary<string, List<Type>>();

            var defaultTypes = ScriptDefaultTypes.DefaultVarArgTypes;
            _typeContext = new TypeContext(groupedTypes, defaultTypes);
            _scriptContext.ImportedNamespaces = new Dictionary<string, List<AssemblyReference>>(ScriptDefaultNamespaces.DefaultNamespaces);
            _allNamespaces = new Dictionary<string, List<AssemblyReference>>() 
            { 
                { 
                    "System", new List<AssemblyReference>()
                    {
                        new AssemblyReference(Assembly.GetAssembly(typeof(string)).GetName().Name, Assembly.GetAssembly(typeof(string)).GetName().Version.ToString()) 
                    }
                } 
            };
        }

        private void UpdateWindowTitle()
        {
            if (ScriptProject.ProjectName != null)
                Text = $"NexBots Studio - {ScriptProject.ProjectName} - {ScriptProject.ProjectType}";
            else
                Text = "NexBots Studio";
        }

        private async void frmScriptBuilder_LoadAsync(object sender, EventArgs e)
        {
            //set controls double buffered
            foreach (Control control in Controls)
            {
                typeof(Control).InvokeMember("DoubleBuffered",
                    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                    null, control, new object[] { true });
            }

            //get app settings
            _appSettings = new ApplicationSettings().GetOrCreateApplicationSettings();
            _slimBarHeight = tlpControls.RowStyles[0].Height;
            _thickBarHeight = tlpControls.RowStyles[1].Height;

            LoadActionBarPreference();
            
            //get scripts folder
            var rpaScriptsFolder = Folders.GetFolder(FolderType.ScriptsFolder, false);

            if (!Directory.Exists(rpaScriptsFolder))
            {
                var rootFolder = Folders.GetFolder(FolderType.RootFolder);
                frmFolderDialog userDialog = new frmFolderDialog("Would you like to create a folder to save your scripts in now? " +
                    "A script folder is required to save scripts generated by this application. " +
                    $"The new script folder path would be '{Path.Combine(rootFolder, "OB Scripts")}'.", 
                    "Unable to locate OB Scripts Folder!", rootFolder);

                userDialog.ShowDialog();
                userDialog.Dispose();
            }

            //get latest files for recent files list on load
            GenerateRecentProjects();

            //set listview column size
            frmScriptBuilder_SizeChanged(null, null);
            Refresh();

            if (Debugger.IsAttached)
            {
                //set this value to 'true' to display the 'Install Default' button, and 'false' to hide it
                installDefaultToolStripMenuItem.Visible = true;
            }
            else //if OpenBots Studio is running in release mode
            {
                _isFirstTimeSetupComplete = false;
                try
                {
                    //scan whether the current user account has unpacked default commands in their local appdata                   
                    await NugetPackageManager.SetupFirstTimeUserEnvironment();
                }
                catch (Exception ex)
                {
                    //packages missing from Program Files
                    MessageBox.Show($"{ex.Message}\n\nFirst time user environment setup failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                _isFirstTimeSetupComplete = true;
            }

            var defaultTypesBinding = new BindingSource(_typeContext.DefaultTypes, null);

            variableType.DataSource = defaultTypesBinding;
            variableType.DisplayMember = "Key";
            variableType.ValueMember = "Value";

            argumentType.DataSource = defaultTypesBinding;
            argumentType.DisplayMember = "Key";
            argumentType.ValueMember = "Value";

            var importedNameSpacesBinding = new BindingSource(_scriptContext.ImportedNamespaces, null);
            lbxImportedNamespaces.DataSource = importedNameSpacesBinding;
            lbxImportedNamespaces.DisplayMember = "Key";

            var allNameSpacesBinding = new BindingSource(_allNamespaces, null);
            cbxAllNamespaces.DataSource = allNameSpacesBinding;
            cbxAllNamespaces.DisplayMember = "Key";

            if (_studioFormShownEarly)
            {
                _studioFormShownEarly = false;
                Show();
                frmScriptBuilder_Shown(null, null);
            }
        }

        private void frmScriptBuilder_Shown(object sender, EventArgs e)
        {
            if (!_isFirstTimeSetupComplete)
            {
                _studioFormShownEarly = true;
                Hide();
                return;
            }

            DialogResult result;

            if (_appSettings == null)
                _appSettings = new ApplicationSettings().GetOrCreateApplicationSettings();

            if (!_appSettings.ClientSettings.IsRestarting)
            {
                Program.SplashForm.Close();
                result = AddProject();
            }
            else
            {
                _appSettings.ClientSettings.IsRestarting = false;
                _appSettings.Save();

                frmProjectBuilder restartProjectBuilder = new frmProjectBuilder()
                {
                    ExistingProjectPath = ScriptProjectPath,
                    ExistingConfigPath = Path.Combine(ScriptProjectPath, "project.obconfig"),
                    Action = ProjectAction.OpenProject,
                    DialogResult = DialogResult.OK
                };
                result = AddProject(restartProjectBuilder);
            }

            if (result != DialogResult.Abort)
                Notify("Welcome! Select a Command to get started!", Color.White);
        }

        private void LoadActionBarPreference()
        {
            //handle action bar preference
            if (_appSettings.ClientSettings.UseSlimActionBar)
            {
                tlpControls.RowStyles[0].Height = _slimBarHeight;
                tlpControls.RowStyles[1].Height = 0;              
            }
            else
            {
                tlpControls.RowStyles[0].Height = 0;
                tlpControls.RowStyles[1].Height = _thickBarHeight;
            }

            Refresh();
        }

        private void LoadCommands()
        {
            //load all commands
            var commandClasses = TypeMethods.GenerateCommandTypes(AContainer);

            _uiImages = new ImageList();
            _automationCommands = TypeMethods.GenerateAutomationCommands(_uiImages, commandClasses);

            var groupedCommands = _automationCommands.Where(x => x.Command.CommandName != "BrokenCodeCommentCommand")
                                                                   .GroupBy(f => f.DisplayGroup);

            tvCommands.Nodes.Clear();
            foreach (var cmd in groupedCommands)
            {
                TreeNode newGroup = new TreeNode(cmd.Key);

                foreach (var subcmd in cmd)
                {
                    TreeNode subNode = new TreeNode(subcmd.ShortName);
                    subNode.ToolTipText = subcmd.Description;
                    newGroup.Nodes.Add(subNode);
                }

                tvCommands.Nodes.Add(newGroup);
            }

            tvCommands.Sort();

            _tvCommandsCopy = new TreeView();
            _tvCommandsCopy.ShowNodeToolTips = true;
            CopyTreeView(tvCommands, _tvCommandsCopy);
            txtCommandSearch.Text = _txtCommandWatermark;
        }           

        private void GenerateRecentProjects()
        {

            flwRecentFiles.Controls.Clear();

            List<string> recentlyOpenedProjectPaths = _appSettings.ClientSettings.RecentProjects;

            if (recentlyOpenedProjectPaths == null || recentlyOpenedProjectPaths.Count() == 0)
            {
                lblRecentProjects.Text = "No Recent Projects Found";
                lblRecentProjects.ForeColor = Color.White;
                lblFilesMissing.ForeColor = Color.White;
                lblFilesMissing.Show();
                flwRecentFiles.Hide();
            }
            else
            {
                foreach (string projectPath in recentlyOpenedProjectPaths)
                {
                    LinkLabel newProjectLink = new LinkLabel
                    {
                        Text = new DirectoryInfo(projectPath).Name,
                        Tag = projectPath,
                        AutoSize = true,
                        LinkColor = Color.AliceBlue,
                        Font = lnkGitIssue.Font,
                        Margin = new Padding(0, 0, 0, 0)
                    };
                    newProjectLink.LinkClicked += NewProjectLink_LinkClicked;
                    flwRecentFiles.Controls.Add(newProjectLink);
                    flwRecentFiles.Show();
                }
            }
        }
        #endregion

        #region NotifyIcon
        private void frmScriptBuilder_FormClosing(object sender, FormClosingEventArgs e)
        {
            _appSettings = _appSettings.GetOrCreateApplicationSettings();
            if (_appSettings.ClientSettings.CloseToTray)
            {
                e.Cancel = true;
                WindowState = FormWindowState.Minimized;
                ShowIcon = false;
                notifyTrayIcon.Visible = true;
                notifyTrayIcon.ShowBalloonTip(2000);
                ShowInTaskbar = false;
                return;
            }

            DialogResult result;

            result = CheckForUnsavedScripts();
            if (result == DialogResult.Cancel)
                e.Cancel = true;
        }

        private void frmScriptBuilder_SizeChanged(object sender, EventArgs e)
        {
            if (_selectedTabScriptActions is ListView)
                _selectedTabScriptActions.Columns[2].Width = Width - 340;
        }

        private void frmScriptBuilder_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyTrayIcon.Visible = false;
            notifyTrayIcon.Dispose();
            notifyTrayIcon = null;
        }

        private void frmScriptBuilder_Resize(object sender, EventArgs e)
        {
            _appSettings = new ApplicationSettings().GetOrCreateApplicationSettings();

            if (WindowState == FormWindowState.Minimized && _appSettings.ClientSettings.MinimizeToTray)
            {
                ShowIcon = false;
                notifyTrayIcon.Visible = true;
                notifyTrayIcon.ShowBalloonTip(1000);
                ShowInTaskbar = false;
            }
        }

        private void notifyTray_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = true;
                notifyTrayIcon.Visible = false;
                WindowState = FormWindowState.Normal;
            }
        }

        private void tsmiNotifyIconQuit_Click(object sender, EventArgs e)
        {
            closeApplicationToolStripMenuItem_Click(sender, e);
        }

        private void tsmiNotifyIconStudioSettings_Click(object sender, EventArgs e)
        {
            settingsToolStripMenuItem_Click(sender, e);
        }

        private void tsmiNotifyIconPackageManager_Click(object sender, EventArgs e)
        {
            packageManagerToolStripMenuItem_Click(sender, e);
        }

        private void tsmiNotifyIconRun_Click(object sender, EventArgs e)
        {
            runToolStripMenuItem_Click(sender, e);
        }
        #endregion

        #region Bottom Notification Panel
        private void tmrNotify_Tick(object sender, EventArgs e)
        {
            if (CurrentEngine == null)
                IsScriptRunning = false;
            else if (CurrentEngine != null && !CurrentEngine.EngineContext.IsChildEngine && 
                     CurrentEngine.EngineContext.CurrentEngineStatus == EngineStatus.Finished && !_isDebugMode)
            {               
                IsScriptRunning = false;
                FormsHelper.ShowAllForms(true);
                _isDebugMode = true;
            }

            if (_appSettings == null)
                return;

            if (_appSettings.ClientSettings.AntiIdleWhileOpen && DateTime.Now > _lastAntiIdleEvent.AddMinutes(1))
                PerformAntiIdle();

            //check if notification is required
            if (_notificationList.Count > 0 && _notificationExpires < DateTime.Now)
            {
                var itemToDisplay = _notificationList[0];
                _notificationList.RemoveAt(0);

                int displayTime;
                switch (itemToDisplay.Item2.Name)
                {
                    case "Transparent":
                        displayTime = 0;
                        break;
                    case "White":
                        displayTime = 1;
                        break;
                    case "Yellow":
                        displayTime = 2;
                        break;
                    case "Red":
                        displayTime = 3;
                        break;
                    default:
                        displayTime = 1;
                        break;
                }
                _notificationExpires = DateTime.Now.AddSeconds(displayTime);
                ShowNotification(itemToDisplay.Item1, itemToDisplay.Item2);
            }           
            else if (_notificationList.Count == 0 && !_isNotificationListEmpty)
            {
                pnlStatus.Invalidate();
                _isNotificationListEmpty = true;
            }
            else if (!_isNotificationListEmpty)
                pnlStatus.Invalidate();
        }

        public void Notify(string notificationText, Color notificationColor)
        {
            _isNotificationListEmpty = false;
            _notificationList.Add(new Tuple<string, Color>(notificationText, notificationColor));
        }

        public void NotifySync(string notificationText, Color notificationColor)
        {
            Notify(notificationText, notificationColor);
            tmrNotify_Tick(null, null);
            tlpControls.Refresh();
        }
        
        private void ShowNotification(string textToDisplay, Color textColor)
        {
            _notificationText = textToDisplay;
            _notificationColor = textColor;
        }
    
        private void pnlStatus_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString(_notificationText, pnlStatus.Font, new SolidBrush(_notificationColor), 30, 4);

            if (!string.IsNullOrEmpty(_notificationText))
                e.Graphics.DrawImage(CoreResources.OpenBots_icon, 5, 3, 20, 20);
        }

        private void pnlStatus_DoubleClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_notificationText))
                return;

            string caption;
            switch (_notificationColor.Name)
            {
                case "White":
                    caption = "Information";
                    break;
                case "Yellow":
                    caption = "Warning";
                    break;
                case "Red":
                    caption = "Error";
                    break;
                default:
                    caption = "Information";
                    break;
            }
            MessageBox.Show(_notificationText, caption);
        }

        private void PerformAntiIdle()
        {
            _lastAntiIdleEvent = DateTime.Now;
            Notify("Anti-Idle Triggered", Color.White);
        }

        #endregion

        #region Create Command Logic
        private void AddNewCommand(string specificCommand = "")
        {
            //bring up new command configuration form
            frmCommandEditor newCommandForm = new frmCommandEditor(_automationCommands, GetConfiguredCommands(), _typeContext)
            {
                CreationModeInstance = CreationMode.Add
            };

            if (specificCommand != "")
                newCommandForm.DefaultStartupCommand = specificCommand;

            newCommandForm.ScriptContext = _scriptContext;
            newCommandForm.AContainer = AContainer;
            newCommandForm.ProjectPath = ScriptProjectPath;
            newCommandForm.HTMLElementRecorderURL = HTMLElementRecorderURL;

            //if a command was selected
            if (newCommandForm.ShowDialog() == DialogResult.OK)
            {
                //add to listview
                CreateUndoSnapshot();
                AddCommandToListView(newCommandForm.SelectedCommand);
            }

            if (newCommandForm.SelectedCommand.CommandName == "SeleniumElementActionCommand")
            {
                CreateUndoSnapshot();
                HTMLElementRecorderURL = newCommandForm.HTMLElementRecorderURL;
            }

            ResetVariableArgumentBindings();
            newCommandForm.Dispose();
            _scriptContext.AddIntellisenseControls(Controls);
        }

        private List<ScriptCommand> GetConfiguredCommands()
        {
            List<ScriptCommand> ConfiguredCommands = new List<ScriptCommand>();
            foreach (ListViewItem item in _selectedTabScriptActions.Items)
            {
                ConfiguredCommands.Add(item.Tag as ScriptCommand);
            }
            return ConfiguredCommands;
        }
        #endregion

        #region TreeView Events
        private void tvCommands_DoubleClick(object sender, EventArgs e)
        {
            if (!(_selectedTabScriptActions is ListView))
                return;

            //handle double clicks outside
            if (tvCommands.SelectedNode == null)
                return;

            //exit if parent node is clicked
            if (tvCommands.SelectedNode.Parent == null)
                return;

            AddNewCommand(tvCommands.SelectedNode.Parent.Text + " - " + tvCommands.SelectedNode.Text);
        }

        private void tvCommands_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                tvCommands_DoubleClick(this, null);
            }
        }

        private void tvCommands_ItemDrag(object sender, ItemDragEventArgs e)
        {
            tvCommands.DoDragDrop(e.Item, DragDropEffects.Copy);
        }

        public void CopyTreeView(TreeView originalTreeView, TreeView copiedTreeView)
        {
            TreeNode copiedTreeNode;
            foreach (TreeNode originalTreeNode in originalTreeView.Nodes)
            {
                copiedTreeNode = new TreeNode(originalTreeNode.Text);
                CopyTreeViewNodes(copiedTreeNode, originalTreeNode);
                copiedTreeView.Nodes.Add(copiedTreeNode);
            }
        }

        public void CopyTreeViewNodes(TreeNode copiedTreeNode, TreeNode originalTreeNode)
        {
            TreeNode copiedChildNode;
            foreach (TreeNode originalChildNode in originalTreeNode.Nodes)
            {
                copiedChildNode = new TreeNode(originalChildNode.Text);
                copiedChildNode.ToolTipText = originalChildNode.ToolTipText;
                copiedTreeNode.Nodes.Add(copiedChildNode);
            }
        }

        private void txtCommandSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtCommandSearch.Text == _txtCommandWatermark)
                return;

            bool childNodefound = false;

            //blocks repainting tree until all controls are loaded
            tvCommands.BeginUpdate();
            tvCommands.Nodes.Clear();
            if (txtCommandSearch.Text != string.Empty)
            {
                foreach (TreeNode parentNodeCopy in _tvCommandsCopy.Nodes)
                {
                    TreeNode searchedParentNode = new TreeNode(parentNodeCopy.Text);
                    tvCommands.Nodes.Add(searchedParentNode);

                    foreach (TreeNode childNodeCopy in parentNodeCopy.Nodes)
                    {
                        if (childNodeCopy.Text.ToLower().Contains(txtCommandSearch.Text.ToLower()))
                        {
                            var searchedChildNode = new TreeNode(childNodeCopy.Text);
                            searchedChildNode.ToolTipText = childNodeCopy.ToolTipText;
                            searchedParentNode.Nodes.Add(searchedChildNode);
                            childNodefound = true;
                        }
                    }
                    if (!childNodefound && !(parentNodeCopy.Text.ToLower().Contains(txtCommandSearch.Text.ToLower())))
                    {
                        tvCommands.Nodes.Remove(searchedParentNode);
                    }
                    else if (parentNodeCopy.Text.ToLower().Contains(txtCommandSearch.Text.ToLower()))
                    {
                        searchedParentNode.Nodes.Clear();
                        foreach (TreeNode childNodeCopy in parentNodeCopy.Nodes)
                        {
                            var searchedChildNode = new TreeNode(childNodeCopy.Text);
                            searchedChildNode.ToolTipText = childNodeCopy.ToolTipText;
                            searchedParentNode.Nodes.Add(searchedChildNode);
                        }
                    }
                    childNodefound = false;
                }
                tvCommands.ExpandAll();
            }
            else
            {
                foreach (TreeNode parentNodeCopy in _tvCommandsCopy.Nodes)
                {
                    tvCommands.Nodes.Add((TreeNode)parentNodeCopy.Clone());
                }
                tvCommands.CollapseAll();
            }

            //enables redrawing tree after all controls have been added
            tvCommands.EndUpdate();
        }

        private void txtCommandSearch_Enter(object sender, EventArgs e)
        {
            if (txtCommandSearch.Text == _txtCommandWatermark)
            {
                txtCommandSearch.Text = "";
                txtCommandSearch.ForeColor = Color.Black;
            }
        }

        private void txtCommandSearch_Leave(object sender, EventArgs e)
        {
            if (txtCommandSearch.Text == "")
            {
                txtCommandSearch.Text = _txtCommandWatermark;
                txtCommandSearch.ForeColor = Color.LightGray;
            }
        }

        private void uiBtnClearCommandSearch_Click(object sender, EventArgs e)
        {
            txtCommandSearch.Clear();
        }

        private void uiBtnReloadCommands_Click(object sender, EventArgs e)
        {
            NotifySync("Loading package assemblies...", Color.White);
            string configPath = Path.Combine(ScriptProjectPath, "project.obconfig");
            var assemblyList = NugetPackageManager.LoadPackageAssemblies(configPath);
            _builder = AppDomainSetupManager.LoadBuilder(assemblyList, _typeContext.GroupedTypes, _allNamespaces, _scriptContext.ImportedNamespaces);            
            AContainer = _builder.Build();
            LoadCommands();
            ReloadAllFiles();
        }

        private void tlpCommands_EnabledChanged(object sender, EventArgs e)
        {
            ((TreeView)tlpCommands.Controls[0]).CollapseAll();
            ((TreeView)tlpCommands.Controls[0]).Nodes.Cast<TreeNode>().ToList().ForEach(n => n.BackColor = Color.FromArgb(59, 59, 59));
            ((TreeView)tlpCommands.Controls[0]).Nodes.Cast<TreeNode>().ToList().ForEach(n => n.ForeColor = Color.White);
        }

        private void tlpProject_EnabledChanged(object sender, EventArgs e)
        {
            ((TreeView)tlpProject.Controls[0]).CollapseAll();
            ((TreeView)tlpProject.Controls[0]).Nodes.Cast<TreeNode>().ToList().ForEach(n => n.BackColor = Color.FromArgb(59, 59, 59));
            ((TreeView)tlpProject.Controls[0]).Nodes.Cast<TreeNode>().ToList().ForEach(n => n.ForeColor = Color.White);
        }
        #endregion

        #region Link Labels
        private void lnkGitProject_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/OpenBotsAI/OpenBots.Studio");
        }
        private void lnkGitLatestReleases_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/OpenBotsAI/OpenBots.Studio/releases");
        }
        private void lnkGitIssue_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/OpenBotsAI/OpenBots.Studio/issues/new");
        }
        private void lnkGitWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://openbots.ai/api/execute-dll/");
        }
        private void NewProjectLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel senderLink = (LinkLabel)sender;
            if (File.Exists(Path.Combine(senderLink.Tag.ToString(), "project.obconfig")))
                OpenProject(senderLink.Tag.ToString());
            else
                Notify($"Could not find 'project.obconfig' for {senderLink.Tag}", Color.Red);
        }
        #endregion        
    }
}

