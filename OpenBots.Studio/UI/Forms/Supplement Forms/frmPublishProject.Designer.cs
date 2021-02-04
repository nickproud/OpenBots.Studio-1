namespace OpenBots.UI.Supplement_Forms
{
    partial class frmPublishProject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPublishProject));
            this.btnCancel = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.btnOkay = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.txtAuthorName = new System.Windows.Forms.TextBox();
            this.lblAuthorName = new System.Windows.Forms.Label();
            this.lblPublish = new System.Windows.Forms.Label();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.lblLocalPath = new System.Windows.Forms.Label();
            this.btnFolderManager = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.cbxLocation = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOkay)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnCancel.DisplayText = "Cancel";
            this.btnCancel.DisplayTextBrush = System.Drawing.Color.White;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.IsMouseOver = false;
            this.btnCancel.Location = new System.Drawing.Point(80, 464);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 60);
            this.btnCancel.TabIndex = 32;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOkay.BackColor = System.Drawing.Color.Transparent;
            this.btnOkay.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnOkay.DisplayText = "OK";
            this.btnOkay.DisplayTextBrush = System.Drawing.Color.White;
            this.btnOkay.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnOkay.Image = ((System.Drawing.Image)(resources.GetObject("btnOkay.Image")));
            this.btnOkay.IsMouseOver = false;
            this.btnOkay.Location = new System.Drawing.Point(20, 464);
            this.btnOkay.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(60, 60);
            this.btnOkay.TabIndex = 31;
            this.btnOkay.TabStop = false;
            this.btnOkay.Text = "OK";
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // txtAuthorName
            // 
            this.txtAuthorName.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAuthorName.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtAuthorName.Location = new System.Drawing.Point(20, 90);
            this.txtAuthorName.Margin = new System.Windows.Forms.Padding(4);
            this.txtAuthorName.Name = "txtAuthorName";
            this.txtAuthorName.Size = new System.Drawing.Size(549, 32);
            this.txtAuthorName.TabIndex = 34;
            // 
            // lblAuthorName
            // 
            this.lblAuthorName.BackColor = System.Drawing.Color.Transparent;
            this.lblAuthorName.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthorName.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblAuthorName.Location = new System.Drawing.Point(15, 60);
            this.lblAuthorName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAuthorName.Name = "lblAuthorName";
            this.lblAuthorName.Size = new System.Drawing.Size(554, 30);
            this.lblAuthorName.TabIndex = 33;
            this.lblAuthorName.Text = "Author Name";
            // 
            // lblPublish
            // 
            this.lblPublish.AutoSize = true;
            this.lblPublish.BackColor = System.Drawing.Color.Transparent;
            this.lblPublish.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPublish.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblPublish.Location = new System.Drawing.Point(12, 9);
            this.lblPublish.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblPublish.Name = "lblPublish";
            this.lblPublish.Size = new System.Drawing.Size(158, 46);
            this.lblPublish.TabIndex = 35;
            this.lblPublish.Text = "Publish";
            // 
            // txtVersion
            // 
            this.txtVersion.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVersion.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtVersion.Location = new System.Drawing.Point(20, 160);
            this.txtVersion.Margin = new System.Windows.Forms.Padding(4);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(550, 32);
            this.txtVersion.TabIndex = 37;
            this.txtVersion.Text = "1.0.0";
            // 
            // lblVersion
            // 
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblVersion.Location = new System.Drawing.Point(15, 130);
            this.lblVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(554, 30);
            this.lblVersion.TabIndex = 36;
            this.lblVersion.Text = "Version";
            // 
            // txtDescription
            // 
            this.txtDescription.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescription.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtDescription.Location = new System.Drawing.Point(20, 230);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(4);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(549, 76);
            this.txtDescription.TabIndex = 39;
            // 
            // lblDescription
            // 
            this.lblDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblDescription.Location = new System.Drawing.Point(15, 200);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(483, 30);
            this.lblDescription.TabIndex = 38;
            this.lblDescription.Text = "Description";
            // 
            // txtLocation
            // 
            this.txtLocation.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLocation.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtLocation.Location = new System.Drawing.Point(20, 345);
            this.txtLocation.Margin = new System.Windows.Forms.Padding(4);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(510, 32);
            this.txtLocation.TabIndex = 45;
            this.txtLocation.TextChanged += new System.EventHandler(this.txtLocation_TextChanged);
            // 
            // lblLocalPath
            // 
            this.lblLocalPath.BackColor = System.Drawing.Color.Transparent;
            this.lblLocalPath.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocalPath.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblLocalPath.Location = new System.Drawing.Point(15, 315);
            this.lblLocalPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLocalPath.Name = "lblLocalPath";
            this.lblLocalPath.Size = new System.Drawing.Size(554, 30);
            this.lblLocalPath.TabIndex = 44;
            this.lblLocalPath.Text = "Local Path";
            // 
            // btnFolderManager
            // 
            this.btnFolderManager.Location = new System.Drawing.Point(537, 345);
            this.btnFolderManager.Name = "btnFolderManager";
            this.btnFolderManager.Size = new System.Drawing.Size(32, 32);
            this.btnFolderManager.TabIndex = 46;
            this.btnFolderManager.Text = "...";
            this.btnFolderManager.UseVisualStyleBackColor = true;
            this.btnFolderManager.Click += new System.EventHandler(this.btnFolderManager_Click);
            // 
            // lblError
            // 
            this.lblError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblError.BackColor = System.Drawing.Color.Transparent;
            this.lblError.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblError.Location = new System.Drawing.Point(152, 494);
            this.lblError.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(417, 30);
            this.lblError.TabIndex = 47;
            // 
            // cbxLocation
            // 
            this.cbxLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxLocation.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.cbxLocation.ForeColor = System.Drawing.Color.SteelBlue;
            this.cbxLocation.FormattingEnabled = true;
            this.cbxLocation.Items.AddRange(new object[] {
            "Local and Server",
            "Local Only"});
            this.cbxLocation.Location = new System.Drawing.Point(20, 415);
            this.cbxLocation.Margin = new System.Windows.Forms.Padding(4);
            this.cbxLocation.Name = "cbxLocation";
            this.cbxLocation.Size = new System.Drawing.Size(549, 33);
            this.cbxLocation.TabIndex = 48;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(16, 385);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(554, 30);
            this.label1.TabIndex = 49;
            this.label1.Text = "Location";
            // 
            // frmPublishProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 542);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbxLocation);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.btnFolderManager);
            this.Controls.Add(this.txtLocation);
            this.Controls.Add(this.lblLocalPath);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtVersion);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblPublish);
            this.Controls.Add(this.txtAuthorName);
            this.Controls.Add(this.lblAuthorName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPublishProject";
            this.Text = "Publish";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmPublishProject_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOkay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenBots.Core.UI.Controls.UIPictureButton btnCancel;
        private OpenBots.Core.UI.Controls.UIPictureButton btnOkay;
        public System.Windows.Forms.TextBox txtAuthorName;
        private System.Windows.Forms.Label lblAuthorName;
        private System.Windows.Forms.Label lblPublish;
        public System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.Label lblVersion;
        public System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescription;
        public System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.Label lblLocalPath;
        private System.Windows.Forms.Button btnFolderManager;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.ComboBox cbxLocation;
        private System.Windows.Forms.Label label1;
    }
}