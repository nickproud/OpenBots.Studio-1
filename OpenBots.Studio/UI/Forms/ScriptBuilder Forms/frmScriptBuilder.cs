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
using Autofac;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.IO;
using OpenBots.Core.Project;
using OpenBots.Core.Script;
using OpenBots.Core.Settings;
using OpenBots.Core.UI.Controls.CustomControls;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Nuget;
using OpenBots.Studio.Utilities;
using OpenBots.UI.CustomControls.CustomUIControls;
using OpenBots.UI.Forms.Supplement_Forms;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using IContainer = Autofac.IContainer;
using Point = System.Drawing.Point;
using OpenBots.Core.Server.User;

namespace OpenBots.UI.Forms.ScriptBuilder_Forms
{
    public partial class frmScriptBuilder : Form, IfrmScriptBuilder
    //Form tracks the overall configuration and enables script editing, saving, and running
    //Features ability to add, drag/drop reorder commands
    {
        #region Instance Variables
        //engine context variables
        private List<ListViewItem> _rowsSelectedForCopy;
        private List<ScriptVariable> _scriptVariables;
        private List<ScriptArgument> _scriptArguments;
        private List<ScriptElement> _scriptElements;
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
        private frmScriptBuilder _parentBuilder;        

        //notification variables
        private List<Tuple<string, Color>> _notificationList = new List<Tuple<string, Color>>();
        private DateTime _notificationExpires;
        private bool _isDisplaying;
        private string _notificationText;
        private Color _notificationColor;
        private string _notificationPaintedText;      

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
                //FormatCommandListView();

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
                            dgvVariables.DataSource = new BindingList<ScriptVariable>(_scriptVariables);
                            dgvArguments.DataSource = new BindingList<ScriptArgument>(_scriptArguments);
                            splitContainerScript.Panel2Collapsed = false;
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
        public IContainer AContainer { get; private set; }
        private ContainerBuilder _builder;
        private string _packagesPath;

        //variable/argument tab variables
        private List<string> _existingVarArgSearchList;
        private string _preEditVarArgName;

        //other scriptbuilder form variables 
        public string HTMLElementRecorderURL { get; set; }
        private List<AutomationCommand> _automationCommands;
        private bool _editMode;
        private ImageList _uiImages;
        private ApplicationSettings _appSettings;
        private DateTime _lastAntiIdleEvent;
        private int _reqdIndex;
        private int _selectedIndex = -1;
        private List<int> _matchingSearchIndex = new List<int>();
        private int _currentIndex = -1;
        private UIListView _selectedTabScriptActions;
        private Point _lastClickPosition;
        private bool _isSequence;
        #endregion

        #region Form Events
        public frmScriptBuilder()
        {
            _selectedTabScriptActions = NewLstScriptActions();
            InitializeComponent();

            //rendering for variable/argument tabs
            dgvVariables.AutoGenerateColumns = false;
            dgvArguments.AutoGenerateColumns = false;
            direction.DataSource = Enum.GetValues(typeof(ScriptArgumentDirection));
        }

        private void UpdateWindowTitle()
        {
            if (ScriptProject.ProjectName != null)
            {
                Text = "OpenBots Studio - " + ScriptProject.ProjectName;
            }
            else
            {
                Text = "OpenBots Studio";
            }
        }

        private void frmScriptBuilder_Load(object sender, EventArgs e)
        {
            if (Debugger.IsAttached)
            {
                //Set this value to 'true' to display the 'Install Default' button, and 'false' to hide it
                installDefaultToolStripMenuItem.Visible = true;
            }
            
            string appDataPath = new DirectoryInfo(EnvironmentSettings.GetEnvironmentVariable()).Parent.FullName;
            _packagesPath = Path.Combine(appDataPath, "packages");
            if (!Directory.Exists(_packagesPath))
                Directory.CreateDirectory(_packagesPath);

            _builder = new ContainerBuilder();

            //set controls double buffered
            foreach (Control control in Controls)
            {
                typeof(Control).InvokeMember("DoubleBuffered",
                    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                    null, control, new object[] { true });
            }

            //get app settings
            _appSettings = new ApplicationSettings();
            _appSettings = _appSettings.GetOrCreateApplicationSettings();

            string clientLoggerFilePath = Path.Combine(Folders.GetFolder(FolderType.LogFolder), "OpenBots Automation Client Logs.txt");
            Logger automationClientLogger = new Logging().CreateFileLogger(clientLoggerFilePath, Serilog.RollingInterval.Day);
            //Core.Sockets.SocketClient.Initialize();
            //Core.Sockets.SocketClient.associatedBuilder = this;

            //handle action bar preference
            //hide action panel

            if (_editMode)
            {
                tlpControls.RowStyles[0].SizeType = SizeType.Absolute;
                tlpControls.RowStyles[0].Height = 0;

                tlpControls.RowStyles[1].SizeType = SizeType.Absolute;
                tlpControls.RowStyles[1].Height = 81;
            }
            else if (_appSettings.ClientSettings.UseSlimActionBar)
            {
                tlpControls.RowStyles[1].SizeType = SizeType.Absolute;
                tlpControls.RowStyles[1].Height = 0;
            }
            else
            {
                tlpControls.RowStyles[0].SizeType = SizeType.Absolute;
                tlpControls.RowStyles[0].Height = 0;
            }

            //get scripts folder
            var rpaScriptsFolder = Folders.GetFolder(FolderType.ScriptsFolder);

            if (!Directory.Exists(rpaScriptsFolder))
            {
                frmDialog userDialog = new frmDialog("Would you like to create a folder to save your scripts in now? " +
                    "A script folder is required to save scripts generated with this application. " +
                    "The new script folder path would be '" + rpaScriptsFolder + "'.", "Unable to locate Script Folder!",
                    DialogType.YesNo, 0);

                if (userDialog.ShowDialog() == DialogResult.OK)
                {
                    Directory.CreateDirectory(rpaScriptsFolder);
                }
            }

            //get latest files for recent files list on load
            GenerateRecentFiles();

            //no height for status bar
            HideNotificationRow();

            //instantiate for script variables
            if (!_editMode)
            {
                _scriptVariables = new List<ScriptVariable>();
                _scriptArguments = new List<ScriptArgument>();
                _scriptElements = new List<ScriptElement>();

                dgvVariables.DataSource = new BindingList<ScriptVariable>(_scriptVariables);
                dgvArguments.DataSource = new BindingList<ScriptArgument>(_scriptArguments);
            }

            //set image list
            _selectedTabScriptActions.SmallImageList = _uiImages;

            //set listview column size
            frmScriptBuilder_SizeChanged(null, null);
        }

        private void LoadCommands(frmScriptBuilder scriptBuilder)
        {
            //load all commands           
            scriptBuilder._automationCommands = TypeMethods.GenerateCommands(AContainer);

            //instantiate and populate display icons for commands
            scriptBuilder._uiImages = UIImage.UIImageList(scriptBuilder._automationCommands);

            var groupedCommands = _automationCommands.GroupBy(f => f.DisplayGroup);

            scriptBuilder.tvCommands.Nodes.Clear();
            foreach (var cmd in groupedCommands)
            {
                TreeNode newGroup = new TreeNode(cmd.Key);

                foreach (var subcmd in cmd)
                {
                    TreeNode subNode = new TreeNode(subcmd.ShortName);
                    subNode.ToolTipText = subcmd.Description;
                    newGroup.Nodes.Add(subNode);
                }

                scriptBuilder.tvCommands.Nodes.Add(newGroup);
            }

            scriptBuilder.tvCommands.Sort();

            scriptBuilder._tvCommandsCopy = new TreeView();
            scriptBuilder._tvCommandsCopy.ShowNodeToolTips = true;
            CopyTreeView(scriptBuilder.tvCommands, scriptBuilder._tvCommandsCopy);
            scriptBuilder.txtCommandSearch.Text = _txtCommandWatermark;
        }

        private void frmScriptBuilder_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result;

            if (_isSequence && uiScriptTabControl.TabPages[0].Text.Contains(" *") && DialogResult == DialogResult.Cancel)
            {
                result = MessageBox.Show($"Would you like to save the sequence before closing?",
                                         $"Save Sequence", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Yes)
                    DialogResult = DialogResult.OK;
                else if (result == DialogResult.Cancel)
                    e.Cancel = true;

                return;
            }
            else if (_isSequence && DialogResult == DialogResult.OK)
                return;

            result = CheckForUnsavedScripts();
            if (result == DialogResult.Cancel)
                e.Cancel = true;
        }

