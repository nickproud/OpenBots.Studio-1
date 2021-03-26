namespace OpenBots.UI.Forms
{
    partial class frmProjectBuilder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProjectBuilder));
            this.lblNewProject = new System.Windows.Forms.Label();
            this.txtNewProjectName = new System.Windows.Forms.TextBox();
            this.lblProjectName = new System.Windows.Forms.Label();
            this.lblOpenProjectDescription = new System.Windows.Forms.Label();
            this.txtExistingProjectLocation = new System.Windows.Forms.TextBox();
            this.lblOpenProject = new System.Windows.Forms.Label();
            this.txtNewProjectLocation = new System.Windows.Forms.TextBox();
            this.btnFileManager = new System.Windows.Forms.Button();
            this.btnFolderManager = new System.Windows.Forms.Button();
            this.lblProjectLocation = new System.Windows.Forms.Label();
            this.btnOpenProject = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.btnCreateProject = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.lblError = new System.Windows.Forms.Label();
            this.btnCreateGalleryProject = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.btnRecentProjects = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.lblProjectType = new System.Windows.Forms.Label();
            this.cbxProjectType = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnOpenProject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCreateProject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCreateGalleryProject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRecentProjects)).BeginInit();
            this.SuspendLayout();
            // 
            // lblNewProject
            // 
            this.lblNewProject.AutoSize = true;
            this.lblNewProject.BackColor = System.Drawing.Color.Transparent;
            this.lblNewProject.Font = new System.Drawing.Font("Segoe UI Semilight", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewProject.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblNewProject.Location = new System.Drawing.Point(8, 4);
            this.lblNewProject.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNewProject.Name = "lblNewProject";
            this.lblNewProject.Size = new System.Drawing.Size(225, 54);
            this.lblNewProject.TabIndex = 14;
            this.lblNewProject.Text = "new project";
            // 
            // txtNewProjectName
            // 
            this.txtNewProjectName.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNewProjectName.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtNewProjectName.Location = new System.Drawing.Point(17, 90);
            this.txtNewProjectName.Margin = new System.Windows.Forms.Padding(4);
            this.txtNewProjectName.Name = "txtNewProjectName";
            this.txtNewProjectName.Size = new System.Drawing.Size(280, 32);
            this.txtNewProjectName.TabIndex = 16;
            this.txtNewProjectName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNewProjectName_KeyDown);
            // 
            // lblProjectName
            // 
            this.lblProjectName.BackColor = System.Drawing.Color.Transparent;
            this.lblProjectName.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProjectName.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblProjectName.Location = new System.Drawing.Point(12, 58);
            this.lblProjectName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(280, 28);
            this.lblProjectName.TabIndex = 17;
            this.lblProjectName.Text = "Project Name";
            // 
            // lblOpenProjectDescription
            // 
            this.lblOpenProjectDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblOpenProjectDescription.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOpenProjectDescription.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblOpenProjectDescription.Location = new System.Drawing.Point(362, 198);
            this.lblOpenProjectDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOpenProjectDescription.Name = "lblOpenProjectDescription";
            this.lblOpenProjectDescription.Size = new System.Drawing.Size(324, 28);
            this.lblOpenProjectDescription.TabIndex = 20;
            this.lblOpenProjectDescription.Text = "Project Config File Path";
            // 
            // txtExistingProjectLocation
            // 
            this.txtExistingProjectLocation.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtExistingProjectLocation.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtExistingProjectLocation.Location = new System.Drawing.Point(367, 228);
            this.txtExistingProjectLocation.Margin = new System.Windows.Forms.Padding(4);
            this.txtExistingProjectLocation.Name = "txtExistingProjectLocation";
            this.txtExistingProjectLocation.Size = new System.Drawing.Size(280, 32);
            this.txtExistingProjectLocation.TabIndex = 19;
            this.txtExistingProjectLocation.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtExistingProjectLocation_KeyDown);
            // 
            // lblOpenProject
            // 
            this.lblOpenProject.AutoSize = true;
            this.lblOpenProject.BackColor = System.Drawing.Color.Transparent;
            this.lblOpenProject.Font = new System.Drawing.Font("Segoe UI Semilight", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOpenProject.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblOpenProject.Location = new System.Drawing.Point(358, 4);
            this.lblOpenProject.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOpenProject.Name = "lblOpenProject";
            this.lblOpenProject.Size = new System.Drawing.Size(243, 54);
            this.lblOpenProject.TabIndex = 23;
            this.lblOpenProject.Text = "open project";
            // 
            // txtNewProjectLocation
            // 
            this.txtNewProjectLocation.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNewProjectLocation.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtNewProjectLocation.Location = new System.Drawing.Point(17, 228);
            this.txtNewProjectLocation.Margin = new System.Windows.Forms.Padding(4);
            this.txtNewProjectLocation.Name = "txtNewProjectLocation";
            this.txtNewProjectLocation.Size = new System.Drawing.Size(280, 32);
            this.txtNewProjectLocation.TabIndex = 24;
            this.txtNewProjectLocation.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNewProjectLocation_KeyDown);
            // 
            // btnFileManager
            // 
            this.btnFileManager.Location = new System.Drawing.Point(654, 228);
            this.btnFileManager.Name = "btnFileManager";
            this.btnFileManager.Size = new System.Drawing.Size(32, 32);
            this.btnFileManager.TabIndex = 25;
            this.btnFileManager.Text = "...";
            this.btnFileManager.UseVisualStyleBackColor = true;
            this.btnFileManager.Click += new System.EventHandler(this.btnFileManager_Click);
            // 
            // btnFolderManager
            // 
            this.btnFolderManager.Location = new System.Drawing.Point(304, 228);
            this.btnFolderManager.Name = "btnFolderManager";
            this.btnFolderManager.Size = new System.Drawing.Size(32, 32);
            this.btnFolderManager.TabIndex = 26;
            this.btnFolderManager.Text = "...";
            this.btnFolderManager.UseVisualStyleBackColor = true;
            this.btnFolderManager.Click += new System.EventHandler(this.btnFolderManager_Click);
            // 
            // lblProjectLocation
            // 
            this.lblProjectLocation.BackColor = System.Drawing.Color.Transparent;
            this.lblProjectLocation.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProjectLocation.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblProjectLocation.Location = new System.Drawing.Point(12, 198);
            this.lblProjectLocation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProjectLocation.Name = "lblProjectLocation";
            this.lblProjectLocation.Size = new System.Drawing.Size(280, 28);
            this.lblProjectLocation.TabIndex = 27;
            this.lblProjectLocation.Text = "Project Location";
            // 
            // btnOpenProject
            // 
            this.btnOpenProject.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenProject.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnOpenProject.DisplayText = "Open";
            this.btnOpenProject.DisplayTextBrush = System.Drawing.Color.White;
            this.btnOpenProject.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnOpenProject.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenProject.Image")));
            this.btnOpenProject.IsMouseOver = false;
            this.btnOpenProject.Location = new System.Drawing.Point(626, 274);
            this.btnOpenProject.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnOpenProject.Name = "btnOpenProject";
            this.btnOpenProject.Size = new System.Drawing.Size(60, 60);
            this.btnOpenProject.TabIndex = 28;
            this.btnOpenProject.TabStop = false;
            this.btnOpenProject.Text = "Open";
            this.btnOpenProject.Click += new System.EventHandler(this.btnOpenProject_Click);
            // 
            // btnCreateProject
            // 
            this.btnCreateProject.BackColor = System.Drawing.Color.Transparent;
            this.btnCreateProject.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnCreateProject.DisplayText = "Create";
            this.btnCreateProject.DisplayTextBrush = System.Drawing.Color.White;
            this.btnCreateProject.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnCreateProject.Image = ((System.Drawing.Image)(resources.GetObject("btnCreateProject.Image")));
            this.btnCreateProject.IsMouseOver = false;
            this.btnCreateProject.Location = new System.Drawing.Point(276, 274);
            this.btnCreateProject.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCreateProject.Name = "btnCreateProject";
            this.btnCreateProject.Size = new System.Drawing.Size(60, 60);
            this.btnCreateProject.TabIndex = 21;
            this.btnCreateProject.TabStop = false;
            this.btnCreateProject.Text = "Create";
            this.btnCreateProject.Click += new System.EventHandler(this.btnCreateProject_Click);
            // 
            // lblError
            // 
            this.lblError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblError.BackColor = System.Drawing.Color.Transparent;
            this.lblError.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblError.Location = new System.Drawing.Point(6, 344);
            this.lblError.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(688, 28);
            this.lblError.TabIndex = 29;
            // 
            // btnCreateGalleryProject
            // 
            this.btnCreateGalleryProject.BackColor = System.Drawing.Color.Transparent;
            this.btnCreateGalleryProject.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnCreateGalleryProject.DisplayText = "Gallery";
            this.btnCreateGalleryProject.DisplayTextBrush = System.Drawing.Color.White;
            this.btnCreateGalleryProject.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnCreateGalleryProject.Image = global::OpenBots.Properties.Resources.OpenBots_gallery_icon;
            this.btnCreateGalleryProject.IsMouseOver = false;
            this.btnCreateGalleryProject.Location = new System.Drawing.Point(216, 274);
            this.btnCreateGalleryProject.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCreateGalleryProject.Name = "btnCreateGalleryProject";
            this.btnCreateGalleryProject.Size = new System.Drawing.Size(60, 60);
            this.btnCreateGalleryProject.TabIndex = 30;
            this.btnCreateGalleryProject.TabStop = false;
            this.btnCreateGalleryProject.Text = "Gallery";
            this.btnCreateGalleryProject.Click += new System.EventHandler(this.btnCreateGalleryProject_Click);
            // 
            // btnRecentProjects
            // 
            this.btnRecentProjects.BackColor = System.Drawing.Color.Transparent;
            this.btnRecentProjects.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnRecentProjects.DisplayText = "Recent";
            this.btnRecentProjects.DisplayTextBrush = System.Drawing.Color.White;
            this.btnRecentProjects.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRecentProjects.Image = global::OpenBots.Properties.Resources.OpenBots_icon;
            this.btnRecentProjects.IsMouseOver = false;
            this.btnRecentProjects.Location = new System.Drawing.Point(566, 274);
            this.btnRecentProjects.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnRecentProjects.Name = "btnRecentProjects";
            this.btnRecentProjects.Size = new System.Drawing.Size(60, 60);
            this.btnRecentProjects.TabIndex = 31;
            this.btnRecentProjects.TabStop = false;
            this.btnRecentProjects.Text = "Recent";
            this.btnRecentProjects.Click += new System.EventHandler(this.btnRecentProjects_Click);
            // 
            // lblProjectType
            // 
            this.lblProjectType.BackColor = System.Drawing.Color.Transparent;
            this.lblProjectType.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProjectType.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblProjectType.Location = new System.Drawing.Point(12, 128);
            this.lblProjectType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProjectType.Name = "lblProjectType";
            this.lblProjectType.Size = new System.Drawing.Size(280, 28);
            this.lblProjectType.TabIndex = 33;
            this.lblProjectType.Text = "Project Type";
            // 
            // cbxProjectType
            // 
            this.cbxProjectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxProjectType.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxProjectType.ForeColor = System.Drawing.Color.SteelBlue;
            this.cbxProjectType.Location = new System.Drawing.Point(17, 160);
            this.cbxProjectType.Margin = new System.Windows.Forms.Padding(4);
            this.cbxProjectType.Name = "cbxProjectType";
            this.cbxProjectType.Size = new System.Drawing.Size(280, 33);
            this.cbxProjectType.TabIndex = 32;
            // 
            // frmProjectBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(703, 380);
            this.Controls.Add(this.lblProjectType);
            this.Controls.Add(this.cbxProjectType);
            this.Controls.Add(this.btnRecentProjects);
            this.Controls.Add(this.btnCreateGalleryProject);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.btnOpenProject);
            this.Controls.Add(this.lblProjectLocation);
            this.Controls.Add(this.btnFolderManager);
            this.Controls.Add(this.btnFileManager);
            this.Controls.Add(this.txtNewProjectLocation);
            this.Controls.Add(this.lblOpenProject);
            this.Controls.Add(this.btnCreateProject);
            this.Controls.Add(this.lblOpenProjectDescription);
            this.Controls.Add(this.txtExistingProjectLocation);
            this.Controls.Add(this.lblProjectName);
            this.Controls.Add(this.txtNewProjectName);
            this.Controls.Add(this.lblNewProject);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmProjectBuilder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Project Manager";
            this.Load += new System.EventHandler(this.frmProjectBuilder_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btnOpenProject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCreateProject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCreateGalleryProject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRecentProjects)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblNewProject;
        private System.Windows.Forms.Label lblProjectName;
        private System.Windows.Forms.Label lblOpenProjectDescription;
        private OpenBots.Core.UI.Controls.UIPictureButton btnCreateProject;
        public System.Windows.Forms.TextBox txtNewProjectName;
        public System.Windows.Forms.TextBox txtExistingProjectLocation;
        private System.Windows.Forms.Label lblOpenProject;
        public System.Windows.Forms.TextBox txtNewProjectLocation;
        private System.Windows.Forms.Button btnFileManager;
        private System.Windows.Forms.Button btnFolderManager;
        private System.Windows.Forms.Label lblProjectLocation;
        private OpenBots.Core.UI.Controls.UIPictureButton btnOpenProject;
        public System.Windows.Forms.Label lblError;
        private Core.UI.Controls.UIPictureButton btnCreateGalleryProject;
        private Core.UI.Controls.UIPictureButton btnRecentProjects;
        private System.Windows.Forms.Label lblProjectType;
        public System.Windows.Forms.ComboBox cbxProjectType;
    }
}