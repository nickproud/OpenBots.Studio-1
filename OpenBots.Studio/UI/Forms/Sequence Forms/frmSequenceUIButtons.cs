using OpenBots.Core.Command;
using OpenBots.Core.Script;
using OpenBots.Core.Settings;
using OpenBots.UI.Forms.Supplement_Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.Sequence_Forms
{
    public partial class frmSequence : Form
    {
        #region UI Buttons
        #region File Actions Tool Strip and Buttons
        private void ClearSelectedListViewItems()
        {
            SelectedTabScriptActions.SelectedItems.Clear();
            SelectedTabScriptActions.Invalidate();
        }

        #region Restart And Close Buttons
        private void uiBtnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
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
            frmScriptVariables scriptVariableEditor = new frmScriptVariables(TypeContext)
            {
                ScriptName = uiScriptTabControl.SelectedTab.Name
            };

            scriptVariableEditor.ScriptContext = ScriptContext;

            if (scriptVariableEditor.ShowDialog() == DialogResult.OK)
            {
                if (!uiScriptTabControl.SelectedTab.Text.Contains(" *"))
                    uiScriptTabControl.SelectedTab.Text += " *";
            }

            ResetVariableArgumentBindings();
            scriptVariableEditor.Dispose();
        }

        private void uiBtnAddArgument_Click(object sender, EventArgs e)
        {
            OpenArgumentManager();
        }

        private void OpenArgumentManager()
        {
            frmScriptArguments scriptArgumentEditor = new frmScriptArguments(TypeContext)
            {
                ScriptName = uiScriptTabControl.SelectedTab.Name,
                ScriptContext = ScriptContext
            };

            if (scriptArgumentEditor.ShowDialog() == DialogResult.OK)
            {
                if (!uiScriptTabControl.SelectedTab.Text.Contains(" *"))
                    uiScriptTabControl.SelectedTab.Text += " *";
            }

            ResetVariableArgumentBindings();
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
                ScriptContext = ScriptContext
            };

            if (scriptElementEditor.ShowDialog() == DialogResult.OK)
            {
                CreateUndoSnapshot();
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
            frmSettings newSettings = new frmSettings(AContainer);
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

        #region Recorder Buttons
        private void uiBtnSaveSequence_Click(object sender, EventArgs e)
        {
            dgvVariables.EndEdit();
            dgvArguments.EndEdit();

            if (SelectedTabScriptActions.Items.Count == 0)
            {
                Notify("You must have at least 1 automation command to save.", Color.Yellow);
                return;
            }

            int beginLoopValidationCount = 0;
            int beginIfValidationCount = 0;
            int tryCatchValidationCount = 0;
            int retryValidationCount = 0;
            int beginSwitchValidationCount = 0;

            foreach (ListViewItem item in SelectedTabScriptActions.Items)
            {
                if (item.Tag is BrokenCodeCommentCommand)
                {
                    Notify("Please verify that all broken code has been removed or replaced.", Color.Yellow);
                    return;
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
                else if (item.Tag.GetType().Name == "BeginSwitchCommand")
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
                    return;
                }

                //end if was found first
                if (beginIfValidationCount < 0)
                {
                    Notify("Please verify the ordering of your ifs.", Color.Yellow);
                    return;
                }

                if (tryCatchValidationCount < 0)
                {
                    Notify("Please verify the ordering of your try/catch blocks.", Color.Yellow);
                    return;
                }

                if (retryValidationCount < 0)
                {
                    Notify("Please verify the ordering of your retry blocks.", Color.Yellow);
                    return;
                }

                if (beginSwitchValidationCount < 0)
                {
                    Notify("Please verify the ordering of your switch/case blocks.", Color.Yellow);
                    return;
                }
            }

            //extras were found
            if (beginLoopValidationCount != 0)
            {
                Notify("Please verify the ordering of your loops.", Color.Yellow);
                return;
            }

            //extras were found
            if (beginIfValidationCount != 0)
            {
                Notify("Please verify the ordering of your ifs.", Color.Yellow);
                return;
            }

            if (tryCatchValidationCount != 0)
            {
                Notify("Please verify the ordering of your try/catch blocks.", Color.Yellow);
                return;
            }

            if (retryValidationCount != 0)
            {
                Notify("Please verify the ordering of your retry blocks.", Color.Yellow);
                return;
            }

            if (beginSwitchValidationCount != 0)
            {
                Notify("Please verify the ordering of your switch/case blocks.", Color.Yellow);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void uiBtnRenameSequence_Click(object sender, EventArgs e)
        {
            frmInputBox renameSequence = new frmInputBox("New Sequence Name", "Rename Sequence");
            renameSequence.txtInput.Text = Text;
            renameSequence.ShowDialog();

            if (renameSequence.DialogResult == DialogResult.OK)
            {
                Text = renameSequence.txtInput.Text;
                if (!uiScriptTabControl.SelectedTab.Text.Contains(" *"))
                    uiScriptTabControl.SelectedTab.Text += " *";
            }

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
