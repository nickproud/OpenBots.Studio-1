using Newtonsoft.Json;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Script;
using OpenBots.Core.UI.DTOs;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Properties;
using OpenBots.Studio.Utilities;
using OpenBots.UI.CustomControls.CustomUIControls;
using OpenBots.UI.Forms.Supplement_Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CoreResources = OpenBots.Core.Properties.Resources;

namespace OpenBots.UI.Forms.Sequence_Forms
{
    public partial class frmSequence : Form
    {
        #region ListView Events
        private UIListView NewLstScriptActions(string title = "newLstScriptActions")
        {
            UIListView newLstScriptActions = new UIListView();
            newLstScriptActions.AllowDrop = true;
            newLstScriptActions.BackColor = Color.WhiteSmoke;
            newLstScriptActions.BorderStyle = BorderStyle.None;
            newLstScriptActions.Columns.AddRange(new ColumnHeader[] {
            new ColumnHeader(),
            new ColumnHeader{ Width = 20 },
            new ColumnHeader{ Text = "Script Commands", Width = -2 } });
            newLstScriptActions.Dock = DockStyle.Fill;
            newLstScriptActions.Font = new Font("Segoe UI", 11, FontStyle.Bold, GraphicsUnit.Point, 0);
            newLstScriptActions.FullRowSelect = true;
            newLstScriptActions.HeaderStyle = ColumnHeaderStyle.None;
            newLstScriptActions.HideSelection = false;
            newLstScriptActions.Location = new Point(3, 3);
            newLstScriptActions.Margin = new Padding(6);
            newLstScriptActions.Name = title;
            newLstScriptActions.OwnerDraw = true;
            newLstScriptActions.Size = new Size(1063, 411);
            newLstScriptActions.TabIndex = 6;
            newLstScriptActions.UseCompatibleStateImageBehavior = false;
            newLstScriptActions.View = View.Details;
            newLstScriptActions.DrawSubItem += new DrawListViewSubItemEventHandler(lstScriptActions_DrawSubItem);
            newLstScriptActions.ItemDrag += new ItemDragEventHandler(lstScriptActions_ItemDrag);
            newLstScriptActions.DragDrop += new DragEventHandler(lstScriptActions_DragDrop);
            newLstScriptActions.DragEnter += new DragEventHandler(lstScriptActions_DragEnter);
            newLstScriptActions.DoubleClick += new EventHandler(lstScriptActions_DoubleClick);
            newLstScriptActions.KeyDown += new KeyEventHandler(lstScriptActions_KeyDown);
            newLstScriptActions.MouseClick += new MouseEventHandler(lstScriptActions_MouseClick);
            newLstScriptActions.MouseMove += new MouseEventHandler(lstScriptActions_MouseMove);
            newLstScriptActions.Tag = new ScriptActionTag();
            newLstScriptActions.ShowItemToolTips = true;

            return newLstScriptActions;
        }

        #region ListView DragDrop
        private void lstScriptActions_ItemDrag(object sender, ItemDragEventArgs e)
        {
            SelectedTabScriptActions.DoDragDrop(SelectedTabScriptActions.SelectedItems, DragDropEffects.Move);
        }

        private void lstScriptActions_DragEnter(object sender, DragEventArgs e)
        {
            int len = e.Data.GetFormats().Length - 1;
            int i;
            for (i = 0; i <= len; i++)
            {
                if (e.Data.GetFormats()[i].Equals("System.Windows.Forms.ListView+SelectedListViewItemCollection"))
                {
                    //The data from the drag source is moved to the target.
                    e.Effect = DragDropEffects.Move;
                }
                else if (e.Data.GetFormats()[i].Equals("System.Windows.Forms.TreeNode"))
                    e.Effect = DragDropEffects.Copy;
            }
        }

