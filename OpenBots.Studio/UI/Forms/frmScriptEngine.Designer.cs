﻿using OpenBots.Core.User32;
using OpenBots.Utilities;
using System.Windows.Forms;

namespace OpenBots.UI.Forms
{
    partial class frmScriptEngine
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
                lstSteppingCommands.DrawItem -= lstSteppingCommands_DrawItem;
                lstSteppingCommands.MouseDoubleClick -= lstSteppingCommands_MouseDoubleClick;
                tmrNotify.Tick -= autoCloseTimer_Tick;
                uiBtnStepInto.Click -= uiBtnStepInto_Click;
                uiBtnStepOver.Click -= uiBtnStepOver_Click;
                uiBtnCancel.Click -= uiBtnCancel_Click;
                uiBtnPause.Click -= uiBtnPause_Click;
                pbBotIcon.Click -= pbBotIcon_Click;
                uiBtnScheduleManagement.Click -= uiBtnScheduleManagement_Click;
                FormClosing -= frmScriptEngine_FormClosing;
                Load -= frmProcessingStatus_Load;
                GlobalHook.HookStopped -= OnHookStopped;
                EngineInstance.ReportProgressEvent -= Engine_ReportProgress;
                EngineInstance.ScriptFinishedEvent -= Engine_ScriptFinishedEvent;
                EngineInstance.LineNumberChangedEvent -= EngineInstance_LineNumberChangedEvent;