        private void frmScriptBuilder_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyTray.Dispose();
        }

        private void GenerateRecentFiles()
        {
            flwRecentFiles.Controls.Clear();

            var scriptPath = Folders.GetFolder(FolderType.ScriptsFolder);

            if (!Directory.Exists(scriptPath))
            {
                lblRecentFiles.Text = "Script Folder does not exist";
                lblFilesMissing.Text = "Directory Not Found: " + scriptPath;
                lblRecentFiles.ForeColor = Color.White;
                lblFilesMissing.ForeColor = Color.White;
                lblFilesMissing.Show();
                flwRecentFiles.Hide();
                return;
            }

            var directory = new DirectoryInfo(scriptPath);
            var recentFiles = directory.GetFiles()
                                       .OrderByDescending(file => file.LastWriteTime)
                                       .Select(f => f.Name);

            if (recentFiles.Count() == 0)
            {
                //Label noFilesLabel = new Label();
                //noFilesLabel.Text = "No Recent Files Found";
                //noFilesLabel.AutoSize = true;
                //noFilesLabel.ForeColor = Color.SteelBlue;
                //noFilesLabel.Font = lnkGitIssue.Font;
                //noFilesLabel.Margin = new Padding(0, 0, 0, 0);
                //flwRecentFiles.Controls.Add(noFilesLabel);
                lblRecentFiles.Text = "No Recent Files Found";
                lblRecentFiles.ForeColor = Color.White;
                lblFilesMissing.ForeColor = Color.White;
                lblFilesMissing.Show();
                flwRecentFiles.Hide();
            }
            else
            {
                foreach (var fil in recentFiles)
                {
                    if (flwRecentFiles.Controls.Count == 7)
                        return;

                    LinkLabel newFileLink = new LinkLabel
                    {
                        Text = fil,
                        AutoSize = true,
                        LinkColor = Color.AliceBlue,
                        Font = lnkGitIssue.Font,
                        Margin = new Padding(0, 0, 0, 0)
                    };
                    newFileLink.LinkClicked += NewFileLink_LinkClicked;
                    flwRecentFiles.Controls.Add(newFileLink);
                }
            }
        }

        private void frmScriptBuilder_Shown(object sender, EventArgs e)
        {
            Program.SplashForm.Close();

            if (_editMode)
                return;

            tpbLoadingSpinner.Visible = true;

            var result = AddProject();
            if (result != DialogResult.Abort)
                Notify("Welcome! Press 'Add Command' to get started!", Color.White);

            tpbLoadingSpinner.Visible = false;
        }

        private void pnlControlContainer_Paint(object sender, PaintEventArgs e)
        {
            //Rectangle rect = new Rectangle(0, 0, pnlControlContainer.Width, pnlControlContainer.Height);
            //using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.White, Color.WhiteSmoke, LinearGradientMode.Vertical))
            //{
            //    e.Graphics.FillRectangle(brush, rect);
            //}

            //Pen steelBluePen = new Pen(Color.SteelBlue, 2);
            //Pen lightSteelBluePen = new Pen(Color.LightSteelBlue, 1);
            ////e.Graphics.DrawLine(steelBluePen, 0, 0, pnlControlContainer.Width, 0);
            //e.Graphics.DrawLine(lightSteelBluePen, 0, 0, pnlControlContainer.Width, 0);
            //e.Graphics.DrawLine(lightSteelBluePen, 0, pnlControlContainer.Height - 1, pnlControlContainer.Width, pnlControlContainer.Height - 1);
        }

        private void pbMainLogo_Click(object sender, EventArgs e)
        {
            frmAbout aboutForm = new frmAbout();
            aboutForm.Show();
        }

        private void frmScriptBuilder_SizeChanged(object sender, EventArgs e)
        {
            _selectedTabScriptActions.Columns[2].Width = Width - 340;
        }

        private void frmScriptBuilder_Resize(object sender, EventArgs e)
        {
            //check when minimized
            if ((WindowState == FormWindowState.Minimized) && (_appSettings.ClientSettings.MinimizeToTray))
            {
                _appSettings = new ApplicationSettings().GetOrCreateApplicationSettings();
                if (_appSettings.ClientSettings.MinimizeToTray)
                {
                    notifyTray.Visible = true;
                    notifyTray.ShowBalloonTip(3000);
                    ShowInTaskbar = false;
                }
            }
        }
        #endregion

        #region Bottom Notification Panel
        private void tmrNotify_Tick(object sender, EventArgs e)
        {
            if (CurrentEngine == null)
            {
                //Set data just once. Right after the engine has been set to null and before IsScriptRunning is set to false;
                if (IsScriptRunning)
                {
                    //dgvVariables.DataSource = new BindingList<ScriptVariable>(_scriptVariables);
                    //dgvArguments.DataSource = new BindingList<ScriptArgument>(_scriptArguments);
                }
                IsScriptRunning = false;
            }

            if (_appSettings == null)
            {
                return;
            }

            if ((_notificationExpires < DateTime.Now) && (_isDisplaying))
            {
                HideNotification();
            }

            if ((_appSettings.ClientSettings.AntiIdleWhileOpen) && (DateTime.Now > _lastAntiIdleEvent.AddMinutes(1)))
            {
                PerformAntiIdle();
            }

            //check if notification is required
            if ((_notificationList.Count > 0) && (_notificationExpires < DateTime.Now))
            {
                var itemToDisplay = _notificationList[0];
                _notificationList.RemoveAt(0);
                _notificationExpires = DateTime.Now.AddSeconds(2);
                ShowNotification(itemToDisplay.Item1, itemToDisplay.Item2);
            }
        }

        public void Notify(string notificationText, Color notificationColor)
        {
            _notificationList.Add(new Tuple<string, Color>(notificationText, notificationColor));
        }

        private void ShowNotification(string textToDisplay, Color textColor)
        {
            _notificationText = textToDisplay;
            _notificationColor = textColor;
            //lblStatus.Left = 20;
            //lblStatus.Text = textToDisplay;

            pnlStatus.SuspendLayout();
            //for (int i = 0; i < 30; i++)
            //{
            //    tlpControls.RowStyles[1].Height = i;
            //}
            ShowNotificationRow();
            pnlStatus.ResumeLayout();
            _isDisplaying = true;
        }

        private void HideNotification()
        {
            pnlStatus.SuspendLayout();
            //for (int i = 30; i > 0; i--)
            //{
            //    tlpControls.RowStyles[1].Height = i;
            //}
            HideNotificationRow();
            pnlStatus.ResumeLayout();
            _isDisplaying = false;
        }

        private void HideNotificationRow()
        {
            tlpControls.RowStyles[4].Height = 0;
        }

        private void ShowNotificationRow()
        {
            tlpControls.RowStyles[4].Height = 30;
        }

        private void PerformAntiIdle()
        {
            _lastAntiIdleEvent = DateTime.Now;
            Notify("Anti-Idle Triggered", Color.White);
        }

        private void notifyTray_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_appSettings.ClientSettings.MinimizeToTray)
            {
                WindowState = FormWindowState.Normal;
                ShowInTaskbar = true;
                notifyTray.Visible = false;
            }
        }
        #endregion

        #region Create Command Logic
        private void AddNewCommand(string specificCommand = "")
        {
            //bring up new command configuration form
            frmCommandEditor newCommandForm = new frmCommandEditor(_automationCommands, GetConfiguredCommands())
            {
                CreationModeInstance = CreationMode.Add
            };

            if (specificCommand != "")
                newCommandForm.DefaultStartupCommand = specificCommand;

            newCommandForm.ScriptEngineContext.Variables = _scriptVariables;
            newCommandForm.ScriptEngineContext.Elements = _scriptElements;
            newCommandForm.ScriptEngineContext.Arguments = _scriptArguments;
            newCommandForm.ScriptEngineContext.Container = AContainer;

            newCommandForm.ScriptEngineContext.ProjectPath = ScriptProjectPath;
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
                _scriptElements = newCommandForm.ScriptEngineContext.Elements;
                HTMLElementRecorderURL = newCommandForm.HTMLElementRecorderURL;
            }
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
            tpbLoadingSpinner.Visible = true;

            string configPath = Path.Combine(ScriptProjectPath, "project.config");
            var assemblyList = NugetPackageManager.LoadPackageAssemblies(configPath);
            _builder = AppDomainSetupManager.LoadBuilder(assemblyList);
            AContainer = _builder.Build();
            LoadCommands(this);
            ReloadAllFiles();

            tpbLoadingSpinner.Visible = false;
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
        private void NewFileLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel senderLink = (LinkLabel)sender;
            OpenFile(Path.Combine(Folders.GetFolder(FolderType.ScriptsFolder), senderLink.Text));
        }

        #endregion
    }
}