        private void lstScriptActions_DragDrop(object sender, DragEventArgs e)
        {
            //returns the location of the mouse pointer in the ListView control
            Point cp = SelectedTabScriptActions.PointToClient(new Point(e.X, e.Y));

            //obtain the item that is located at the specified location of the mouse pointer
            ListViewItem dragToItem = SelectedTabScriptActions.GetItemAt(cp.X, cp.Y);

            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                TreeNode commandNode = ((TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode"));

                if (commandNode.Nodes.Count != 0)
                    return;

                var commandName = commandNode.Text;
                var commandGroupName = commandNode.Parent.Text;

                var newCommandName = _automationCommands.Where(x => x.ShortName == commandName && x.DisplayGroup == commandGroupName)
                                                              .Select(x => x.Command).FirstOrDefault().GetType();

                dynamic newCommandInstance = TypeMethods.CreateTypeInstance(AContainer, newCommandName.Name);

                CreateUndoSnapshot();
                if (dragToItem != null)
                    AddCommandToListView(newCommandInstance, dragToItem.Index);
                else
                    AddCommandToListView(newCommandInstance, SelectedTabScriptActions.Items.Count);
            }
            else
            {
                //return if the items are not selected in the ListView control
                if (SelectedTabScriptActions.SelectedItems.Count == 0)
                {
                    return;
                }

                CreateUndoSnapshot();
              
                if (dragToItem == null)
                {
                    return;
                }

                List<ScriptCommand> commandsToMove = new List<ScriptCommand>();

                for (int i = 0; i <= SelectedTabScriptActions.SelectedItems.Count - 1; i++)
                {
                    var command = (ScriptCommand)SelectedTabScriptActions.SelectedItems[i].Tag;
                    commandsToMove.Add(command);
                }               

                //obtain the index of the item at the mouse pointer
                int dragIndex = dragToItem.Index;

                ListViewItem[] sel = new ListViewItem[SelectedTabScriptActions.SelectedItems.Count];
                for (int i = 0; i <= SelectedTabScriptActions.SelectedItems.Count - 1; i++)
                {
                    sel[i] = SelectedTabScriptActions.SelectedItems[i];
                }
                for (int i = 0; i < sel.GetLength(0); i++)
                {
                    //obtain the ListViewItem to be dragged to the target location
                    ListViewItem dragItem = sel[i];
                    int itemIndex = dragIndex;
                    if (itemIndex == dragItem.Index)
                    {
                        return;
                    }
                    if (dragItem.Index < itemIndex)
                        itemIndex++;
                    else
                        itemIndex = dragIndex + i;

                    //insert the item at the mouse pointer
                    ListViewItem insertItem = (ListViewItem)dragItem.Clone();
                    SelectedTabScriptActions.Items.Insert(itemIndex, insertItem);

                    //removes the item from the initial location while the item is moved to the new location
                    SelectedTabScriptActions.Items.Remove(dragItem);
                    SelectedTabScriptActions.Invalidate();
                }
            }
        }

        #endregion

        #region ListView Copy, Paste, Edit, Delete
        private void lstScriptActions_KeyDown(object sender, KeyEventArgs e)
        {
            //delete from listview if required
            if (e.KeyCode == Keys.Delete)
            {
                CreateUndoSnapshot();
                DeleteSelectedCode();          
            }
            else if (e.KeyCode == Keys.Enter)
            {
                //if user presses enter simulate double click event
                lstScriptActions_DoubleClick(null, null);
            }
            else if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.X:
                        CutRows();
                        break;
                    case Keys.C:
                        CopyRows();
                        break;
                    case Keys.V:
                        PasteRows();
                        break;
                    case Keys.Z:
                        UndoChange();
                        break;
                    case Keys.R:
                        RedoChange();
                        break;
                    case Keys.A:
                        foreach (ListViewItem item in SelectedTabScriptActions.Items)
                            item.Selected = true;
                        break;
                    case Keys.S:
                        uiBtnSaveSequence_Click(null, null);
                        break;
                    case Keys.E:
                        SetSelectedCodeToCommented(false);
                        break;
                    case Keys.D:
                        SetSelectedCodeToCommented(true);
                        break;
                    case Keys.B:
                        //AddRemoveBreakpoint();
                        break;
                    case Keys.J:
                        OpenArgumentManager();
                        break;
                    case Keys.K:
                        OpenVariableManager();
                        break;
                    case Keys.L:
                        OpenElementManager();
                        break;
                    case Keys.M:
                        shortcutMenuToolStripMenuItem_Click(null, null);
                        break;
                    case Keys.O:
                        aboutOpenBotsToolStripMenuItem_Click(null, null);
                        break;
                    case Keys.Back:
                        clearAllToolStripMenuItem_Click(null, null);
                        break;
                }                             
            }
        }

