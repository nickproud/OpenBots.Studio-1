namespace OpenBots.UI.Forms.Supplement_Forms
{
    partial class frmVariableArgumentSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVariableArgumentSelector));
            this.lblHeader = new System.Windows.Forms.Label();
            this.uiBtnOk = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnCancel = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.tcVariableArgument = new OpenBots.UI.CustomControls.CustomUIControls.UITabControl();
            this.variables = new System.Windows.Forms.TabPage();
            this.tlpVariables = new System.Windows.Forms.TableLayoutPanel();
            this.txtVariableSearch = new System.Windows.Forms.TextBox();
            this.lstVariables = new System.Windows.Forms.ListBox();
            this.arguments = new System.Windows.Forms.TabPage();
            this.tlpArguments = new System.Windows.Forms.TableLayoutPanel();
            this.txtArgumentSearch = new System.Windows.Forms.TextBox();
            this.lstArguments = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            this.tcVariableArgument.SuspendLayout();
            this.variables.SuspendLayout();
            this.tlpVariables.SuspendLayout();
            this.arguments.SuspendLayout();
            this.tlpArguments.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI Light", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(7, 4);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(407, 32);
            this.lblHeader.TabIndex = 1;
            this.lblHeader.Text = "Insert a variable/argument from the list";
            // 
            // uiBtnOk
            // 
            this.uiBtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.uiBtnOk.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOk.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOk.DisplayText = "Ok";
            this.uiBtnOk.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnOk.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOk.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnOk.Image")));
            this.uiBtnOk.IsMouseOver = false;
            this.uiBtnOk.Location = new System.Drawing.Point(13, 430);
            this.uiBtnOk.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.uiBtnOk.Name = "uiBtnOk";
            this.uiBtnOk.Size = new System.Drawing.Size(45, 49);
            this.uiBtnOk.TabIndex = 18;
            this.uiBtnOk.TabStop = false;
            this.uiBtnOk.Text = "Ok";
            this.uiBtnOk.Click += new System.EventHandler(this.uiBtnOk_Click);
            // 
            // uiBtnCancel
            // 
            this.uiBtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.uiBtnCancel.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnCancel.DisplayText = "Cancel";
            this.uiBtnCancel.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnCancel.Image")));
            this.uiBtnCancel.IsMouseOver = false;
            this.uiBtnCancel.Location = new System.Drawing.Point(58, 430);
            this.uiBtnCancel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(45, 49);
            this.uiBtnCancel.TabIndex = 19;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Text = "Cancel";
            this.uiBtnCancel.Click += new System.EventHandler(this.uiBtnCancel_Click);
            // 
            // tcVariableArgument
            // 
            this.tcVariableArgument.AllowDrop = true;
            this.tcVariableArgument.Controls.Add(this.variables);
            this.tcVariableArgument.Controls.Add(this.arguments);
            this.tcVariableArgument.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcVariableArgument.Location = new System.Drawing.Point(12, 40);
            this.tcVariableArgument.Margin = new System.Windows.Forms.Padding(2);
            this.tcVariableArgument.Name = "tcVariableArgument";
            this.tcVariableArgument.SelectedIndex = 0;
            this.tcVariableArgument.Size = new System.Drawing.Size(422, 375);
            this.tcVariableArgument.TabIndex = 21;
            this.tcVariableArgument.SelectedIndexChanged += new System.EventHandler(this.tcVariableArgument_SelectedIndexChanged);
            // 
            // variables
            // 
            this.variables.Controls.Add(this.tlpVariables);
            this.variables.Font = new System.Drawing.Font("Segoe UI Semilight", 12F);
            this.variables.Location = new System.Drawing.Point(4, 26);
            this.variables.Margin = new System.Windows.Forms.Padding(2);
            this.variables.Name = "variables";
            this.variables.Padding = new System.Windows.Forms.Padding(2);
            this.variables.Size = new System.Drawing.Size(414, 345);
            this.variables.TabIndex = 0;
            this.variables.Text = "Variables";
            this.variables.UseVisualStyleBackColor = true;
            // 
            // tlpVariables
            // 
            this.tlpVariables.ColumnCount = 1;
            this.tlpVariables.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpVariables.Controls.Add(this.txtVariableSearch, 0, 0);
            this.tlpVariables.Controls.Add(this.lstVariables, 0, 1);
            this.tlpVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpVariables.Location = new System.Drawing.Point(2, 2);
            this.tlpVariables.Margin = new System.Windows.Forms.Padding(2);
            this.tlpVariables.Name = "tlpVariables";
            this.tlpVariables.RowCount = 2;
            this.tlpVariables.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlpVariables.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpVariables.Size = new System.Drawing.Size(410, 341);
            this.tlpVariables.TabIndex = 0;
            // 
            // txtVariableSearch
            // 
            this.txtVariableSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtVariableSearch.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVariableSearch.ForeColor = System.Drawing.Color.LightGray;
            this.txtVariableSearch.Location = new System.Drawing.Point(3, 3);
            this.txtVariableSearch.Name = "txtVariableSearch";
            this.txtVariableSearch.Size = new System.Drawing.Size(404, 27);
            this.txtVariableSearch.TabIndex = 20;
            this.txtVariableSearch.Text = "Type Here to Search";
            this.txtVariableSearch.TextChanged += new System.EventHandler(this.txtVariableSearch_TextChanged);
            this.txtVariableSearch.Enter += new System.EventHandler(this.txtVariableSearch_Enter);
            this.txtVariableSearch.Leave += new System.EventHandler(this.txtVariableSearch_Leave);
            // 
            // lstVariables
            // 
            this.lstVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstVariables.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstVariables.FormattingEnabled = true;
            this.lstVariables.ItemHeight = 21;
            this.lstVariables.Location = new System.Drawing.Point(3, 31);
            this.lstVariables.Name = "lstVariables";
            this.lstVariables.Size = new System.Drawing.Size(404, 307);
            this.lstVariables.TabIndex = 0;
            this.lstVariables.DoubleClick += new System.EventHandler(this.lstVariables_DoubleClick);
            // 
            // arguments
            // 
            this.arguments.Controls.Add(this.tlpArguments);
            this.arguments.Location = new System.Drawing.Point(4, 26);
            this.arguments.Margin = new System.Windows.Forms.Padding(2);
            this.arguments.Name = "arguments";
            this.arguments.Padding = new System.Windows.Forms.Padding(2);
            this.arguments.Size = new System.Drawing.Size(414, 345);
            this.arguments.TabIndex = 1;
            this.arguments.Text = "Arguments";
            this.arguments.UseVisualStyleBackColor = true;
            // 
            // tlpArguments
            // 
            this.tlpArguments.ColumnCount = 1;
            this.tlpArguments.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpArguments.Controls.Add(this.txtArgumentSearch, 0, 0);
            this.tlpArguments.Controls.Add(this.lstArguments, 0, 1);
            this.tlpArguments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpArguments.Location = new System.Drawing.Point(2, 2);
            this.tlpArguments.Margin = new System.Windows.Forms.Padding(2);
            this.tlpArguments.Name = "tlpArguments";
            this.tlpArguments.RowCount = 2;
            this.tlpArguments.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlpArguments.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpArguments.Size = new System.Drawing.Size(410, 341);
            this.tlpArguments.TabIndex = 22;
            // 
            // txtArgumentSearch
            // 
            this.txtArgumentSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtArgumentSearch.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtArgumentSearch.ForeColor = System.Drawing.Color.LightGray;
            this.txtArgumentSearch.Location = new System.Drawing.Point(3, 3);
            this.txtArgumentSearch.Name = "txtArgumentSearch";
            this.txtArgumentSearch.Size = new System.Drawing.Size(404, 27);
            this.txtArgumentSearch.TabIndex = 20;
            this.txtArgumentSearch.Text = "Type Here to Search";
            this.txtArgumentSearch.TextChanged += new System.EventHandler(this.txtArgumentSearch_TextChanged);
            this.txtArgumentSearch.Enter += new System.EventHandler(this.txtArgumentSearch_Enter);
            this.txtArgumentSearch.Leave += new System.EventHandler(this.txtArgumentSearch_Leave);
            // 
            // lstArguments
            // 
            this.lstArguments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstArguments.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstArguments.FormattingEnabled = true;
            this.lstArguments.ItemHeight = 21;
            this.lstArguments.Location = new System.Drawing.Point(3, 31);
            this.lstArguments.Name = "lstArguments";
            this.lstArguments.Size = new System.Drawing.Size(404, 307);
            this.lstArguments.TabIndex = 0;
            this.lstArguments.DoubleClick += new System.EventHandler(this.lstArguments_DoubleClick);
            // 
            // frmVariableArgumentSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(447, 491);
            this.Controls.Add(this.tcVariableArgument);
            this.Controls.Add(this.uiBtnOk);
            this.Controls.Add(this.uiBtnCancel);
            this.Controls.Add(this.lblHeader);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmVariableArgumentSelector";
            this.Text = "Insert a Variable/Argument";
            this.Load += new System.EventHandler(this.frmVariableSelector_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            this.tcVariableArgument.ResumeLayout(false);
            this.variables.ResumeLayout(false);
            this.tlpVariables.ResumeLayout(false);
            this.tlpVariables.PerformLayout();
            this.arguments.ResumeLayout(false);
            this.tlpArguments.ResumeLayout(false);
            this.tlpArguments.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnOk;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnCancel;
        public System.Windows.Forms.ListBox lstVariables;
        public System.Windows.Forms.Label lblHeader;
        public System.Windows.Forms.TextBox txtVariableSearch;
        private CustomControls.CustomUIControls.UITabControl tcVariableArgument;
        private System.Windows.Forms.TabPage variables;
        private System.Windows.Forms.TabPage arguments;
        public System.Windows.Forms.TextBox txtArgumentSearch;
        public System.Windows.Forms.ListBox lstArguments;
        private System.Windows.Forms.TableLayoutPanel tlpVariables;
        private System.Windows.Forms.TableLayoutPanel tlpArguments;
    }
}