namespace OpenBots.UI.Forms
{
    partial class frmAttendedMode
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
            this.cboSelectedProject = new System.Windows.Forms.ComboBox();
            this.uiBtnClose = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnRun = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.attendedScriptWatcher = new System.IO.FileSystemWatcher();
            this.uiBtnSettings = new OpenBots.Core.UI.Controls.UIPictureButton();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRun)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.attendedScriptWatcher)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSettings)).BeginInit();
            this.SuspendLayout();
            // 
            // cboSelectedProject
            // 
            this.cboSelectedProject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelectedProject.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSelectedProject.FormattingEnabled = true;
            this.cboSelectedProject.Location = new System.Drawing.Point(74, 16);
            this.cboSelectedProject.Margin = new System.Windows.Forms.Padding(4);
            this.cboSelectedProject.Name = "cboSelectedProject";
            this.cboSelectedProject.Size = new System.Drawing.Size(444, 33);
            this.cboSelectedProject.TabIndex = 0;
            this.cboSelectedProject.MouseHover += new System.EventHandler(this.cboSelectedProject_MouseHover);
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
            this.uiBtnClose.Location = new System.Drawing.Point(5, 4);
            this.uiBtnClose.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnClose.Name = "uiBtnClose";
            this.uiBtnClose.Size = new System.Drawing.Size(60, 62);
            this.uiBtnClose.TabIndex = 14;
            this.uiBtnClose.TabStop = false;
            this.uiBtnClose.Text = "Close";
            this.uiBtnClose.Click += new System.EventHandler(this.uiBtnClose_Click);
            // 
            // uiBtnRun
            // 
            this.uiBtnRun.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnRun.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnRun.DisplayText = "Run";
            this.uiBtnRun.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnRun.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnRun.Image = global::OpenBots.Properties.Resources.action_bar_run;
            this.uiBtnRun.IsMouseOver = false;
            this.uiBtnRun.Location = new System.Drawing.Point(527, 4);
            this.uiBtnRun.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnRun.Name = "uiBtnRun";
            this.uiBtnRun.Size = new System.Drawing.Size(60, 62);
            this.uiBtnRun.TabIndex = 13;
            this.uiBtnRun.TabStop = false;
            this.uiBtnRun.Text = "Run";
            this.uiBtnRun.Click += new System.EventHandler(this.uiBtnRun_Click);
            // 
            // attendedScriptWatcher
            // 
            this.attendedScriptWatcher.EnableRaisingEvents = true;
            this.attendedScriptWatcher.SynchronizingObject = this;
            this.attendedScriptWatcher.Changed += new System.IO.FileSystemEventHandler(this.attendedScriptWatcher_Changed);
            this.attendedScriptWatcher.Created += new System.IO.FileSystemEventHandler(this.attendedScriptWatcher_Created);
            this.attendedScriptWatcher.Deleted += new System.IO.FileSystemEventHandler(this.attendedScriptWatcher_Deleted);
            this.attendedScriptWatcher.Renamed += new System.IO.RenamedEventHandler(this.attendedScriptWatcher_Renamed);
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
            this.uiBtnSettings.Location = new System.Drawing.Point(587, 4);
            this.uiBtnSettings.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnSettings.Name = "uiBtnSettings";
            this.uiBtnSettings.Size = new System.Drawing.Size(60, 62);
            this.uiBtnSettings.TabIndex = 15;
            this.uiBtnSettings.TabStop = false;
            this.uiBtnSettings.Text = "Settings";
            this.uiBtnSettings.Click += new System.EventHandler(this.uiBtnSettings_Click);
            // 
            // frmAttendedMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.ClientSize = new System.Drawing.Size(654, 70);
            this.Controls.Add(this.uiBtnSettings);
            this.Controls.Add(this.uiBtnClose);
            this.Controls.Add(this.uiBtnRun);
            this.Controls.Add(this.cboSelectedProject);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = global::OpenBots.Properties.Resources.OpenBots_ico;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmAttendedMode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "OpenBots Studio Attended Mode";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmAttendedMode_Load);
            this.Shown += new System.EventHandler(this.frmAttendedMode_Shown);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.frmAttendedMode_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmAttendedMode_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmAttendedMode_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmAttendedMode_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRun)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.attendedScriptWatcher)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSettings)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboSelectedProject;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnRun;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnClose;
        private System.IO.FileSystemWatcher attendedScriptWatcher;
        private Core.UI.Controls.UIPictureButton uiBtnSettings;
    }
}