        private void lstScriptActions_DoubleClick(object sender, EventArgs e)
        {
            try 
            { 
                if (SelectedTabScriptActions.SelectedItems.Count != 1)
                    return;

                //bring up edit mode to edit the action
                ListViewItem selectedCommandItem = SelectedTabScriptActions.SelectedItems[0];

                //set selected command from the listview item tag object which was assigned to the command
                var currentCommand = (ScriptCommand)selectedCommandItem.Tag;


                //create new command editor form
                frmCommandEditor editCommand = new frmCommandEditor(_automationCommands, GetConfiguredCommands(), TypeContext);

                editCommand.ScriptEngineContext.Container = AContainer;

                //creation mode edit locks form to current command
                editCommand.CreationModeInstance = CreationMode.Edit;

                editCommand.EditingCommand = currentCommand;

                //create clone of current command so databinding does not affect if changes are not saved
                editCommand.OriginalCommand = CommonMethods.Clone(currentCommand);

                editCommand.ScriptEngineContext.Variables = new List<ScriptVariable>(ScriptVariables);
                editCommand.ScriptEngineContext.Arguments = new List<ScriptArgument>(ScriptArguments);
                editCommand.ScriptEngineContext.Elements = new List<ScriptElement>(ScriptElements);
                editCommand.ScriptEngineContext.ImportedNamespaces = ImportedNamespaces;

                editCommand.ScriptEngineContext.ProjectPath = ScriptProjectPath;

                if (currentCommand.CommandName == "SeleniumElementActionCommand")
                    editCommand.HTMLElementRecorderURL = HTMLElementRecorderURL;

                //show edit command form and save changes on OK result
                if (editCommand.ShowDialog() == DialogResult.OK)
                {
                    CreateUndoSnapshot();
                    selectedCommandItem.Tag = editCommand.SelectedCommand;
                    selectedCommandItem.Text = editCommand.SelectedCommand.GetDisplayValue();
                    selectedCommandItem.SubItems.Add(editCommand.SelectedCommand.GetDisplayValue());

                    ScriptVariables = editCommand.ScriptEngineContext.Variables;
                    ScriptArguments = editCommand.ScriptEngineContext.Arguments;                    
                }

                if (editCommand.SelectedCommand.CommandName == "SeleniumElementActionCommand")
                {
                    CreateUndoSnapshot();
                    ScriptElements = editCommand.ScriptEngineContext.Elements;
                    HTMLElementRecorderURL = editCommand.HTMLElementRecorderURL;
                }

                ResetVariableArgumentBindings();
                editCommand.Dispose();                
            }
            catch(Exception ex)
            {
                Notify($"An Error Occurred: {ex.Message}", Color.Red);
            }
        }

        private void CutRows()
        {
            CreateUndoSnapshot();

            //initialize list of items to copy
            if (_rowsSelectedForCopy == null)
            {
                _rowsSelectedForCopy = new List<ListViewItem>();
            }
            else
            {
                _rowsSelectedForCopy.Clear();
            }

            //copy into list for all selected
            if (SelectedTabScriptActions.SelectedItems.Count >= 1)
            {
                foreach (ListViewItem item in SelectedTabScriptActions.SelectedItems)
                {
                    _rowsSelectedForCopy.Add(item);
                    SelectedTabScriptActions.Items.Remove(item);
                }

                Notify(_rowsSelectedForCopy.Count + " item(s) cut to clipboard!", Color.White);
            }
        }

        private void CopyRows()
        {
            //initialize list of items to copy
            if (_rowsSelectedForCopy == null)
            {
                _rowsSelectedForCopy = new List<ListViewItem>();
            }
            else
            {
                _rowsSelectedForCopy.Clear();
            }

            //copy into list for all selected
            if (SelectedTabScriptActions.SelectedItems.Count >= 1)
            {
                foreach (ListViewItem item in SelectedTabScriptActions.SelectedItems)
                {
                    _rowsSelectedForCopy.Add(item);
                }

                Notify(_rowsSelectedForCopy.Count + " item(s) copied to clipboard!", Color.White);
            }
        }

