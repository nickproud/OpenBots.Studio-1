using Autofac;
using Newtonsoft.Json;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Nuget;
using OpenBots.Core.IO;
using OpenBots.Core.Script;
using OpenBots.Core.Settings;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Studio.Utilities;
using OpenBots.UI.CustomControls.CustomUIControls;
using OpenBots.UI.Forms.Supplement_Forms;
using OpenBots.UI.Supplement_Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OpenBots.Core.Model.EngineModel;
using System.ComponentModel;
using OpenBots.Core.Server.User;

namespace OpenBots.UI.Forms.Sequence_Forms
{
    public partial class frmSequence : Form
    {
        #region UI Buttons
        #region File Actions Tool Strip and Buttons
        //Helper method for RunTaskCommand
        private bool SaveToFile(bool saveAs)
        {
            bool isSuccessfulSave = false;
            if (SelectedTabScriptActions.Items.Count == 0)
            {
                Notify("You must have at least 1 automation command to save.", Color.Yellow);
                return isSuccessfulSave;
            }

            int beginLoopValidationCount = 0;
            int beginIfValidationCount = 0;
            int tryCatchValidationCount = 0;
            int retryValidationCount = 0;
            int beginSwitchValidationCount = 0;

            foreach (ListViewItem item in SelectedTabScriptActions.Items)
            {
                if(item.Tag is BrokenCodeCommentCommand)
                {
                    Notify("Please verify that all broken code has been removed or replaced.", Color.Yellow);
                    return isSuccessfulSave;
                }
                else if ((item.Tag.GetType().Name == "LoopCollectionCommand") || (item.Tag.GetType().Name == "LoopContinuouslyCommand") ||
                    (item.Tag.GetType().Name == "LoopNumberOfTimesCommand") || (item.Tag.GetType().Name == "BeginLoopCommand") ||
                    (item.Tag.GetType().Name == "BeginMultiLoopCommand"))
                {
                    beginLoopValidationCount++;
                }
                else if (item.Tag.GetType().Name == "EndLoopCommand")
                {
                    beginLoopValidationCount--;
                }
                else if ((item.Tag.GetType().Name == "BeginIfCommand") || (item.Tag.GetType().Name == "BeginMultiIfCommand"))
                {
                    beginIfValidationCount++;
                }
                else if (item.Tag.GetType().Name == "EndIfCommand")
                {
                    beginIfValidationCount--;
                }
                else if (item.Tag.GetType().Name == "BeginTryCommand")
                {
                    tryCatchValidationCount++;
                }
                else if (item.Tag.GetType().Name == "EndTryCommand")
                {
                    tryCatchValidationCount--;
                }
                else if (item.Tag.GetType().Name == "BeginRetryCommand")
                {
                    retryValidationCount++;
                }
                else if (item.Tag.GetType().Name == "EndRetryCommand")
                {
                    retryValidationCount--;
                }
                else if(item.Tag.GetType().Name == "BeginSwitchCommand")
                {
                    beginSwitchValidationCount++;
                }
                else if (item.Tag.GetType().Name == "EndSwitchCommand")
                {
                    beginSwitchValidationCount--;
                }

                //end loop was found first
                if (beginLoopValidationCount < 0)
                {
                    Notify("Please verify the ordering of your loops.", Color.Yellow);
                    return isSuccessfulSave;
                }

                //end if was found first
                if (beginIfValidationCount < 0)
                {
                    Notify("Please verify the ordering of your ifs.", Color.Yellow);
                    return isSuccessfulSave;
                }

                if (tryCatchValidationCount < 0)
                {
                    Notify("Please verify the ordering of your try/catch blocks.", Color.Yellow);
                    return isSuccessfulSave;
                }

                if (retryValidationCount < 0)
                {
                    Notify("Please verify the ordering of your retry blocks.", Color.Yellow);
                    return isSuccessfulSave;
                }

                if (beginSwitchValidationCount < 0)
                {
                    Notify("Please verify the ordering of your switch/case blocks.", Color.Yellow);
                    return isSuccessfulSave;
                }
            }

            //extras were found
            if (beginLoopValidationCount != 0)
            {
                Notify("Please verify the ordering of your loops.", Color.Yellow);
                return isSuccessfulSave;
            }

            //extras were found
            if (beginIfValidationCount != 0)
            {
                Notify("Please verify the ordering of your ifs.", Color.Yellow);
                return isSuccessfulSave;
            }

            if (tryCatchValidationCount != 0)
            {
                Notify("Please verify the ordering of your try/catch blocks.", Color.Yellow);
                return isSuccessfulSave;
            }

            if (retryValidationCount != 0)
            {
                Notify("Please verify the ordering of your retry blocks.", Color.Yellow);
                return isSuccessfulSave;
            }

            if (beginSwitchValidationCount != 0)
            {
                Notify("Please verify the ordering of your switch/case blocks.", Color.Yellow);
                return isSuccessfulSave;
            }
            isSuccessfulSave = true;
            return isSuccessfulSave;
        }

        private void ClearSelectedListViewItems()
        {
            SelectedTabScriptActions.SelectedItems.Clear();
            _selectedIndex = -1;
            SelectedTabScriptActions.Invalidate();
        }

