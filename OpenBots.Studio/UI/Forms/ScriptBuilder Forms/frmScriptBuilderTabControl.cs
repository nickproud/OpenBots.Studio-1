using OpenBots.Core.Enums;
using OpenBots.Core.Script;
using OpenBots.Properties;
using OpenBots.UI.CustomControls.CustomUIControls;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.ScriptBuilder_Forms
{
    public partial class frmScriptBuilder : Form
    {
        #region Script Tab Control
        private void uiScriptTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (uiScriptTabControl.TabCount > 0)
            {
                ScriptFilePath = uiScriptTabControl.SelectedTab.ToolTipText.ToString();
                string fileExtension = Path.GetExtension(ScriptFilePath).ToLower();

                switch (fileExtension)
                {
                    case ".obscript":
                        if (!_isScriptRunning)
                            splitContainerScript.Panel2Collapsed = false;

                        _selectedTabScriptActions = (UIListView)uiScriptTabControl.SelectedTab.Controls[0];
                        ScriptObject scriptObject = (ScriptObject)uiScriptTabControl.SelectedTab.Tag;
                        if (scriptObject != null)
                        {
                            _scriptVariables = scriptObject.ScriptVariables;
                            _scriptArguments = scriptObject.ScriptArguments;
                            _scriptElements = scriptObject.ScriptElements;
                            _importedNamespaces = scriptObject.ImportedNamespaces;

                            if (!_isRunTaskCommand)
                            {
                                ResetVariableArgumentBindings();
                            }
                        }
                        break;
                    case ".py":
                    case ".tag":
                    case ".cs":
                        _selectedTabScriptActions = (Scintilla)uiScriptTabControl.SelectedTab.Controls[0];
                        splitContainerScript.Panel2Collapsed = true;
                        break;
                }
                          
            }
        }

        //TODO Finish close button rendering
        private void uiScriptTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabControl = (TabControl)sender;
            Point imageLocation = new Point(15, 5);

            try
            {
                Image closeImage = Resources.close_button;
                Rectangle tabRect = tabControl.GetTabRect(e.Index);
                tabRect.Offset(2, 2);
                string title = tabControl.TabPages[e.Index].Text + "  ";
                Font font = uiPaneTabs.Font;
                Brush titleBrush = new SolidBrush(Color.Black);
                e.Graphics.DrawString(title, font, titleBrush, new PointF(tabRect.X, tabRect.Y));
                if (tabControl.SelectedIndex >= 1)
                    e.Graphics.DrawImage(closeImage, new Point(tabRect.X + (tabRect.Width - imageLocation.X), imageLocation.Y));
            }
            catch (Exception ex)
            {
                Notify("An Error Occurred: " + ex.Message, Color.Red);
            }
        }

        private void uiScriptTabControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _lastClickPosition = Cursor.Position;
                cmsScriptTabActions.Show(Cursor.Position);
            }

        //TODO Finish close button click event
        //    else
        //    {
        //        TabControl tabControl = (TabControl)sender;
        //        Point imageHitArea = new Point(13, 2);
        //        Point point = e.Location;
        //        Rectangle tabRect = tabControl.GetTabRect(tabControl.SelectedIndex);
        //        int tabWidth = tabRect.Width - imageHitArea.X;
        //        tabRect.Offset(tabWidth, imageHitArea.Y);
        //        tabRect.Width = 16;
        //        tabRect.Height = 16;
        //        if (tabControl.SelectedIndex >= 1 && tabRect.Contains(point))
        //            tabControl.TabPages.Remove(tabControl.TabPages[tabControl.SelectedIndex]);
        //    }
        }

        private void uiScriptTabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (IsScriptRunning)
                e.Cancel = true;
        }

        private void tsmiCloseTab_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < uiScriptTabControl.TabCount; i++)
            {
                Rectangle tabRect = uiScriptTabControl.GetTabRect(i);
                if (tabRect.Contains(uiScriptTabControl.PointToClient(_lastClickPosition))
                    && uiScriptTabControl.TabCount > 1)
                {

                    TabPage tab = uiScriptTabControl.TabPages[i];
                    DialogResult result = CheckForUnsavedScript(tab);
                    if (result == DialogResult.Cancel)
                        return;
                    uiScriptTabControl.TabPages.RemoveAt(i);
                }
            }
        }

        private void tsmiReloadTab_Click(object sender, EventArgs e)
        {
            string tabFilePath = "";
            for (int i = 0; i < uiScriptTabControl.TabCount; i++)
            {
                Rectangle tabRect = uiScriptTabControl.GetTabRect(i);
                if (tabRect.Contains(uiScriptTabControl.PointToClient(_lastClickPosition)))
                {

                    TabPage tab = uiScriptTabControl.TabPages[i];
                    tabFilePath = tab.ToolTipText;
                    DialogResult result = CheckForUnsavedScript(tab);
                    if (result == DialogResult.Cancel)
                        return;
                    uiScriptTabControl.TabPages.RemoveAt(i);
                }
            }

            OpenOpenBotsFile(tabFilePath);
        }

        private void reloadAllTabsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReloadAllFiles();
        }

        private void ReloadAllFiles()
        {
            int currentTabIndex = uiScriptTabControl.SelectedIndex;
            List<string> tabFilePaths = new List<string>();

            foreach (TabPage openTab in uiScriptTabControl.TabPages)
            {
                uiScriptTabControl.SelectedTab = openTab;
               
                DialogResult result = CheckForUnsavedScript(openTab);
                if (result == DialogResult.Cancel)
                    return;

                tabFilePaths.Add(openTab.ToolTipText);
                uiScriptTabControl.TabPages.Remove(openTab);
            }

            foreach (string path in tabFilePaths)
            {
                switch (Path.GetExtension(path).ToLower())
                {
                    case ".obscript":
                        OpenOpenBotsFile(path);
                        break;
                    case ".py":
                        OpenTextEditorFile(path, ProjectType.Python);
                        break;
                    case ".tag":
                        OpenTextEditorFile(path, ProjectType.TagUI);
                        break;
                    case ".cs":
                        OpenTextEditorFile(path, ProjectType.CSScript);
                        break;
                }
            }               

            uiScriptTabControl.SelectedIndex = currentTabIndex;
        }

        private bool CloseAllFiles()
        {
            bool allTabsClosed = false;

            foreach (TabPage openTab in uiScriptTabControl.TabPages)
            {
                uiScriptTabControl.SelectedTab = openTab;

                DialogResult result = CheckForUnsavedScript(openTab);
                if (result == DialogResult.Cancel)
                    return allTabsClosed;

                uiScriptTabControl.TabPages.Remove(openTab);
            }

            allTabsClosed = true;
            return allTabsClosed;
        }

        private void tsmiCloseAllButThis_Click(object sender, EventArgs e)
        {
            //iterate through each tab, and check if it's the selected tab
            //if it is, store and continue
            //if it isn't, check for unsaved changes
            TabPage keepTab = new TabPage();
            for (int i = 0; i < uiScriptTabControl.TabCount; i++)
            {
                Rectangle tabRect = uiScriptTabControl.GetTabRect(i);
                if (!tabRect.Contains(uiScriptTabControl.PointToClient(_lastClickPosition)))
                {
                    TabPage tab = uiScriptTabControl.TabPages[i];
                    DialogResult result = CheckForUnsavedScript(tab);
                    if (result == DialogResult.Cancel)
                        return;
                }
                else if (tabRect.Contains(uiScriptTabControl.PointToClient(_lastClickPosition)))
                    keepTab = uiScriptTabControl.TabPages[i];
            }
            foreach (TabPage tab in uiScriptTabControl.TabPages)
            {
                if (tab.ToolTipText != keepTab.ToolTipText)
                    uiScriptTabControl.TabPages.Remove(tab);
            }
        }

        private void UpdateTabPage(TabPage tab, string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            tab.Text = fileName;
            tab.Name = fileName;
            tab.ToolTipText = filePath;
            tab.Controls[0].Name = fileName;
        }

        private DialogResult CheckForUnsavedScripts()
        {
            DialogResult result = new DialogResult();
            if (uiScriptTabControl.TabCount > 0)
            {
                foreach (TabPage tab in uiScriptTabControl.TabPages)
                {
                    result = CheckForUnsavedScript(tab);
                }
            }
            return result;
        }

        private DialogResult CheckForUnsavedScript(TabPage tab)
        {
            DialogResult result = new DialogResult();
            if (tab.Text.EndsWith(" *"))
            {
                string tabName = tab.Text.Replace(" *", "");
                result = MessageBox.Show($"Would you like to save '{tabName}' before closing this tab?",
                                         $"Save '{tabName}'", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Yes)
                {
                    ClearSelectedListViewItems();
                    uiScriptTabControl.SelectedTab = tab;

                    if (_selectedTabScriptActions is ListView)
                    {
                        if (!SaveToOpenBotsFile(false))
                        {
                            result = DialogResult.Cancel;
                            return result;
                        }
                    }
                    else
                    {
                        if (!SaveToTextEditorFile(false))
                        {
                            result = DialogResult.Cancel;
                            return result;
                        }
                    }                  
                }
                else if (result == DialogResult.Cancel)
                    return result;
            }
            return result;
        }
        #endregion
    }
}
