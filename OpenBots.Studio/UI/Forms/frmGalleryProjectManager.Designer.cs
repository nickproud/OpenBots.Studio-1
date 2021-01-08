using OpenBots.UI.CustomControls.CustomUIControls;
using static OpenBots.UI.CustomControls.CustomUIControls.UIListBox;

namespace OpenBots.UI.Forms
{
    partial class frmGalleryProjectManager
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
            OpenBots.Core.Utilities.FormsUtilities.Theme theme4 = new OpenBots.Core.Utilities.FormsUtilities.Theme();
            OpenBots.Core.Utilities.FormsUtilities.Theme theme3 = new OpenBots.Core.Utilities.FormsUtilities.Theme();
            OpenBots.Core.Utilities.FormsUtilities.Theme theme5 = new OpenBots.Core.Utilities.FormsUtilities.Theme();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGalleryProjectManager));
            OpenBots.Core.Utilities.FormsUtilities.Theme theme6 = new OpenBots.Core.Utilities.FormsUtilities.Theme();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Automations");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Samples");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Templates");
            this.tlpProjectLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblError = new System.Windows.Forms.Label();
            this.imlNodes = new System.Windows.Forms.ImageList(this.components);
            this.pnlProjectVersion = new OpenBots.UI.CustomControls.CustomUIControls.UIPanel();
            this.pbxOBStudio = new System.Windows.Forms.PictureBox();
            this.cbxVersion = new System.Windows.Forms.ComboBox();
            this.lblVersionTitleLabel = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlProjectSearch = new OpenBots.UI.CustomControls.CustomUIControls.UIPanel();
            this.pbxOBGallery = new System.Windows.Forms.PictureBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.lblGalleryProjects = new System.Windows.Forms.Label();
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
            this.uiBtnOpen = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnCancel = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.pnlGalleryProjects = new OpenBots.UI.CustomControls.CustomUIControls.UIPanel();
            this.tpbLoadingSpinner = new OpenBots.UI.CustomControls.CustomUIControls.UITransparentPictureBox();
            this.lbxGalleryProjects = new OpenBots.UI.CustomControls.CustomUIControls.UIListBox();
            this.tvProjectFeeds = new OpenBots.UI.CustomControls.CustomUIControls.UITreeView();
            this.tlpProjectLayout.SuspendLayout();
            this.pnlProjectVersion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxOBStudio)).BeginInit();
            this.pnlProjectSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxOBGallery)).BeginInit();
            this.pnlProjectDetails.SuspendLayout();
            this.tlpMetadata.SuspendLayout();
            this.pnlMetadata.SuspendLayout();
            this.pnlFinishButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            this.pnlGalleryProjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tpbLoadingSpinner)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpProjectLayout
            // 
            this.tlpProjectLayout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.tlpProjectLayout.ColumnCount = 3;
            this.tlpProjectLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tlpProjectLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpProjectLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpProjectLayout.Controls.Add(this.lblError, 1, 2);
            this.tlpProjectLayout.Controls.Add(this.pnlProjectVersion, 2, 0);
            this.tlpProjectLayout.Controls.Add(this.pnlProjectSearch, 1, 0);
            this.tlpProjectLayout.Controls.Add(this.pnlProjectDetails, 2, 1);
            this.tlpProjectLayout.Controls.Add(this.pnlFinishButtons, 2, 2);
            this.tlpProjectLayout.Controls.Add(this.pnlGalleryProjects, 1, 1);
            this.tlpProjectLayout.Controls.Add(this.tvProjectFeeds, 0, 1);
            this.tlpProjectLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProjectLayout.Location = new System.Drawing.Point(0, 0);
            this.tlpProjectLayout.Name = "tlpProjectLayout";
            this.tlpProjectLayout.RowCount = 3;
            this.tlpProjectLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tlpProjectLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProjectLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.tlpProjectLayout.Size = new System.Drawing.Size(1466, 942);
            this.tlpProjectLayout.TabIndex = 0;
            // 
            // lblError
            // 
            this.lblError.BackColor = System.Drawing.Color.Transparent;
            this.lblError.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblError.Location = new System.Drawing.Point(304, 860);
            this.lblError.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(520, 28);
            this.lblError.TabIndex = 39;
            // 
            // imlNodes
            // 
            this.imlNodes.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlNodes.ImageStream")));
            this.imlNodes.TransparentColor = System.Drawing.Color.Transparent;
            this.imlNodes.Images.SetKeyName(0, "openbots-gallery-icon.png");
            // 
            // pnlProjectVersion
            // 
            this.pnlProjectVersion.Controls.Add(this.pbxOBStudio);
            this.pnlProjectVersion.Controls.Add(this.cbxVersion);
            this.pnlProjectVersion.Controls.Add(this.lblVersionTitleLabel);
            this.pnlProjectVersion.Controls.Add(this.lblTitle);
            this.pnlProjectVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProjectVersion.Location = new System.Drawing.Point(886, 3);
            this.pnlProjectVersion.Name = "pnlProjectVersion";
            this.pnlProjectVersion.Size = new System.Drawing.Size(577, 104);
            this.pnlProjectVersion.TabIndex = 2;
            theme1.BgGradientEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            theme1.BgGradientStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.pnlProjectVersion.Theme = theme1;
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
            this.cbxVersion.Location = new System.Drawing.Point(111, 69);
            this.cbxVersion.Margin = new System.Windows.Forms.Padding(4);
            this.cbxVersion.Name = "cbxVersion";
            this.cbxVersion.Size = new System.Drawing.Size(458, 36);
            this.cbxVersion.TabIndex = 42;
            this.cbxVersion.SelectedIndexChanged += new System.EventHandler(this.cbxVersion_SelectedIndexChanged);
            // 
            // lblVersionTitleLabel
            // 
            this.lblVersionTitleLabel.AutoSize = true;
            this.lblVersionTitleLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 14F);
            this.lblVersionTitleLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblVersionTitleLabel.Location = new System.Drawing.Point(8, 69);
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
            this.lblTitle.Location = new System.Drawing.Point(69, 16);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(499, 43);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "title";
            // 
            // pnlProjectSearch
            // 
            this.pnlProjectSearch.Controls.Add(this.pbxOBGallery);
            this.pnlProjectSearch.Controls.Add(this.lblSearch);
            this.pnlProjectSearch.Controls.Add(this.lblGalleryProjects);
            this.pnlProjectSearch.Controls.Add(this.txtSampleSearch);
            this.pnlProjectSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProjectSearch.Location = new System.Drawing.Point(303, 3);
            this.pnlProjectSearch.Name = "pnlProjectSearch";
            this.pnlProjectSearch.Size = new System.Drawing.Size(577, 104);
            this.pnlProjectSearch.TabIndex = 0;
            theme2.BgGradientEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            theme2.BgGradientStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.pnlProjectSearch.Theme = theme2;
            // 
            // pbxOBGallery
            // 
            this.pbxOBGallery.Image = global::OpenBots.Properties.Resources.OpenBots_gallery_icon;
            this.pbxOBGallery.Location = new System.Drawing.Point(13, 13);
            this.pbxOBGallery.Name = "pbxOBGallery";
            this.pbxOBGallery.Size = new System.Drawing.Size(50, 50);
            this.pbxOBGallery.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxOBGallery.TabIndex = 44;
            this.pbxOBGallery.TabStop = false;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI Semibold", 14F);
            this.lblSearch.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblSearch.Location = new System.Drawing.Point(7, 71);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(94, 32);
            this.lblSearch.TabIndex = 35;
            this.lblSearch.Text = "Search:";
            // 
            // lblGalleryProjects
            // 
            this.lblGalleryProjects.AutoSize = true;
            this.lblGalleryProjects.BackColor = System.Drawing.Color.Transparent;
            this.lblGalleryProjects.Font = new System.Drawing.Font("Segoe UI Semilight", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGalleryProjects.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblGalleryProjects.Location = new System.Drawing.Point(70, 9);
            this.lblGalleryProjects.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGalleryProjects.Name = "lblGalleryProjects";
            this.lblGalleryProjects.Size = new System.Drawing.Size(375, 54);
            this.lblGalleryProjects.TabIndex = 33;
            this.lblGalleryProjects.Text = "Gallery Automations";
            // 
            // txtSampleSearch
            // 
            this.txtSampleSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSampleSearch.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtSampleSearch.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtSampleSearch.Location = new System.Drawing.Point(101, 69);
            this.txtSampleSearch.Margin = new System.Windows.Forms.Padding(4);
            this.txtSampleSearch.Name = "txtSampleSearch";
            this.txtSampleSearch.Size = new System.Drawing.Size(473, 34);
            this.txtSampleSearch.TabIndex = 34;
            this.txtSampleSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSampleSearch_KeyDown);
            this.txtSampleSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSampleSearch_KeyPress);
            // 
            // pnlProjectDetails
            // 
            this.pnlProjectDetails.Controls.Add(this.tlpMetadata);
            this.pnlProjectDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProjectDetails.Location = new System.Drawing.Point(886, 113);
            this.pnlProjectDetails.Name = "pnlProjectDetails";
            this.pnlProjectDetails.Size = new System.Drawing.Size(577, 744);
            this.pnlProjectDetails.TabIndex = 37;
            theme4.BgGradientEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            theme4.BgGradientStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.pnlProjectDetails.Theme = theme4;
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
            this.tlpMetadata.Size = new System.Drawing.Size(577, 744);
            this.tlpMetadata.TabIndex = 63;
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
            this.pnlMetadata.Size = new System.Drawing.Size(571, 368);
            this.pnlMetadata.TabIndex = 61;
            theme3.BgGradientEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            theme3.BgGradientStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.pnlMetadata.Theme = theme3;
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
            this.lvDependencies.Location = new System.Drawing.Point(3, 375);
            this.lvDependencies.Name = "lvDependencies";
            this.lvDependencies.Size = new System.Drawing.Size(571, 366);
            this.lvDependencies.TabIndex = 58;
            this.lvDependencies.UseCompatibleStateImageBehavior = false;
            this.lvDependencies.View = System.Windows.Forms.View.Details;
            // 
            // DependencyName
            // 
            this.DependencyName.Text = "Name";
            this.DependencyName.Width = 491;
            // 
            // Range
            // 
            this.Range.Text = "Range";
            this.Range.Width = 207;
            // 
            // pnlFinishButtons
            // 
            this.pnlFinishButtons.Controls.Add(this.uiBtnOpen);
            this.pnlFinishButtons.Controls.Add(this.uiBtnCancel);
            this.pnlFinishButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFinishButtons.Location = new System.Drawing.Point(886, 863);
            this.pnlFinishButtons.Name = "pnlFinishButtons";
            this.pnlFinishButtons.Size = new System.Drawing.Size(577, 76);
            this.pnlFinishButtons.TabIndex = 38;
            theme5.BgGradientEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            theme5.BgGradientStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.pnlFinishButtons.Theme = theme5;
            // 
            // uiBtnOpen
            // 
            this.uiBtnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.uiBtnOpen.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOpen.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOpen.DisplayText = "Open";
            this.uiBtnOpen.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnOpen.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOpen.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnOpen.Image")));
            this.uiBtnOpen.IsMouseOver = false;
            this.uiBtnOpen.Location = new System.Drawing.Point(449, 13);
            this.uiBtnOpen.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.uiBtnOpen.Name = "uiBtnOpen";
            this.uiBtnOpen.Size = new System.Drawing.Size(60, 58);
            this.uiBtnOpen.TabIndex = 39;
            this.uiBtnOpen.TabStop = false;
            this.uiBtnOpen.Text = "Open";
            this.uiBtnOpen.Click += new System.EventHandler(this.uiBtnOpen_Click);
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
            this.uiBtnCancel.Location = new System.Drawing.Point(510, 13);
            this.uiBtnCancel.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(60, 58);
            this.uiBtnCancel.TabIndex = 40;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Text = "Cancel";
            this.uiBtnCancel.Click += new System.EventHandler(this.uiBtnCancel_Click);
            // 
            // pnlGalleryProjects
            // 
            this.pnlGalleryProjects.Controls.Add(this.tpbLoadingSpinner);
            this.pnlGalleryProjects.Controls.Add(this.lbxGalleryProjects);
            this.pnlGalleryProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGalleryProjects.Location = new System.Drawing.Point(303, 113);
            this.pnlGalleryProjects.Name = "pnlGalleryProjects";
            this.pnlGalleryProjects.Size = new System.Drawing.Size(577, 744);
            this.pnlGalleryProjects.TabIndex = 40;
            theme6.BgGradientEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            theme6.BgGradientStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.pnlGalleryProjects.Theme = theme6;
            // 
            // tpbLoadingSpinner
            // 
            this.tpbLoadingSpinner.BackColor = System.Drawing.Color.Transparent;
            this.tpbLoadingSpinner.ErrorImage = null;
            this.tpbLoadingSpinner.Image = global::OpenBots.Properties.Resources.loading_spinner;
            this.tpbLoadingSpinner.InitialImage = null;
            this.tpbLoadingSpinner.Location = new System.Drawing.Point(93, 201);
            this.tpbLoadingSpinner.Name = "tpbLoadingSpinner";
            this.tpbLoadingSpinner.Size = new System.Drawing.Size(404, 350);
            this.tpbLoadingSpinner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.tpbLoadingSpinner.TabIndex = 43;
            this.tpbLoadingSpinner.TabStop = false;
            // 
            // lbxGalleryProjects
            // 
            this.lbxGalleryProjects.AutoScroll = true;
            this.lbxGalleryProjects.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbxGalleryProjects.ClickedItem = null;
            this.lbxGalleryProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxGalleryProjects.DoubleClickedItem = null;
            this.lbxGalleryProjects.LastSelectedItem = null;
            this.lbxGalleryProjects.Location = new System.Drawing.Point(0, 0);
            this.lbxGalleryProjects.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lbxGalleryProjects.Name = "lbxGalleryProjects";
            this.lbxGalleryProjects.Size = new System.Drawing.Size(577, 744);
            this.lbxGalleryProjects.TabIndex = 41;
            this.lbxGalleryProjects.ItemClick += new OpenBots.UI.CustomControls.CustomUIControls.UIListBox.ItemClickEventHandler(this.lbxGalleryProjects_ItemClick);
            // 
            // tvProjectFeeds
            // 
            this.tvProjectFeeds.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.tvProjectFeeds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvProjectFeeds.Font = new System.Drawing.Font("Segoe UI Semilight", 12F);
            this.tvProjectFeeds.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.tvProjectFeeds.ImageIndex = 0;
            this.tvProjectFeeds.ImageList = this.imlNodes;
            this.tvProjectFeeds.Location = new System.Drawing.Point(3, 113);
            this.tvProjectFeeds.Name = "tvProjectFeeds";
            treeNode1.Name = "Automations";
            treeNode1.Text = "Automations";
            treeNode2.Name = "Samples";
            treeNode2.Text = "Samples";
            treeNode3.Name = "Templates";
            treeNode3.Text = "Templates";
            this.tvProjectFeeds.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            this.tvProjectFeeds.SelectedImageIndex = 0;
            this.tvProjectFeeds.ShowNodeToolTips = true;
            this.tvProjectFeeds.Size = new System.Drawing.Size(294, 744);
            this.tvProjectFeeds.TabIndex = 41;
            this.tvProjectFeeds.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvProjectFeeds_BeforeCollapse);
            this.tvProjectFeeds.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvProjectFeeds_NodeMouseDoubleClick);
            // 
            // frmGalleryProjectManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(1466, 942);
            this.Controls.Add(this.tlpProjectLayout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "frmGalleryProjectManager";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Gallery Project Manager";
            this.Load += new System.EventHandler(this.frmGalleryProject_LoadAsync);
            this.tlpProjectLayout.ResumeLayout(false);
            this.pnlProjectVersion.ResumeLayout(false);
            this.pnlProjectVersion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxOBStudio)).EndInit();
            this.pnlProjectSearch.ResumeLayout(false);
            this.pnlProjectSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxOBGallery)).EndInit();
            this.pnlProjectDetails.ResumeLayout(false);
            this.tlpMetadata.ResumeLayout(false);
            this.pnlMetadata.ResumeLayout(false);
            this.pnlMetadata.PerformLayout();
            this.pnlFinishButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            this.pnlGalleryProjects.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tpbLoadingSpinner)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.TextBox txtSampleSearch;
        private System.Windows.Forms.Label lblGalleryProjects;
        private System.Windows.Forms.TableLayoutPanel tlpProjectLayout;
        private UIPanel pnlProjectSearch;
        private UIPanel pnlProjectVersion;
        private System.Windows.Forms.ComboBox cbxVersion;
        private System.Windows.Forms.Label lblVersionTitleLabel;
        private System.Windows.Forms.Label lblTitle;
        private UIPanel pnlProjectDetails;
        private System.Windows.Forms.PictureBox pbxOBStudio;
        private UIPanel pnlFinishButtons;
        private Core.UI.Controls.UIPictureButton uiBtnOpen;
        private Core.UI.Controls.UIPictureButton uiBtnCancel;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.PictureBox pbxOBGallery;
        public System.Windows.Forms.Label lblError;
        private UIPanel pnlGalleryProjects;
        private UIListBox lbxGalleryProjects;
        private UITransparentPictureBox tpbLoadingSpinner;
        private System.Windows.Forms.TableLayoutPanel tlpMetadata;
        private UIPanel pnlMetadata;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblPublishDate;
        private System.Windows.Forms.Label lblDownloadsLabel;
        private System.Windows.Forms.Label lblPublishDateLabel;
        private System.Windows.Forms.Label lblVersionLabel;
        private System.Windows.Forms.Label lblAuthorsLabel;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblDescriptionLabel;
        private System.Windows.Forms.Label lblAuthors;
        private System.Windows.Forms.Label lblDependenciesLabel;
        private System.Windows.Forms.Label lblDownloads;
        private System.Windows.Forms.Label lblLicenseURLLabel;
        private System.Windows.Forms.Label lblProjectURLLabel;
        private System.Windows.Forms.LinkLabel llblProjectURL;
        private System.Windows.Forms.LinkLabel llblLicenseURL;
        private System.Windows.Forms.ListView lvDependencies;
        private System.Windows.Forms.ColumnHeader DependencyName;
        private System.Windows.Forms.ColumnHeader Range;
        private UITreeView tvProjectFeeds;
        private System.Windows.Forms.ImageList imlNodes;
    }
}