        public void PopulateExecutionCommands(List<ScriptAction> commandDetails)
        {

            foreach (ScriptAction item in commandDetails)
            {
                if (item.ScriptCommand != null)
                    SelectedTabScriptActions.Items.Add(CreateScriptCommandListViewItem(item.ScriptCommand));
                else
                {
                    var brokenCodeCommentCommand = new BrokenCodeCommentCommand();
                    brokenCodeCommentCommand.v_Comment = item.SerializationError;
                    SelectedTabScriptActions.Items.Add(CreateScriptCommandListViewItem(brokenCodeCommentCommand));
                }
                if (item.AdditionalScriptCommands?.Count > 0)
                    PopulateExecutionCommands(item.AdditionalScriptCommands);
            }

            if (pnlCommandHelper.Visible)
            {
                uiScriptTabControl.SelectedTab.Controls.Remove(pnlCommandHelper);
                uiScriptTabControl.SelectedTab.Controls[0].Show();
            }
            else if (!uiScriptTabControl.SelectedTab.Controls[0].Visible)
                uiScriptTabControl.SelectedTab.Controls[0].Show();
        }

        #region Restart And Close Buttons
        private void uiBtnClose_Click(object sender, EventArgs e)
        {
            if (_isSequence)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }

            Application.Exit();
        }
        #endregion
        #endregion

        #region Options Tool Strip and Buttons
        private void uiBtnAddVariable_Click(object sender, EventArgs e)
        {
            OpenVariableManager();
        }

        private void OpenVariableManager()
        {
            frmScriptVariables scriptVariableEditor = new frmScriptVariables
            {
                ScriptName = uiScriptTabControl.SelectedTab.Name,
                ScriptVariables = ScriptVariables,
                ScriptArguments = SriptArguments
            };

            if (scriptVariableEditor.ShowDialog() == DialogResult.OK)
            {
                ScriptVariables = scriptVariableEditor.ScriptVariables;
                if (!uiScriptTabControl.SelectedTab.Text.Contains(" *"))
                    uiScriptTabControl.SelectedTab.Text += " *";

                dgvVariables.DataSource = new BindingList<ScriptVariable>(ScriptVariables);
            }

            scriptVariableEditor.Dispose();
        }

        private void uiBtnAddArgument_Click(object sender, EventArgs e)
        {
            OpenArgumentManager();
        }

        private void OpenArgumentManager()
        {
            frmScriptArguments scriptArgumentEditor = new frmScriptArguments
            {
                ScriptName = uiScriptTabControl.SelectedTab.Name,
                ScriptArguments = SriptArguments,
                ScriptVariables = ScriptVariables
            };

            if (scriptArgumentEditor.ShowDialog() == DialogResult.OK)
            {
                SriptArguments = scriptArgumentEditor.ScriptArguments;
                if (!uiScriptTabControl.SelectedTab.Text.Contains(" *"))
                    uiScriptTabControl.SelectedTab.Text += " *";

                dgvArguments.DataSource = new BindingList<ScriptArgument>(SriptArguments);
            }

            scriptArgumentEditor.Dispose();
        }

        private void uiBtnAddElement_Click(object sender, EventArgs e)
        {
            OpenElementManager();
        }

        private void OpenElementManager()
        {
            frmScriptElements scriptElementEditor = new frmScriptElements
            {
                ScriptName = uiScriptTabControl.SelectedTab.Name,
                ScriptElements = ScriptElements
            };

            if (scriptElementEditor.ShowDialog() == DialogResult.OK)
            {
                CreateUndoSnapshot();
                ScriptElements = scriptElementEditor.ScriptElements;
            }

            scriptElementEditor.Dispose();
        }

        private void uiBtnSettings_Click(object sender, EventArgs e)
        {
            OpenSettingsManager();
        }

        private void OpenSettingsManager()
        {
            //show settings dialog
            frmSettings newSettings = new frmSettings();
            newSettings.ShowDialog();

            //reload app settings
            _appSettings = new ApplicationSettings().GetOrCreateApplicationSettings();

            newSettings.Dispose();
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uiBtnClearAll_Click(sender, e);
        }

        private void uiBtnClearAll_Click(object sender, EventArgs e)
        {
            CreateUndoSnapshot();
            SelectedTabScriptActions.Items.Clear();
        }

        private void aboutOpenBotsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout frmAboutForm = new frmAbout();
            frmAboutForm.Show();
        }
        #endregion

        #region Script Events Tool Strip and Buttons        
        private void breakpointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRemoveBreakpoint();
        }

        private void uiBtnBreakpoint_Click(object sender, EventArgs e)
        {
            AddRemoveBreakpoint();
        }
        #endregion

        #region Recorder Buttons
        private void uiBtnSaveSequence_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void uiBtnRenameSequence_Click(object sender, EventArgs e)
        {
            frmInputBox renameSequence = new frmInputBox("New Sequence Name", "Rename Sequence");
            renameSequence.txtInput.Text = Text;
            renameSequence.ShowDialog();

            if (renameSequence.DialogResult == DialogResult.OK)
                Text = renameSequence.txtInput.Text;

            renameSequence.Dispose();
        }

        private void shortcutMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShortcutMenu shortcutMenuForm = new frmShortcutMenu();
            shortcutMenuForm.Show();
        }

        private void openShortcutMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            shortcutMenuToolStripMenuItem_Click(sender, e);
        }
        #endregion
        #endregion
    }
}