                foreach (Control control in Controls)
                    control.Dispose();

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScriptEngine));
            this.lstSteppingCommands = new System.Windows.Forms.ListBox();
            this.tmrNotify = new System.Windows.Forms.Timer(this.components);
            this.lblCloseTimer = new System.Windows.Forms.Label();
            this.lblMainLogo = new System.Windows.Forms.Label();
            this.lblKillProcNote = new System.Windows.Forms.Label();
            this.lblAction = new System.Windows.Forms.Label();
            this.uiBtnStepInto = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnStepOver = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnCancel = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnPause = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.pbBotIcon = new System.Windows.Forms.PictureBox();
            this.uiBtnScheduleManagement = new OpenBots.Core.UI.Controls.UIPictureButton();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnStepInto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnStepOver)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnPause)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBotIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnScheduleManagement)).BeginInit();
            this.SuspendLayout();
            // 
            // lstSteppingCommands
            // 
            this.lstSteppingCommands.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSteppingCommands.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstSteppingCommands.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstSteppingCommands.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lstSteppingCommands.FormattingEnabled = true;
            this.lstSteppingCommands.ItemHeight = 21;
            this.lstSteppingCommands.Location = new System.Drawing.Point(10, 57);
            this.lstSteppingCommands.Name = "lstSteppingCommands";
            this.lstSteppingCommands.Size = new System.Drawing.Size(678, 109);
            this.lstSteppingCommands.TabIndex = 0;
            this.lstSteppingCommands.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstSteppingCommands_DrawItem);
            this.lstSteppingCommands.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstSteppingCommands_MouseDoubleClick);
            // 
            // tmrNotify
            // 
            this.tmrNotify.Interval = 5000;
            this.tmrNotify.Tick += new System.EventHandler(this.autoCloseTimer_Tick);
            // 
            // lblCloseTimer
            // 
            this.lblCloseTimer.AutoSize = true;
            this.lblCloseTimer.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCloseTimer.ForeColor = System.Drawing.Color.White;
            this.lblCloseTimer.Location = new System.Drawing.Point(12, 176);
            this.lblCloseTimer.Name = "lblCloseTimer";
            this.lblCloseTimer.Size = new System.Drawing.Size(0, 17);
            this.lblCloseTimer.TabIndex = 3;
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblMainLogo.Font = new System.Drawing.Font("Segoe UI Semilight", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblMainLogo.Location = new System.Drawing.Point(1, 5);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(301, 45);
            this.lblMainLogo.TabIndex = 4;
            this.lblMainLogo.Text = "NexBots is executing";
            // 
            // lblKillProcNote
            // 
            this.lblKillProcNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblKillProcNote.AutoSize = true;
            this.lblKillProcNote.BackColor = System.Drawing.Color.Transparent;
            this.lblKillProcNote.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKillProcNote.ForeColor = System.Drawing.Color.White;
            this.lblKillProcNote.Location = new System.Drawing.Point(4, 184);
            this.lblKillProcNote.Name = "lblKillProcNote";
            this.lblKillProcNote.Size = new System.Drawing.Size(333, 20);
            this.lblKillProcNote.TabIndex = 17;
            this.lblKillProcNote.Text = "Press the \'Pause/Break\' key to stop automation.";
            // 
            // lblAction
            // 
            this.lblAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAction.AutoSize = true;
            this.lblAction.BackColor = System.Drawing.Color.Transparent;
            this.lblAction.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAction.ForeColor = System.Drawing.Color.White;
            this.lblAction.Location = new System.Drawing.Point(5, 209);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new System.Drawing.Size(65, 20);
            this.lblAction.TabIndex = 19;
            this.lblAction.Text = "Action...";
            // 
            // uiBtnStepInto
            // 
            this.uiBtnStepInto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiBtnStepInto.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnStepInto.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnStepInto.DisplayText = "Step Into";
            this.uiBtnStepInto.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnStepInto.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnStepInto.Image = global::OpenBots.Properties.Resources.engine_step_into;
            this.uiBtnStepInto.IsMouseOver = false;
            this.uiBtnStepInto.Location = new System.Drawing.Point(542, 2);
            this.uiBtnStepInto.Name = "uiBtnStepInto";
            this.uiBtnStepInto.Size = new System.Drawing.Size(48, 52);
            this.uiBtnStepInto.TabIndex = 23;
            this.uiBtnStepInto.TabStop = false;
            this.uiBtnStepInto.Text = "Step Into";
            this.uiBtnStepInto.Visible = false;
            this.uiBtnStepInto.Click += new System.EventHandler(this.uiBtnStepInto_Click);
            // 
            // uiBtnStepOver
            // 
            this.uiBtnStepOver.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiBtnStepOver.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnStepOver.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnStepOver.DisplayText = "Step Over";
            this.uiBtnStepOver.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnStepOver.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnStepOver.Image = global::OpenBots.Properties.Resources.engine_step_over;
            this.uiBtnStepOver.IsMouseOver = false;
            this.uiBtnStepOver.Location = new System.Drawing.Point(494, 2);
            this.uiBtnStepOver.Name = "uiBtnStepOver";
            this.uiBtnStepOver.Size = new System.Drawing.Size(48, 52);
            this.uiBtnStepOver.TabIndex = 22;
            this.uiBtnStepOver.TabStop = false;
            this.uiBtnStepOver.Text = "Step Over";
            this.uiBtnStepOver.Visible = false;
            this.uiBtnStepOver.Click += new System.EventHandler(this.uiBtnStepOver_Click);
            // 
            // uiBtnCancel
            // 
            this.uiBtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiBtnCancel.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnCancel.DisplayText = "Cancel";
            this.uiBtnCancel.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnCancel.Image = global::OpenBots.Properties.Resources.engine_cancel;
            this.uiBtnCancel.IsMouseOver = false;
            this.uiBtnCancel.Location = new System.Drawing.Point(638, 2);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(48, 52);
            this.uiBtnCancel.TabIndex = 14;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Text = "Cancel";
            this.uiBtnCancel.Click += new System.EventHandler(this.uiBtnCancel_Click);
            // 
            // uiBtnPause
            // 
            this.uiBtnPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiBtnPause.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnPause.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnPause.DisplayText = "Pause";
            this.uiBtnPause.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnPause.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnPause.Image = global::OpenBots.Properties.Resources.engine_pause;
            this.uiBtnPause.IsMouseOver = false;
            this.uiBtnPause.Location = new System.Drawing.Point(590, 2);
            this.uiBtnPause.Name = "uiBtnPause";
            this.uiBtnPause.Size = new System.Drawing.Size(48, 52);
            this.uiBtnPause.TabIndex = 15;
            this.uiBtnPause.TabStop = false;
            this.uiBtnPause.Text = "Pause";
            this.uiBtnPause.Click += new System.EventHandler(this.uiBtnPause_Click);
            // 
            // pbBotIcon
            // 
            this.pbBotIcon.BackColor = System.Drawing.Color.Transparent;
            this.pbBotIcon.Image = global::OpenBots.Properties.Resources.executing;
            this.pbBotIcon.Location = new System.Drawing.Point(12, 56);
            this.pbBotIcon.Name = "pbBotIcon";
            this.pbBotIcon.Size = new System.Drawing.Size(672, 125);
            this.pbBotIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbBotIcon.TabIndex = 18;
            this.pbBotIcon.TabStop = false;
            this.pbBotIcon.Click += new System.EventHandler(this.pbBotIcon_Click);
            // 
            // uiBtnScheduleManagement
            // 
            this.uiBtnScheduleManagement.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnScheduleManagement.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnScheduleManagement.DisplayText = "Schedule";
            this.uiBtnScheduleManagement.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnScheduleManagement.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnScheduleManagement.Image = global::OpenBots.Properties.Resources.action_bar_schedule;
            this.uiBtnScheduleManagement.IsMouseOver = false;
            this.uiBtnScheduleManagement.Location = new System.Drawing.Point(638, 184);
            this.uiBtnScheduleManagement.Name = "uiBtnScheduleManagement";
            this.uiBtnScheduleManagement.Size = new System.Drawing.Size(49, 50);
            this.uiBtnScheduleManagement.TabIndex = 24;
            this.uiBtnScheduleManagement.TabStop = false;
            this.uiBtnScheduleManagement.Text = "Schedule";
            this.uiBtnScheduleManagement.Visible = false;
            this.uiBtnScheduleManagement.Click += new System.EventHandler(this.uiBtnScheduleManagement_Click);
            // 
            // frmScriptEngine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.ClientSize = new System.Drawing.Size(697, 237);
            this.Controls.Add(this.uiBtnScheduleManagement);
            this.Controls.Add(this.uiBtnStepInto);
            this.Controls.Add(this.uiBtnStepOver);
            this.Controls.Add(this.lblAction);
            this.Controls.Add(this.lblKillProcNote);
            this.Controls.Add(this.uiBtnCancel);
            this.Controls.Add(this.uiBtnPause);
            this.Controls.Add(this.lblMainLogo);
            this.Controls.Add(this.lblCloseTimer);
            this.Controls.Add(this.lstSteppingCommands);
            this.Controls.Add(this.pbBotIcon);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmScriptEngine";
            this.Text = "NexBots Engine";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmScriptEngine_FormClosing);
            this.Load += new System.EventHandler(this.frmProcessingStatus_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnStepInto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnStepOver)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnPause)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBotIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnScheduleManagement)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox lstSteppingCommands;
        private System.Windows.Forms.Timer tmrNotify;
        private System.Windows.Forms.Label lblCloseTimer;
        private System.Windows.Forms.Label lblMainLogo;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnCancel;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnPause;
        private System.Windows.Forms.Label lblKillProcNote;
        private System.Windows.Forms.PictureBox pbBotIcon;
        private System.Windows.Forms.Label lblAction;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnStepInto;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnStepOver;
        private Core.UI.Controls.UIPictureButton uiBtnScheduleManagement;
    }
}