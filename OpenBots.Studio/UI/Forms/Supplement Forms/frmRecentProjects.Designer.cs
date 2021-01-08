namespace OpenBots.UI.Forms.Supplement_Forms
{
    partial class frmRecentProjects
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRecentProjects));
            this.lblRecentProjects = new System.Windows.Forms.Label();
            this.lvRecentProjects = new System.Windows.Forms.ListView();
            this.ProjectName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ProjectPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lblRecentProjects
            // 
            this.lblRecentProjects.AutoSize = true;
            this.lblRecentProjects.BackColor = System.Drawing.Color.Transparent;
            this.lblRecentProjects.Font = new System.Drawing.Font("Segoe UI", 26.25F);
            this.lblRecentProjects.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblRecentProjects.Location = new System.Drawing.Point(1, -2);
            this.lblRecentProjects.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRecentProjects.Name = "lblRecentProjects";
            this.lblRecentProjects.Size = new System.Drawing.Size(322, 60);
            this.lblRecentProjects.TabIndex = 1;
            this.lblRecentProjects.Text = "Recent Projects";
            // 
            // lvRecentProjects
            // 
            this.lvRecentProjects.AllowColumnReorder = true;
            this.lvRecentProjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvRecentProjects.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ProjectName,
            this.ProjectPath});
            this.lvRecentProjects.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lvRecentProjects.FullRowSelect = true;
            this.lvRecentProjects.GridLines = true;
            this.lvRecentProjects.HideSelection = false;
            this.lvRecentProjects.Location = new System.Drawing.Point(12, 61);
            this.lvRecentProjects.Name = "lvRecentProjects";
            this.lvRecentProjects.ShowItemToolTips = true;
            this.lvRecentProjects.Size = new System.Drawing.Size(605, 385);
            this.lvRecentProjects.TabIndex = 3;
            this.lvRecentProjects.UseCompatibleStateImageBehavior = false;
            this.lvRecentProjects.View = System.Windows.Forms.View.Details;
            this.lvRecentProjects.DoubleClick += new System.EventHandler(this.lvRecentProjects_DoubleClick);
            // 
            // ProjectName
            // 
            this.ProjectName.Text = "Project Name";
            this.ProjectName.Width = 194;
            // 
            // ProjectPath
            // 
            this.ProjectPath.Text = "Project Path ";
            this.ProjectPath.Width = 406;
            // 
            // frmRecentProjects
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 458);
            this.Controls.Add(this.lvRecentProjects);
            this.Controls.Add(this.lblRecentProjects);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "frmRecentProjects";
            this.Text = "recent projects";
            this.Load += new System.EventHandler(this.frmRecentProjects_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRecentProjects;
        private System.Windows.Forms.ListView lvRecentProjects;
        private System.Windows.Forms.ColumnHeader ProjectName;
        private System.Windows.Forms.ColumnHeader ProjectPath;
    }
}