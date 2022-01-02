﻿using OpenBots.Core.Script;
using OpenBots.UI.CustomControls.CustomUIControls;
using System;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.ScriptBuilder_Forms
{
    partial class frmScriptBuilder
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScriptBuilder));
            this.cmsProjectFolderActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCopyFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDeleteFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNewFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNewScriptFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPasteFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRenameFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMainNewFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMainNewScriptFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMainPasteFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrNotify = new System.Windows.Forms.Timer(this.components);
            this.cmsScriptActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.enableSelectedCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableSelectedCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addRemoveBreakpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutSelectedCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copySelectedCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteSelectedCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSelectedCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openShortcutMenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runFromThisCommandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugFromThisCommandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsNotifyTrayIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiNotifyIconStudioSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNotifyIconPackageManager = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNotifyIconRun = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNotifyIconQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlControlContainer = new System.Windows.Forms.Panel();
            this.grpSearch = new OpenBots.UI.CustomControls.CustomUIControls.UIGroupBox();
            this.pbSearch = new System.Windows.Forms.PictureBox();
            this.lblCurrentlyViewing = new System.Windows.Forms.Label();
            this.lblTotalResults = new System.Windows.Forms.Label();
            this.txtScriptSearch = new System.Windows.Forms.TextBox();
            this.grpSaveClose = new OpenBots.UI.CustomControls.CustomUIControls.UIGroupBox();
            this.uiBtnRestart = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnClose = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.grpFileActions = new OpenBots.UI.CustomControls.CustomUIControls.UIGroupBox();
            this.uiBtnPackageManager = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnSaveAll = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnPublishProject = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnProject = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnImport = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnSaveAs = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnSave = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnNew = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnOpen = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.grpRecordRun = new OpenBots.UI.CustomControls.CustomUIControls.UIGroupBox();
            this.uiBtnBreakpoint = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnRecordAdvancedUISequence = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnRecordElementSequence = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnRunScript = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnRecordUISequence = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnDebugScript = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnScheduleManagement = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.grpVariable = new OpenBots.UI.CustomControls.CustomUIControls.UIGroupBox();
            this.uiBtnExtensionsManager = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnAddArgument = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnAddElement = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnClearAll = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnSettings = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnAddVariable = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.splitContainerStudioControls = new OpenBots.UI.CustomControls.CustomUIControls.UISplitContainer();
            this.uiPaneTabs = new System.Windows.Forms.TabControl();
            this.tpProject = new System.Windows.Forms.TabPage();
            this.tlpProject = new System.Windows.Forms.TableLayoutPanel();
            this.tvProject = new OpenBots.UI.CustomControls.CustomUIControls.UITreeView();
            this.imgListProjectPane = new System.Windows.Forms.ImageList(this.components);
            this.pnlProjectButtons = new System.Windows.Forms.Panel();
            this.uiBtnCollapse = new OpenBots.UI.CustomControls.CustomUIControls.UIIconButton();
            this.uiBtnOpenDirectory = new OpenBots.UI.CustomControls.CustomUIControls.UIIconButton();
            this.uiBtnExpand = new OpenBots.UI.CustomControls.CustomUIControls.UIIconButton();
            this.uiBtnRefresh = new OpenBots.UI.CustomControls.CustomUIControls.UIIconButton();
            this.tpCommands = new System.Windows.Forms.TabPage();
            this.tlpCommands = new System.Windows.Forms.TableLayoutPanel();
            this.tvCommands = new OpenBots.UI.CustomControls.CustomUIControls.UITreeView();
            this.pnlCommandSearch = new System.Windows.Forms.Panel();
            this.uiBtnReloadCommands = new OpenBots.UI.CustomControls.CustomUIControls.UIIconButton();
            this.uiBtnClearCommandSearch = new OpenBots.UI.CustomControls.CustomUIControls.UIIconButton();
            this.txtCommandSearch = new System.Windows.Forms.TextBox();
            this.splitContainerScript = new OpenBots.UI.CustomControls.CustomUIControls.UISplitContainer();
            this.uiScriptTabControl = new OpenBots.UI.CustomControls.CustomUIControls.UITabControl();
            this.uiVariableArgumentTabs = new OpenBots.UI.CustomControls.CustomUIControls.UITabControl();
            this.variables = new System.Windows.Forms.TabPage();
            this.dgvVariables = new OpenBots.UI.CustomControls.CustomUIControls.UIDataGridView();
            this.variableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.variableType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.variableValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VariableEditor = new System.Windows.Forms.DataGridViewButtonColumn();
            this.arguments = new System.Windows.Forms.TabPage();
            this.dgvArguments = new OpenBots.UI.CustomControls.CustomUIControls.UIDataGridView();
            this.argumentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.argumentType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.argumentValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ArgumentEditor = new System.Windows.Forms.DataGridViewButtonColumn();
            this.direction = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.imports = new System.Windows.Forms.TabPage();
            this.tlpImports = new System.Windows.Forms.TableLayoutPanel();
            this.cbxAllNamespaces = new System.Windows.Forms.ComboBox();
            this.lbxImportedNamespaces = new System.Windows.Forms.ListBox();
            this.pnlCommandHelper = new System.Windows.Forms.Panel();
            this.flwRecentFiles = new OpenBots.UI.CustomControls.CustomUIControls.UIFlowLayoutPanel();
            this.lblFilesMissing = new System.Windows.Forms.Label();
            this.pbRecentFiles = new System.Windows.Forms.PictureBox();
            this.pbLinks = new System.Windows.Forms.PictureBox();
            this.pbOpenBotsIcon = new System.Windows.Forms.PictureBox();
            this.lblRecentProjects = new System.Windows.Forms.Label();
            this.lnkGitWiki = new System.Windows.Forms.LinkLabel();
            this.lnkGitIssue = new System.Windows.Forms.LinkLabel();
            this.lnkGitLatestReleases = new System.Windows.Forms.LinkLabel();
            this.lnkGitProject = new System.Windows.Forms.LinkLabel();
            this.lblWelcomeToOpenBots = new System.Windows.Forms.Label();
            this.lblWelcomeDescription = new System.Windows.Forms.Label();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmCommand = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlDivider = new System.Windows.Forms.Panel();
            this.msOpenBotsMenu = new OpenBots.UI.CustomControls.CustomUIControls.UIMenuStrip();
            this.fileActionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.publishProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.variablesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.argumentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.elementManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showSearchBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutOpenBotsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extensionManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptActionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scheduleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shortcutMenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recorderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.elementRecorderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uiRecorderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uiAdvancedRecorderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.breakpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.packageManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.installDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSearchBox = new System.Windows.Forms.ToolStripTextBox();
            this.tsSearchButton = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSearchResult = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpControls = new System.Windows.Forms.TableLayoutPanel();
            this.cmsProjectFileActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCopyFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDeleteFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRenameFile = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsScriptTabActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCloseTab = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCloseAllButThis = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReloadTab = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadAllTabsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsProjectMainFolderActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiMainRenameFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.ttScriptBuilder = new System.Windows.Forms.ToolTip(this.components);
            this.imgListIntellisense = new System.Windows.Forms.ImageList(this.components);
            this.cmsProjectFolderActions.SuspendLayout();
            this.cmsScriptActions.SuspendLayout();
            this.cmsNotifyTrayIcon.SuspendLayout();
            this.pnlControlContainer.SuspendLayout();
            this.grpSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSearch)).BeginInit();
            this.grpSaveClose.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRestart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClose)).BeginInit();
            this.grpFileActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnPackageManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSaveAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnPublishProject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnProject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSaveAs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnNew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpen)).BeginInit();
            this.grpRecordRun.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnBreakpoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRecordAdvancedUISequence)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRecordElementSequence)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRunScript)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRecordUISequence)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnDebugScript)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnScheduleManagement)).BeginInit();
            this.grpVariable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnExtensionsManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAddArgument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAddElement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClearAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAddVariable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerStudioControls)).BeginInit();
            this.splitContainerStudioControls.Panel1.SuspendLayout();
            this.splitContainerStudioControls.Panel2.SuspendLayout();
            this.splitContainerStudioControls.SuspendLayout();
            this.uiPaneTabs.SuspendLayout();
            this.tpProject.SuspendLayout();
            this.tlpProject.SuspendLayout();
            this.pnlProjectButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCollapse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpenDirectory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnExpand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRefresh)).BeginInit();
            this.tpCommands.SuspendLayout();
            this.tlpCommands.SuspendLayout();
            this.pnlCommandSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnReloadCommands)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClearCommandSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerScript)).BeginInit();
            this.splitContainerScript.Panel1.SuspendLayout();
            this.splitContainerScript.Panel2.SuspendLayout();
            this.splitContainerScript.SuspendLayout();
            this.uiVariableArgumentTabs.SuspendLayout();
            this.variables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariables)).BeginInit();
            this.arguments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArguments)).BeginInit();
            this.imports.SuspendLayout();
            this.tlpImports.SuspendLayout();
            this.pnlCommandHelper.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRecentFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLinks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOpenBotsIcon)).BeginInit();
            this.msOpenBotsMenu.SuspendLayout();
            this.tlpControls.SuspendLayout();
            this.cmsProjectFileActions.SuspendLayout();
            this.cmsScriptTabActions.SuspendLayout();
            this.cmsProjectMainFolderActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmsProjectFolderActions
            // 
            this.cmsProjectFolderActions.AllowDrop = true;
            this.cmsProjectFolderActions.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.cmsProjectFolderActions.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsProjectFolderActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCopyFolder,
            this.tsmiDeleteFolder,
            this.tsmiNewFolder,
            this.tsmiNewScriptFile,
            this.tsmiPasteFolder,
            this.tsmiRenameFolder});
            this.cmsProjectFolderActions.Name = "cmsProjectActions";
            this.cmsProjectFolderActions.Size = new System.Drawing.Size(179, 160);
            // 
            // tsmiCopyFolder
            // 
            this.tsmiCopyFolder.Image = global::OpenBots.Properties.Resources.copy;
            this.tsmiCopyFolder.Name = "tsmiCopyFolder";
            this.tsmiCopyFolder.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.tsmiCopyFolder.Size = new System.Drawing.Size(178, 26);
            this.tsmiCopyFolder.Text = "Copy";
            this.tsmiCopyFolder.Click += new System.EventHandler(this.tsmiCopyFolder_Click);
            // 
            // tsmiDeleteFolder
            // 
            this.tsmiDeleteFolder.Image = global::OpenBots.Properties.Resources.delete;
            this.tsmiDeleteFolder.Name = "tsmiDeleteFolder";
            this.tsmiDeleteFolder.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.tsmiDeleteFolder.Size = new System.Drawing.Size(178, 26);
            this.tsmiDeleteFolder.Text = "Delete";
            this.tsmiDeleteFolder.Click += new System.EventHandler(this.tsmiDeleteFolder_Click);
            // 
            // tsmiNewFolder
            // 
            this.tsmiNewFolder.Image = global::OpenBots.Properties.Resources.ProjectFolderIcon;
            this.tsmiNewFolder.Name = "tsmiNewFolder";
            this.tsmiNewFolder.Size = new System.Drawing.Size(178, 26);
            this.tsmiNewFolder.Text = "New Folder";
            this.tsmiNewFolder.Click += new System.EventHandler(this.tsmiNewFolder_Click);
            // 
            // tsmiNewScriptFile
            // 
            this.tsmiNewScriptFile.Image = global::OpenBots.Properties.Resources.OpenBots_icon;
            this.tsmiNewScriptFile.Name = "tsmiNewScriptFile";
            this.tsmiNewScriptFile.Size = new System.Drawing.Size(178, 26);
            this.tsmiNewScriptFile.Text = "New Script File";
            this.tsmiNewScriptFile.Click += new System.EventHandler(this.tsmiNewScriptFile_Click);
            // 
            // tsmiPasteFolder
            // 
            this.tsmiPasteFolder.Image = global::OpenBots.Properties.Resources.paste;
            this.tsmiPasteFolder.Name = "tsmiPasteFolder";
            this.tsmiPasteFolder.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.tsmiPasteFolder.Size = new System.Drawing.Size(178, 26);
            this.tsmiPasteFolder.Text = "Paste";
            this.tsmiPasteFolder.Click += new System.EventHandler(this.tsmiPasteFolder_Click);
            // 
            // tsmiRenameFolder
            // 
            this.tsmiRenameFolder.Image = global::OpenBots.Properties.Resources.create;
            this.tsmiRenameFolder.Name = "tsmiRenameFolder";
            this.tsmiRenameFolder.Size = new System.Drawing.Size(178, 26);
            this.tsmiRenameFolder.Text = "Rename";
            this.tsmiRenameFolder.Click += new System.EventHandler(this.tsmiRenameFolder_Click);
            // 
            // tsmiMainNewFolder
            // 
            this.tsmiMainNewFolder.Image = global::OpenBots.Properties.Resources.ProjectFolderIcon;
            this.tsmiMainNewFolder.Name = "tsmiMainNewFolder";
            this.tsmiMainNewFolder.Size = new System.Drawing.Size(180, 26);
            this.tsmiMainNewFolder.Text = "New Folder";
            this.tsmiMainNewFolder.Click += new System.EventHandler(this.tsmiNewFolder_Click);
            // 
            // tsmiMainNewScriptFile
            // 
            this.tsmiMainNewScriptFile.Image = global::OpenBots.Properties.Resources.OpenBots_icon;
            this.tsmiMainNewScriptFile.Name = "tsmiMainNewScriptFile";
            this.tsmiMainNewScriptFile.Size = new System.Drawing.Size(180, 26);
            this.tsmiMainNewScriptFile.Text = "New Script File";
            this.tsmiMainNewScriptFile.Click += new System.EventHandler(this.tsmiNewScriptFile_Click);
            // 
            // tsmiMainPasteFolder
            // 
            this.tsmiMainPasteFolder.Image = global::OpenBots.Properties.Resources.paste;
            this.tsmiMainPasteFolder.Name = "tsmiMainPasteFolder";
            this.tsmiMainPasteFolder.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.tsmiMainPasteFolder.Size = new System.Drawing.Size(180, 26);
            this.tsmiMainPasteFolder.Text = "Paste";
            this.tsmiMainPasteFolder.Click += new System.EventHandler(this.tsmiPasteFolder_Click);
            // 
            // tmrNotify
            // 
            this.tmrNotify.Enabled = true;
            this.tmrNotify.Interval = 500;
            this.tmrNotify.Tick += new System.EventHandler(this.tmrNotify_Tick);
            // 
            // cmsScriptActions
            // 
            this.cmsScriptActions.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.cmsScriptActions.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.cmsScriptActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableSelectedCodeToolStripMenuItem,
            this.disableSelectedCodeToolStripMenuItem,
            this.addRemoveBreakpointToolStripMenuItem,
            this.cutSelectedCodeToolStripMenuItem,
            this.copySelectedCodeToolStripMenuItem,
            this.pasteSelectedCodeToolStripMenuItem,
            this.deleteSelectedCodeToolStripMenuItem,
            this.viewCodeToolStripMenuItem,
            this.openShortcutMenuToolStripMenuItem,
            this.runFromThisCommandToolStripMenuItem,
            this.debugFromThisCommandToolStripMenuItem});
            this.cmsScriptActions.Name = "cmsScriptActions";
            this.cmsScriptActions.Size = new System.Drawing.Size(281, 268);
            // 
            // enableSelectedCodeToolStripMenuItem
            // 
            this.enableSelectedCodeToolStripMenuItem.Name = "enableSelectedCodeToolStripMenuItem";
            this.enableSelectedCodeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.enableSelectedCodeToolStripMenuItem.Size = new System.Drawing.Size(280, 24);
            this.enableSelectedCodeToolStripMenuItem.Text = "Enable Selected Code";
            this.enableSelectedCodeToolStripMenuItem.Click += new System.EventHandler(this.enableSelectedCodeToolStripMenuItem_Click);
            // 
            // disableSelectedCodeToolStripMenuItem
            // 
            this.disableSelectedCodeToolStripMenuItem.Name = "disableSelectedCodeToolStripMenuItem";
            this.disableSelectedCodeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.disableSelectedCodeToolStripMenuItem.Size = new System.Drawing.Size(280, 24);
            this.disableSelectedCodeToolStripMenuItem.Text = "Disable Selected Code";
            this.disableSelectedCodeToolStripMenuItem.Click += new System.EventHandler(this.disableSelectedCodeToolStripMenuItem_Click);
            // 
            // addRemoveBreakpointToolStripMenuItem
            // 
            this.addRemoveBreakpointToolStripMenuItem.Name = "addRemoveBreakpointToolStripMenuItem";
            this.addRemoveBreakpointToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.addRemoveBreakpointToolStripMenuItem.Size = new System.Drawing.Size(280, 24);
            this.addRemoveBreakpointToolStripMenuItem.Text = "Add/Remove Breakpoint";
            this.addRemoveBreakpointToolStripMenuItem.Click += new System.EventHandler(this.addRemoveBreakpointToolStripMenuItem_Click);
            // 
            // cutSelectedCodeToolStripMenuItem
            // 
            this.cutSelectedCodeToolStripMenuItem.Name = "cutSelectedCodeToolStripMenuItem";
            this.cutSelectedCodeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutSelectedCodeToolStripMenuItem.Size = new System.Drawing.Size(280, 24);
            this.cutSelectedCodeToolStripMenuItem.Text = "Cut Selected Code";
            this.cutSelectedCodeToolStripMenuItem.Click += new System.EventHandler(this.cutSelectedCodeToolStripMenuItem_Click);
            // 
            // copySelectedCodeToolStripMenuItem
            // 
            this.copySelectedCodeToolStripMenuItem.Name = "copySelectedCodeToolStripMenuItem";
            this.copySelectedCodeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copySelectedCodeToolStripMenuItem.Size = new System.Drawing.Size(280, 24);
            this.copySelectedCodeToolStripMenuItem.Text = "Copy Selected Code";
            this.copySelectedCodeToolStripMenuItem.Click += new System.EventHandler(this.copySelectedCodeToolStripMenuItem_Click);
            // 
            // pasteSelectedCodeToolStripMenuItem
            // 
            this.pasteSelectedCodeToolStripMenuItem.Name = "pasteSelectedCodeToolStripMenuItem";
            this.pasteSelectedCodeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteSelectedCodeToolStripMenuItem.Size = new System.Drawing.Size(280, 24);
            this.pasteSelectedCodeToolStripMenuItem.Text = "Paste Selected Code";
            this.pasteSelectedCodeToolStripMenuItem.Click += new System.EventHandler(this.pasteSelectedCodeToolStripMenuItem_Click);
            // 
            // deleteSelectedCodeToolStripMenuItem
            // 
            this.deleteSelectedCodeToolStripMenuItem.Name = "deleteSelectedCodeToolStripMenuItem";
            this.deleteSelectedCodeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteSelectedCodeToolStripMenuItem.Size = new System.Drawing.Size(280, 24);
            this.deleteSelectedCodeToolStripMenuItem.Text = "Delete Selected Code";
            this.deleteSelectedCodeToolStripMenuItem.Click += new System.EventHandler(this.deleteSelectedCodeToolStripMenuItem_Click);
            // 
            // viewCodeToolStripMenuItem
            // 
            this.viewCodeToolStripMenuItem.Name = "viewCodeToolStripMenuItem";
            this.viewCodeToolStripMenuItem.Size = new System.Drawing.Size(280, 24);
            this.viewCodeToolStripMenuItem.Text = "View Code";
            this.viewCodeToolStripMenuItem.Click += new System.EventHandler(this.viewCodeToolStripMenuItem_Click);
            // 
            // openShortcutMenuToolStripMenuItem
            // 
            this.openShortcutMenuToolStripMenuItem.Name = "openShortcutMenuToolStripMenuItem";
            this.openShortcutMenuToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.openShortcutMenuToolStripMenuItem.Size = new System.Drawing.Size(280, 24);
            this.openShortcutMenuToolStripMenuItem.Text = "Open Shortcut Menu";
            this.openShortcutMenuToolStripMenuItem.Click += new System.EventHandler(this.openShortcutMenuToolStripMenuItem_Click);
            // 
            // runFromThisCommandToolStripMenuItem
            // 
            this.runFromThisCommandToolStripMenuItem.Name = "runFromThisCommandToolStripMenuItem";
            this.runFromThisCommandToolStripMenuItem.Size = new System.Drawing.Size(280, 24);
            this.runFromThisCommandToolStripMenuItem.Text = "Run From This Command";
            this.runFromThisCommandToolStripMenuItem.Click += new System.EventHandler(this.runFromThisCommandToolStripMenuItem_Click);
            // 
            // debugFromThisCommandToolStripMenuItem
            // 
            this.debugFromThisCommandToolStripMenuItem.Name = "debugFromThisCommandToolStripMenuItem";
            this.debugFromThisCommandToolStripMenuItem.Size = new System.Drawing.Size(280, 24);
            this.debugFromThisCommandToolStripMenuItem.Text = "Debug From This Command";
            this.debugFromThisCommandToolStripMenuItem.Click += new System.EventHandler(this.debugFromThisCommandToolStripMenuItem_Click);
            // 
            // notifyTrayIcon
            // 
            this.notifyTrayIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyTrayIcon.BalloonTipText = "OpenBots Studio is still running in your system tray. Double-click to restore Ope" +
    "nBots Studio to full size!\r\n";
            this.notifyTrayIcon.ContextMenuStrip = this.cmsNotifyTrayIcon;
            this.notifyTrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyTrayIcon.Icon")));
            this.notifyTrayIcon.Text = "OpenBots Studio";
            this.notifyTrayIcon.Visible = true;
            this.notifyTrayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyTray_MouseDoubleClick);
            // 
            // cmsNotifyTrayIcon
            // 
            this.cmsNotifyTrayIcon.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsNotifyTrayIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNotifyIconStudioSettings,
            this.tsmiNotifyIconPackageManager,
            this.tsmiNotifyIconRun,
            this.tsmiNotifyIconQuit});
            this.cmsNotifyTrayIcon.Name = "cmsNotifyTrayIcon";
            this.cmsNotifyTrayIcon.Size = new System.Drawing.Size(169, 92);
            // 
            // tsmiNotifyIconStudioSettings
            // 
            this.tsmiNotifyIconStudioSettings.Name = "tsmiNotifyIconStudioSettings";
            this.tsmiNotifyIconStudioSettings.Size = new System.Drawing.Size(168, 22);
            this.tsmiNotifyIconStudioSettings.Text = "Studio Settings";
            this.tsmiNotifyIconStudioSettings.Click += new System.EventHandler(this.tsmiNotifyIconStudioSettings_Click);
            // 
            // tsmiNotifyIconPackageManager
            // 
            this.tsmiNotifyIconPackageManager.Name = "tsmiNotifyIconPackageManager";
            this.tsmiNotifyIconPackageManager.Size = new System.Drawing.Size(168, 22);
            this.tsmiNotifyIconPackageManager.Text = "Package Manager";
            this.tsmiNotifyIconPackageManager.Click += new System.EventHandler(this.tsmiNotifyIconPackageManager_Click);
            // 
            // tsmiNotifyIconRun
            // 
            this.tsmiNotifyIconRun.Name = "tsmiNotifyIconRun";
            this.tsmiNotifyIconRun.Size = new System.Drawing.Size(168, 22);
            this.tsmiNotifyIconRun.Text = "Run";
            this.tsmiNotifyIconRun.Click += new System.EventHandler(this.tsmiNotifyIconRun_Click);
            // 
            // tsmiNotifyIconQuit
            // 
            this.tsmiNotifyIconQuit.Name = "tsmiNotifyIconQuit";
            this.tsmiNotifyIconQuit.Size = new System.Drawing.Size(168, 22);
            this.tsmiNotifyIconQuit.Text = "Quit";
            this.tsmiNotifyIconQuit.Click += new System.EventHandler(this.tsmiNotifyIconQuit_Click);
            // 
            // pnlControlContainer
            // 
            this.pnlControlContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.tlpControls.SetColumnSpan(this.pnlControlContainer, 3);
            this.pnlControlContainer.Controls.Add(this.grpSearch);
            this.pnlControlContainer.Controls.Add(this.grpSaveClose);
            this.pnlControlContainer.Controls.Add(this.grpFileActions);
            this.pnlControlContainer.Controls.Add(this.grpRecordRun);
            this.pnlControlContainer.Controls.Add(this.grpVariable);
            this.pnlControlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlControlContainer.Location = new System.Drawing.Point(0, 30);
            this.pnlControlContainer.Margin = new System.Windows.Forms.Padding(0);
            this.pnlControlContainer.Name = "pnlControlContainer";
            this.pnlControlContainer.Size = new System.Drawing.Size(1417, 78);
            this.pnlControlContainer.TabIndex = 7;
            // 
            // grpSearch
            // 
            this.grpSearch.BackColor = System.Drawing.Color.Transparent;
            this.grpSearch.Controls.Add(this.pbSearch);
            this.grpSearch.Controls.Add(this.lblCurrentlyViewing);
            this.grpSearch.Controls.Add(this.lblTotalResults);
            this.grpSearch.Controls.Add(this.txtScriptSearch);
            this.grpSearch.Location = new System.Drawing.Point(1116, 0);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.Size = new System.Drawing.Size(192, 71);
            this.grpSearch.TabIndex = 20;
            this.grpSearch.TabStop = false;
            this.grpSearch.Text = "Search";
            this.grpSearch.TitleBackColor = System.Drawing.Color.Transparent;
            this.grpSearch.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSearch.TitleForeColor = System.Drawing.Color.GhostWhite;
            this.grpSearch.TitleHatchStyle = System.Drawing.Drawing2D.HatchStyle.Horizontal;
            // 
            // pbSearch
            // 
            this.pbSearch.Image = global::OpenBots.Properties.Resources.action_bar_search;
            this.pbSearch.Location = new System.Drawing.Point(158, 19);
            this.pbSearch.Name = "pbSearch";
            this.pbSearch.Size = new System.Drawing.Size(24, 22);
            this.pbSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbSearch.TabIndex = 17;
            this.pbSearch.TabStop = false;
            this.pbSearch.Click += new System.EventHandler(this.pbSearch_Click);
            this.pbSearch.MouseEnter += new System.EventHandler(this.pbSearch_MouseEnter);
            this.pbSearch.MouseLeave += new System.EventHandler(this.pbSearch_MouseLeave);
            // 
            // lblCurrentlyViewing
            // 
            this.lblCurrentlyViewing.AutoSize = true;
            this.lblCurrentlyViewing.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentlyViewing.ForeColor = System.Drawing.Color.DimGray;
            this.lblCurrentlyViewing.Location = new System.Drawing.Point(5, 59);
            this.lblCurrentlyViewing.Name = "lblCurrentlyViewing";
            this.lblCurrentlyViewing.Size = new System.Drawing.Size(102, 13);
            this.lblCurrentlyViewing.TabIndex = 3;
            this.lblCurrentlyViewing.Text = "Viewing Result X/Y";
            this.lblCurrentlyViewing.Visible = false;
            // 
            // lblTotalResults
            // 
            this.lblTotalResults.AutoSize = true;
            this.lblTotalResults.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalResults.ForeColor = System.Drawing.Color.DimGray;
            this.lblTotalResults.Location = new System.Drawing.Point(5, 45);
            this.lblTotalResults.Name = "lblTotalResults";
            this.lblTotalResults.Size = new System.Drawing.Size(118, 13);
            this.lblTotalResults.TabIndex = 2;
            this.lblTotalResults.Text = "X Total Results Found";
            this.lblTotalResults.Visible = false;
            // 
            // txtScriptSearch
            // 
            this.txtScriptSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtScriptSearch.Location = new System.Drawing.Point(5, 19);
            this.txtScriptSearch.Name = "txtScriptSearch";
            this.txtScriptSearch.Size = new System.Drawing.Size(151, 23);
            this.txtScriptSearch.TabIndex = 0;
            this.txtScriptSearch.TextChanged += new System.EventHandler(this.txtScriptSearch_TextChanged);
            // 
            // grpSaveClose
            // 
            this.grpSaveClose.BackColor = System.Drawing.Color.Transparent;
            this.grpSaveClose.Controls.Add(this.uiBtnRestart);
            this.grpSaveClose.Controls.Add(this.uiBtnClose);
            this.grpSaveClose.Location = new System.Drawing.Point(1310, 0);
            this.grpSaveClose.Name = "grpSaveClose";
            this.grpSaveClose.Size = new System.Drawing.Size(102, 71);
            this.grpSaveClose.TabIndex = 19;
            this.grpSaveClose.TabStop = false;
            this.grpSaveClose.Text = "Save and Close";
            this.grpSaveClose.TitleBackColor = System.Drawing.Color.Transparent;
            this.grpSaveClose.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSaveClose.TitleForeColor = System.Drawing.Color.GhostWhite;
            this.grpSaveClose.TitleHatchStyle = System.Drawing.Drawing2D.HatchStyle.Horizontal;
            // 
            // uiBtnRestart
            // 
            this.uiBtnRestart.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnRestart.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnRestart.DisplayText = "Restart";
            this.uiBtnRestart.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnRestart.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnRestart.Image = global::OpenBots.Properties.Resources.action_bar_restart;
            this.uiBtnRestart.IsMouseOver = false;
            this.uiBtnRestart.Location = new System.Drawing.Point(0, 19);
            this.uiBtnRestart.Name = "uiBtnRestart";
            this.uiBtnRestart.Size = new System.Drawing.Size(48, 50);
            this.uiBtnRestart.TabIndex = 19;
            this.uiBtnRestart.TabStop = false;
            this.uiBtnRestart.Text = "Restart";
            this.uiBtnRestart.Click += new System.EventHandler(this.uiBtnRestart_Click);
            // 
            // uiBtnClose
            // 
            this.uiBtnClose.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnClose.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnClose.DisplayText = "Close";
            this.uiBtnClose.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnClose.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnClose.Image = global::OpenBots.Properties.Resources.action_bar_close;
            this.uiBtnClose.IsMouseOver = false;
            this.uiBtnClose.Location = new System.Drawing.Point(48, 19);
            this.uiBtnClose.Name = "uiBtnClose";
            this.uiBtnClose.Size = new System.Drawing.Size(48, 50);
            this.uiBtnClose.TabIndex = 13;
            this.uiBtnClose.TabStop = false;
            this.uiBtnClose.Text = "Close";
            this.uiBtnClose.Click += new System.EventHandler(this.uiBtnClose_Click);
            // 
            // grpFileActions
            // 
            this.grpFileActions.BackColor = System.Drawing.Color.Transparent;
            this.grpFileActions.Controls.Add(this.uiBtnPackageManager);
            this.grpFileActions.Controls.Add(this.uiBtnSaveAll);
            this.grpFileActions.Controls.Add(this.uiBtnPublishProject);
            this.grpFileActions.Controls.Add(this.uiBtnProject);
            this.grpFileActions.Controls.Add(this.uiBtnImport);
            this.grpFileActions.Controls.Add(this.uiBtnSaveAs);
            this.grpFileActions.Controls.Add(this.uiBtnSave);
            this.grpFileActions.Controls.Add(this.uiBtnNew);
            this.grpFileActions.Controls.Add(this.uiBtnOpen);
            this.grpFileActions.Location = new System.Drawing.Point(9, 0);
            this.grpFileActions.Name = "grpFileActions";
            this.grpFileActions.Size = new System.Drawing.Size(442, 71);
            this.grpFileActions.TabIndex = 16;
            this.grpFileActions.TabStop = false;
            this.grpFileActions.Text = "File Actions";
            this.grpFileActions.TitleBackColor = System.Drawing.Color.Transparent;
            this.grpFileActions.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpFileActions.TitleForeColor = System.Drawing.Color.GhostWhite;
            this.grpFileActions.TitleHatchStyle = System.Drawing.Drawing2D.HatchStyle.Horizontal;
            // 
            // uiBtnPackageManager
            // 
            this.uiBtnPackageManager.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnPackageManager.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnPackageManager.DisplayText = "Packages";
            this.uiBtnPackageManager.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnPackageManager.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnPackageManager.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnPackageManager.Image")));
            this.uiBtnPackageManager.IsMouseOver = false;
            this.uiBtnPackageManager.Location = new System.Drawing.Point(384, 19);
            this.uiBtnPackageManager.Name = "uiBtnPackageManager";
            this.uiBtnPackageManager.Size = new System.Drawing.Size(48, 50);
            this.uiBtnPackageManager.TabIndex = 18;
            this.uiBtnPackageManager.TabStop = false;
            this.uiBtnPackageManager.Text = "Packages";
            this.uiBtnPackageManager.Click += new System.EventHandler(this.uiBtnPackageManager_Click);
            // 
            // uiBtnSaveAll
            // 
            this.uiBtnSaveAll.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnSaveAll.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnSaveAll.DisplayText = "Save All";
            this.uiBtnSaveAll.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnSaveAll.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnSaveAll.Image = global::OpenBots.Properties.Resources.action_bar_saveall;
            this.uiBtnSaveAll.IsMouseOver = false;
            this.uiBtnSaveAll.Location = new System.Drawing.Point(288, 19);
            this.uiBtnSaveAll.Name = "uiBtnSaveAll";
            this.uiBtnSaveAll.Size = new System.Drawing.Size(48, 50);
            this.uiBtnSaveAll.TabIndex = 17;
            this.uiBtnSaveAll.TabStop = false;
            this.uiBtnSaveAll.Text = "Save All";
            this.uiBtnSaveAll.Click += new System.EventHandler(this.uiBtnSaveAll_Click);
            // 
            // uiBtnPublishProject
            // 
            this.uiBtnPublishProject.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnPublishProject.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnPublishProject.DisplayText = "Publish";
            this.uiBtnPublishProject.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnPublishProject.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnPublishProject.Image = global::OpenBots.Properties.Resources.action_bar_publish;
            this.uiBtnPublishProject.IsMouseOver = false;
            this.uiBtnPublishProject.Location = new System.Drawing.Point(336, 19);
            this.uiBtnPublishProject.Name = "uiBtnPublishProject";
            this.uiBtnPublishProject.Size = new System.Drawing.Size(48, 50);
            this.uiBtnPublishProject.TabIndex = 16;
            this.uiBtnPublishProject.TabStop = false;
            this.uiBtnPublishProject.Text = "Publish";
            this.uiBtnPublishProject.Click += new System.EventHandler(this.uiBtnPublishProject_Click);
            // 
            // uiBtnProject
            // 
            this.uiBtnProject.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnProject.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnProject.DisplayText = "Project";
            this.uiBtnProject.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.uiBtnProject.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnProject.Image")));
            this.uiBtnProject.IsMouseOver = false;
            this.uiBtnProject.Location = new System.Drawing.Point(0, 19);
            this.uiBtnProject.Name = "uiBtnProject";
            this.uiBtnProject.Size = new System.Drawing.Size(48, 50);
            this.uiBtnProject.TabIndex = 15;
            this.uiBtnProject.TabStop = false;
            this.uiBtnProject.Text = "Project";
            this.uiBtnProject.Click += new System.EventHandler(this.uiBtnProject_Click);
            // 
            // uiBtnImport
            // 
            this.uiBtnImport.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnImport.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnImport.DisplayText = "Import";
            this.uiBtnImport.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnImport.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnImport.Image = global::OpenBots.Properties.Resources.action_bar_import;
            this.uiBtnImport.IsMouseOver = false;
            this.uiBtnImport.Location = new System.Drawing.Point(144, 19);
            this.uiBtnImport.Name = "uiBtnImport";
            this.uiBtnImport.Size = new System.Drawing.Size(48, 50);
            this.uiBtnImport.TabIndex = 14;
            this.uiBtnImport.TabStop = false;
            this.uiBtnImport.Text = "Import";
            this.uiBtnImport.Click += new System.EventHandler(this.uiBtnImport_Click);
            // 
            // uiBtnSaveAs
            // 
            this.uiBtnSaveAs.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnSaveAs.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnSaveAs.DisplayText = "Save As";
            this.uiBtnSaveAs.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnSaveAs.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnSaveAs.Image = global::OpenBots.Properties.Resources.action_bar_saveas;
            this.uiBtnSaveAs.IsMouseOver = false;
            this.uiBtnSaveAs.Location = new System.Drawing.Point(240, 19);
            this.uiBtnSaveAs.Name = "uiBtnSaveAs";
            this.uiBtnSaveAs.Size = new System.Drawing.Size(48, 50);
            this.uiBtnSaveAs.TabIndex = 13;
            this.uiBtnSaveAs.TabStop = false;
            this.uiBtnSaveAs.Text = "Save As";
            this.uiBtnSaveAs.Click += new System.EventHandler(this.uiBtnSaveAs_Click);
            // 
            // uiBtnSave
            // 
            this.uiBtnSave.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnSave.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnSave.DisplayText = "Save";
            this.uiBtnSave.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnSave.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnSave.Image = global::OpenBots.Properties.Resources.action_bar_save;
            this.uiBtnSave.IsMouseOver = false;
            this.uiBtnSave.Location = new System.Drawing.Point(192, 19);
            this.uiBtnSave.Name = "uiBtnSave";
            this.uiBtnSave.Size = new System.Drawing.Size(48, 50);
            this.uiBtnSave.TabIndex = 11;
            this.uiBtnSave.TabStop = false;
            this.uiBtnSave.Text = "Save";
            this.uiBtnSave.Click += new System.EventHandler(this.uiBtnSave_Click);
            // 
            // uiBtnNew
            // 
            this.uiBtnNew.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnNew.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnNew.DisplayText = "New";
            this.uiBtnNew.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.uiBtnNew.Image = global::OpenBots.Properties.Resources.action_bar_new;
            this.uiBtnNew.IsMouseOver = false;
            this.uiBtnNew.Location = new System.Drawing.Point(48, 19);
            this.uiBtnNew.Name = "uiBtnNew";
            this.uiBtnNew.Size = new System.Drawing.Size(48, 50);
            this.uiBtnNew.TabIndex = 12;
            this.uiBtnNew.TabStop = false;
            this.uiBtnNew.Text = "New";
            this.uiBtnNew.Click += new System.EventHandler(this.uiBtnNew_Click);
            // 
            // uiBtnOpen
            // 
            this.uiBtnOpen.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOpen.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOpen.DisplayText = "Open";
            this.uiBtnOpen.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnOpen.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOpen.Image = global::OpenBots.Properties.Resources.action_bar_open;
            this.uiBtnOpen.IsMouseOver = false;
            this.uiBtnOpen.Location = new System.Drawing.Point(96, 19);
            this.uiBtnOpen.Name = "uiBtnOpen";
            this.uiBtnOpen.Size = new System.Drawing.Size(48, 50);
            this.uiBtnOpen.TabIndex = 10;
            this.uiBtnOpen.TabStop = false;
            this.uiBtnOpen.Text = "Open";
            this.uiBtnOpen.Click += new System.EventHandler(this.uiBtnOpen_Click);
            // 
            // grpRecordRun
            // 
            this.grpRecordRun.BackColor = System.Drawing.Color.Transparent;
            this.grpRecordRun.Controls.Add(this.uiBtnBreakpoint);
            this.grpRecordRun.Controls.Add(this.uiBtnRecordAdvancedUISequence);
            this.grpRecordRun.Controls.Add(this.uiBtnRecordElementSequence);
            this.grpRecordRun.Controls.Add(this.uiBtnRunScript);
            this.grpRecordRun.Controls.Add(this.uiBtnRecordUISequence);
            this.grpRecordRun.Controls.Add(this.uiBtnDebugScript);
            this.grpRecordRun.Controls.Add(this.uiBtnScheduleManagement);
            this.grpRecordRun.Location = new System.Drawing.Point(761, 0);
            this.grpRecordRun.Name = "grpRecordRun";
            this.grpRecordRun.Size = new System.Drawing.Size(357, 71);
            this.grpRecordRun.TabIndex = 18;
            this.grpRecordRun.TabStop = false;
            this.grpRecordRun.Text = "Record and Run";
            this.grpRecordRun.TitleBackColor = System.Drawing.Color.Transparent;
            this.grpRecordRun.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpRecordRun.TitleForeColor = System.Drawing.Color.GhostWhite;
            this.grpRecordRun.TitleHatchStyle = System.Drawing.Drawing2D.HatchStyle.Horizontal;
            // 
            // uiBtnBreakpoint
            // 
            this.uiBtnBreakpoint.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnBreakpoint.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnBreakpoint.DisplayText = "Break";
            this.uiBtnBreakpoint.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnBreakpoint.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnBreakpoint.Image = global::OpenBots.Properties.Resources.action_bar_breakpoint;
            this.uiBtnBreakpoint.IsMouseOver = false;
            this.uiBtnBreakpoint.Location = new System.Drawing.Point(295, 19);
            this.uiBtnBreakpoint.Name = "uiBtnBreakpoint";
            this.uiBtnBreakpoint.Size = new System.Drawing.Size(48, 50);
            this.uiBtnBreakpoint.TabIndex = 23;
            this.uiBtnBreakpoint.TabStop = false;
            this.uiBtnBreakpoint.Text = "Break";
            this.uiBtnBreakpoint.Click += new System.EventHandler(this.uiBtnBreakpoint_Click);
            // 
            // uiBtnRecordAdvancedUISequence
            // 
            this.uiBtnRecordAdvancedUISequence.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnRecordAdvancedUISequence.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnRecordAdvancedUISequence.DisplayText = "Adv UI";
            this.uiBtnRecordAdvancedUISequence.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnRecordAdvancedUISequence.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnRecordAdvancedUISequence.Image = global::OpenBots.Properties.Resources.action_bar_element_recorder;
            this.uiBtnRecordAdvancedUISequence.IsMouseOver = false;
            this.uiBtnRecordAdvancedUISequence.Location = new System.Drawing.Point(151, 19);
            this.uiBtnRecordAdvancedUISequence.Name = "uiBtnRecordAdvancedUISequence";
            this.uiBtnRecordAdvancedUISequence.Size = new System.Drawing.Size(48, 50);
            this.uiBtnRecordAdvancedUISequence.TabIndex = 22;
            this.uiBtnRecordAdvancedUISequence.TabStop = false;
            this.uiBtnRecordAdvancedUISequence.Text = "Adv UI";
            this.uiBtnRecordAdvancedUISequence.Click += new System.EventHandler(this.uiBtnRecordAdvancedUISequence_Click);
            // 
            // uiBtnRecordElementSequence
            // 
            this.uiBtnRecordElementSequence.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnRecordElementSequence.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnRecordElementSequence.DisplayText = "Web";
            this.uiBtnRecordElementSequence.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnRecordElementSequence.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnRecordElementSequence.Image = global::OpenBots.Properties.Resources.action_bar_element_recorder;
            this.uiBtnRecordElementSequence.IsMouseOver = false;
            this.uiBtnRecordElementSequence.Location = new System.Drawing.Point(55, 19);
            this.uiBtnRecordElementSequence.Name = "uiBtnRecordElementSequence";
            this.uiBtnRecordElementSequence.Size = new System.Drawing.Size(48, 50);
            this.uiBtnRecordElementSequence.TabIndex = 16;
            this.uiBtnRecordElementSequence.TabStop = false;
            this.uiBtnRecordElementSequence.Text = "Web";
            this.uiBtnRecordElementSequence.Click += new System.EventHandler(this.uiBtnRecordElementSequence_Click);
            // 
            // uiBtnRunScript
            // 
            this.uiBtnRunScript.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnRunScript.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnRunScript.DisplayText = "Run";
            this.uiBtnRunScript.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnRunScript.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnRunScript.Image = global::OpenBots.Properties.Resources.action_bar_run;
            this.uiBtnRunScript.IsMouseOver = false;
            this.uiBtnRunScript.Location = new System.Drawing.Point(199, 19);
            this.uiBtnRunScript.Name = "uiBtnRunScript";
            this.uiBtnRunScript.Size = new System.Drawing.Size(48, 50);
            this.uiBtnRunScript.TabIndex = 21;
            this.uiBtnRunScript.TabStop = false;
            this.uiBtnRunScript.Text = "Run";
            this.uiBtnRunScript.Click += new System.EventHandler(this.uiBtnRunScript_Click);
            // 
            // uiBtnRecordUISequence
            // 
            this.uiBtnRecordUISequence.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnRecordUISequence.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnRecordUISequence.DisplayText = "UI";
            this.uiBtnRecordUISequence.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnRecordUISequence.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnRecordUISequence.Image = global::OpenBots.Properties.Resources.action_bar_element_recorder;
            this.uiBtnRecordUISequence.IsMouseOver = false;
            this.uiBtnRecordUISequence.Location = new System.Drawing.Point(103, 19);
            this.uiBtnRecordUISequence.Name = "uiBtnRecordUISequence";
            this.uiBtnRecordUISequence.Size = new System.Drawing.Size(48, 50);
            this.uiBtnRecordUISequence.TabIndex = 19;
            this.uiBtnRecordUISequence.TabStop = false;
            this.uiBtnRecordUISequence.Text = "UI";
            this.uiBtnRecordUISequence.Click += new System.EventHandler(this.uiBtnRecordUISequence_Click);
            // 
            // uiBtnDebugScript
            // 
            this.uiBtnDebugScript.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnDebugScript.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnDebugScript.DisplayText = "Debug";
            this.uiBtnDebugScript.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnDebugScript.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnDebugScript.Image = global::OpenBots.Properties.Resources.action_bar_debug;
            this.uiBtnDebugScript.IsMouseOver = false;
            this.uiBtnDebugScript.Location = new System.Drawing.Point(247, 19);
            this.uiBtnDebugScript.Name = "uiBtnDebugScript";
            this.uiBtnDebugScript.Size = new System.Drawing.Size(48, 50);
            this.uiBtnDebugScript.TabIndex = 12;
            this.uiBtnDebugScript.TabStop = false;
            this.uiBtnDebugScript.Text = "Debug";
            this.uiBtnDebugScript.Click += new System.EventHandler(this.uiBtnDebugScript_Click);
            // 
            // uiBtnScheduleManagement
            // 
            this.uiBtnScheduleManagement.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnScheduleManagement.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnScheduleManagement.DisplayText = "Schedule";
            this.uiBtnScheduleManagement.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnScheduleManagement.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnScheduleManagement.Image = global::OpenBots.Properties.Resources.action_bar_schedule;
            this.uiBtnScheduleManagement.IsMouseOver = false;
            this.uiBtnScheduleManagement.Location = new System.Drawing.Point(3, 19);
            this.uiBtnScheduleManagement.Name = "uiBtnScheduleManagement";
            this.uiBtnScheduleManagement.Size = new System.Drawing.Size(52, 50);
            this.uiBtnScheduleManagement.TabIndex = 13;
            this.uiBtnScheduleManagement.TabStop = false;
            this.uiBtnScheduleManagement.Text = "Schedule";
            this.uiBtnScheduleManagement.Click += new System.EventHandler(this.uiBtnScheduleManagement_Click);
            // 
            // grpVariable
            // 
            this.grpVariable.BackColor = System.Drawing.Color.Transparent;
            this.grpVariable.Controls.Add(this.uiBtnExtensionsManager);
            this.grpVariable.Controls.Add(this.uiBtnAddArgument);
            this.grpVariable.Controls.Add(this.uiBtnAddElement);
            this.grpVariable.Controls.Add(this.uiBtnClearAll);
            this.grpVariable.Controls.Add(this.uiBtnSettings);
            this.grpVariable.Controls.Add(this.uiBtnAddVariable);
            this.grpVariable.Location = new System.Drawing.Point(450, 0);
            this.grpVariable.Name = "grpVariable";
            this.grpVariable.Size = new System.Drawing.Size(304, 71);
            this.grpVariable.TabIndex = 17;
            this.grpVariable.TabStop = false;
            this.grpVariable.Text = "Variables/Elements and Settings";
            this.grpVariable.TitleBackColor = System.Drawing.Color.Transparent;
            this.grpVariable.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpVariable.TitleForeColor = System.Drawing.Color.GhostWhite;
            this.grpVariable.TitleHatchStyle = System.Drawing.Drawing2D.HatchStyle.Horizontal;
            // 
            // uiBtnExtensionsManager
            // 
            this.uiBtnExtensionsManager.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnExtensionsManager.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnExtensionsManager.DisplayText = "Extensions";
            this.uiBtnExtensionsManager.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnExtensionsManager.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnExtensionsManager.Image = global::OpenBots.Properties.Resources.action_bar_extensions;
            this.uiBtnExtensionsManager.IsMouseOver = false;
            this.uiBtnExtensionsManager.Location = new System.Drawing.Point(202, 19);
            this.uiBtnExtensionsManager.Name = "uiBtnExtensionsManager";
            this.uiBtnExtensionsManager.Size = new System.Drawing.Size(48, 50);
            this.uiBtnExtensionsManager.TabIndex = 17;
            this.uiBtnExtensionsManager.TabStop = false;
            this.uiBtnExtensionsManager.Text = "Extensions";
            this.uiBtnExtensionsManager.Click += new System.EventHandler(this.uiBtnExtensionsManager_Click);
            // 
            // uiBtnAddArgument
            // 
            this.uiBtnAddArgument.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnAddArgument.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnAddArgument.DisplayText = "Arguments";
            this.uiBtnAddArgument.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnAddArgument.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnAddArgument.Image = global::OpenBots.Properties.Resources.action_bar_variable;
            this.uiBtnAddArgument.IsMouseOver = false;
            this.uiBtnAddArgument.Location = new System.Drawing.Point(56, 19);
            this.uiBtnAddArgument.Name = "uiBtnAddArgument";
            this.uiBtnAddArgument.Size = new System.Drawing.Size(50, 50);
            this.uiBtnAddArgument.TabIndex = 16;
            this.uiBtnAddArgument.TabStop = false;
            this.uiBtnAddArgument.Text = "Arguments";
            this.uiBtnAddArgument.Click += new System.EventHandler(this.uiBtnAddArgument_Click);
            // 
            // uiBtnAddElement
            // 
            this.uiBtnAddElement.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnAddElement.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnAddElement.DisplayText = "Elements";
            this.uiBtnAddElement.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnAddElement.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnAddElement.Image = global::OpenBots.Properties.Resources.action_bar_element;
            this.uiBtnAddElement.IsMouseOver = false;
            this.uiBtnAddElement.Location = new System.Drawing.Point(106, 19);
            this.uiBtnAddElement.Name = "uiBtnAddElement";
            this.uiBtnAddElement.Size = new System.Drawing.Size(48, 50);
            this.uiBtnAddElement.TabIndex = 15;
            this.uiBtnAddElement.TabStop = false;
            this.uiBtnAddElement.Text = "Elements";
            this.uiBtnAddElement.Click += new System.EventHandler(this.uiBtnAddElement_Click);
            // 
            // uiBtnClearAll
            // 
            this.uiBtnClearAll.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnClearAll.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnClearAll.DisplayText = "Clear";
            this.uiBtnClearAll.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnClearAll.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnClearAll.Image = global::OpenBots.Properties.Resources.action_bar_clear;
            this.uiBtnClearAll.IsMouseOver = false;
            this.uiBtnClearAll.Location = new System.Drawing.Point(250, 19);
            this.uiBtnClearAll.Name = "uiBtnClearAll";
            this.uiBtnClearAll.Size = new System.Drawing.Size(48, 50);
            this.uiBtnClearAll.TabIndex = 14;
            this.uiBtnClearAll.TabStop = false;
            this.uiBtnClearAll.Text = "Clear";
            this.uiBtnClearAll.Click += new System.EventHandler(this.uiBtnClearAll_Click);
            // 
            // uiBtnSettings
            // 
            this.uiBtnSettings.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnSettings.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnSettings.DisplayText = "Settings";
            this.uiBtnSettings.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnSettings.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnSettings.Image = global::OpenBots.Properties.Resources.action_bar_options;
            this.uiBtnSettings.IsMouseOver = false;
            this.uiBtnSettings.Location = new System.Drawing.Point(154, 19);
            this.uiBtnSettings.Name = "uiBtnSettings";
            this.uiBtnSettings.Size = new System.Drawing.Size(48, 50);
            this.uiBtnSettings.TabIndex = 12;
            this.uiBtnSettings.TabStop = false;
            this.uiBtnSettings.Text = "Settings";
            this.uiBtnSettings.Click += new System.EventHandler(this.uiBtnSettings_Click);
            // 
            // uiBtnAddVariable
            // 
            this.uiBtnAddVariable.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnAddVariable.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnAddVariable.DisplayText = "Variables";
            this.uiBtnAddVariable.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnAddVariable.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnAddVariable.Image = global::OpenBots.Properties.Resources.action_bar_variable;
            this.uiBtnAddVariable.IsMouseOver = false;
            this.uiBtnAddVariable.Location = new System.Drawing.Point(6, 19);
            this.uiBtnAddVariable.Name = "uiBtnAddVariable";
            this.uiBtnAddVariable.Size = new System.Drawing.Size(50, 50);
            this.uiBtnAddVariable.TabIndex = 13;
            this.uiBtnAddVariable.TabStop = false;
            this.uiBtnAddVariable.Text = "Variables";
            this.uiBtnAddVariable.Click += new System.EventHandler(this.uiBtnAddVariable_Click);
            // 
            // pnlStatus
            // 
            this.pnlStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.tlpControls.SetColumnSpan(this.pnlStatus, 3);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.pnlStatus.Location = new System.Drawing.Point(0, 710);
            this.pnlStatus.Margin = new System.Windows.Forms.Padding(0);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(1417, 24);
            this.pnlStatus.TabIndex = 3;
            this.pnlStatus.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlStatus_Paint);
            this.pnlStatus.DoubleClick += new System.EventHandler(this.pnlStatus_DoubleClick);
            // 
            // splitContainerStudioControls
            // 
            this.tlpControls.SetColumnSpan(this.splitContainerStudioControls, 3);
            this.splitContainerStudioControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerStudioControls.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerStudioControls.Location = new System.Drawing.Point(3, 116);
            this.splitContainerStudioControls.Name = "splitContainerStudioControls";
            // 
            // splitContainerStudioControls.Panel1
            // 
            this.splitContainerStudioControls.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainerStudioControls.Panel1.Controls.Add(this.uiPaneTabs);
            // 
            // splitContainerStudioControls.Panel2
            // 
            this.splitContainerStudioControls.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainerStudioControls.Panel2.Controls.Add(this.splitContainerScript);
            this.splitContainerStudioControls.Panel2MinSize = 500;
            this.splitContainerStudioControls.Size = new System.Drawing.Size(1411, 591);
            this.splitContainerStudioControls.SplitterDistance = 328;
            this.splitContainerStudioControls.SplitterWidth = 5;
            this.splitContainerStudioControls.TabIndex = 4;
            // 
            // uiPaneTabs
            // 
            this.uiPaneTabs.Controls.Add(this.tpProject);
            this.uiPaneTabs.Controls.Add(this.tpCommands);
            this.uiPaneTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPaneTabs.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.uiPaneTabs.Location = new System.Drawing.Point(0, 0);
            this.uiPaneTabs.Name = "uiPaneTabs";
            this.uiPaneTabs.SelectedIndex = 0;
            this.uiPaneTabs.Size = new System.Drawing.Size(328, 591);
            this.uiPaneTabs.TabIndex = 26;
            // 
            // tpProject
            // 
            this.tpProject.Controls.Add(this.tlpProject);
            this.tpProject.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.tpProject.ForeColor = System.Drawing.Color.SteelBlue;
            this.tpProject.Location = new System.Drawing.Point(4, 26);
            this.tpProject.Margin = new System.Windows.Forms.Padding(2);
            this.tpProject.Name = "tpProject";
            this.tpProject.Padding = new System.Windows.Forms.Padding(2);
            this.tpProject.Size = new System.Drawing.Size(320, 561);
            this.tpProject.TabIndex = 5;
            this.tpProject.Text = "Project";
            this.tpProject.UseVisualStyleBackColor = true;
            // 
            // tlpProject
            // 
            this.tlpProject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(136)))), ((int)(((byte)(204)))));
            this.tlpProject.ColumnCount = 1;
            this.tlpProject.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProject.Controls.Add(this.tvProject, 0, 1);
            this.tlpProject.Controls.Add(this.pnlProjectButtons, 0, 0);
            this.tlpProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProject.Location = new System.Drawing.Point(2, 2);
            this.tlpProject.Margin = new System.Windows.Forms.Padding(2);
            this.tlpProject.Name = "tlpProject";
            this.tlpProject.RowCount = 2;
            this.tlpProject.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpProject.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProject.Size = new System.Drawing.Size(316, 557);
            this.tlpProject.TabIndex = 1;
            this.tlpProject.EnabledChanged += new System.EventHandler(this.tlpProject_EnabledChanged);
            // 
            // tvProject
            // 
            this.tvProject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.tvProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvProject.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.tvProject.ForeColor = System.Drawing.Color.White;
            this.tvProject.ImageIndex = 0;
            this.tvProject.ImageList = this.imgListProjectPane;
            this.tvProject.Location = new System.Drawing.Point(2, 27);
            this.tvProject.Margin = new System.Windows.Forms.Padding(2);
            this.tvProject.Name = "tvProject";
            this.tvProject.SelectedImageIndex = 0;
            this.tvProject.ShowLines = false;
            this.tvProject.ShowNodeToolTips = true;
            this.tvProject.Size = new System.Drawing.Size(312, 528);
            this.tvProject.TabIndex = 0;
            this.tvProject.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvProject_BeforeExpand);
            this.tvProject.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvProject_NodeMouseClick);
            this.tvProject.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvProject_DoubleClick);
            this.tvProject.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvProject_KeyDown);
            // 
            // imgListProjectPane
            // 
            this.imgListProjectPane.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListProjectPane.ImageStream")));
            this.imgListProjectPane.TransparentColor = System.Drawing.Color.Transparent;
            this.imgListProjectPane.Images.SetKeyName(0, "ProjectFolderIcon.png");
            this.imgListProjectPane.Images.SetKeyName(1, "openbots-icon.png");
            this.imgListProjectPane.Images.SetKeyName(2, "ProjectGenericFileIcon.png");
            this.imgListProjectPane.Images.SetKeyName(3, "microsoft-excel-icon.png");
            this.imgListProjectPane.Images.SetKeyName(4, "microsoft-word-icon.png");
            this.imgListProjectPane.Images.SetKeyName(5, "pdf-icon.png");
            this.imgListProjectPane.Images.SetKeyName(6, "python-icon.png");
            this.imgListProjectPane.Images.SetKeyName(7, "tagui-icon.png");
            this.imgListProjectPane.Images.SetKeyName(8, "c-sharp-icon.png");
            this.imgListProjectPane.Images.SetKeyName(9, "powershell-icon.png");
            // 
            // pnlProjectButtons
            // 
            this.pnlProjectButtons.Controls.Add(this.uiBtnCollapse);
            this.pnlProjectButtons.Controls.Add(this.uiBtnOpenDirectory);
            this.pnlProjectButtons.Controls.Add(this.uiBtnExpand);
            this.pnlProjectButtons.Controls.Add(this.uiBtnRefresh);
            this.pnlProjectButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProjectButtons.Location = new System.Drawing.Point(2, 2);
            this.pnlProjectButtons.Margin = new System.Windows.Forms.Padding(2);
            this.pnlProjectButtons.Name = "pnlProjectButtons";
            this.pnlProjectButtons.Size = new System.Drawing.Size(312, 21);
            this.pnlProjectButtons.TabIndex = 1;
            // 
            // uiBtnCollapse
            // 
            this.uiBtnCollapse.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnCollapse.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnCollapse.DisplayText = null;
            this.uiBtnCollapse.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnCollapse.Image = global::OpenBots.Properties.Resources.project_collapse;
            this.uiBtnCollapse.IsMouseOver = false;
            this.uiBtnCollapse.Location = new System.Drawing.Point(45, 2);
            this.uiBtnCollapse.Margin = new System.Windows.Forms.Padding(2);
            this.uiBtnCollapse.Name = "uiBtnCollapse";
            this.uiBtnCollapse.Size = new System.Drawing.Size(20, 20);
            this.uiBtnCollapse.TabIndex = 3;
            this.uiBtnCollapse.TabStop = false;
            this.uiBtnCollapse.Text = null;
            this.ttScriptBuilder.SetToolTip(this.uiBtnCollapse, "Collapse");
            this.uiBtnCollapse.Click += new System.EventHandler(this.uiBtnCollapse_Click);
            // 
            // uiBtnOpenDirectory
            // 
            this.uiBtnOpenDirectory.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOpenDirectory.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOpenDirectory.DisplayText = null;
            this.uiBtnOpenDirectory.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnOpenDirectory.Image = global::OpenBots.Properties.Resources.project_open_directory;
            this.uiBtnOpenDirectory.IsMouseOver = false;
            this.uiBtnOpenDirectory.Location = new System.Drawing.Point(67, 2);
            this.uiBtnOpenDirectory.Margin = new System.Windows.Forms.Padding(2);
            this.uiBtnOpenDirectory.Name = "uiBtnOpenDirectory";
            this.uiBtnOpenDirectory.Size = new System.Drawing.Size(20, 20);
            this.uiBtnOpenDirectory.TabIndex = 2;
            this.uiBtnOpenDirectory.TabStop = false;
            this.uiBtnOpenDirectory.Text = null;
            this.ttScriptBuilder.SetToolTip(this.uiBtnOpenDirectory, "Open Project Directory in File Explorer");
            this.uiBtnOpenDirectory.Click += new System.EventHandler(this.uiBtnOpenDirectory_Click);
            // 
            // uiBtnExpand
            // 
            this.uiBtnExpand.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnExpand.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnExpand.DisplayText = null;
            this.uiBtnExpand.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnExpand.Image = global::OpenBots.Properties.Resources.project_expand;
            this.uiBtnExpand.IsMouseOver = false;
            this.uiBtnExpand.Location = new System.Drawing.Point(22, 2);
            this.uiBtnExpand.Margin = new System.Windows.Forms.Padding(2);
            this.uiBtnExpand.Name = "uiBtnExpand";
            this.uiBtnExpand.Size = new System.Drawing.Size(20, 20);
            this.uiBtnExpand.TabIndex = 1;
            this.uiBtnExpand.TabStop = false;
            this.uiBtnExpand.Text = null;
            this.ttScriptBuilder.SetToolTip(this.uiBtnExpand, "Expand");
            this.uiBtnExpand.Click += new System.EventHandler(this.uiBtnExpand_Click);
            // 
            // uiBtnRefresh
            // 
            this.uiBtnRefresh.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnRefresh.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnRefresh.DisplayText = null;
            this.uiBtnRefresh.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnRefresh.Image = global::OpenBots.Properties.Resources.browser_refresh;
            this.uiBtnRefresh.IsMouseOver = false;
            this.uiBtnRefresh.Location = new System.Drawing.Point(0, 2);
            this.uiBtnRefresh.Margin = new System.Windows.Forms.Padding(2);
            this.uiBtnRefresh.Name = "uiBtnRefresh";
            this.uiBtnRefresh.Size = new System.Drawing.Size(20, 20);
            this.uiBtnRefresh.TabIndex = 0;
            this.uiBtnRefresh.TabStop = false;
            this.uiBtnRefresh.Text = null;
            this.ttScriptBuilder.SetToolTip(this.uiBtnRefresh, "Reload Project Directory");
            this.uiBtnRefresh.Click += new System.EventHandler(this.uiBtnRefresh_Click);
            // 
            // tpCommands
            // 
            this.tpCommands.Controls.Add(this.tlpCommands);
            this.tpCommands.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.tpCommands.ForeColor = System.Drawing.Color.SteelBlue;
            this.tpCommands.Location = new System.Drawing.Point(4, 26);
            this.tpCommands.Margin = new System.Windows.Forms.Padding(2);
            this.tpCommands.Name = "tpCommands";
            this.tpCommands.Padding = new System.Windows.Forms.Padding(2);
            this.tpCommands.Size = new System.Drawing.Size(320, 561);
            this.tpCommands.TabIndex = 4;
            this.tpCommands.Text = "Commands";
            this.tpCommands.UseVisualStyleBackColor = true;
            // 
            // tlpCommands
            // 
            this.tlpCommands.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(136)))), ((int)(((byte)(204)))));
            this.tlpCommands.ColumnCount = 1;
            this.tlpCommands.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCommands.Controls.Add(this.tvCommands, 0, 1);
            this.tlpCommands.Controls.Add(this.pnlCommandSearch, 0, 0);
            this.tlpCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCommands.Location = new System.Drawing.Point(2, 2);
            this.tlpCommands.Margin = new System.Windows.Forms.Padding(2);
            this.tlpCommands.Name = "tlpCommands";
            this.tlpCommands.RowCount = 2;
            this.tlpCommands.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpCommands.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCommands.Size = new System.Drawing.Size(316, 557);
            this.tlpCommands.TabIndex = 10;
            this.tlpCommands.EnabledChanged += new System.EventHandler(this.tlpCommands_EnabledChanged);
            // 
            // tvCommands
            // 
            this.tvCommands.AllowDrop = true;
            this.tvCommands.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.tvCommands.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvCommands.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.tvCommands.ForeColor = System.Drawing.Color.White;
            this.tvCommands.Location = new System.Drawing.Point(3, 28);
            this.tvCommands.Name = "tvCommands";
            this.tvCommands.ShowLines = false;
            this.tvCommands.ShowNodeToolTips = true;
            this.tvCommands.Size = new System.Drawing.Size(310, 526);
            this.tvCommands.TabIndex = 9;
            this.tvCommands.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvCommands_ItemDrag);
            this.tvCommands.DoubleClick += new System.EventHandler(this.tvCommands_DoubleClick);
            this.tvCommands.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tvCommands_KeyPress);
            // 
            // pnlCommandSearch
            // 
            this.pnlCommandSearch.Controls.Add(this.uiBtnReloadCommands);
            this.pnlCommandSearch.Controls.Add(this.uiBtnClearCommandSearch);
            this.pnlCommandSearch.Controls.Add(this.txtCommandSearch);
            this.pnlCommandSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCommandSearch.Location = new System.Drawing.Point(2, 2);
            this.pnlCommandSearch.Margin = new System.Windows.Forms.Padding(2);
            this.pnlCommandSearch.Name = "pnlCommandSearch";
            this.pnlCommandSearch.Size = new System.Drawing.Size(312, 21);
            this.pnlCommandSearch.TabIndex = 10;
            // 
            // uiBtnReloadCommands
            // 
            this.uiBtnReloadCommands.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiBtnReloadCommands.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnReloadCommands.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnReloadCommands.DisplayText = null;
            this.uiBtnReloadCommands.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnReloadCommands.Image = global::OpenBots.Properties.Resources.browser_refresh;
            this.uiBtnReloadCommands.IsMouseOver = false;
            this.uiBtnReloadCommands.Location = new System.Drawing.Point(290, 1);
            this.uiBtnReloadCommands.Margin = new System.Windows.Forms.Padding(2);
            this.uiBtnReloadCommands.Name = "uiBtnReloadCommands";
            this.uiBtnReloadCommands.Size = new System.Drawing.Size(20, 20);
            this.uiBtnReloadCommands.TabIndex = 2;
            this.uiBtnReloadCommands.TabStop = false;
            this.uiBtnReloadCommands.Text = null;
            this.ttScriptBuilder.SetToolTip(this.uiBtnReloadCommands, "Reload Command Assemblies");
            this.uiBtnReloadCommands.Click += new System.EventHandler(this.uiBtnReloadCommands_Click);
            // 
            // uiBtnClearCommandSearch
            // 
            this.uiBtnClearCommandSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiBtnClearCommandSearch.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnClearCommandSearch.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnClearCommandSearch.DisplayText = null;
            this.uiBtnClearCommandSearch.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnClearCommandSearch.Image = global::OpenBots.Properties.Resources.commandsearch_clear;
            this.uiBtnClearCommandSearch.IsMouseOver = false;
            this.uiBtnClearCommandSearch.Location = new System.Drawing.Point(268, 1);
            this.uiBtnClearCommandSearch.Margin = new System.Windows.Forms.Padding(2);
            this.uiBtnClearCommandSearch.Name = "uiBtnClearCommandSearch";
            this.uiBtnClearCommandSearch.Size = new System.Drawing.Size(20, 20);
            this.uiBtnClearCommandSearch.TabIndex = 1;
            this.uiBtnClearCommandSearch.TabStop = false;
            this.uiBtnClearCommandSearch.Text = null;
            this.ttScriptBuilder.SetToolTip(this.uiBtnClearCommandSearch, "Clear Search");
            this.uiBtnClearCommandSearch.Click += new System.EventHandler(this.uiBtnClearCommandSearch_Click);
            // 
            // txtCommandSearch
            // 
            this.txtCommandSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCommandSearch.ForeColor = System.Drawing.Color.LightGray;
            this.txtCommandSearch.Location = new System.Drawing.Point(0, 0);
            this.txtCommandSearch.Margin = new System.Windows.Forms.Padding(2);
            this.txtCommandSearch.Name = "txtCommandSearch";
            this.txtCommandSearch.Size = new System.Drawing.Size(266, 25);
            this.txtCommandSearch.TabIndex = 0;
            this.txtCommandSearch.Text = "Type Here to Search";
            this.txtCommandSearch.TextChanged += new System.EventHandler(this.txtCommandSearch_TextChanged);
            this.txtCommandSearch.Enter += new System.EventHandler(this.txtCommandSearch_Enter);
            this.txtCommandSearch.Leave += new System.EventHandler(this.txtCommandSearch_Leave);
            // 
            // splitContainerScript
            // 
            this.splitContainerScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerScript.Location = new System.Drawing.Point(0, 0);
            this.splitContainerScript.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainerScript.Name = "splitContainerScript";
            this.splitContainerScript.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerScript.Panel1
            // 
            this.splitContainerScript.Panel1.Controls.Add(this.uiScriptTabControl);
            // 
            // splitContainerScript.Panel2
            // 
            this.splitContainerScript.Panel2.Controls.Add(this.uiVariableArgumentTabs);
            this.splitContainerScript.Size = new System.Drawing.Size(1078, 591);
            this.splitContainerScript.SplitterDistance = 396;
            this.splitContainerScript.SplitterWidth = 5;
            this.splitContainerScript.TabIndex = 4;
            // 
            // uiScriptTabControl
            // 
            this.uiScriptTabControl.AllowDrop = true;
            this.uiScriptTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiScriptTabControl.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.uiScriptTabControl.Location = new System.Drawing.Point(0, 0);
            this.uiScriptTabControl.Margin = new System.Windows.Forms.Padding(2);
            this.uiScriptTabControl.Name = "uiScriptTabControl";
            this.uiScriptTabControl.SelectedIndex = 0;
            this.uiScriptTabControl.ShowToolTips = true;
            this.uiScriptTabControl.Size = new System.Drawing.Size(1078, 396);
            this.uiScriptTabControl.TabIndex = 3;
            this.uiScriptTabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.uiScriptTabControl_DrawItem);
            this.uiScriptTabControl.SelectedIndexChanged += new System.EventHandler(this.uiScriptTabControl_SelectedIndexChanged);
            this.uiScriptTabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.uiScriptTabControl_Selecting);
            this.uiScriptTabControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.uiScriptTabControl_MouseClick);
            // 
            // uiVariableArgumentTabs
            // 
            this.uiVariableArgumentTabs.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.uiVariableArgumentTabs.AllowDrop = true;
            this.uiVariableArgumentTabs.Controls.Add(this.variables);
            this.uiVariableArgumentTabs.Controls.Add(this.arguments);
            this.uiVariableArgumentTabs.Controls.Add(this.imports);
            this.uiVariableArgumentTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiVariableArgumentTabs.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.uiVariableArgumentTabs.Location = new System.Drawing.Point(0, 0);
            this.uiVariableArgumentTabs.Margin = new System.Windows.Forms.Padding(2);
            this.uiVariableArgumentTabs.Multiline = true;
            this.uiVariableArgumentTabs.Name = "uiVariableArgumentTabs";
            this.uiVariableArgumentTabs.SelectedIndex = 0;
            this.uiVariableArgumentTabs.Size = new System.Drawing.Size(1078, 190);
            this.uiVariableArgumentTabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.uiVariableArgumentTabs.TabIndex = 0;
            this.uiVariableArgumentTabs.SelectedIndexChanged += new System.EventHandler(this.uiVariableArgumentTabs_SelectedIndexChanged);
            // 
            // variables
            // 
            this.variables.Controls.Add(this.dgvVariables);
            this.variables.Location = new System.Drawing.Point(4, 4);
            this.variables.Margin = new System.Windows.Forms.Padding(2);
            this.variables.Name = "variables";
            this.variables.Padding = new System.Windows.Forms.Padding(2);
            this.variables.Size = new System.Drawing.Size(1070, 160);
            this.variables.TabIndex = 0;
            this.variables.Text = "Variables";
            this.variables.UseVisualStyleBackColor = true;
            // 
            // dgvVariables
            // 
            this.dgvVariables.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvVariables.BorderColor = System.Drawing.Color.Empty;
            this.dgvVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVariables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.variableName,
            this.variableType,
            this.variableValue,
            this.VariableEditor});
            this.dgvVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVariables.IsDoubleBuffered = false;
            this.dgvVariables.ListBoxShown = false;
            this.dgvVariables.Location = new System.Drawing.Point(2, 2);
            this.dgvVariables.Margin = new System.Windows.Forms.Padding(2);
            this.dgvVariables.Name = "dgvVariables";
            this.dgvVariables.RowHeadersWidth = 51;
            this.dgvVariables.RowTemplate.Height = 24;
            this.dgvVariables.Size = new System.Drawing.Size(1066, 156);
            this.dgvVariables.TabIndex = 0;
            this.dgvVariables.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVariablesArguments_CellContentClick);
            this.dgvVariables.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVariablesArguments_CellEndEdit);
            this.dgvVariables.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVariablesArguments_CellEnter);
            this.dgvVariables.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVariablesArguments_CellValueChanged);
            this.dgvVariables.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvVariablesArguments_CurrentCellDirtyStateChanged);
            this.dgvVariables.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvVariablesArguments_DataBindingComplete);
            this.dgvVariables.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvVariablesArguments_DataError);
            this.dgvVariables.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvVariablesArguments_DefaultValuesNeeded);
            this.dgvVariables.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvVariablesArguments_EditingControlShowing);
            this.dgvVariables.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvVariablesArguments_RowsAdded);
            this.dgvVariables.SelectionChanged += new System.EventHandler(this.dgvVariables_SelectionChanged);
            this.dgvVariables.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvVariablesArguments_UserDeletingRow);
            this.dgvVariables.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvVariablesArguments_KeyDown);
            // 
            // variableName
            // 
            this.variableName.DataPropertyName = "VariableName";
            this.variableName.FillWeight = 130.4813F;
            this.variableName.HeaderText = "Variable Name";
            this.variableName.MinimumWidth = 6;
            this.variableName.Name = "variableName";
            // 
            // variableType
            // 
            this.variableType.DataPropertyName = "VariableType";
            this.variableType.FillWeight = 130.4813F;
            this.variableType.HeaderText = "Variable Type";
            this.variableType.MinimumWidth = 6;
            this.variableType.Name = "variableType";
            this.variableType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // variableValue
            // 
            this.variableValue.DataPropertyName = "VariableValue";
            this.variableValue.FillWeight = 130.4813F;
            this.variableValue.HeaderText = "Variable Value";
            this.variableValue.MinimumWidth = 6;
            this.variableValue.Name = "variableValue";
            // 
            // VariableEditor
            // 
            this.VariableEditor.FillWeight = 8.55615F;
            this.VariableEditor.HeaderText = "";
            this.VariableEditor.MinimumWidth = 4;
            this.VariableEditor.Name = "VariableEditor";
            this.VariableEditor.Text = "...";
            this.VariableEditor.UseColumnTextForButtonValue = true;
            // 
            // arguments
            // 
            this.arguments.Controls.Add(this.dgvArguments);
            this.arguments.Location = new System.Drawing.Point(4, 4);
            this.arguments.Margin = new System.Windows.Forms.Padding(2);
            this.arguments.Name = "arguments";
            this.arguments.Padding = new System.Windows.Forms.Padding(2);
            this.arguments.Size = new System.Drawing.Size(1070, 160);
            this.arguments.TabIndex = 1;
            this.arguments.Text = "Arguments";
            this.arguments.UseVisualStyleBackColor = true;
            // 
            // dgvArguments
            // 
            this.dgvArguments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvArguments.BorderColor = System.Drawing.Color.Empty;
            this.dgvArguments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvArguments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.argumentName,
            this.argumentType,
            this.argumentValue,
            this.ArgumentEditor,
            this.direction});
            this.dgvArguments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvArguments.IsDoubleBuffered = false;
            this.dgvArguments.ListBoxShown = false;
            this.dgvArguments.Location = new System.Drawing.Point(2, 2);
            this.dgvArguments.Margin = new System.Windows.Forms.Padding(2);
            this.dgvArguments.Name = "dgvArguments";
            this.dgvArguments.RowHeadersWidth = 51;
            this.dgvArguments.RowTemplate.Height = 24;
            this.dgvArguments.Size = new System.Drawing.Size(1066, 156);
            this.dgvArguments.TabIndex = 2;
            this.dgvArguments.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVariablesArguments_CellContentClick);
            this.dgvArguments.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVariablesArguments_CellEndEdit);
            this.dgvArguments.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVariablesArguments_CellEnter);
            this.dgvArguments.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVariablesArguments_CellValueChanged);
            this.dgvArguments.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvVariablesArguments_CurrentCellDirtyStateChanged);
            this.dgvArguments.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvVariablesArguments_DataBindingComplete);
            this.dgvArguments.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvVariablesArguments_DataError);
            this.dgvArguments.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvVariablesArguments_DefaultValuesNeeded);
            this.dgvArguments.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvVariablesArguments_EditingControlShowing);
            this.dgvArguments.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvVariablesArguments_RowsAdded);
            this.dgvArguments.SelectionChanged += new System.EventHandler(this.dgvArguments_SelectionChanged);
            this.dgvArguments.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvVariablesArguments_UserDeletingRow);
            this.dgvArguments.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvVariablesArguments_KeyDown);
            // 
            // argumentName
            // 
            this.argumentName.DataPropertyName = "ArgumentName";
            this.argumentName.FillWeight = 145.205F;
            this.argumentName.HeaderText = "Argument Name";
            this.argumentName.MinimumWidth = 6;
            this.argumentName.Name = "argumentName";
            // 
            // argumentType
            // 
            this.argumentType.DataPropertyName = "ArgumentType";
            this.argumentType.FillWeight = 145.205F;
            this.argumentType.HeaderText = "Argument Type";
            this.argumentType.MinimumWidth = 6;
            this.argumentType.Name = "argumentType";
            this.argumentType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // argumentValue
            // 
            this.argumentValue.DataPropertyName = "ArgumentValue";
            this.argumentValue.FillWeight = 145.205F;
            this.argumentValue.HeaderText = "Argument Value";
            this.argumentValue.MinimumWidth = 6;
            this.argumentValue.Name = "argumentValue";
            // 
            // ArgumentEditor
            // 
            this.ArgumentEditor.FillWeight = 10.69519F;
            this.ArgumentEditor.HeaderText = "";
            this.ArgumentEditor.MinimumWidth = 4;
            this.ArgumentEditor.Name = "ArgumentEditor";
            this.ArgumentEditor.Text = "...";
            this.ArgumentEditor.UseColumnTextForButtonValue = true;
            // 
            // direction
            // 
            this.direction.DataPropertyName = "Direction";
            this.direction.FillWeight = 53.68988F;
            this.direction.HeaderText = "Direction";
            this.direction.MinimumWidth = 6;
            this.direction.Name = "direction";
            // 
            // imports
            // 
            this.imports.Controls.Add(this.tlpImports);
            this.imports.Location = new System.Drawing.Point(4, 4);
            this.imports.Margin = new System.Windows.Forms.Padding(2);
            this.imports.Name = "imports";
            this.imports.Size = new System.Drawing.Size(1070, 160);
            this.imports.TabIndex = 2;
            this.imports.Text = "Imports";
            this.imports.UseVisualStyleBackColor = true;
            // 
            // tlpImports
            // 
            this.tlpImports.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.tlpImports.ColumnCount = 1;
            this.tlpImports.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpImports.Controls.Add(this.cbxAllNamespaces, 0, 0);
            this.tlpImports.Controls.Add(this.lbxImportedNamespaces, 0, 1);
            this.tlpImports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpImports.Location = new System.Drawing.Point(0, 0);
            this.tlpImports.Margin = new System.Windows.Forms.Padding(2);
            this.tlpImports.Name = "tlpImports";
            this.tlpImports.RowCount = 2;
            this.tlpImports.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tlpImports.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpImports.Size = new System.Drawing.Size(1070, 160);
            this.tlpImports.TabIndex = 0;
            // 
            // cbxAllNamespaces
            // 
            this.cbxAllNamespaces.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbxAllNamespaces.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbxAllNamespaces.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxAllNamespaces.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxAllNamespaces.ForeColor = System.Drawing.Color.Black;
            this.cbxAllNamespaces.Location = new System.Drawing.Point(2, 2);
            this.cbxAllNamespaces.Margin = new System.Windows.Forms.Padding(2);
            this.cbxAllNamespaces.Name = "cbxAllNamespaces";
            this.cbxAllNamespaces.Size = new System.Drawing.Size(1066, 25);
            this.cbxAllNamespaces.TabIndex = 1;
            this.cbxAllNamespaces.SelectionChangeCommitted += new System.EventHandler(this.cbxAllNamespaces_SelectionChangeCommitted);
            this.cbxAllNamespaces.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbxAllNamespaces_KeyDown);
            this.cbxAllNamespaces.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbxAllNamespaces_KeyPress);
            // 
            // lbxImportedNamespaces
            // 
            this.lbxImportedNamespaces.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxImportedNamespaces.FormattingEnabled = true;
            this.lbxImportedNamespaces.ItemHeight = 17;
            this.lbxImportedNamespaces.Location = new System.Drawing.Point(2, 26);
            this.lbxImportedNamespaces.Margin = new System.Windows.Forms.Padding(2);
            this.lbxImportedNamespaces.Name = "lbxImportedNamespaces";
            this.lbxImportedNamespaces.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbxImportedNamespaces.Size = new System.Drawing.Size(1066, 132);
            this.lbxImportedNamespaces.Sorted = true;
            this.lbxImportedNamespaces.TabIndex = 2;
            this.lbxImportedNamespaces.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbxImportedNamespaces_KeyDown);
            this.lbxImportedNamespaces.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lbxImportedNamespaces_KeyPress);
            // 
            // pnlCommandHelper
            // 
            this.pnlCommandHelper.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.pnlCommandHelper.Controls.Add(this.flwRecentFiles);
            this.pnlCommandHelper.Controls.Add(this.lblFilesMissing);
            this.pnlCommandHelper.Controls.Add(this.pbRecentFiles);
            this.pnlCommandHelper.Controls.Add(this.pbLinks);
            this.pnlCommandHelper.Controls.Add(this.pbOpenBotsIcon);
            this.pnlCommandHelper.Controls.Add(this.lblRecentProjects);
            this.pnlCommandHelper.Controls.Add(this.lnkGitWiki);
            this.pnlCommandHelper.Controls.Add(this.lnkGitIssue);
            this.pnlCommandHelper.Controls.Add(this.lnkGitLatestReleases);
            this.pnlCommandHelper.Controls.Add(this.lnkGitProject);
            this.pnlCommandHelper.Controls.Add(this.lblWelcomeToOpenBots);
            this.pnlCommandHelper.Controls.Add(this.lblWelcomeDescription);
            this.pnlCommandHelper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCommandHelper.Location = new System.Drawing.Point(3, 3);
            this.pnlCommandHelper.Margin = new System.Windows.Forms.Padding(4);
            this.pnlCommandHelper.Name = "pnlCommandHelper";
            this.pnlCommandHelper.Size = new System.Drawing.Size(1063, 411);
            this.pnlCommandHelper.TabIndex = 7;
            // 
            // flwRecentFiles
            // 
            this.flwRecentFiles.AutoScroll = true;
            this.flwRecentFiles.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flwRecentFiles.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flwRecentFiles.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.flwRecentFiles.Location = new System.Drawing.Point(145, 291);
            this.flwRecentFiles.Margin = new System.Windows.Forms.Padding(4);
            this.flwRecentFiles.Name = "flwRecentFiles";
            this.flwRecentFiles.Size = new System.Drawing.Size(496, 180);
            this.flwRecentFiles.TabIndex = 12;
            this.flwRecentFiles.WrapContents = false;
            // 
            // lblFilesMissing
            // 
            this.lblFilesMissing.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilesMissing.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblFilesMissing.Location = new System.Drawing.Point(144, 289);
            this.lblFilesMissing.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFilesMissing.Name = "lblFilesMissing";
            this.lblFilesMissing.Size = new System.Drawing.Size(448, 80);
            this.lblFilesMissing.TabIndex = 16;
            this.lblFilesMissing.Text = "there were no script files found in your script directory.";
            this.lblFilesMissing.Visible = false;
            // 
            // pbRecentFiles
            // 
            this.pbRecentFiles.Image = ((System.Drawing.Image)(resources.GetObject("pbRecentFiles.Image")));
            this.pbRecentFiles.Location = new System.Drawing.Point(15, 262);
            this.pbRecentFiles.Margin = new System.Windows.Forms.Padding(4);
            this.pbRecentFiles.Name = "pbRecentFiles";
            this.pbRecentFiles.Size = new System.Drawing.Size(105, 105);
            this.pbRecentFiles.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbRecentFiles.TabIndex = 15;
            this.pbRecentFiles.TabStop = false;
            // 
            // pbLinks
            // 
            this.pbLinks.Image = ((System.Drawing.Image)(resources.GetObject("pbLinks.Image")));
            this.pbLinks.Location = new System.Drawing.Point(15, 135);
            this.pbLinks.Margin = new System.Windows.Forms.Padding(4);
            this.pbLinks.Name = "pbLinks";
            this.pbLinks.Size = new System.Drawing.Size(105, 105);
            this.pbLinks.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLinks.TabIndex = 14;
            this.pbLinks.TabStop = false;
            // 
            // pbOpenBotsIcon
            // 
            this.pbOpenBotsIcon.Image = ((System.Drawing.Image)(resources.GetObject("pbOpenBotsIcon.Image")));
            this.pbOpenBotsIcon.Location = new System.Drawing.Point(15, 10);
            this.pbOpenBotsIcon.Margin = new System.Windows.Forms.Padding(4);
            this.pbOpenBotsIcon.Name = "pbOpenBotsIcon";
            this.pbOpenBotsIcon.Size = new System.Drawing.Size(105, 105);
            this.pbOpenBotsIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbOpenBotsIcon.TabIndex = 13;
            this.pbOpenBotsIcon.TabStop = false;
            // 
            // lblRecentProjects
            // 
            this.lblRecentProjects.AutoSize = true;
            this.lblRecentProjects.Font = new System.Drawing.Font("Segoe UI Semilight", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecentProjects.ForeColor = System.Drawing.Color.AliceBlue;
            this.lblRecentProjects.Location = new System.Drawing.Point(138, 251);
            this.lblRecentProjects.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRecentProjects.Name = "lblRecentProjects";
            this.lblRecentProjects.Size = new System.Drawing.Size(154, 30);
            this.lblRecentProjects.TabIndex = 8;
            this.lblRecentProjects.Text = "Recent Projects";
            // 
            // lnkGitWiki
            // 
            this.lnkGitWiki.AutoSize = true;
            this.lnkGitWiki.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkGitWiki.ForeColor = System.Drawing.Color.White;
            this.lnkGitWiki.LinkColor = System.Drawing.Color.AliceBlue;
            this.lnkGitWiki.Location = new System.Drawing.Point(145, 211);
            this.lnkGitWiki.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lnkGitWiki.Name = "lnkGitWiki";
            this.lnkGitWiki.Size = new System.Drawing.Size(169, 20);
            this.lnkGitWiki.TabIndex = 6;
            this.lnkGitWiki.TabStop = true;
            this.lnkGitWiki.Text = "View Documentation/Wiki";
            this.lnkGitWiki.VisitedLinkColor = System.Drawing.Color.LightSteelBlue;
            this.lnkGitWiki.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGitWiki_LinkClicked);
            // 
            // lnkGitIssue
            // 
            this.lnkGitIssue.AutoSize = true;
            this.lnkGitIssue.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkGitIssue.ForeColor = System.Drawing.Color.White;
            this.lnkGitIssue.LinkColor = System.Drawing.Color.AliceBlue;
            this.lnkGitIssue.Location = new System.Drawing.Point(145, 186);
            this.lnkGitIssue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lnkGitIssue.Name = "lnkGitIssue";
            this.lnkGitIssue.Size = new System.Drawing.Size(251, 20);
            this.lnkGitIssue.TabIndex = 5;
            this.lnkGitIssue.TabStop = true;
            this.lnkGitIssue.Text = "Request Enhancement or Report a bug";
            this.lnkGitIssue.VisitedLinkColor = System.Drawing.Color.LightSteelBlue;
            this.lnkGitIssue.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGitIssue_LinkClicked);
            // 
            // lnkGitLatestReleases
            // 
            this.lnkGitLatestReleases.AutoSize = true;
            this.lnkGitLatestReleases.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkGitLatestReleases.ForeColor = System.Drawing.Color.White;
            this.lnkGitLatestReleases.LinkColor = System.Drawing.Color.AliceBlue;
            this.lnkGitLatestReleases.Location = new System.Drawing.Point(145, 161);
            this.lnkGitLatestReleases.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lnkGitLatestReleases.Name = "lnkGitLatestReleases";
            this.lnkGitLatestReleases.Size = new System.Drawing.Size(137, 20);
            this.lnkGitLatestReleases.TabIndex = 4;
            this.lnkGitLatestReleases.TabStop = true;
            this.lnkGitLatestReleases.Text = "View Latest Releases";
            this.lnkGitLatestReleases.VisitedLinkColor = System.Drawing.Color.LightSteelBlue;
            this.lnkGitLatestReleases.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGitLatestReleases_LinkClicked);
            // 
            // lnkGitProject
            // 
            this.lnkGitProject.AutoSize = true;
            this.lnkGitProject.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkGitProject.ForeColor = System.Drawing.Color.White;
            this.lnkGitProject.LinkColor = System.Drawing.Color.AliceBlue;
            this.lnkGitProject.Location = new System.Drawing.Point(145, 136);
            this.lnkGitProject.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lnkGitProject.Name = "lnkGitProject";
            this.lnkGitProject.Size = new System.Drawing.Size(153, 20);
            this.lnkGitProject.TabIndex = 3;
            this.lnkGitProject.TabStop = true;
            this.lnkGitProject.Text = "View Project on GitHub";
            this.lnkGitProject.VisitedLinkColor = System.Drawing.Color.LightSteelBlue;
            this.lnkGitProject.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGitProject_LinkClicked);
            // 
            // lblWelcomeToOpenBots
            // 
            this.lblWelcomeToOpenBots.AutoSize = true;
            this.lblWelcomeToOpenBots.Font = new System.Drawing.Font("Segoe UI Semilight", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWelcomeToOpenBots.ForeColor = System.Drawing.Color.AliceBlue;
            this.lblWelcomeToOpenBots.Location = new System.Drawing.Point(139, 5);
            this.lblWelcomeToOpenBots.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWelcomeToOpenBots.Name = "lblWelcomeToOpenBots";
            this.lblWelcomeToOpenBots.Size = new System.Drawing.Size(290, 30);
            this.lblWelcomeToOpenBots.TabIndex = 2;
            this.lblWelcomeToOpenBots.Text = "Welcome to OpenBots Studio!";
            // 
            // lblWelcomeDescription
            // 
            this.lblWelcomeDescription.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWelcomeDescription.ForeColor = System.Drawing.Color.White;
            this.lblWelcomeDescription.Location = new System.Drawing.Point(142, 40);
            this.lblWelcomeDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWelcomeDescription.Name = "lblWelcomeDescription";
            this.lblWelcomeDescription.Size = new System.Drawing.Size(350, 94);
            this.lblWelcomeDescription.TabIndex = 1;
            this.lblWelcomeDescription.Text = "Start building automation by double-clicking a command from the list to the left." +
    "";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 20;
            // 
            // clmCommand
            // 
            this.clmCommand.Text = "Script Commands";
            this.clmCommand.Width = 800;
            // 
            // pnlDivider
            // 
            this.pnlDivider.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.tlpControls.SetColumnSpan(this.pnlDivider, 4);
            this.pnlDivider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDivider.Location = new System.Drawing.Point(0, 108);
            this.pnlDivider.Margin = new System.Windows.Forms.Padding(0);
            this.pnlDivider.Name = "pnlDivider";
            this.pnlDivider.Size = new System.Drawing.Size(1417, 5);
            this.pnlDivider.TabIndex = 13;
            // 
            // msOpenBotsMenu
            // 
            this.msOpenBotsMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.tlpControls.SetColumnSpan(this.msOpenBotsMenu, 3);
            this.msOpenBotsMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.msOpenBotsMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.msOpenBotsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileActionsToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.scriptActionsToolStripMenuItem,
            this.recorderToolStripMenuItem,
            this.runToolStripMenuItem,
            this.debugToolStripMenuItem,
            this.breakpointToolStripMenuItem,
            this.packageManagerToolStripMenuItem,
            this.installDefaultToolStripMenuItem,
            this.tsSearchBox,
            this.tsSearchButton,
            this.tsSearchResult});
            this.msOpenBotsMenu.Location = new System.Drawing.Point(0, 0);
            this.msOpenBotsMenu.Name = "msOpenBotsMenu";
            this.msOpenBotsMenu.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.msOpenBotsMenu.Size = new System.Drawing.Size(1417, 30);
            this.msOpenBotsMenu.TabIndex = 1;
            this.msOpenBotsMenu.Text = "menuStrip1";
            // 
            // fileActionsToolStripMenuItem
            // 
            this.fileActionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addProjectToolStripMenuItem,
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.importFileToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.saveAllToolStripMenuItem,
            this.publishProjectToolStripMenuItem,
            this.restartApplicationToolStripMenuItem,
            this.closeApplicationToolStripMenuItem});
            this.fileActionsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.fileActionsToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_add_command;
            this.fileActionsToolStripMenuItem.Name = "fileActionsToolStripMenuItem";
            this.fileActionsToolStripMenuItem.Size = new System.Drawing.Size(100, 26);
            this.fileActionsToolStripMenuItem.Text = "File Actions";
            // 
            // addProjectToolStripMenuItem
            // 
            this.addProjectToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.addProjectToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_project;
            this.addProjectToolStripMenuItem.Name = "addProjectToolStripMenuItem";
            this.addProjectToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.addProjectToolStripMenuItem.Text = "Project Manager";
            this.addProjectToolStripMenuItem.Click += new System.EventHandler(this.addProjectToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.newToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.newToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_add_command;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.newToolStripMenuItem.Text = "New File";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.openToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_open;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.openToolStripMenuItem.Text = "Open Existing File";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // importFileToolStripMenuItem
            // 
            this.importFileToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.importFileToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_import;
            this.importFileToolStripMenuItem.Name = "importFileToolStripMenuItem";
            this.importFileToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.importFileToolStripMenuItem.Text = "Import File";
            this.importFileToolStripMenuItem.Click += new System.EventHandler(this.importFileToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.saveToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_save;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.saveAsToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_saveas;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // saveAllToolStripMenuItem
            // 
            this.saveAllToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.saveAllToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_saveall;
            this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.saveAllToolStripMenuItem.Text = "Save All";
            this.saveAllToolStripMenuItem.Click += new System.EventHandler(this.saveAllToolStripMenuItem_Click);
            // 
            // publishProjectToolStripMenuItem
            // 
            this.publishProjectToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.publishProjectToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_publish;
            this.publishProjectToolStripMenuItem.Name = "publishProjectToolStripMenuItem";
            this.publishProjectToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.publishProjectToolStripMenuItem.Text = "Publish Project";
            this.publishProjectToolStripMenuItem.Click += new System.EventHandler(this.publishProjectToolStripMenuItem_Click);
            // 
            // restartApplicationToolStripMenuItem
            // 
            this.restartApplicationToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.restartApplicationToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_restart;
            this.restartApplicationToolStripMenuItem.Name = "restartApplicationToolStripMenuItem";
            this.restartApplicationToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.restartApplicationToolStripMenuItem.Text = "Restart Application";
            this.restartApplicationToolStripMenuItem.Click += new System.EventHandler(this.restartApplicationToolStripMenuItem_Click);
            // 
            // closeApplicationToolStripMenuItem
            // 
            this.closeApplicationToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.closeApplicationToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_close;
            this.closeApplicationToolStripMenuItem.Name = "closeApplicationToolStripMenuItem";
            this.closeApplicationToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.closeApplicationToolStripMenuItem.Text = "Close Application";
            this.closeApplicationToolStripMenuItem.Click += new System.EventHandler(this.closeApplicationToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.variablesToolStripMenuItem,
            this.argumentsToolStripMenuItem,
            this.elementManagerToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.showSearchBarToolStripMenuItem,
            this.aboutOpenBotsToolStripMenuItem,
            this.extensionManagerToolStripMenuItem});
            this.optionsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.optionsToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_options;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(81, 26);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // variablesToolStripMenuItem
            // 
            this.variablesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.variablesToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_variable;
            this.variablesToolStripMenuItem.Name = "variablesToolStripMenuItem";
            this.variablesToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.variablesToolStripMenuItem.Text = "Variable Manager";
            this.variablesToolStripMenuItem.Click += new System.EventHandler(this.variablesToolStripMenuItem_Click);
            // 
            // argumentsToolStripMenuItem
            // 
            this.argumentsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.argumentsToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_variable;
            this.argumentsToolStripMenuItem.Name = "argumentsToolStripMenuItem";
            this.argumentsToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.argumentsToolStripMenuItem.Text = "Argument Manager";
            this.argumentsToolStripMenuItem.Click += new System.EventHandler(this.argumentsToolStripMenuItem_Click);
            // 
            // elementManagerToolStripMenuItem
            // 
            this.elementManagerToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.elementManagerToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_element;
            this.elementManagerToolStripMenuItem.Name = "elementManagerToolStripMenuItem";
            this.elementManagerToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.elementManagerToolStripMenuItem.Text = "Element Manager";
            this.elementManagerToolStripMenuItem.Click += new System.EventHandler(this.elementManagerToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.settingsToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_options;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.settingsToolStripMenuItem.Text = "Settings Manager";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // showSearchBarToolStripMenuItem
            // 
            this.showSearchBarToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.showSearchBarToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("showSearchBarToolStripMenuItem.Image")));
            this.showSearchBarToolStripMenuItem.Name = "showSearchBarToolStripMenuItem";
            this.showSearchBarToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.showSearchBarToolStripMenuItem.Text = "Show Search Bar";
            this.showSearchBarToolStripMenuItem.Click += new System.EventHandler(this.showSearchBarToolStripMenuItem_Click);
            // 
            // aboutOpenBotsToolStripMenuItem
            // 
            this.aboutOpenBotsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.aboutOpenBotsToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_project;
            this.aboutOpenBotsToolStripMenuItem.Name = "aboutOpenBotsToolStripMenuItem";
            this.aboutOpenBotsToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.aboutOpenBotsToolStripMenuItem.Text = "About OpenBots Studio";
            this.aboutOpenBotsToolStripMenuItem.Click += new System.EventHandler(this.aboutOpenBotsToolStripMenuItem_Click);
            // 
            // extensionManagerToolStripMenuItem
            // 
            this.extensionManagerToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.extensionManagerToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_extensions;
            this.extensionManagerToolStripMenuItem.Name = "extensionManagerToolStripMenuItem";
            this.extensionManagerToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.extensionManagerToolStripMenuItem.Text = "Extensions Manager";
            this.extensionManagerToolStripMenuItem.Click += new System.EventHandler(this.extensionManagerToolStripMenuItem_Click);
            // 
            // scriptActionsToolStripMenuItem
            // 
            this.scriptActionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scheduleToolStripMenuItem,
            this.shortcutMenuToolStripMenuItem,
            this.clearAllToolStripMenuItem});
            this.scriptActionsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.scriptActionsToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_record;
            this.scriptActionsToolStripMenuItem.Name = "scriptActionsToolStripMenuItem";
            this.scriptActionsToolStripMenuItem.Size = new System.Drawing.Size(112, 26);
            this.scriptActionsToolStripMenuItem.Text = "Script Actions";
            // 
            // scheduleToolStripMenuItem
            // 
            this.scheduleToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.scheduleToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_schedule;
            this.scheduleToolStripMenuItem.Name = "scheduleToolStripMenuItem";
            this.scheduleToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.scheduleToolStripMenuItem.Text = "Schedule";
            this.scheduleToolStripMenuItem.Click += new System.EventHandler(this.scheduleToolStripMenuItem_Click);
            // 
            // shortcutMenuToolStripMenuItem
            // 
            this.shortcutMenuToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.shortcutMenuToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_shortcut;
            this.shortcutMenuToolStripMenuItem.Name = "shortcutMenuToolStripMenuItem";
            this.shortcutMenuToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.shortcutMenuToolStripMenuItem.Text = "Shortcut Menu";
            this.shortcutMenuToolStripMenuItem.Click += new System.EventHandler(this.shortcutMenuToolStripMenuItem_Click);
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.clearAllToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_clear;
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.clearAllToolStripMenuItem.Text = "Clear All Commands";
            this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.clearAllToolStripMenuItem_Click);
            // 
            // recorderToolStripMenuItem
            // 
            this.recorderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.elementRecorderToolStripMenuItem,
            this.uiRecorderToolStripMenuItem,
            this.uiAdvancedRecorderToolStripMenuItem});
            this.recorderToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.recorderToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_element_recorder;
            this.recorderToolStripMenuItem.Name = "recorderToolStripMenuItem";
            this.recorderToolStripMenuItem.Size = new System.Drawing.Size(116, 26);
            this.recorderToolStripMenuItem.Text = "Recorder Tools";
            // 
            // elementRecorderToolStripMenuItem
            // 
            this.elementRecorderToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.elementRecorderToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_element_recorder;
            this.elementRecorderToolStripMenuItem.Name = "elementRecorderToolStripMenuItem";
            this.elementRecorderToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.elementRecorderToolStripMenuItem.Text = "Web Element Recorder";
            this.elementRecorderToolStripMenuItem.Click += new System.EventHandler(this.elementRecorderToolStripMenuItem_Click);
            // 
            // uiRecorderToolStripMenuItem
            // 
            this.uiRecorderToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.uiRecorderToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_element_recorder;
            this.uiRecorderToolStripMenuItem.Name = "uiRecorderToolStripMenuItem";
            this.uiRecorderToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.uiRecorderToolStripMenuItem.Text = "UI Recorder";
            this.uiRecorderToolStripMenuItem.Click += new System.EventHandler(this.uiRecorderToolStripMenuItem_Click);
            // 
            // uiAdvancedRecorderToolStripMenuItem
            // 
            this.uiAdvancedRecorderToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.uiAdvancedRecorderToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_element_recorder;
            this.uiAdvancedRecorderToolStripMenuItem.Name = "uiAdvancedRecorderToolStripMenuItem";
            this.uiAdvancedRecorderToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.uiAdvancedRecorderToolStripMenuItem.Text = "Advanced UI Recorder";
            this.uiAdvancedRecorderToolStripMenuItem.Click += new System.EventHandler(this.uiAdvancedRecorderToolStripMenuItem_Click);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.runToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_run;
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(60, 26);
            this.runToolStripMenuItem.Text = "Run";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.debugToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_debug;
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(74, 26);
            this.debugToolStripMenuItem.Text = "Debug";
            this.debugToolStripMenuItem.Click += new System.EventHandler(this.debugToolStripMenuItem_Click);
            // 
            // breakpointToolStripMenuItem
            // 
            this.breakpointToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.breakpointToolStripMenuItem.Image = global::OpenBots.Properties.Resources.action_bar_breakpoint;
            this.breakpointToolStripMenuItem.Name = "breakpointToolStripMenuItem";
            this.breakpointToolStripMenuItem.Size = new System.Drawing.Size(96, 26);
            this.breakpointToolStripMenuItem.Text = "Breakpoint";
            this.breakpointToolStripMenuItem.Click += new System.EventHandler(this.breakpointToolStripMenuItem_Click);
            // 
            // packageManagerToolStripMenuItem
            // 
            this.packageManagerToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.packageManagerToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("packageManagerToolStripMenuItem.Image")));
            this.packageManagerToolStripMenuItem.Name = "packageManagerToolStripMenuItem";
            this.packageManagerToolStripMenuItem.Size = new System.Drawing.Size(133, 26);
            this.packageManagerToolStripMenuItem.Text = "Package Manager";
            this.packageManagerToolStripMenuItem.Click += new System.EventHandler(this.packageManagerToolStripMenuItem_Click);
            // 
            // installDefaultToolStripMenuItem
            // 
            this.installDefaultToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.installDefaultToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("installDefaultToolStripMenuItem.Image")));
            this.installDefaultToolStripMenuItem.Name = "installDefaultToolStripMenuItem";
            this.installDefaultToolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.installDefaultToolStripMenuItem.Text = "Install Default Commands";
            this.installDefaultToolStripMenuItem.Visible = false;
            this.installDefaultToolStripMenuItem.Click += new System.EventHandler(this.installDefaultToolStripMenuItem_Click);
            // 
            // tsSearchBox
            // 
            this.tsSearchBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tsSearchBox.Name = "tsSearchBox";
            this.tsSearchBox.Size = new System.Drawing.Size(100, 26);
            this.tsSearchBox.Visible = false;
            this.tsSearchBox.TextChanged += new System.EventHandler(this.txtScriptSearch_TextChanged);
            // 
            // tsSearchButton
            // 
            this.tsSearchButton.ForeColor = System.Drawing.Color.White;
            this.tsSearchButton.Image = ((System.Drawing.Image)(resources.GetObject("tsSearchButton.Image")));
            this.tsSearchButton.Name = "tsSearchButton";
            this.tsSearchButton.Size = new System.Drawing.Size(32, 26);
            this.tsSearchButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsSearchButton.Visible = false;
            this.tsSearchButton.Click += new System.EventHandler(this.pbSearch_Click);
            // 
            // tsSearchResult
            // 
            this.tsSearchResult.ForeColor = System.Drawing.Color.White;
            this.tsSearchResult.Name = "tsSearchResult";
            this.tsSearchResult.Size = new System.Drawing.Size(12, 26);
            this.tsSearchResult.Visible = false;
            // 
            // tlpControls
            // 
            this.tlpControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.tlpControls.ColumnCount = 3;
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 255F));
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 524F));
            this.tlpControls.Controls.Add(this.msOpenBotsMenu, 0, 0);
            this.tlpControls.Controls.Add(this.pnlDivider, 0, 2);
            this.tlpControls.Controls.Add(this.splitContainerStudioControls, 0, 3);
            this.tlpControls.Controls.Add(this.pnlStatus, 0, 4);
            this.tlpControls.Controls.Add(this.pnlControlContainer, 0, 1);
            this.tlpControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpControls.Location = new System.Drawing.Point(0, 0);
            this.tlpControls.Margin = new System.Windows.Forms.Padding(0);
            this.tlpControls.Name = "tlpControls";
            this.tlpControls.RowCount = 5;
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tlpControls.Size = new System.Drawing.Size(1417, 734);
            this.tlpControls.TabIndex = 2;
            // 
            // cmsProjectFileActions
            // 
            this.cmsProjectFileActions.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.cmsProjectFileActions.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsProjectFileActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCopyFile,
            this.tsmiDeleteFile,
            this.tsmiRenameFile});
            this.cmsProjectFileActions.Name = "cmsProjectFileActions";
            this.cmsProjectFileActions.Size = new System.Drawing.Size(166, 82);
            // 
            // tsmiCopyFile
            // 
            this.tsmiCopyFile.Image = global::OpenBots.Properties.Resources.copy;
            this.tsmiCopyFile.Name = "tsmiCopyFile";
            this.tsmiCopyFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.tsmiCopyFile.Size = new System.Drawing.Size(165, 26);
            this.tsmiCopyFile.Text = "Copy";
            this.tsmiCopyFile.Click += new System.EventHandler(this.tsmiCopyFile_Click);
            // 
            // tsmiDeleteFile
            // 
            this.tsmiDeleteFile.Image = global::OpenBots.Properties.Resources.delete;
            this.tsmiDeleteFile.Name = "tsmiDeleteFile";
            this.tsmiDeleteFile.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.tsmiDeleteFile.Size = new System.Drawing.Size(165, 26);
            this.tsmiDeleteFile.Text = "Delete";
            this.tsmiDeleteFile.Click += new System.EventHandler(this.tsmiDeleteFile_Click);
            // 
            // tsmiRenameFile
            // 
            this.tsmiRenameFile.Image = global::OpenBots.Properties.Resources.create;
            this.tsmiRenameFile.Name = "tsmiRenameFile";
            this.tsmiRenameFile.Size = new System.Drawing.Size(165, 26);
            this.tsmiRenameFile.Text = "Rename";
            this.tsmiRenameFile.Click += new System.EventHandler(this.tsmiRenameFile_Click);
            // 
            // cmsScriptTabActions
            // 
            this.cmsScriptTabActions.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.cmsScriptTabActions.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsScriptTabActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCloseTab,
            this.tsmiCloseAllButThis,
            this.tsmiReloadTab,
            this.reloadAllTabsToolStripMenuItem});
            this.cmsScriptTabActions.Name = "cmsScriptTabActions";
            this.cmsScriptTabActions.Size = new System.Drawing.Size(189, 100);
            // 
            // tsmiCloseTab
            // 
            this.tsmiCloseTab.Name = "tsmiCloseTab";
            this.tsmiCloseTab.Size = new System.Drawing.Size(188, 24);
            this.tsmiCloseTab.Text = "Close Tab";
            this.tsmiCloseTab.Click += new System.EventHandler(this.tsmiCloseTab_Click);
            // 
            // tsmiCloseAllButThis
            // 
            this.tsmiCloseAllButThis.Name = "tsmiCloseAllButThis";
            this.tsmiCloseAllButThis.Size = new System.Drawing.Size(188, 24);
            this.tsmiCloseAllButThis.Text = "Close All But This";
            this.tsmiCloseAllButThis.Click += new System.EventHandler(this.tsmiCloseAllButThis_Click);
            // 
            // tsmiReloadTab
            // 
            this.tsmiReloadTab.Name = "tsmiReloadTab";
            this.tsmiReloadTab.Size = new System.Drawing.Size(188, 24);
            this.tsmiReloadTab.Text = "Reload Tab";
            this.tsmiReloadTab.Click += new System.EventHandler(this.tsmiReloadTab_Click);
            // 
            // reloadAllTabsToolStripMenuItem
            // 
            this.reloadAllTabsToolStripMenuItem.Name = "reloadAllTabsToolStripMenuItem";
            this.reloadAllTabsToolStripMenuItem.Size = new System.Drawing.Size(188, 24);
            this.reloadAllTabsToolStripMenuItem.Text = "Reload All Tabs";
            this.reloadAllTabsToolStripMenuItem.Click += new System.EventHandler(this.reloadAllTabsToolStripMenuItem_Click);
            // 
            // cmsProjectMainFolderActions
            // 
            this.cmsProjectMainFolderActions.AllowDrop = true;
            this.cmsProjectMainFolderActions.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.cmsProjectMainFolderActions.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsProjectMainFolderActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiMainNewFolder,
            this.tsmiMainNewScriptFile,
            this.tsmiMainPasteFolder,
            this.tsmiMainRenameFolder});
            this.cmsProjectMainFolderActions.Name = "cmsProjectMainFolderActions";
            this.cmsProjectMainFolderActions.Size = new System.Drawing.Size(181, 108);
            // 
            // tsmiMainRenameFolder
            // 
            this.tsmiMainRenameFolder.Image = global::OpenBots.Properties.Resources.create;
            this.tsmiMainRenameFolder.Name = "tsmiMainRenameFolder";
            this.tsmiMainRenameFolder.Size = new System.Drawing.Size(180, 26);
            this.tsmiMainRenameFolder.Text = "Rename Project";
            this.tsmiMainRenameFolder.Click += new System.EventHandler(this.tsmiRenameFolder_Click);
            // 
            // imgListIntellisense
            // 
            this.imgListIntellisense.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListIntellisense.ImageStream")));
            this.imgListIntellisense.TransparentColor = System.Drawing.Color.Transparent;
            this.imgListIntellisense.Images.SetKeyName(0, "intellisense-method.png");
            this.imgListIntellisense.Images.SetKeyName(1, "intellisense-extensionmethod.png");
            this.imgListIntellisense.Images.SetKeyName(2, "intellisense-property.png");
            this.imgListIntellisense.Images.SetKeyName(3, "intellisense-class.png");
            this.imgListIntellisense.Images.SetKeyName(4, "intellisense-enumerator.png");
            this.imgListIntellisense.Images.SetKeyName(5, "intellisense-event.png");
            this.imgListIntellisense.Images.SetKeyName(6, "intellisense-interface.png");
            this.imgListIntellisense.Images.SetKeyName(7, "intellisense-namespace.png");
            this.imgListIntellisense.Images.SetKeyName(8, "intellisense-structure.png");
            this.imgListIntellisense.Images.SetKeyName(9, "intellisense-field.png");
            this.imgListIntellisense.Images.SetKeyName(10, "intellisense-delegate.png");
            this.imgListIntellisense.Images.SetKeyName(11, "intellisense-keyword.png");
            this.imgListIntellisense.Images.SetKeyName(12, "intellisense-typeparameter.png");
            this.imgListIntellisense.Images.SetKeyName(13, "intellisense-parameter.png");
            this.imgListIntellisense.Images.SetKeyName(14, "intellisense-constant.png");
            // 
            // frmScriptBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1417, 734);
            this.Controls.Add(this.tlpControls);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.msOpenBotsMenu;
            this.Name = "frmScriptBuilder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NexBots Studio";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmScriptBuilder_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmScriptBuilder_FormClosed);
            this.Load += new System.EventHandler(this.frmScriptBuilder_LoadAsync);
            this.Shown += new System.EventHandler(this.frmScriptBuilder_Shown);
            this.SizeChanged += new System.EventHandler(this.frmScriptBuilder_SizeChanged);
            this.Resize += new System.EventHandler(this.frmScriptBuilder_Resize);
            this.cmsProjectFolderActions.ResumeLayout(false);
            this.cmsScriptActions.ResumeLayout(false);
            this.cmsNotifyTrayIcon.ResumeLayout(false);
            this.pnlControlContainer.ResumeLayout(false);
            this.grpSearch.ResumeLayout(false);
            this.grpSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSearch)).EndInit();
            this.grpSaveClose.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRestart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClose)).EndInit();
            this.grpFileActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnPackageManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSaveAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnPublishProject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnProject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSaveAs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnNew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpen)).EndInit();
            this.grpRecordRun.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnBreakpoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRecordAdvancedUISequence)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRecordElementSequence)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRunScript)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRecordUISequence)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnDebugScript)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnScheduleManagement)).EndInit();
            this.grpVariable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnExtensionsManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAddArgument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAddElement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClearAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAddVariable)).EndInit();
            this.splitContainerStudioControls.Panel1.ResumeLayout(false);
            this.splitContainerStudioControls.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerStudioControls)).EndInit();
            this.splitContainerStudioControls.ResumeLayout(false);
            this.uiPaneTabs.ResumeLayout(false);
            this.tpProject.ResumeLayout(false);
            this.tlpProject.ResumeLayout(false);
            this.pnlProjectButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCollapse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpenDirectory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnExpand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRefresh)).EndInit();
            this.tpCommands.ResumeLayout(false);
            this.tlpCommands.ResumeLayout(false);
            this.pnlCommandSearch.ResumeLayout(false);
            this.pnlCommandSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnReloadCommands)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClearCommandSearch)).EndInit();
            this.splitContainerScript.Panel1.ResumeLayout(false);
            this.splitContainerScript.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerScript)).EndInit();
            this.splitContainerScript.ResumeLayout(false);
            this.uiVariableArgumentTabs.ResumeLayout(false);
            this.variables.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariables)).EndInit();
            this.arguments.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvArguments)).EndInit();
            this.imports.ResumeLayout(false);
            this.tlpImports.ResumeLayout(false);
            this.pnlCommandHelper.ResumeLayout(false);
            this.pnlCommandHelper.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRecentFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLinks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOpenBotsIcon)).EndInit();
            this.msOpenBotsMenu.ResumeLayout(false);
            this.msOpenBotsMenu.PerformLayout();
            this.tlpControls.ResumeLayout(false);
            this.tlpControls.PerformLayout();
            this.cmsProjectFileActions.ResumeLayout(false);
            this.cmsScriptTabActions.ResumeLayout(false);
            this.cmsProjectMainFolderActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmrNotify;
        private System.Windows.Forms.ContextMenuStrip cmsScriptActions;
        private System.Windows.Forms.ToolStripMenuItem enableSelectedCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableSelectedCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addRemoveBreakpointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copySelectedCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteSelectedCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutSelectedCodeToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyTrayIcon;
        private System.Windows.Forms.ToolStripMenuItem viewCodeToolStripMenuItem;
        private ContextMenuStrip cmsProjectFolderActions;
        private ToolStripMenuItem tsmiDeleteFolder;
        private ToolStripMenuItem tsmiCopyFolder;
        private ToolStripMenuItem tsmiRenameFolder;
        private Panel pnlControlContainer;
        private TableLayoutPanel tlpControls;
        private CustomControls.CustomUIControls.UIMenuStrip msOpenBotsMenu;
        private ToolStripMenuItem fileActionsToolStripMenuItem;
        private ToolStripMenuItem addProjectToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem importFileToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem restartApplicationToolStripMenuItem;
        private ToolStripMenuItem closeApplicationToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem variablesToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem showSearchBarToolStripMenuItem;
        private ToolStripMenuItem aboutOpenBotsToolStripMenuItem;
        private ToolStripMenuItem scriptActionsToolStripMenuItem;
        private ToolStripMenuItem scheduleToolStripMenuItem;
        private ToolStripMenuItem debugToolStripMenuItem;
        private ToolStripTextBox tsSearchBox;
        private ToolStripMenuItem tsSearchButton;
        private ToolStripMenuItem tsSearchResult;
        private Panel pnlDivider;
        private CustomControls.CustomUIControls.UISplitContainer splitContainerStudioControls;
        private TabControl uiPaneTabs;
        private TabPage tpProject;
        private CustomControls.CustomUIControls.UITreeView tvProject;
        private TabPage tpCommands;
        private CustomControls.CustomUIControls.UITreeView tvCommands;
        private Panel pnlStatus;
        private CustomControls.CustomUIControls.UIGroupBox grpSearch;
        private PictureBox pbSearch;
        private Label lblCurrentlyViewing;
        private Label lblTotalResults;
        private TextBox txtScriptSearch;
        private CustomControls.CustomUIControls.UIGroupBox grpSaveClose;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnRestart;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnClose;
        private CustomControls.CustomUIControls.UIGroupBox grpFileActions;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnProject;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnImport;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnSaveAs;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnSave;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnNew;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnOpen;
        private CustomControls.CustomUIControls.UIGroupBox grpRecordRun;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnRecordUISequence;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnDebugScript;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnScheduleManagement;
        private CustomControls.CustomUIControls.UIGroupBox grpVariable;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnClearAll;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnSettings;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnAddVariable;
        private ToolStripMenuItem tsmiNewFolder;
        private ToolStripMenuItem tsmiMainNewFolder;
        private ToolStripMenuItem tsmiNewScriptFile;
        private ToolStripMenuItem tsmiMainNewScriptFile;
        private ContextMenuStrip cmsProjectFileActions;
        private ToolStripMenuItem tsmiDeleteFile;
        private ToolStripMenuItem tsmiCopyFile;
        private ToolStripMenuItem tsmiRenameFile;
        private ToolStripMenuItem tsmiPasteFolder;
        private ToolStripMenuItem tsmiMainPasteFolder;
        private ImageList imgListProjectPane;
        public CustomControls.CustomUIControls.UITabControl uiScriptTabControl;
        private Panel pnlCommandHelper;
        private CustomControls.CustomUIControls.UIFlowLayoutPanel flwRecentFiles;
        private Label lblFilesMissing;
        private PictureBox pbRecentFiles;
        private PictureBox pbLinks;
        private PictureBox pbOpenBotsIcon;
        private Label lblRecentProjects;
        private LinkLabel lnkGitWiki;
        private LinkLabel lnkGitIssue;
        private LinkLabel lnkGitLatestReleases;
        private LinkLabel lnkGitProject;
        private Label lblWelcomeToOpenBots;
        private Label lblWelcomeDescription;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader clmCommand;
        private ContextMenuStrip cmsScriptTabActions;
        private ToolStripMenuItem tsmiCloseTab;
        private ToolStripMenuItem tsmiCloseAllButThis;
        private ContextMenuStrip cmsProjectMainFolderActions;
        private ToolStripMenuItem runToolStripMenuItem;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnRunScript;
        private ToolStripMenuItem elementManagerToolStripMenuItem;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnRecordElementSequence;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnAddElement;
        private TableLayoutPanel tlpProject;
        private Panel pnlProjectButtons;
        private CustomControls.CustomUIControls.UIIconButton uiBtnCollapse;
        private CustomControls.CustomUIControls.UIIconButton uiBtnOpenDirectory;
        private CustomControls.CustomUIControls.UIIconButton uiBtnExpand;
        private CustomControls.CustomUIControls.UIIconButton uiBtnRefresh;
        private TableLayoutPanel tlpCommands;
        private Panel pnlCommandSearch;
        private TextBox txtCommandSearch;
        private ToolStripMenuItem recorderToolStripMenuItem;
        private ToolStripMenuItem uiRecorderToolStripMenuItem;
        private ToolStripMenuItem elementRecorderToolStripMenuItem;
        private ToolStripMenuItem uiAdvancedRecorderToolStripMenuItem;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnRecordAdvancedUISequence;
        private CustomControls.CustomUIControls.UIIconButton uiBtnClearCommandSearch;
        private ToolStripMenuItem breakpointToolStripMenuItem;
        private Core.UI.Controls.UIPictureButton uiBtnBreakpoint;
        private ToolStripMenuItem deleteSelectedCodeToolStripMenuItem;
        private ToolStripMenuItem publishProjectToolStripMenuItem;
        private Core.UI.Controls.UIPictureButton uiBtnPublishProject;
        private ToolStripMenuItem saveAllToolStripMenuItem;
        private Core.UI.Controls.UIPictureButton uiBtnSaveAll;
        private ToolStripMenuItem shortcutMenuToolStripMenuItem;
        private ToolStripMenuItem openShortcutMenuToolStripMenuItem;
        private ToolStripMenuItem clearAllToolStripMenuItem;
        private ToolStripMenuItem packageManagerToolStripMenuItem;
        private Core.UI.Controls.UIPictureButton uiBtnPackageManager;
        private CustomControls.CustomUIControls.UIIconButton uiBtnReloadCommands;
        private ToolStripMenuItem tsmiReloadTab;
        private ToolTip ttScriptBuilder;
        private ToolStripMenuItem installDefaultToolStripMenuItem;
        private ToolStripMenuItem reloadAllTabsToolStripMenuItem;
        private ToolStripMenuItem tsmiMainRenameFolder;
        private ToolStripMenuItem argumentsToolStripMenuItem;
        private CustomControls.CustomUIControls.UISplitContainer splitContainerScript;
        private CustomControls.CustomUIControls.UITabControl uiVariableArgumentTabs;
        private TabPage variables;
        private TabPage arguments;
        private UIDataGridView dgvVariables;
        private UIDataGridView dgvArguments;
        private Core.UI.Controls.UIPictureButton uiBtnAddArgument;
        private ToolStripMenuItem runFromThisCommandToolStripMenuItem;
        private TabPage imports;
        private TableLayoutPanel tlpImports;
        private ComboBox cbxAllNamespaces;
        private ListBox lbxImportedNamespaces;
        private ToolStripMenuItem extensionManagerToolStripMenuItem;
        private Core.UI.Controls.UIPictureButton uiBtnExtensionsManager;
        private ToolStripMenuItem debugFromThisCommandToolStripMenuItem;
        private ImageList imgListIntellisense;
        private ContextMenuStrip cmsNotifyTrayIcon;
        private ToolStripMenuItem tsmiNotifyIconQuit;
        private ToolStripMenuItem tsmiNotifyIconStudioSettings;
        private ToolStripMenuItem tsmiNotifyIconPackageManager;
        private ToolStripMenuItem tsmiNotifyIconRun;
        private DataGridViewTextBoxColumn variableName;
        private DataGridViewComboBoxColumn variableType;
        private DataGridViewTextBoxColumn variableValue;
        private DataGridViewButtonColumn VariableEditor;
        private DataGridViewTextBoxColumn argumentName;
        private DataGridViewComboBoxColumn argumentType;
        private DataGridViewTextBoxColumn argumentValue;
        private DataGridViewButtonColumn ArgumentEditor;
        private DataGridViewComboBoxColumn direction;
    }
}