        private void PasteRows()
        {
            CreateUndoSnapshot();

            if (_rowsSelectedForCopy != null)
            {

                if (SelectedTabScriptActions.SelectedItems.Count == 0)
                {
                    MessageBox.Show("In order to paste, you must first select a command to paste under.",
                        "Select Command To Paste Under");
                    return;
                }

                int destinationIndex = SelectedTabScriptActions.SelectedItems[0].Index + 1;

                foreach (ListViewItem item in _rowsSelectedForCopy)
                {
                    dynamic duplicatedCommand = CommonMethods.Clone(item.Tag);
                    duplicatedCommand.GenerateID();
                    SelectedTabScriptActions.Items.Insert(destinationIndex, CreateScriptCommandListViewItem(duplicatedCommand));
                    destinationIndex += 1;
                }

                SelectedTabScriptActions.Invalidate();
                Notify(_rowsSelectedForCopy.Count + " item(s) pasted!", Color.White);
            }
        }

        private void UndoChange()
        {
            if (((ScriptActionTag)SelectedTabScriptActions.Tag).UndoList.Count > 0)
            {
                CreateRedoSnapshot();
                SelectedTabScriptActions.Items.Clear();

                foreach (ListViewItem rowItem in ((ScriptActionTag)SelectedTabScriptActions.Tag).UndoList.Last())
                {
                    SelectedTabScriptActions.Items.Add(rowItem);
                }

                ((ScriptActionTag)SelectedTabScriptActions.Tag).UndoList
                    .RemoveAt(((ScriptActionTag)SelectedTabScriptActions.Tag).UndoList.Count - 1);

                SelectedTabScriptActions.Invalidate();
            }
        }

        private void RedoChange()
        {
            if (((ScriptActionTag)SelectedTabScriptActions.Tag).RedoList.Count > 0)
            {
                CreateUndoSnapshot();
                SelectedTabScriptActions.Items.Clear();

                foreach (ListViewItem rowItem in ((ScriptActionTag)SelectedTabScriptActions.Tag).RedoList.Last())
                {
                    SelectedTabScriptActions.Items.Add(rowItem);
                }

                ((ScriptActionTag)SelectedTabScriptActions.Tag).RedoList
                    .RemoveAt(((ScriptActionTag)SelectedTabScriptActions.Tag).RedoList.Count - 1);

                SelectedTabScriptActions.Invalidate();
            }
        }

        private void CreateUndoSnapshot()
        {
            if (!uiScriptTabControl.SelectedTab.Text.Contains(" *"))
                uiScriptTabControl.SelectedTab.Text += " *";

            List<ListViewItem> itemList = new List<ListViewItem>();
            foreach (ListViewItem rowItem in SelectedTabScriptActions.Items)
            {
                ListViewItem copiedRowItem = (ListViewItem)rowItem.Clone();
                itemList.Add(copiedRowItem);
            }

            ((ScriptActionTag)SelectedTabScriptActions.Tag).UndoList.Add(itemList);

            if (((ScriptActionTag)SelectedTabScriptActions.Tag).UndoList.Count > 10)
                ((ScriptActionTag)SelectedTabScriptActions.Tag).UndoList.RemoveAt(0);
        }

        private void CreateRedoSnapshot()
        {
            if (!uiScriptTabControl.SelectedTab.Text.Contains(" *"))
                uiScriptTabControl.SelectedTab.Text += " *";

            List<ListViewItem> itemList = new List<ListViewItem>();
            foreach (ListViewItem rowItem in SelectedTabScriptActions.Items)
            {
                ListViewItem copiedRowItem = (ListViewItem)rowItem.Clone();
                itemList.Add(copiedRowItem);
            }

            ((ScriptActionTag)SelectedTabScriptActions.Tag).RedoList.Add(itemList);

            if (((ScriptActionTag)SelectedTabScriptActions.Tag).RedoList.Count > 10)
                ((ScriptActionTag)SelectedTabScriptActions.Tag).RedoList.RemoveAt(0);
        }
        #endregion

