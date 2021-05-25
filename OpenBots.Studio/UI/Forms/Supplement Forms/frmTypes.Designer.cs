namespace OpenBots.UI.Forms.Supplement_Forms
{
    partial class frmTypes
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTypes));
            this.tlpCommands = new System.Windows.Forms.TableLayoutPanel();
            this.tvTypes = new OpenBots.UI.CustomControls.CustomUIControls.UITreeView();
            this.pnlCommandSearch = new System.Windows.Forms.Panel();
            this.uiBtnCollapse = new OpenBots.UI.CustomControls.CustomUIControls.UIIconButton();
            this.uiBtnExpand = new OpenBots.UI.CustomControls.CustomUIControls.UIIconButton();
            this.uiBtnClearTypeSearch = new OpenBots.UI.CustomControls.CustomUIControls.UIIconButton();
            this.txtTypeSearch = new System.Windows.Forms.TextBox();
            this.pnlResultButtons = new System.Windows.Forms.Panel();
            this.uiBtnOk = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnCancel = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.flpTypeConstruction = new System.Windows.Forms.FlowLayoutPanel();
            this.ttTypeButtons = new System.Windows.Forms.ToolTip(this.components);
            this.tlpCommands.SuspendLayout();
            this.pnlCommandSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCollapse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnExpand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClearTypeSearch)).BeginInit();
            this.pnlResultButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpCommands
            // 
            this.tlpCommands.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.tlpCommands.ColumnCount = 1;
            this.tlpCommands.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCommands.Controls.Add(this.tvTypes, 0, 2);
            this.tlpCommands.Controls.Add(this.pnlCommandSearch, 0, 0);
            this.tlpCommands.Controls.Add(this.pnlResultButtons, 0, 3);
            this.tlpCommands.Controls.Add(this.flpTypeConstruction, 0, 1);
            this.tlpCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCommands.Location = new System.Drawing.Point(0, 0);
            this.tlpCommands.Margin = new System.Windows.Forms.Padding(2);
            this.tlpCommands.Name = "tlpCommands";
            this.tlpCommands.RowCount = 4;
            this.tlpCommands.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpCommands.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tlpCommands.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCommands.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tlpCommands.Size = new System.Drawing.Size(838, 670);
            this.tlpCommands.TabIndex = 11;
            // 
            // tvTypes
            // 
            this.tvTypes.AllowDrop = true;
            this.tvTypes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.tvTypes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvTypes.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tvTypes.ForeColor = System.Drawing.Color.White;
            this.tvTypes.Location = new System.Drawing.Point(4, 70);
            this.tvTypes.Margin = new System.Windows.Forms.Padding(4);
            this.tvTypes.Name = "tvTypes";
            this.tvTypes.ShowLines = false;
            this.tvTypes.ShowNodeToolTips = true;
            this.tvTypes.Size = new System.Drawing.Size(830, 528);
            this.tvTypes.TabIndex = 9;
            this.tvTypes.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvTypes_AfterSelect);
            // 
            // pnlCommandSearch
            // 
            this.pnlCommandSearch.Controls.Add(this.uiBtnCollapse);
            this.pnlCommandSearch.Controls.Add(this.uiBtnExpand);
            this.pnlCommandSearch.Controls.Add(this.uiBtnClearTypeSearch);
            this.pnlCommandSearch.Controls.Add(this.txtTypeSearch);
            this.pnlCommandSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCommandSearch.Location = new System.Drawing.Point(2, 2);
            this.pnlCommandSearch.Margin = new System.Windows.Forms.Padding(2);
            this.pnlCommandSearch.Name = "pnlCommandSearch";
            this.pnlCommandSearch.Size = new System.Drawing.Size(834, 27);
            this.pnlCommandSearch.TabIndex = 10;
            // 
            // uiBtnCollapse
            // 
            this.uiBtnCollapse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiBtnCollapse.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnCollapse.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnCollapse.DisplayText = null;
            this.uiBtnCollapse.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnCollapse.Image = global::OpenBots.Properties.Resources.project_collapse;
            this.uiBtnCollapse.IsMouseOver = false;
            this.uiBtnCollapse.Location = new System.Drawing.Point(806, 1);
            this.uiBtnCollapse.Margin = new System.Windows.Forms.Padding(2);
            this.uiBtnCollapse.Name = "uiBtnCollapse";
            this.uiBtnCollapse.Size = new System.Drawing.Size(25, 25);
            this.uiBtnCollapse.TabIndex = 4;
            this.uiBtnCollapse.TabStop = false;
            this.uiBtnCollapse.Text = null;
            this.ttTypeButtons.SetToolTip(this.uiBtnCollapse, "Collapse");
            this.uiBtnCollapse.Click += new System.EventHandler(this.uiBtnCollapse_Click);
            // 
            // uiBtnExpand
            // 
            this.uiBtnExpand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiBtnExpand.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnExpand.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnExpand.DisplayText = null;
            this.uiBtnExpand.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnExpand.Image = global::OpenBots.Properties.Resources.project_expand;
            this.uiBtnExpand.IsMouseOver = false;
            this.uiBtnExpand.Location = new System.Drawing.Point(778, 1);
            this.uiBtnExpand.Margin = new System.Windows.Forms.Padding(2);
            this.uiBtnExpand.Name = "uiBtnExpand";
            this.uiBtnExpand.Size = new System.Drawing.Size(25, 25);
            this.uiBtnExpand.TabIndex = 2;
            this.uiBtnExpand.TabStop = false;
            this.uiBtnExpand.Text = null;
            this.ttTypeButtons.SetToolTip(this.uiBtnExpand, "Expand");
            this.uiBtnExpand.Click += new System.EventHandler(this.uiBtnExpand_Click);
            // 
            // uiBtnClearTypeSearch
            // 
            this.uiBtnClearTypeSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiBtnClearTypeSearch.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnClearTypeSearch.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnClearTypeSearch.DisplayText = null;
            this.uiBtnClearTypeSearch.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnClearTypeSearch.Image = global::OpenBots.Properties.Resources.commandsearch_clear;
            this.uiBtnClearTypeSearch.IsMouseOver = false;
            this.uiBtnClearTypeSearch.Location = new System.Drawing.Point(750, 1);
            this.uiBtnClearTypeSearch.Margin = new System.Windows.Forms.Padding(2);
            this.uiBtnClearTypeSearch.Name = "uiBtnClearTypeSearch";
            this.uiBtnClearTypeSearch.Size = new System.Drawing.Size(25, 25);
            this.uiBtnClearTypeSearch.TabIndex = 1;
            this.uiBtnClearTypeSearch.TabStop = false;
            this.uiBtnClearTypeSearch.Text = null;
            this.ttTypeButtons.SetToolTip(this.uiBtnClearTypeSearch, "Clear Search");
            this.uiBtnClearTypeSearch.Click += new System.EventHandler(this.uiBtnClearTypeSearch_Click);
            // 
            // txtTypeSearch
            // 
            this.txtTypeSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTypeSearch.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.txtTypeSearch.ForeColor = System.Drawing.Color.LightGray;
            this.txtTypeSearch.Location = new System.Drawing.Point(0, 0);
            this.txtTypeSearch.Margin = new System.Windows.Forms.Padding(2);
            this.txtTypeSearch.Name = "txtTypeSearch";
            this.txtTypeSearch.Size = new System.Drawing.Size(746, 30);
            this.txtTypeSearch.TabIndex = 0;
            this.txtTypeSearch.Text = "Type Here to Search";
            this.txtTypeSearch.TextChanged += new System.EventHandler(this.txtTypeSearch_TextChanged);
            this.txtTypeSearch.Enter += new System.EventHandler(this.txtTypeSearch_Enter);
            this.txtTypeSearch.Leave += new System.EventHandler(this.txtTypeSearch_Leave);
            // 
            // pnlResultButtons
            // 
            this.pnlResultButtons.Controls.Add(this.uiBtnOk);
            this.pnlResultButtons.Controls.Add(this.uiBtnCancel);
            this.pnlResultButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlResultButtons.Location = new System.Drawing.Point(3, 605);
            this.pnlResultButtons.Name = "pnlResultButtons";
            this.pnlResultButtons.Size = new System.Drawing.Size(832, 62);
            this.pnlResultButtons.TabIndex = 12;
            // 
            // uiBtnOk
            // 
            this.uiBtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.uiBtnOk.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOk.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOk.DisplayText = "Ok";
            this.uiBtnOk.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnOk.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOk.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnOk.Image")));
            this.uiBtnOk.IsMouseOver = false;
            this.uiBtnOk.Location = new System.Drawing.Point(709, 0);
            this.uiBtnOk.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.uiBtnOk.Name = "uiBtnOk";
            this.uiBtnOk.Size = new System.Drawing.Size(60, 60);
            this.uiBtnOk.TabIndex = 18;
            this.uiBtnOk.TabStop = false;
            this.uiBtnOk.Text = "Ok";
            this.uiBtnOk.Click += new System.EventHandler(this.uiBtnOk_Click);
            // 
            // uiBtnCancel
            // 
            this.uiBtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.uiBtnCancel.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnCancel.DisplayText = "Cancel";
            this.uiBtnCancel.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnCancel.Image")));
            this.uiBtnCancel.IsMouseOver = false;
            this.uiBtnCancel.Location = new System.Drawing.Point(769, 0);
            this.uiBtnCancel.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(60, 60);
            this.uiBtnCancel.TabIndex = 19;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Text = "Cancel";
            this.uiBtnCancel.Click += new System.EventHandler(this.uiBtnCancel_Click);
            // 
            // flpTypeConstruction
            // 
            this.flpTypeConstruction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpTypeConstruction.Location = new System.Drawing.Point(3, 34);
            this.flpTypeConstruction.Name = "flpTypeConstruction";
            this.flpTypeConstruction.Size = new System.Drawing.Size(832, 29);
            this.flpTypeConstruction.TabIndex = 13;
            // 
            // frmTypes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 670);
            this.Controls.Add(this.tlpCommands);
            this.Name = "frmTypes";
            this.Text = "Types";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmTypes_Load);
            this.tlpCommands.ResumeLayout(false);
            this.pnlCommandSearch.ResumeLayout(false);
            this.pnlCommandSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCollapse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnExpand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClearTypeSearch)).EndInit();
            this.pnlResultButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpCommands;
        private CustomControls.CustomUIControls.UITreeView tvTypes;
        private System.Windows.Forms.Panel pnlCommandSearch;
        private CustomControls.CustomUIControls.UIIconButton uiBtnClearTypeSearch;
        private System.Windows.Forms.TextBox txtTypeSearch;
        private CustomControls.CustomUIControls.UIIconButton uiBtnExpand;
        private CustomControls.CustomUIControls.UIIconButton uiBtnCollapse;
        private System.Windows.Forms.ToolTip ttTypeButtons;
        private System.Windows.Forms.Panel pnlResultButtons;
        private Core.UI.Controls.UIPictureButton uiBtnOk;
        private Core.UI.Controls.UIPictureButton uiBtnCancel;
        private System.Windows.Forms.FlowLayoutPanel flpTypeConstruction;
    }
}