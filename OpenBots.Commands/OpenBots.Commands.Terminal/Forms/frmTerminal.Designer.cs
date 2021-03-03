namespace OpenBots.Commands.Terminal.Forms
{
    partial class frmTerminal
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
            this.msTerminal = new System.Windows.Forms.MenuStrip();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fieldsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rowColToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fieldIndexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenEmulator = new OpenBots.Commands.Terminal.Forms.OpenEmulator();
            this.msTerminal.SuspendLayout();
            this.SuspendLayout();
            // 
            // msTerminal
            // 
            this.msTerminal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.msTerminal.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.msTerminal.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.msTerminal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.fieldsToolStripMenuItem,
            this.rowColToolStripMenuItem,
            this.fieldIndexToolStripMenuItem});
            this.msTerminal.Location = new System.Drawing.Point(0, 0);
            this.msTerminal.Name = "msTerminal";
            this.msTerminal.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.msTerminal.Size = new System.Drawing.Size(710, 29);
            this.msTerminal.TabIndex = 5;
            this.msTerminal.Text = "menuStrip1";
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(79, 25);
            this.connectToolStripMenuItem.Text = "Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(78, 25);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // fieldsToolStripMenuItem
            // 
            this.fieldsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.fieldsToolStripMenuItem.Name = "fieldsToolStripMenuItem";
            this.fieldsToolStripMenuItem.Size = new System.Drawing.Size(62, 25);
            this.fieldsToolStripMenuItem.Text = "Fields";
            this.fieldsToolStripMenuItem.Click += new System.EventHandler(this.fieldsToolStripMenuItem_Click);
            // 
            // rowColToolStripMenuItem
            // 
            this.rowColToolStripMenuItem.Enabled = false;
            this.rowColToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.rowColToolStripMenuItem.Name = "rowColToolStripMenuItem";
            this.rowColToolStripMenuItem.Size = new System.Drawing.Size(119, 25);
            this.rowColToolStripMenuItem.Text = "Row: 0, Col:  0";
            // 
            // fieldIndexToolStripMenuItem
            // 
            this.fieldIndexToolStripMenuItem.Enabled = false;
            this.fieldIndexToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.fieldIndexToolStripMenuItem.Name = "fieldIndexToolStripMenuItem";
            this.fieldIndexToolStripMenuItem.Size = new System.Drawing.Size(103, 25);
            this.fieldIndexToolStripMenuItem.Text = "Field Index: ";
            // 
            // OpenEmulator
            // 
            this.OpenEmulator.BackColor = System.Drawing.SystemColors.WindowText;
            this.OpenEmulator.Coordinates = new System.Drawing.Point(0, 0);
            this.OpenEmulator.FieldIndex = 0;
            this.OpenEmulator.Location = new System.Drawing.Point(0, 29);
            this.OpenEmulator.Margin = new System.Windows.Forms.Padding(2);
            this.OpenEmulator.Name = "OpenEmulator";
            this.OpenEmulator.Password = null;
            this.OpenEmulator.Size = new System.Drawing.Size(703, 447);
            this.OpenEmulator.TabIndex = 0;
            this.OpenEmulator.Text = "";
            this.OpenEmulator.Username = null;
            this.OpenEmulator.SelectionChanged += new System.EventHandler(this.OpenEmulator_SelectionChanged);
            this.OpenEmulator.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.OpenEmulator_PreviewKeyDown);
            // 
            // frmTerminal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 487);
            this.Controls.Add(this.OpenEmulator);
            this.Controls.Add(this.msTerminal);
            this.MainMenuStrip = this.msTerminal;
            this.Name = "frmTerminal";
            this.Text = "Terminal";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTerminal_FormClosing);
            this.msTerminal.ResumeLayout(false);
            this.msTerminal.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public OpenBots.Commands.Terminal.Forms.OpenEmulator OpenEmulator;
        private System.Windows.Forms.MenuStrip msTerminal;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rowColToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fieldsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fieldIndexToolStripMenuItem;
    }
}