namespace OpenBots.UI.Forms.Supplement_Forms
{
    partial class frmHTMLDisplayForm
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
            OpenBots.Core.Utilities.FormsUtilities.Theme theme1 = new OpenBots.Core.Utilities.FormsUtilities.Theme();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHTMLDisplayForm));
            this.webBrowserHTML = new System.Windows.Forms.WebBrowser();
            this.tlpDisplayForm = new System.Windows.Forms.TableLayoutPanel();
            this.pnlButtons = new OpenBots.UI.CustomControls.CustomUIControls.UIPanel();
            this.btnCancel = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.btnOkay = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.tlpDisplayForm.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOkay)).BeginInit();
            this.SuspendLayout();
            // 
            // webBrowserHTML
            // 
            this.webBrowserHTML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserHTML.Location = new System.Drawing.Point(4, 4);
            this.webBrowserHTML.Margin = new System.Windows.Forms.Padding(4);
            this.webBrowserHTML.MinimumSize = new System.Drawing.Size(27, 25);
            this.webBrowserHTML.Name = "webBrowserHTML";
            this.webBrowserHTML.Size = new System.Drawing.Size(1396, 898);
            this.webBrowserHTML.TabIndex = 0;
            // 
            // tlpDisplayForm
            // 
            this.tlpDisplayForm.ColumnCount = 1;
            this.tlpDisplayForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDisplayForm.Controls.Add(this.webBrowserHTML, 0, 0);
            this.tlpDisplayForm.Controls.Add(this.pnlButtons, 0, 1);
            this.tlpDisplayForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDisplayForm.Location = new System.Drawing.Point(0, 0);
            this.tlpDisplayForm.Name = "tlpDisplayForm";
            this.tlpDisplayForm.RowCount = 2;
            this.tlpDisplayForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDisplayForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tlpDisplayForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpDisplayForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpDisplayForm.Size = new System.Drawing.Size(1404, 986);
            this.tlpDisplayForm.TabIndex = 1;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnOkay);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlButtons.Location = new System.Drawing.Point(3, 909);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(1398, 74);
            this.pnlButtons.TabIndex = 1;
            theme1.BgGradientEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            theme1.BgGradientStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.pnlButtons.Theme = theme1;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnCancel.DisplayText = "Cancel";
            this.btnCancel.DisplayTextBrush = System.Drawing.Color.White;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.IsMouseOver = false;
            this.btnCancel.Location = new System.Drawing.Point(1332, 8);
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
            this.btnOkay.BackColor = System.Drawing.Color.Transparent;
            this.btnOkay.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnOkay.DisplayText = "OK";
            this.btnOkay.DisplayTextBrush = System.Drawing.Color.White;
            this.btnOkay.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnOkay.Image = ((System.Drawing.Image)(resources.GetObject("btnOkay.Image")));
            this.btnOkay.IsMouseOver = false;
            this.btnOkay.Location = new System.Drawing.Point(1272, 8);
            this.btnOkay.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(60, 60);
            this.btnOkay.TabIndex = 31;
            this.btnOkay.TabStop = false;
            this.btnOkay.Text = "OK";
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // frmHTMLDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1404, 986);
            this.Controls.Add(this.tlpDisplayForm);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmHTMLDisplayForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HTML Display";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmHTMLDisplayForm_Load);
            this.tlpDisplayForm.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOkay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowserHTML;
        private System.Windows.Forms.TableLayoutPanel tlpDisplayForm;
        private CustomControls.CustomUIControls.UIPanel pnlButtons;
        private Core.UI.Controls.UIPictureButton btnCancel;
        private Core.UI.Controls.UIPictureButton btnOkay;
    }
}