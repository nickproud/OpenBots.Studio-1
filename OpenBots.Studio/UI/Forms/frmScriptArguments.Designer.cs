namespace OpenBots.UI.Forms
{
    partial class frmScriptArguments
    {
        /// <summary>
        /// Required designer argument.
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScriptArguments));
            this.lblMainLogo = new System.Windows.Forms.Label();
            this.uiBtnCancel = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnOK = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.tlpArguments = new System.Windows.Forms.TableLayoutPanel();
            this.tvScriptArguments = new OpenBots.UI.CustomControls.CustomUIControls.UITreeView();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblDefineName = new System.Windows.Forms.Label();
            this.uiBtnNew = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.pnlBottom = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOK)).BeginInit();
            this.tlpArguments.SuspendLayout();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnNew)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblMainLogo.Font = new System.Drawing.Font("Segoe UI Semilight", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblMainLogo.Location = new System.Drawing.Point(-3, -2);
            this.lblMainLogo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(173, 54);
            this.lblMainLogo.TabIndex = 7;
            this.lblMainLogo.Text = "arguments";
            // 
            // uiBtnCancel
            // 
            this.uiBtnCancel.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnCancel.DisplayText = "Cancel";
            this.uiBtnCancel.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnCancel.Image")));
            this.uiBtnCancel.IsMouseOver = false;
            this.uiBtnCancel.Location = new System.Drawing.Point(70, 1);
            this.uiBtnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(60, 59);
            this.uiBtnCancel.TabIndex = 15;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Text = "Cancel";
            this.uiBtnCancel.Click += new System.EventHandler(this.uiBtnCancel_Click);
            // 
            // uiBtnOK
            // 
            this.uiBtnOK.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOK.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOK.DisplayText = "Ok";
            this.uiBtnOK.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnOK.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOK.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnOK.Image")));
            this.uiBtnOK.IsMouseOver = false;
            this.uiBtnOK.Location = new System.Drawing.Point(10, 1);
            this.uiBtnOK.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnOK.Name = "uiBtnOK";
            this.uiBtnOK.Size = new System.Drawing.Size(60, 59);
            this.uiBtnOK.TabIndex = 14;
            this.uiBtnOK.TabStop = false;
            this.uiBtnOK.Text = "Ok";
            this.uiBtnOK.Click += new System.EventHandler(this.uiBtnOK_Click);
            // 
            // tlpArguments
            // 
            this.tlpArguments.BackColor = System.Drawing.Color.Transparent;
            this.tlpArguments.ColumnCount = 1;
            this.tlpArguments.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpArguments.Controls.Add(this.tvScriptArguments, 0, 1);
            this.tlpArguments.Controls.Add(this.pnlTop, 0, 0);
            this.tlpArguments.Controls.Add(this.pnlBottom, 0, 2);
            this.tlpArguments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpArguments.Location = new System.Drawing.Point(0, 0);
            this.tlpArguments.Margin = new System.Windows.Forms.Padding(4);
            this.tlpArguments.Name = "tlpArguments";
            this.tlpArguments.RowCount = 3;
            this.tlpArguments.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 119F));
            this.tlpArguments.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpArguments.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 63F));
            this.tlpArguments.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpArguments.Size = new System.Drawing.Size(865, 647);
            this.tlpArguments.TabIndex = 17;
            // 
            // tvScriptArguments
            // 
            this.tvScriptArguments.BackColor = System.Drawing.Color.DimGray;
            this.tvScriptArguments.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvScriptArguments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvScriptArguments.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tvScriptArguments.ForeColor = System.Drawing.Color.White;
            this.tvScriptArguments.Location = new System.Drawing.Point(4, 123);
            this.tvScriptArguments.Margin = new System.Windows.Forms.Padding(4);
            this.tvScriptArguments.Name = "tvScriptArguments";
            this.tvScriptArguments.ShowLines = false;
            this.tvScriptArguments.Size = new System.Drawing.Size(857, 457);
            this.tvScriptArguments.TabIndex = 18;
            this.tvScriptArguments.DoubleClick += new System.EventHandler(this.tvScriptArguments_DoubleClick);
            this.tvScriptArguments.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvScriptArguments_KeyDown);
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.pnlTop.Controls.Add(this.lblDefineName);
            this.pnlTop.Controls.Add(this.uiBtnNew);
            this.pnlTop.Controls.Add(this.lblMainLogo);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(865, 119);
            this.pnlTop.TabIndex = 18;
            // 
            // lblDefineName
            // 
            this.lblDefineName.AutoSize = true;
            this.lblDefineName.BackColor = System.Drawing.Color.Transparent;
            this.lblDefineName.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDefineName.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.lblDefineName.Location = new System.Drawing.Point(73, 60);
            this.lblDefineName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDefineName.Name = "lblDefineName";
            this.lblDefineName.Size = new System.Drawing.Size(346, 50);
            this.lblDefineName.TabIndex = 16;
            this.lblDefineName.Text = "Double-Click to edit existing arguments\r\nPress \'DEL\' key to delete existing varia" +
    "bles";
            // 
            // uiBtnNew
            // 
            this.uiBtnNew.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnNew.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnNew.DisplayText = "Add";
            this.uiBtnNew.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.uiBtnNew.Image = global::OpenBots.Properties.Resources.action_bar_new;
            this.uiBtnNew.IsMouseOver = false;
            this.uiBtnNew.Location = new System.Drawing.Point(10, 55);
            this.uiBtnNew.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnNew.Name = "uiBtnNew";
            this.uiBtnNew.Size = new System.Drawing.Size(60, 60);
            this.uiBtnNew.TabIndex = 13;
            this.uiBtnNew.TabStop = false;
            this.uiBtnNew.Text = "Add";
            this.uiBtnNew.Click += new System.EventHandler(this.uiBtnNew_Click);
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.Transparent;
            this.pnlBottom.Controls.Add(this.uiBtnOK);
            this.pnlBottom.Controls.Add(this.uiBtnCancel);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(0, 584);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(865, 63);
            this.pnlBottom.TabIndex = 19;
            // 
            // frmScriptArguments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(865, 647);
            this.Controls.Add(this.tlpArguments);
            this.Icon = OpenBots.Properties.Resources.OpenBots_ico;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "frmScriptArguments";
            this.Text = "Arguments";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmScriptArguments_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOK)).EndInit();
            this.tlpArguments.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnNew)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblMainLogo;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnCancel;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnOK;
        private System.Windows.Forms.TableLayoutPanel tlpArguments;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlBottom;
        private CustomControls.CustomUIControls.UITreeView tvScriptArguments;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnNew;
        private System.Windows.Forms.Label lblDefineName;
    }
}