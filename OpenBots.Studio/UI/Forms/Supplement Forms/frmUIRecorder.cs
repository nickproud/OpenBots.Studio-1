﻿using Autofac;
using OpenBots.Core.Command;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Settings;
using OpenBots.Core.UI.Forms;
using OpenBots.Core.User32;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Properties;
using OpenBots.UI.Forms.ScriptBuilder_Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OpenBots.UI.Supplement_Forms
{
    public partial class frmScreenRecorder : UIForm
    {
        public frmScriptBuilder CallBackForm { get; set; }
        private List<ScriptCommand> _scriptCommandList;
        private ApplicationSettings _appSettings;
        private IContainer _container;

        public frmScreenRecorder(IContainer container)
        {
            _appSettings = new ApplicationSettings();
            _appSettings = _appSettings.GetOrCreateApplicationSettings();
            _container = container;

            InitializeComponent();
        }

        private void frmSequenceRecorder_Load(object sender, EventArgs e)
        {
            cbxHookStop.DataSource = Enum.GetValues(typeof(Keys));
            cbxHookStop.SelectedIndex = cbxHookStop.Items.IndexOf(Keys.F2);
        }        

        private void chkGroupIntoSequences_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkCaptureMouse_CheckedChanged(object sender, EventArgs e)
        {
            chkGroupMovesIntoSequences.Checked = chkCaptureMouse.Checked;
            chkGroupMovesIntoSequences.Enabled = chkGroupMovesIntoSequences.Checked;
        }       

        private void chkActivateTopLeft_CheckedChanged(object sender, EventArgs e)
        {
            if (chkActivateTopLeft.Checked)
                chkTrackWindowsOpenLocation.Checked = false;
        }

        private void chkTrackWindowsOpenLocation_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTrackWindowsOpenLocation.Checked)
                chkActivateTopLeft.Checked = false;
        }

        private void uiBtnRecord_Click(object sender, EventArgs e)
        {
            if (uiBtnRecord.DisplayText == "Start")
            {
                Height = 120;
                Width = 500;
                BringToFront();
                MoveFormToBottomRight(this);
                TopMost = true;
                pnlOptions.Hide();
                lblRecording.Show();
                FormBorderStyle = 0;
                uiBtnRecord.DisplayText = "Stop";
                uiBtnRecord.Image = Resources.various_stop_button;
                uiBtnRecord.Location = new Point(lblRecording.Right + 5, lblRecording.Location.Y);

                int.TryParse(txtHookResolution.Text, out int samplingResolution);

                GlobalHook.HookStopped += new EventHandler(OnHookStopped);
                GlobalHook.StartScreenRecordingHook(chkCaptureClicks.Checked, chkCaptureMouse.Checked,
                    chkGroupMovesIntoSequences.Checked, chkCaptureKeyboard.Checked, chkCaptureWindowEvents.Checked,
                    chkActivateTopLeft.Checked, chkTrackWindowSize.Checked, chkTrackWindowsOpenLocation.Checked,
                    samplingResolution, (Keys)cbxHookStop.SelectedValue, _container);
                lblRecording.Text = "Press '" + cbxHookStop.Text + "' key to stop recording!";
                // WindowHook.StartHook();

                _scriptCommandList = new List<ScriptCommand>();
            }
            else
                GlobalHook.StopHook();
        }

        private void OnHookStopped(object sender, EventArgs e)
        {
            //isRecording = false;
            GlobalHook.HookStopped -= new EventHandler(OnHookStopped);

            //if (!isRecording)
            //{
            //    return;
            //}

            pnlOptions.Show();
            lblRecording.Hide();
            FinalizeRecording();
        }

        private void FinalizeRecording()
        {
            string sequenceComment = $"UI Sequence Recorded {DateTime.Now}";

            _scriptCommandList = GlobalHook.GeneratedCommands;
            var outputList = new List<ScriptCommand>();

            if (chkGroupIntoSequence.Checked)
            {
                dynamic newSequence = TypeMethods.CreateTypeInstance(_container, "SequenceCommand");
                newSequence.v_Comment = sequenceComment;

                foreach (ScriptCommand cmd in _scriptCommandList)
                    newSequence.ScriptActions.Add(cmd);

                if (newSequence.ScriptActions.Count > 0)
                    outputList.Add(newSequence);
            }
            else if (chkGroupMovesIntoSequences.Checked)
            {
                dynamic newSequence = TypeMethods.CreateTypeInstance(_container, "SequenceCommand");
                newSequence.v_Comment = sequenceComment;

                foreach (ScriptCommand cmd in _scriptCommandList)
                {

                    if (cmd.CommandName == "SendMouseMoveCommand")
                    {
                        var sendMouseCmd = (ISendMouseMoveCommand)cmd;
                        if (sendMouseCmd.v_MouseClick != "None")
                        {
                            outputList.Add(newSequence);
                            newSequence = TypeMethods.CreateTypeInstance(_container, "SequenceCommand");
                            newSequence.v_Comment = sequenceComment;
                            outputList.Add(cmd);
                        }
                        else
                            newSequence.ScriptActions.Add(cmd);
                    }
                    else if (cmd.CommandName == "SendKeystrokesCommand")
                    {
                        outputList.Add(newSequence);
                        newSequence = TypeMethods.CreateTypeInstance(_container, "SequenceCommand");
                        newSequence.v_Comment = sequenceComment;
                        outputList.Add(cmd);
                    }
                    else
                        newSequence.ScriptActions.Add(cmd);
                }

                if (newSequence.ScriptActions.Count > 0)
                    outputList.Add(newSequence);
            }

            else
                outputList = _scriptCommandList;

            dynamic commentCommand = TypeMethods.CreateTypeInstance(_container, "AddCodeCommentCommand");
            commentCommand.v_Comment = sequenceComment;
            outputList.Insert(0, commentCommand);

            foreach (var cmd in outputList)
                CallBackForm.AddCommandToListView(cmd);

            Close();
        }
    }
}











