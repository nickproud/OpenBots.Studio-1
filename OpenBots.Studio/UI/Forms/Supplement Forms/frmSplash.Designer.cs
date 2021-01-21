namespace OpenBots.UI.Forms.Supplement_Forms
{
    partial class frmSplash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSplash));
            this.lblAppVersion = new System.Windows.Forms.Label();
            this.lblFirstTimeSetup = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblAppVersion
            // 
            this.lblAppVersion.AutoSize = true;
            this.lblAppVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblAppVersion.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppVersion.ForeColor = System.Drawing.Color.Black;
            this.lblAppVersion.Location = new System.Drawing.Point(297, 211);
            this.lblAppVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAppVersion.Name = "lblAppVersion";
            this.lblAppVersion.Size = new System.Drawing.Size(30, 37);
            this.lblAppVersion.TabIndex = 3;
            this.lblAppVersion.Text = "v";
            // 
            // lblFirstTimeSetup
            // 
            this.lblFirstTimeSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFirstTimeSetup.AutoSize = true;
            this.lblFirstTimeSetup.BackColor = System.Drawing.Color.Transparent;
            this.lblFirstTimeSetup.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblFirstTimeSetup.ForeColor = System.Drawing.Color.Black;
            this.lblFirstTimeSetup.Location = new System.Drawing.Point(1, 271);
            this.lblFirstTimeSetup.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFirstTimeSetup.Name = "lblFirstTimeSetup";
            this.lblFirstTimeSetup.Size = new System.Drawing.Size(289, 25);
            this.lblFirstTimeSetup.TabIndex = 4;
            this.lblFirstTimeSetup.Text = "Performing first time user setup...";
            this.lblFirstTimeSetup.Visible = false;
            // 
            // frmSplash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = global::OpenBots.Properties.Resources.OpenBots_splash;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(450, 300);
            this.Controls.Add(this.lblFirstTimeSetup);
            this.Controls.Add(this.lblAppVersion);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmSplash";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Welcome to OpenBots Studio!";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmSplash_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label lblAppVersion;
        public System.Windows.Forms.Label lblFirstTimeSetup;
    }
}