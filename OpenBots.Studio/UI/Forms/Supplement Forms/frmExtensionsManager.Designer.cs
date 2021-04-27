namespace OpenBots.UI.Forms.Supplement_Forms
{
    partial class frmExtentionsManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExtentionsManager));
            this.btnInstallChromeNativeMessaging = new System.Windows.Forms.Button();
            this.lblMainLogo = new System.Windows.Forms.Label();
            this.lblChromeNativeMessaging = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnInstallChromeNativeMessaging
            // 
            this.btnInstallChromeNativeMessaging.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.btnInstallChromeNativeMessaging.Location = new System.Drawing.Point(281, 69);
            this.btnInstallChromeNativeMessaging.Name = "btnInstallChromeNativeMessaging";
            this.btnInstallChromeNativeMessaging.Size = new System.Drawing.Size(111, 35);
            this.btnInstallChromeNativeMessaging.TabIndex = 45;
            this.btnInstallChromeNativeMessaging.Text = "Install";
            this.btnInstallChromeNativeMessaging.UseVisualStyleBackColor = true;
            this.btnInstallChromeNativeMessaging.Click += new System.EventHandler(this.btnInstallChromeNativeMessaging_Click);
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblMainLogo.Font = new System.Drawing.Font("Segoe UI Semilight", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblMainLogo.Location = new System.Drawing.Point(0, 0);
            this.lblMainLogo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(369, 54);
            this.lblMainLogo.TabIndex = 47;
            this.lblMainLogo.Text = "extensions manager";
            // 
            // lblChromeNativeMessaging
            // 
            this.lblChromeNativeMessaging.AutoSize = true;
            this.lblChromeNativeMessaging.BackColor = System.Drawing.Color.Transparent;
            this.lblChromeNativeMessaging.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChromeNativeMessaging.ForeColor = System.Drawing.Color.White;
            this.lblChromeNativeMessaging.Location = new System.Drawing.Point(4, 72);
            this.lblChromeNativeMessaging.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblChromeNativeMessaging.Name = "lblChromeNativeMessaging";
            this.lblChromeNativeMessaging.Size = new System.Drawing.Size(252, 28);
            this.lblChromeNativeMessaging.TabIndex = 48;
            this.lblChromeNativeMessaging.Text = "Chrome Native Messaging";
            // 
            // frmExtentionsManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(409, 123);
            this.Controls.Add(this.lblChromeNativeMessaging);
            this.Controls.Add(this.lblMainLogo);
            this.Controls.Add(this.btnInstallChromeNativeMessaging);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmExtentionsManager";
            this.Text = "Extensions Manager";
            this.Load += new System.EventHandler(this.frmExtentionsManager_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnInstallChromeNativeMessaging;
        private System.Windows.Forms.Label lblMainLogo;
        private System.Windows.Forms.Label lblChromeNativeMessaging;
    }
}