        #region ListView Create Item
        private ListViewItem CreateScriptCommandListViewItem(ScriptCommand cmdDetails)
        {
            ListViewItem newCommand = new ListViewItem();
            newCommand.Text = cmdDetails.GetDisplayValue();
            newCommand.SubItems.Add(cmdDetails.GetDisplayValue());
            newCommand.SubItems.Add(cmdDetails.GetDisplayValue());
            newCommand.ToolTipText = cmdDetails.GetDisplayValue();
            newCommand.Tag = cmdDetails;
            newCommand.ForeColor = Color.SteelBlue;
            newCommand.BackColor = Color.DimGray;

            if (_uiImages != null)
                newCommand.ImageIndex = _uiImages.Images.IndexOfKey(cmdDetails.GetType().Name);

            return newCommand;
        }
        #endregion

        #region ListView Comment, Coloring, ToolStrip
        private void IndentListViewItems()
        {
            int indent = 0;
            ScriptCommand cmd;
            foreach (ListViewItem rowItem in SelectedTabScriptActions.Items)
            {
                if (rowItem is null)
                {
                    continue;
                }
                cmd = ((ScriptCommand)(rowItem.Tag));
                if ((cmd.CommandName == "BeginIfCommand") || (cmd.CommandName == "BeginMultiIfCommand") ||
                    (cmd.CommandName == "LoopCollectionCommand") || (cmd.CommandName == "LoopContinuouslyCommand") ||
                    (cmd.CommandName == "LoopNumberOfTimesCommand") || (cmd.CommandName == "BeginTryCommand") ||
                    (cmd.CommandName == "BeginLoopCommand") || (cmd.CommandName == "BeginMultiLoopCommand") ||
                    (cmd.CommandName == "BeginRetryCommand"))
                {
                    indent += 2;
                    rowItem.IndentCount = indent;
                    indent += 2;
                }
                else if (cmd.CommandName == "BeginSwitchCommand")
                {
                    indent += 2;
                    rowItem.IndentCount = indent;
                    indent += 4;
                }
                else if ((cmd.CommandName == "EndLoopCommand") || (cmd.CommandName == "EndIfCommand") ||
                    (cmd.CommandName == "EndTryCommand") || (cmd.CommandName == "EndRetryCommand"))
                {
                    indent -= 2;
                    if (indent < 0) indent = 0;
                    rowItem.IndentCount = indent;
                    indent -= 2;
                    if (indent < 0) indent = 0;
                }
                else if (cmd.CommandName == "EndSwitchCommand")
                {
                    indent -= 4;
                    if (indent < 0) indent = 0;
                    rowItem.IndentCount = indent;
                    indent -= 2;
                    if (indent < 0) indent = 0;
                }
                else if ((cmd.CommandName == "ElseCommand") || (cmd.CommandName == "CatchCommand") ||
                    (cmd.CommandName == "FinallyCommand") || (cmd.CommandName == "CaseCommand"))
                {
                    indent -= 2;
                    if (indent < 0) indent = 0;
                    rowItem.IndentCount = indent;
                    indent += 2;
                    if (indent < 0) indent = 0;
                }
                else
                {
                    rowItem.IndentCount = indent;
                }

            }
        }

        private void AutoSizeLineNumberColumn()
        {
            //auto adjust column width based on # of commands
            int columnWidth = (20 * SelectedTabScriptActions.Items.Count.ToString().Length);
            SelectedTabScriptActions.Columns[0].Width = columnWidth;
        }

        private void lstScriptActions_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            //handle indents
            IndentListViewItems();

            //auto size line numbers based on command count
            AutoSizeLineNumberColumn();

            //get listviewitem
            ListViewItem item = e.Item;

            //get script command reference
            var command = (ScriptCommand)item.Tag;

            //create modified bounds
            var modifiedBounds = e.Bounds;

