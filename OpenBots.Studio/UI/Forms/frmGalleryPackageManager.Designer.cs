using OpenBots.UI.CustomControls.CustomUIControls;
using static OpenBots.UI.CustomControls.CustomUIControls.UIListBox;

namespace OpenBots.UI.Forms
{
    partial class frmGalleryPackageManager
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
            OpenBots.Core.Utilities.FormsUtilities.Theme theme1 = new OpenBots.Core.Utilities.FormsUtilities.Theme();
            OpenBots.Core.Utilities.FormsUtilities.Theme theme2 = new OpenBots.Core.Utilities.FormsUtilities.Theme();
            OpenBots.Core.Utilities.FormsUtilities.Theme theme3 = new OpenBots.Core.Utilities.FormsUtilities.Theme();
            OpenBots.Core.Utilities.FormsUtilities.Theme theme5 = new OpenBots.Core.Utilities.FormsUtilities.Theme();
            OpenBots.Core.Utilities.FormsUtilities.Theme theme4 = new OpenBots.Core.Utilities.FormsUtilities.Theme();
            OpenBots.Core.Utilities.FormsUtilities.Theme theme6 = new OpenBots.Core.Utilities.FormsUtilities.Theme();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGalleryPackageManager));
            this.tlpPackageLayout = new System.Windows.Forms.TableLayoutPanel();
            this.pnlNugetPackages = new OpenBots.UI.CustomControls.CustomUIControls.UIPanel();
            this.tpbLoadingSpinner = new OpenBots.UI.CustomControls.CustomUIControls.UITransparentPictureBox();
            this.lbxNugetPackages = new OpenBots.UI.CustomControls.CustomUIControls.UIListBox();
            this.lblError = new System.Windows.Forms.Label();
            this.pnlProjectVersion = new OpenBots.UI.CustomControls.CustomUIControls.UIPanel();
            this.txtInstalled = new System.Windows.Forms.TextBox();
            this.btnUninstall = new System.Windows.Forms.Button();
            this.lblInstalled = new System.Windows.Forms.Label();
            this.btnInstall = new System.Windows.Forms.Button();
            this.pbxOBStudio = new System.Windows.Forms.PictureBox();
            this.cbxVersion = new System.Windows.Forms.ComboBox();
            this.lblVersionTitleLabel = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlProjectSearch = new OpenBots.UI.CustomControls.CustomUIControls.UIPanel();
            this.chbxIncludePrerelease = new System.Windows.Forms.CheckBox();
            this.pbxPackageCategory = new System.Windows.Forms.PictureBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.lblPackageCategory = new System.Windows.Forms.Label();
            this.txtSampleSearch = new System.Windows.Forms.TextBox();
            this.pnlProjectDetails = new OpenBots.UI.CustomControls.CustomUIControls.UIPanel();
            this.tlpMetadata = new System.Windows.Forms.TableLayoutPanel();
            this.pnlMetadata = new OpenBots.UI.CustomControls.CustomUIControls.UIPanel();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblPublishDate = new System.Windows.Forms.Label();
            this.lblDownloadsLabel = new System.Windows.Forms.Label();
            this.lblPublishDateLabel = new System.Windows.Forms.Label();
            this.lblVersionLabel = new System.Windows.Forms.Label();
            this.lblAuthorsLabel = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblDescriptionLabel = new System.Windows.Forms.Label();
            this.lblAuthors = new System.Windows.Forms.Label();
            this.lblDependenciesLabel = new System.Windows.Forms.Label();
            this.lblDownloads = new System.Windows.Forms.Label();
            this.lblLicenseURLLabel = new System.Windows.Forms.Label();
            this.lblProjectURLLabel = new System.Windows.Forms.Label();
            this.llblProjectURL = new System.Windows.Forms.LinkLabel();
            this.llblLicenseURL = new System.Windows.Forms.LinkLabel();
            this.lvDependencies = new System.Windows.Forms.ListView();
            this.DependencyName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Range = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlFinishButtons = new OpenBots.UI.CustomControls.CustomUIControls.UIPanel();
            this.uiBtnCancel = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.tvPackageFeeds = new OpenBots.UI.CustomControls.CustomUIControls.UITreeView();
            this.imlNodes = new System.Windows.Forms.ImageList(this.components);
            this.btnSyncCommandsAndStudio = new System.Windows.Forms.Button();
            this.tlpPackageLayout.SuspendLayout();
            this.pnlNugetPackages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tpbLoadingSpinner)).BeginInit();
            this.pnlProjectVersion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxOBStudio)).BeginInit();
            this.pnlProjectSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxPackageCategory)).BeginInit();
            this.pnlProjectDetails.SuspendLayout();
            this.tlpMetadata.SuspendLayout();
            this.pnlMetadata.SuspendLayout();
            this.pnlFinishButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpPackageLayout
            // 
            this.tlpPackageLayout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.tlpPackageLayout.ColumnCount = 3;
            this.tlpPackageLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tlpPackageLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPackageLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPackageLayout.Controls.Add(this.pnlNugetPackages, 1, 1);
            this.tlpPackageLayout.Controls.Add(this.lblError, 1, 2);
            this.tlpPackageLayout.Controls.Add(this.pnlProjectVersion, 2, 0);
            this.tlpPackageLayout.Controls.Add(this.pnlProjectSearch, 1, 0);
            this.tlpPackageLayout.Controls.Add(this.pnlProjectDetails, 2, 1);
            this.tlpPackageLayout.Controls.Add(this.pnlFinishButtons, 2, 2);
            this.tlpPackageLayout.Controls.Add(this.tvPackageFeeds, 0, 1);
            this.tlpPackageLayout.Controls.Add(this.btnSyncCommandsAndStudio, 0, 0);
            this.tlpPackageLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPackageLayout.Location = new System.Drawing.Point(0, 0);
            this.tlpPackageLayout.Name = "tlpPackageLayout";
            this.tlpPackageLayout.RowCount = 3;
            this.tlpPackageLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 153F));
            this.tlpPackageLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPackageLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.tlpPackageLayout.Size = new System.Drawing.Size(1449, 973);
            this.tlpPackageLayout.TabIndex = 36;
            // 
            // pnlNugetPackages
            // 
            this.pnlNugetPackages.Controls.Add(this.tpbLoadingSpinner);
            this.pnlNugetPackages.Controls.Add(this.lbxNugetPackages);
            this.pnlNugetPackages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlNugetPackages.Location = new System.Drawing.Point(303, 156);
            this.pnlNugetPackages.Name = "pnlNugetPackages";
            this.pnlNugetPackages.Size = new System.Drawing.Size(568, 732);
            this.pnlNugetPackages.TabIndex = 37;
            theme1.BgGradientEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            theme1.BgGradientStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.pnlNugetPackages.Theme = theme1;
            // 
            // tpbLoadingSpinner
            // 
            this.tpbLoadingSpinner.BackColor = System.Drawing.Color.Transparent;
            this.tpbLoadingSpinner.ErrorImage = null;
            this.tpbLoadingSpinner.Image = global::OpenBots.Properties.Resources.loading_spinner;
            this.tpbLoadingSpinner.InitialImage = null;
            this.tpbLoadingSpinner.Location = new System.Drawing.Point(102, 128);
            this.tpbLoadingSpinner.Name = "tpbLoadingSpinner";
            this.tpbLoadingSpinner.Size = new System.Drawing.Size(360, 266);
            this.tpbLoadingSpinner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.tpbLoadingSpinner.TabIndex = 41;
            this.tpbLoadingSpinner.TabStop = false;
            // 
            // lbxNugetPackages
            // 
            this.lbxNugetPackages.AutoScroll = true;
            this.lbxNugetPackages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbxNugetPackages.ClickedItem = null;
            this.lbxNugetPackages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxNugetPackages.DoubleClickedItem = null;
            this.lbxNugetPackages.LastSelectedItem = null;
            this.lbxNugetPackages.Location = new System.Drawing.Point(0, 0);
            this.lbxNugetPackages.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lbxNugetPackages.Name = "lbxNugetPackages";
            this.lbxNugetPackages.Size = new System.Drawing.Size(568, 732);
            this.lbxNugetPackages.TabIndex = 37;
            this.lbxNugetPackages.ItemClick += new OpenBots.UI.CustomControls.CustomUIControls.UIListBox.ItemClickEventHandler(this.lbxNugetPackages_ItemClick);
            this.lbxNugetPackages.ListBoxScroll += new System.Windows.Forms.ScrollEventHandler(this.lbxNugetPackages_ListBoxScroll);
            // 
            // lblError
            // 
            this.lblError.BackColor = System.Drawing.Color.Transparent;
            this.lblError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblError.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblError.Location = new System.Drawing.Point(304, 891);
            this.lblError.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(566, 82);
            this.lblError.TabIndex = 39;
            // 
            // pnlProjectVersion
            // 
            this.pnlProjectVersion.Controls.Add(this.txtInstalled);
            this.pnlProjectVersion.Controls.Add(this.btnUninstall);
            this.pnlProjectVersion.Controls.Add(this.lblInstalled);
            this.pnlProjectVersion.Controls.Add(this.btnInstall);
            this.pnlProjectVersion.Controls.Add(this.pbxOBStudio);
            this.pnlProjectVersion.Controls.Add(this.cbxVersion);
            this.pnlProjectVersion.Controls.Add(this.lblVersionTitleLabel);
            this.pnlProjectVersion.Controls.Add(this.lblTitle);
            this.pnlProjectVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProjectVersion.Location = new System.Drawing.Point(877, 3);
            this.pnlProjectVersion.Name = "pnlProjectVersion";
            this.pnlProjectVersion.Size = new System.Drawing.Size(569, 147);
            this.pnlProjectVersion.TabIndex = 2;
            theme2.BgGradientEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            theme2.BgGradientStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.pnlProjectVersion.Theme = theme2;
            // 
            // txtInstalled
            // 
            this.txtInstalled.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInstalled.Enabled = false;
            this.txtInstalled.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtInstalled.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtInstalled.Location = new System.Drawing.Point(121, 72);
            this.txtInstalled.Margin = new System.Windows.Forms.Padding(4);
            this.txtInstalled.Name = "txtInstalled";
            this.txtInstalled.Size = new System.Drawing.Size(333, 34);
            this.txtInstalled.TabIndex = 48;
            this.txtInstalled.Visible = false;
            // 
            // btnUninstall
            // 
            this.btnUninstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUninstall.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.btnUninstall.Location = new System.Drawing.Point(455, 70);
            this.btnUninstall.Name = "btnUninstall";
            this.btnUninstall.Size = new System.Drawing.Size(111, 35);
            this.btnUninstall.TabIndex = 47;
            this.btnUninstall.Text = "Uninstall";
            this.btnUninstall.UseVisualStyleBackColor = true;
            this.btnUninstall.Visible = false;
            this.btnUninstall.Click += new System.EventHandler(this.btnUninstall_Click);
            // 
            // lblInstalled
            // 
            this.lblInstalled.AutoSize = true;
            this.lblInstalled.Font = new System.Drawing.Font("Segoe UI Semibold", 14F);
            this.lblInstalled.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblInstalled.Location = new System.Drawing.Point(8, 72);
            this.lblInstalled.Name = "lblInstalled";
            this.lblInstalled.Size = new System.Drawing.Size(113, 32);
            this.lblInstalled.TabIndex = 45;
            this.lblInstalled.Text = "Installed:";
            this.lblInstalled.Visible = false;
            // 
            // btnInstall
            // 
            this.btnInstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInstall.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.btnInstall.Location = new System.Drawing.Point(455, 108);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(111, 35);
            this.btnInstall.TabIndex = 44;
            this.btnInstall.Text = "Install";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // pbxOBStudio
            // 
            this.pbxOBStudio.Image = global::OpenBots.Properties.Resources.OpenBots_icon;
            this.pbxOBStudio.Location = new System.Drawing.Point(14, 13);
            this.pbxOBStudio.Name = "pbxOBStudio";
            this.pbxOBStudio.Size = new System.Drawing.Size(50, 50);
            this.pbxOBStudio.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxOBStudio.TabIndex = 43;
            this.pbxOBStudio.TabStop = false;
            // 
            // cbxVersion
            // 
            this.cbxVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxVersion.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cbxVersion.ForeColor = System.Drawing.Color.SteelBlue;
            this.cbxVersion.FormattingEnabled = true;
            this.cbxVersion.Location = new System.Drawing.Point(121, 110);
            this.cbxVersion.Margin = new System.Windows.Forms.Padding(4);
            this.cbxVersion.Name = "cbxVersion";
            this.cbxVersion.Size = new System.Drawing.Size(333, 36);
            this.cbxVersion.TabIndex = 42;
            this.cbxVersion.SelectedIndexChanged += new System.EventHandler(this.cbxVersion_SelectedIndexChanged);
            // 
            // lblVersionTitleLabel
            // 
            this.lblVersionTitleLabel.AutoSize = true;
            this.lblVersionTitleLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 14F);
            this.lblVersionTitleLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblVersionTitleLabel.Location = new System.Drawing.Point(8, 111);
            this.lblVersionTitleLabel.Name = "lblVersionTitleLabel";
            this.lblVersionTitleLabel.Size = new System.Drawing.Size(100, 32);
            this.lblVersionTitleLabel.TabIndex = 2;
            this.lblVersionTitleLabel.Text = "Version:";
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.AutoEllipsis = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semilight", 20F);
            this.lblTitle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblTitle.Location = new System.Drawing.Point(70, 17);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(491, 46);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "title";
            // 
            // pnlProjectSearch
            // 
            this.pnlProjectSearch.Controls.Add(this.chbxIncludePrerelease);
            this.pnlProjectSearch.Controls.Add(this.pbxPackageCategory);
            this.pnlProjectSearch.Controls.Add(this.lblSearch);
            this.pnlProjectSearch.Controls.Add(this.lblPackageCategory);
            this.pnlProjectSearch.Controls.Add(this.txtSampleSearch);
            this.pnlProjectSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProjectSearch.Location = new System.Drawing.Point(303, 3);
            this.pnlProjectSearch.Name = "pnlProjectSearch";
            this.pnlProjectSearch.Size = new System.Drawing.Size(568, 147);
            this.pnlProjectSearch.TabIndex = 0;
            theme3.BgGradientEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            theme3.BgGradientStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.pnlProjectSearch.Theme = theme3;
            // 
            // chbxIncludePrerelease
            // 
            this.chbxIncludePrerelease.AutoSize = true;
            this.chbxIncludePrerelease.Font = new System.Drawing.Font("Segoe UI Semibold", 14F);
            this.chbxIncludePrerelease.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.chbxIncludePrerelease.Location = new System.Drawing.Point(13, 72);
            this.chbxIncludePrerelease.Name = "chbxIncludePrerelease";
            this.chbxIncludePrerelease.Size = new System.Drawing.Size(236, 36);
            this.chbxIncludePrerelease.TabIndex = 46;
            this.chbxIncludePrerelease.Text = "Include Prerelease";
            this.chbxIncludePrerelease.UseVisualStyleBackColor = true;
            this.chbxIncludePrerelease.CheckedChanged += new System.EventHandler(this.chbxIncludePrerelease_CheckedChanged);
            // 
            // pbxPackageCategory
            // 
            this.pbxPackageCategory.Image = global::OpenBots.Properties.Resources.OpenBots_icon;
            this.pbxPackageCategory.Location = new System.Drawing.Point(13, 13);
            this.pbxPackageCategory.Name = "pbxPackageCategory";
            this.pbxPackageCategory.Size = new System.Drawing.Size(50, 50);
            this.pbxPackageCategory.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxPackageCategory.TabIndex = 44;
            this.pbxPackageCategory.TabStop = false;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI Semibold", 14F);
            this.lblSearch.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblSearch.Location = new System.Drawing.Point(8, 111);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(94, 32);
            this.lblSearch.TabIndex = 35;
            this.lblSearch.Text = "Search:";
            // 
            // lblPackageCategory
            // 
            this.lblPackageCategory.AutoSize = true;
            this.lblPackageCategory.BackColor = System.Drawing.Color.Transparent;
            this.lblPackageCategory.Font = new System.Drawing.Font("Segoe UI Semilight", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPackageCategory.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblPackageCategory.Location = new System.Drawing.Point(70, 9);
            this.lblPackageCategory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPackageCategory.Name = "lblPackageCategory";
            this.lblPackageCategory.Size = new System.Drawing.Size(398, 54);
            this.lblPackageCategory.TabIndex = 33;
            this.lblPackageCategory.Text = "Project Dependencies";
            // 
            // txtSampleSearch
            // 
            this.txtSampleSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSampleSearch.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtSampleSearch.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtSampleSearch.Location = new System.Drawing.Point(105, 110);
            this.txtSampleSearch.Margin = new System.Windows.Forms.Padding(4);
            this.txtSampleSearch.Name = "txtSampleSearch";
            this.txtSampleSearch.Size = new System.Drawing.Size(460, 34);
            this.txtSampleSearch.TabIndex = 34;
            this.txtSampleSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSampleSearch_KeyDown);
            this.txtSampleSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSampleSearch_KeyPress);
            // 
            // pnlProjectDetails
            // 
            this.pnlProjectDetails.Controls.Add(this.tlpMetadata);
            this.pnlProjectDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProjectDetails.Location = new System.Drawing.Point(877, 156);
            this.pnlProjectDetails.Name = "pnlProjectDetails";
            this.pnlProjectDetails.Size = new System.Drawing.Size(569, 732);
            this.pnlProjectDetails.TabIndex = 37;
            theme5.BgGradientEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            theme5.BgGradientStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.pnlProjectDetails.Theme = theme5;
            // 
            // tlpMetadata
            // 
            this.tlpMetadata.ColumnCount = 1;
            this.tlpMetadata.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMetadata.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tlpMetadata.Controls.Add(this.pnlMetadata, 0, 0);
            this.tlpMetadata.Controls.Add(this.lvDependencies, 0, 1);
            this.tlpMetadata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMetadata.Location = new System.Drawing.Point(0, 0);
            this.tlpMetadata.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tlpMetadata.Name = "tlpMetadata";
            this.tlpMetadata.RowCount = 2;
            this.tlpMetadata.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMetadata.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMetadata.Size = new System.Drawing.Size(569, 732);
            this.tlpMetadata.TabIndex = 62;
            // 
            // pnlMetadata
            // 
            this.pnlMetadata.Controls.Add(this.lblDescription);
            this.pnlMetadata.Controls.Add(this.lblPublishDate);
            this.pnlMetadata.Controls.Add(this.lblDownloadsLabel);
            this.pnlMetadata.Controls.Add(this.lblPublishDateLabel);
            this.pnlMetadata.Controls.Add(this.lblVersionLabel);
            this.pnlMetadata.Controls.Add(this.lblAuthorsLabel);
            this.pnlMetadata.Controls.Add(this.lblVersion);
            this.pnlMetadata.Controls.Add(this.lblDescriptionLabel);
            this.pnlMetadata.Controls.Add(this.lblAuthors);
            this.pnlMetadata.Controls.Add(this.lblDependenciesLabel);
            this.pnlMetadata.Controls.Add(this.lblDownloads);
            this.pnlMetadata.Controls.Add(this.lblLicenseURLLabel);
            this.pnlMetadata.Controls.Add(this.lblProjectURLLabel);
            this.pnlMetadata.Controls.Add(this.llblProjectURL);
            this.pnlMetadata.Controls.Add(this.llblLicenseURL);
            this.pnlMetadata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMetadata.Location = new System.Drawing.Point(3, 2);
            this.pnlMetadata.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlMetadata.Name = "pnlMetadata";
            this.pnlMetadata.Size = new System.Drawing.Size(563, 362);
            this.pnlMetadata.TabIndex = 61;
            theme4.BgGradientEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            theme4.BgGradientStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.pnlMetadata.Theme = theme4;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoEllipsis = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI Semilight", 12F);
            this.lblDescription.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblDescription.Location = new System.Drawing.Point(8, 36);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(552, 135);
            this.lblDescription.TabIndex = 53;
            this.lblDescription.Text = "description";
            // 
            // lblPublishDate
            // 
            this.lblPublishDate.AutoSize = true;
            this.lblPublishDate.Font = new System.Drawing.Font("Segoe UI Semilight", 12F);
            this.lblPublishDate.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblPublishDate.Location = new System.Drawing.Point(160, 310);
            this.lblPublishDate.Name = "lblPublishDate";
            this.lblPublishDate.Size = new System.Drawing.Size(114, 28);
            this.lblPublishDate.TabIndex = 60;
            this.lblPublishDate.Text = "publish date";
            // 
            // lblDownloadsLabel
            // 
            this.lblDownloadsLabel.AutoSize = true;
            this.lblDownloadsLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblDownloadsLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblDownloadsLabel.Location = new System.Drawing.Point(8, 282);
            this.lblDownloadsLabel.Name = "lblDownloadsLabel";
            this.lblDownloadsLabel.Size = new System.Drawing.Size(118, 28);
            this.lblDownloadsLabel.TabIndex = 45;
            this.lblDownloadsLabel.Text = "Downloads:";
            // 
            // lblPublishDateLabel
            // 
            this.lblPublishDateLabel.AutoSize = true;
            this.lblPublishDateLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblPublishDateLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblPublishDateLabel.Location = new System.Drawing.Point(8, 310);
            this.lblPublishDateLabel.Name = "lblPublishDateLabel";
            this.lblPublishDateLabel.Size = new System.Drawing.Size(132, 28);
            this.lblPublishDateLabel.TabIndex = 59;
            this.lblPublishDateLabel.Text = "Publish Date:";
            // 
            // lblVersionLabel
            // 
            this.lblVersionLabel.AutoSize = true;
            this.lblVersionLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblVersionLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblVersionLabel.Location = new System.Drawing.Point(8, 170);
            this.lblVersionLabel.Name = "lblVersionLabel";
            this.lblVersionLabel.Size = new System.Drawing.Size(85, 28);
            this.lblVersionLabel.TabIndex = 44;
            this.lblVersionLabel.Text = "Version:";
            // 
            // lblAuthorsLabel
            // 
            this.lblAuthorsLabel.AutoSize = true;
            this.lblAuthorsLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblAuthorsLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblAuthorsLabel.Location = new System.Drawing.Point(8, 198);
            this.lblAuthorsLabel.Name = "lblAuthorsLabel";
            this.lblAuthorsLabel.Size = new System.Drawing.Size(103, 28);
            this.lblAuthorsLabel.TabIndex = 47;
            this.lblAuthorsLabel.Text = "Author(s):";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI Semilight", 12F);
            this.lblVersion.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblVersion.Location = new System.Drawing.Point(160, 170);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(72, 28);
            this.lblVersion.TabIndex = 57;
            this.lblVersion.Text = "version";
            // 
            // lblDescriptionLabel
            // 
            this.lblDescriptionLabel.AutoSize = true;
            this.lblDescriptionLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblDescriptionLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblDescriptionLabel.Location = new System.Drawing.Point(8, 7);
            this.lblDescriptionLabel.Name = "lblDescriptionLabel";
            this.lblDescriptionLabel.Size = new System.Drawing.Size(120, 28);
            this.lblDescriptionLabel.TabIndex = 46;
            this.lblDescriptionLabel.Text = "Description:";
            // 
            // lblAuthors
            // 
            this.lblAuthors.AutoEllipsis = true;
            this.lblAuthors.Font = new System.Drawing.Font("Segoe UI Semilight", 12F);
            this.lblAuthors.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblAuthors.Location = new System.Drawing.Point(160, 198);
            this.lblAuthors.Name = "lblAuthors";
            this.lblAuthors.Size = new System.Drawing.Size(400, 28);
            this.lblAuthors.TabIndex = 56;
            this.lblAuthors.Text = "authors";
            // 
            // lblDependenciesLabel
            // 
            this.lblDependenciesLabel.AutoSize = true;
            this.lblDependenciesLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblDependenciesLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblDependenciesLabel.Location = new System.Drawing.Point(8, 338);
            this.lblDependenciesLabel.Name = "lblDependenciesLabel";
            this.lblDependenciesLabel.Size = new System.Drawing.Size(146, 28);
            this.lblDependenciesLabel.TabIndex = 48;
            this.lblDependenciesLabel.Text = "Dependencies:";
            // 
            // lblDownloads
            // 
            this.lblDownloads.AutoSize = true;
            this.lblDownloads.Font = new System.Drawing.Font("Segoe UI Semilight", 12F);
            this.lblDownloads.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblDownloads.Location = new System.Drawing.Point(160, 282);
            this.lblDownloads.Name = "lblDownloads";
            this.lblDownloads.Size = new System.Drawing.Size(103, 28);
            this.lblDownloads.TabIndex = 55;
            this.lblDownloads.Text = "downloads";
            // 
            // lblLicenseURLLabel
            // 
            this.lblLicenseURLLabel.AutoSize = true;
            this.lblLicenseURLLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblLicenseURLLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblLicenseURLLabel.Location = new System.Drawing.Point(8, 226);
            this.lblLicenseURLLabel.Name = "lblLicenseURLLabel";
            this.lblLicenseURLLabel.Size = new System.Drawing.Size(126, 28);
            this.lblLicenseURLLabel.TabIndex = 49;
            this.lblLicenseURLLabel.Text = "License URL:";
            // 
            // lblProjectURLLabel
            // 
            this.lblProjectURLLabel.AutoSize = true;
            this.lblProjectURLLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblProjectURLLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblProjectURLLabel.Location = new System.Drawing.Point(8, 254);
            this.lblProjectURLLabel.Name = "lblProjectURLLabel";
            this.lblProjectURLLabel.Size = new System.Drawing.Size(122, 28);
            this.lblProjectURLLabel.TabIndex = 50;
            this.lblProjectURLLabel.Text = "Project URL:";
            // 
            // llblProjectURL
            // 
            this.llblProjectURL.ActiveLinkColor = System.Drawing.Color.Coral;
            this.llblProjectURL.AutoSize = true;
            this.llblProjectURL.Font = new System.Drawing.Font("Segoe UI Semilight", 12F);
            this.llblProjectURL.LinkColor = System.Drawing.Color.SteelBlue;
            this.llblProjectURL.Location = new System.Drawing.Point(160, 254);
            this.llblProjectURL.Name = "llblProjectURL";
            this.llblProjectURL.Size = new System.Drawing.Size(219, 28);
            this.llblProjectURL.TabIndex = 52;
            this.llblProjectURL.TabStop = true;
            this.llblProjectURL.Text = "View Project Information";
            this.llblProjectURL.VisitedLinkColor = System.Drawing.Color.Coral;
            this.llblProjectURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblProjectURL_LinkClicked);
            // 
            // llblLicenseURL
            // 
            this.llblLicenseURL.ActiveLinkColor = System.Drawing.Color.Coral;
            this.llblLicenseURL.AutoSize = true;
            this.llblLicenseURL.Font = new System.Drawing.Font("Segoe UI Semilight", 12F);
            this.llblLicenseURL.LinkColor = System.Drawing.Color.SteelBlue;
            this.llblLicenseURL.Location = new System.Drawing.Point(160, 226);
            this.llblLicenseURL.Name = "llblLicenseURL";
            this.llblLicenseURL.Size = new System.Drawing.Size(222, 28);
            this.llblLicenseURL.TabIndex = 51;
            this.llblLicenseURL.TabStop = true;
            this.llblLicenseURL.Text = "View License Information";
            this.llblLicenseURL.VisitedLinkColor = System.Drawing.Color.Coral;
            this.llblLicenseURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblLicense_LinkClicked);
            // 
            // lvDependencies
            // 
            this.lvDependencies.AllowColumnReorder = true;
            this.lvDependencies.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.lvDependencies.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.DependencyName,
            this.Range});
            this.lvDependencies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvDependencies.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lvDependencies.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lvDependencies.FullRowSelect = true;
            this.lvDependencies.GridLines = true;
            this.lvDependencies.HideSelection = false;
            this.lvDependencies.Location = new System.Drawing.Point(3, 369);
            this.lvDependencies.Name = "lvDependencies";
            this.lvDependencies.Size = new System.Drawing.Size(563, 360);
            this.lvDependencies.TabIndex = 58;
            this.lvDependencies.UseCompatibleStateImageBehavior = false;
            this.lvDependencies.View = System.Windows.Forms.View.Details;
            // 
            // DependencyName
            // 
            this.DependencyName.Text = "Name";
            this.DependencyName.Width = 423;
            // 
            // Range
            // 
            this.Range.Text = "Range";
            this.Range.Width = 207;
            // 
            // pnlFinishButtons
            // 
            this.pnlFinishButtons.Controls.Add(this.uiBtnCancel);
            this.pnlFinishButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFinishButtons.Location = new System.Drawing.Point(877, 894);
            this.pnlFinishButtons.Name = "pnlFinishButtons";
            this.pnlFinishButtons.Size = new System.Drawing.Size(569, 76);
            this.pnlFinishButtons.TabIndex = 38;
            theme6.BgGradientEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            theme6.BgGradientStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.pnlFinishButtons.Theme = theme6;
            // 
            // uiBtnCancel
            // 
            this.uiBtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.uiBtnCancel.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnCancel.DisplayText = "Cancel";
            this.uiBtnCancel.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnCancel.Image")));
            this.uiBtnCancel.IsMouseOver = false;
            this.uiBtnCancel.Location = new System.Drawing.Point(502, 13);
            this.uiBtnCancel.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(60, 58);
            this.uiBtnCancel.TabIndex = 40;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Text = "Cancel";
            this.uiBtnCancel.Click += new System.EventHandler(this.uiBtnCancel_Click);
            // 
            // tvPackageFeeds
            // 
            this.tvPackageFeeds.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.tvPackageFeeds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvPackageFeeds.Font = new System.Drawing.Font("Segoe UI Semilight", 12F);
            this.tvPackageFeeds.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.tvPackageFeeds.ImageIndex = 0;
            this.tvPackageFeeds.ImageList = this.imlNodes;
            this.tvPackageFeeds.Location = new System.Drawing.Point(3, 156);
            this.tvPackageFeeds.Name = "tvPackageFeeds";
            this.tvPackageFeeds.SelectedImageIndex = 0;
            this.tvPackageFeeds.ShowNodeToolTips = true;
            this.tvPackageFeeds.Size = new System.Drawing.Size(294, 732);
            this.tvPackageFeeds.TabIndex = 40;
            this.tvPackageFeeds.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvPackageFeeds_BeforeCollapse);
            this.tvPackageFeeds.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvPackageFeeds_NodeMouseDoubleClick);
            // 
            // imlNodes
            // 
            this.imlNodes.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlNodes.ImageStream")));
            this.imlNodes.TransparentColor = System.Drawing.Color.Transparent;
            this.imlNodes.Images.SetKeyName(0, "studioIcon");
            this.imlNodes.Images.SetKeyName(1, "galleryIcon");
            this.imlNodes.Images.SetKeyName(2, "nugetIcon");
            this.imlNodes.Images.SetKeyName(3, "settings.png");
            // 
            // btnSyncCommandsAndStudio
            // 
            this.btnSyncCommandsAndStudio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSyncCommandsAndStudio.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.btnSyncCommandsAndStudio.Location = new System.Drawing.Point(3, 114);
            this.btnSyncCommandsAndStudio.Name = "btnSyncCommandsAndStudio";
            this.btnSyncCommandsAndStudio.Size = new System.Drawing.Size(294, 36);
            this.btnSyncCommandsAndStudio.TabIndex = 41;
            this.btnSyncCommandsAndStudio.Text = "Sync Commands and Studio";
            this.btnSyncCommandsAndStudio.UseVisualStyleBackColor = true;
            this.btnSyncCommandsAndStudio.Visible = false;
            this.btnSyncCommandsAndStudio.Click += new System.EventHandler(this.btnSyncCommandsAndStudio_Click);
            // 
            // frmGalleryPackageManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(1449, 973);
            this.Controls.Add(this.tlpPackageLayout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "frmGalleryPackageManager";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Gallery Package Manager";
            this.Load += new System.EventHandler(this.frmGalleryProject_LoadAsync);
            this.tlpPackageLayout.ResumeLayout(false);
            this.pnlNugetPackages.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tpbLoadingSpinner)).EndInit();
            this.pnlProjectVersion.ResumeLayout(false);
            this.pnlProjectVersion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxOBStudio)).EndInit();
            this.pnlProjectSearch.ResumeLayout(false);
            this.pnlProjectSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxPackageCategory)).EndInit();
            this.pnlProjectDetails.ResumeLayout(false);
            this.tlpMetadata.ResumeLayout(false);
            this.pnlMetadata.ResumeLayout(false);
            this.pnlMetadata.PerformLayout();
            this.pnlFinishButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.TextBox txtSampleSearch;
        private System.Windows.Forms.Label lblPackageCategory;
        private System.Windows.Forms.TableLayoutPanel tlpPackageLayout;
        private UIPanel pnlProjectSearch;
        private UIPanel pnlProjectVersion;
        private System.Windows.Forms.Label lblDependenciesLabel;
        private System.Windows.Forms.Label lblAuthorsLabel;
        private System.Windows.Forms.Label lblDescriptionLabel;
        private System.Windows.Forms.Label lblDownloadsLabel;
        private System.Windows.Forms.Label lblVersionLabel;
        private System.Windows.Forms.ComboBox cbxVersion;
        private System.Windows.Forms.Label lblVersionTitleLabel;
        private System.Windows.Forms.Label lblTitle;
        private UIPanel pnlProjectDetails;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblAuthors;
        private System.Windows.Forms.Label lblDownloads;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.LinkLabel llblProjectURL;
        private System.Windows.Forms.LinkLabel llblLicenseURL;
        private System.Windows.Forms.Label lblProjectURLLabel;
        private System.Windows.Forms.Label lblLicenseURLLabel;
        private System.Windows.Forms.ListView lvDependencies;
        private System.Windows.Forms.ColumnHeader DependencyName;
        private System.Windows.Forms.ColumnHeader Range;
        private System.Windows.Forms.PictureBox pbxOBStudio;
        private System.Windows.Forms.Label lblPublishDate;
        private System.Windows.Forms.Label lblPublishDateLabel;
        private UIPanel pnlFinishButtons;
        private Core.UI.Controls.UIPictureButton uiBtnCancel;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.PictureBox pbxPackageCategory;
        public System.Windows.Forms.Label lblError;
        private UITreeView tvPackageFeeds;
        private System.Windows.Forms.ImageList imlNodes;
        private System.Windows.Forms.Button btnInstall;
        private UITransparentPictureBox tpbLoadingSpinner;
        private UIPanel pnlNugetPackages;
        private UIListBox lbxNugetPackages;
        public System.Windows.Forms.TextBox txtInstalled;
        private System.Windows.Forms.Button btnUninstall;
        private System.Windows.Forms.Label lblInstalled;
        private System.Windows.Forms.CheckBox chbxIncludePrerelease;
        private UIPanel pnlMetadata;
        private System.Windows.Forms.TableLayoutPanel tlpMetadata;
        private System.Windows.Forms.Button btnSyncCommandsAndStudio;
    }
}