            //switch between column index
            switch (e.ColumnIndex)
            {
                case 0:
                    //draw row number
                    e.Graphics.DrawString((e.ItemIndex + 1).ToString(),
                        SelectedTabScriptActions.Font, Brushes.LightSlateGray, modifiedBounds);
                    break;
                case 1:
                    try
                    {
                        if (command.PauseBeforeExecution)
                        {
                            var breakPointImg = new Bitmap(CoreResources.command_breakpoint, new Size(18, 18));
                            e.Graphics.DrawImage(breakPointImg, modifiedBounds.Left, modifiedBounds.Top + 3);
                        }
                        else if (command.IsCommented)
                        {
                            var commentedImg = new Bitmap(CoreResources.command_disabled, new Size(18, 18));
                            e.Graphics.DrawImage(commentedImg, modifiedBounds.Left, modifiedBounds.Top + 3);
                        }
                        else
                        {
                            //draw command icon
                            var img = _uiImages.Images[command.GetType().Name];
                            if (img != null)
                                e.Graphics.DrawImage(img, modifiedBounds.Left, modifiedBounds.Top + 3);
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        //icon draw failure
                        Console.WriteLine(ex);
                    }
                    break;
                case 2:
                    //write command text
                    Brush commandNameBrush, commandBackgroundBrush;
                    if ((e.Item.Focused) || (e.Item.Selected))
                    {
                        //selected item coloring
                        commandNameBrush = Brushes.White;
                        commandBackgroundBrush = Brushes.DodgerBlue;
                    }
                    else if (command.PauseBeforeExecution)
                    {
                        //pause before execution coloring
                        commandNameBrush = Brushes.Red;
                        commandBackgroundBrush = Brushes.MistyRose;
                    }
                    else if ((command.CommandName == "AddCodeCommentCommand") || (command.IsCommented))
                    {
                        //comments and commented command coloring
                        commandNameBrush = Brushes.MediumSeaGreen;
                        commandBackgroundBrush = Brushes.Honeydew;
                    }
                    else if ((command.CommandName == "BrokenCodeCommentCommand"))
                    {
                        //comments and commented command coloring
                        commandNameBrush = Brushes.Black;
                        commandBackgroundBrush = Brushes.DarkGray;
                    }
                    else
                    {
                        //standard coloring
                        commandNameBrush = Brushes.SteelBlue;
                        commandBackgroundBrush = Brushes.WhiteSmoke;
                    }

                    //fill with background color
                    e.Graphics.FillRectangle(commandBackgroundBrush, modifiedBounds);

                    //get indent count
                    var indentPixels = (item.IndentCount * 15);

                    //set indented X position
                    modifiedBounds.X += indentPixels;

                    //draw string
                    e.Graphics.DrawString(command.GetDisplayValue(), SelectedTabScriptActions.Font,
                                          commandNameBrush, modifiedBounds);
                    break;  
            }
        }

        private void lstScriptActions_MouseMove(object sender, MouseEventArgs e)
        {
            SelectedTabScriptActions.Invalidate();
        }

        private void lstScriptActions_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (SelectedTabScriptActions.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    cmsScriptActions.Show(Cursor.Position);
                }
            }
        }

        private void DeleteSelectedCode()
        {
            SelectAllScopedCode();

            foreach (ListViewItem item in SelectedTabScriptActions.SelectedItems)
                SelectedTabScriptActions.Items.Remove(item);

            SelectedTabScriptActions.Invalidate();

            GC.Collect();
        }

        private void SelectAllScopedCode()
        {
            foreach (ListViewItem item in SelectedTabScriptActions.SelectedItems)
            {
                switch (((ScriptCommand)item.Tag).CommandName)
                {
                    case "LoopCollectionCommand":
                    case "LoopContinuouslyCommand":
                    case "LoopNumberOfTimesCommand":
                    case "BeginLoopCommand":
                    case "BeginMultiLoopCommand":
                        FindEndCommand(item, "EndLoopCommand");
                        break;
                    case "BeginIfCommand":
                    case "BeginMultiIfCommand":
                        FindEndCommand(item, "EndIfCommand");
                        break;
                    case "BeginTryCommand":
                        FindEndCommand(item, "EndTryCommand");
                        break;
                    case "BeginRetryCommand":
                        FindEndCommand(item, "EndRetryCommand");
                        break;
                    case "BeginSwitchCommand":
                        FindEndCommand(item, "EndSwitchCommand");
                        break;
                    default:
                        break;
                }
            }
        }

        private void FindEndCommand(ListViewItem item, string endCommandName)
        {
            for (int itemIndex = item.Index; itemIndex < SelectedTabScriptActions.Items.Count; itemIndex++)
            {
                SelectedTabScriptActions.Items[itemIndex].Selected = true;
                if (((ScriptCommand)SelectedTabScriptActions.Items[itemIndex].Tag).CommandName == endCommandName &&
                    SelectedTabScriptActions.Items[itemIndex].IndentCount == item.IndentCount)
                    break;
            }
        }

        private void SetSelectedCodeToCommented(bool setCommented)
        {
            SelectAllScopedCode();

            //warn if nothing was selected
            if (SelectedTabScriptActions.SelectedItems.Count == 0)
                Notify("No code was selected!", Color.Yellow);
            else
                CreateUndoSnapshot();

            //get each item and set appropriately
            foreach (ListViewItem item in SelectedTabScriptActions.SelectedItems)
            {
                var selectedCommand = (ScriptCommand)item.Tag;
                selectedCommand.IsCommented = setCommented;
            }

            //recolor
            SelectedTabScriptActions.Invalidate();
        }

        private void AddRemoveBreakpoint()
        {
            //warn if nothing was selected
            if (SelectedTabScriptActions.SelectedItems.Count == 0)
                Notify("No code was selected!", Color.Yellow);
            else
                CreateUndoSnapshot();

            //get each item and set appropriately
            foreach (ListViewItem item in SelectedTabScriptActions.SelectedItems)
            {
                var selectedCommand = (ScriptCommand)item.Tag;
                selectedCommand.PauseBeforeExecution = !selectedCommand.PauseBeforeExecution;
            }

            //recolor
            SelectedTabScriptActions.Invalidate();
        }

        private void disableSelectedCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedCodeToCommented(true);
        }

        private void enableSelectedCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedCodeToCommented(false);
        }

        private void addRemoveBreakpointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRemoveBreakpoint();
        }

        private void cutSelectedCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CutRows();
        }

        private void copySelectedCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyRows();
        }

        private void pasteSelectedCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteRows();
        }

        private void deleteSelectedCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedCode();
        }

        private void viewCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currentCommand = SelectedTabScriptActions.SelectedItems[0].Tag;
            var jsonText = JsonConvert.SerializeObject(currentCommand, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });
            var dialog = new frmDialog(jsonText, "Command Code", DialogType.OkOnly, 0);
            dialog.ShowDialog();
            dialog.Dispose();
        }

        private void moveToParentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var commandGroup = new List<ScriptCommand>();
            //loop each
            for (int i = SelectedTabScriptActions.SelectedItems.Count - 1; i >= 0; i--)
            {
                //add to list and remove existing
                commandGroup.Add((ScriptCommand)SelectedTabScriptActions.SelectedItems[i].Tag);
                SelectedTabScriptActions.Items.Remove(SelectedTabScriptActions.SelectedItems[i]);
            }

            commandGroup.Reverse();
            MoveToParentCommands.AddRange(commandGroup);           
        }

        public void AddCommandToListView(ScriptCommand selectedCommand, int index = -1)
        {
            if(!uiScriptTabControl.SelectedTab.Controls[0].Visible)
                uiScriptTabControl.SelectedTab.Controls[0].Show();

            ListViewItem command;

            //valid command verification for drag/dropped commands
            if (selectedCommand != null)
                command = CreateScriptCommandListViewItem(selectedCommand);
            else
                return;

            int insertionIndex;

            if (index == -1)
            {
                //insert to end by default
                insertionIndex = SelectedTabScriptActions.Items.Count;

                //verify setting to insert inline is selected and if an item is currently selected
                if ((_appSettings.ClientSettings.InsertCommandsInline) && (SelectedTabScriptActions.SelectedItems.Count > 0))
                {
                    //insert inline
                    insertionIndex = SelectedTabScriptActions.SelectedItems[0].Index + 1;
                }
            }
            else
                insertionIndex = index;           

            //insert command
            SelectedTabScriptActions.Items.Insert(insertionIndex, command);
            ClearSelectedListViewItems();
            command.Selected = true;

            //special types also get a following command and comment
            if ((selectedCommand.CommandName == "LoopCollectionCommand") || (selectedCommand.CommandName == "LoopContinuouslyCommand") ||
                (selectedCommand.CommandName == "LoopNumberOfTimesCommand") || (selectedCommand.CommandName == "BeginLoopCommand") ||
                (selectedCommand.CommandName == "BeginMultiLoopCommand"))
            {
                dynamic addCodeCommentCommand = TypeMethods.CreateTypeInstance(AContainer, "AddCodeCommentCommand");
                addCodeCommentCommand.v_Comment = "Items in this section will run within the loop";
                SelectedTabScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(addCodeCommentCommand));

                dynamic endLoopCommand = TypeMethods.CreateTypeInstance(AContainer, "EndLoopCommand");
                SelectedTabScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(endLoopCommand));
            }
            else if ((selectedCommand.CommandName == "BeginIfCommand") || (selectedCommand.CommandName == "BeginMultiIfCommand"))
            {
                dynamic addCodeCommentCommand = TypeMethods.CreateTypeInstance(AContainer, "AddCodeCommentCommand");
                addCodeCommentCommand.v_Comment = "Items in this section will run if the statement is true";
                SelectedTabScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(addCodeCommentCommand));
                
                dynamic endIfCommand = TypeMethods.CreateTypeInstance(AContainer, "EndIfCommand");
                SelectedTabScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(endIfCommand));
            }
            else if (selectedCommand.CommandName == "BeginTryCommand")
            {
                dynamic addCodeCommentCommand = TypeMethods.CreateTypeInstance(AContainer, "AddCodeCommentCommand");
                addCodeCommentCommand.v_Comment = "Items in this section will be handled if error occurs";
                SelectedTabScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(addCodeCommentCommand));

                dynamic catchCommand = TypeMethods.CreateTypeInstance(AContainer, "CatchCommand");
                SelectedTabScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(catchCommand));

                dynamic codeCommentCommand = TypeMethods.CreateTypeInstance(AContainer, "AddCodeCommentCommand");
                codeCommentCommand.v_Comment = "This section executes if error occurs above";
                SelectedTabScriptActions.Items.Insert(insertionIndex + 3, CreateScriptCommandListViewItem(codeCommentCommand));

                dynamic endTryCommand = TypeMethods.CreateTypeInstance(AContainer, "EndTryCommand");
                SelectedTabScriptActions.Items.Insert(insertionIndex + 4, CreateScriptCommandListViewItem(endTryCommand));
            }
            else if (selectedCommand.CommandName == "BeginRetryCommand")
            {
                dynamic addCodeCommentCommand = TypeMethods.CreateTypeInstance(AContainer, "AddCodeCommentCommand");
                addCodeCommentCommand.v_Comment = "Items in this section will be retried as long as the condition is not met or an error is thrown";
                SelectedTabScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(addCodeCommentCommand));
                
                dynamic endRetryCommand = TypeMethods.CreateTypeInstance(AContainer, "EndRetryCommand");
                SelectedTabScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(endRetryCommand));
            }
            else if (selectedCommand.CommandName == "BeginSwitchCommand")
            {
                dynamic caseCommand = TypeMethods.CreateTypeInstance(AContainer, "CaseCommand");
                caseCommand.v_CaseValue = "Default";
                SelectedTabScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(caseCommand));

                dynamic addCodeCommentCommand = TypeMethods.CreateTypeInstance(AContainer, "AddCodeCommentCommand");
                addCodeCommentCommand.v_Comment = "Items in this section will run if no case statements match";
                SelectedTabScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(addCodeCommentCommand));
                
                dynamic endSwitchCommand = TypeMethods.CreateTypeInstance(AContainer, "EndSwitchCommand");
                SelectedTabScriptActions.Items.Insert(insertionIndex + 3, CreateScriptCommandListViewItem(endSwitchCommand));
            }

            CreateUndoSnapshot();
            SelectedTabScriptActions.Invalidate();
            AutoSizeLineNumberColumn();
        }
        #endregion        
        #endregion
    }
}
