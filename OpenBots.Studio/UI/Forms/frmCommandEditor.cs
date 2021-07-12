//Copyright (c) 2019 Jason Bayldon
//Modifications - Copyright (c) 2020 OpenBots Inc.
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
using Autofac;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Script;
using OpenBots.Core.UI.Controls;
using OpenBots.Core.UI.Forms;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.UI.CustomControls;
using OpenBots.UI.CustomControls.CustomUIControls;
using OpenBots.UI.Forms.Supplement_Forms;
using OpenBots.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace OpenBots.UI.Forms
{
    public partial class frmCommandEditor : UIForm, IfrmCommandEditor
    {
        //list of available commands
        public List<AutomationCommand> CommandList { get; set; } = new List<AutomationCommand>();
        //engine context assigned from frmScriptBuilder
        public IScriptContext ScriptContext { get; set; }
        public string ProjectPath { get; set; }
        public IContainer AContainer { get; set; }
        //reference to currently selected command
        public ScriptCommand SelectedCommand { get; set; }
        //reference to original command
        public ScriptCommand OriginalCommand { get; set; }
        //assigned by frmScriptBuilder to restrict inputs for editing existing commands
        public CreationMode CreationModeInstance { get; set; }
        //startup command, assigned from frmScriptBuilder
        public string DefaultStartupCommand { get; set; }
        //editing command, assigned from frmScriptBuilder when editing a command
        public ScriptCommand EditingCommand { get; set; }
        //track existing commands for visibility
        public List<ScriptCommand> ConfiguredCommands { get; set; }
        public string HTMLElementRecorderURL { get; set; }
        public TypeContext TypeContext { get; set; }
        private ICommandControls _commandControls;
        private ToolTip _errorToolTip;
        public FlowLayoutPanel flw_InputVariables { get; set; }

        #region Form Events
        //handle events for the form

        public frmCommandEditor(List<AutomationCommand> commands, List<ScriptCommand> existingCommands, TypeContext typeContext)
        {
            InitializeComponent();
            TypeContext = typeContext;
            CommandList = commands;
            ConfiguredCommands = existingCommands;
        }

        private void frmNewCommand_Load(object sender, EventArgs e)
        {
            // Initialize CommandControls with Current Editor
            _commandControls = new CommandControls(this, TypeContext, AContainer, ProjectPath);
            _errorToolTip = AddValidationErrorToolTip();

            //order list
            CommandList = CommandList.OrderBy(itm => itm.FullName).ToList();

            //set command list
            cboSelectedCommand.DataSource = CommandList;

            //Set DisplayMember to track DisplayValue from the class
            cboSelectedCommand.DisplayMember = "FullName";

            if ((CreationModeInstance == CreationMode.Add) && (DefaultStartupCommand != null) && (CommandList.Where(x => x.FullName == DefaultStartupCommand).Count() > 0))
                cboSelectedCommand.SelectedIndex = cboSelectedCommand.FindStringExact(DefaultStartupCommand);
            else if (CreationModeInstance == CreationMode.Edit)
            {
                // var requiredCommand = commandList.Where(x => x.FullName.Contains(defaultStartupCommand)).FirstOrDefault(); //&& x.CommandClass.Name == originalCommand.CommandName).FirstOrDefault();

                var requiredCommand = CommandList.Where(x => x.Command.ToString() == EditingCommand.ToString()).FirstOrDefault();

                if (requiredCommand == null)
                    MessageBox.Show("Command was not found! " + DefaultStartupCommand);
                else
                    cboSelectedCommand.SelectedIndex = cboSelectedCommand.FindStringExact(requiredCommand.FullName);
            }
            else
                cboSelectedCommand.SelectedIndex = 0;

            //force commit event to populate the flow layout
            cboSelectedCommand_SelectionChangeCommitted(null, null);

            //apply original variables if command is being updated
            if (OriginalCommand != null)
            {
                //update bindings
                foreach (Control c in flw_InputVariables.Controls)
                {
                    foreach (Binding b in c.DataBindings)
                        b.ReadValue();

                    //helper for box
                    if (c is UIPictureBox)
                    {
                        var typedControl = (UIPictureBox)c;

                        dynamic cmd;
                        if (SelectedCommand.CommandName == "SurfaceAutomationCommand")
                        {
                            cmd = (IImageCommands)SelectedCommand;

                            if (!string.IsNullOrEmpty(cmd.v_ImageCapture))
                                typedControl.Image = CommonMethods.Base64ToImage(cmd.v_ImageCapture);
                        }
                        else if (SelectedCommand.CommandName == "CaptureImageCommand")
                        {
                            cmd = (IImageCommands)SelectedCommand;

                            if (!string.IsNullOrEmpty(cmd.v_ImageCapture))
                                typedControl.Image = CommonMethods.Base64ToImage(cmd.v_ImageCapture);
                        }
                    }
                }
                //handle selection change events
            }

            //gracefully handle post initialization setups (drop downs, etc)
            AfterFormInitialization();
            ScriptContext.AddIntellisenseControls(Controls);
        }

        private void frmCommandEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            ScriptContext.RemoveIntellisenseControls(Controls);
        }

        private void frmCommandEditor_Shown(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.Sizable;
            SelectedCommand.Shown();
        }

        private void frmCommandEditor_Resize(object sender, EventArgs e)
        {
            foreach (Control item in flw_InputVariables.Controls)
                item.Width = Width - 70;
        }

        private void cboSelectedCommand_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //clear controls
            flw_InputVariables.Controls.Clear();

            //find underlying command item
            var selectedCommandItem = cboSelectedCommand.Text;

            //get command
            var userSelectedCommand = CommandList.Where(itm => itm.FullName == selectedCommandItem).FirstOrDefault();

            //create new command for binding
            SelectedCommand = (ScriptCommand)Activator.CreateInstance(userSelectedCommand.CommandClass);
            SelectedCommand.CommandIcon = null;
            //Todo: MAKE OPTION TO RENDER ON THE FLY

            //if (true)
            //{
            //    var renderedControls = selectedCommand.Render(null);
            //    userSelectedCommand.UIControls = new List<Control>();
            //    userSelectedCommand.UIControls.AddRange(renderedControls);
            //}

            //update data source
            userSelectedCommand.Command = SelectedCommand;

            //copy original properties
            if (OriginalCommand != null)
                CopyPropertiesTo(OriginalCommand, SelectedCommand);

            try
            {
                //bind controls to new data source
                userSelectedCommand.Bind(this, _commandControls);
            }
            catch (Exception ex)
            {
                frmDialog errorForm = new frmDialog(ex.Message, ex.GetType()?.ToString(), DialogType.CancelOnly, 0); ;
                errorForm.ShowDialog();
            }

            Label descriptionLabel = new Label();
            descriptionLabel.AutoSize = true;
            descriptionLabel.Font = new Font("Segoe UI", 12);
            descriptionLabel.ForeColor = Color.White;
            descriptionLabel.Name = "lbl_" + userSelectedCommand.ShortName;
            descriptionLabel.Text = userSelectedCommand.Description;
            descriptionLabel.Padding = new Padding(0, 0, 0, 5);
            flw_InputVariables.Controls.Add(descriptionLabel);

            Label separator = new Label();
            separator.AutoSize = false;
            separator.Height = 2;
            separator.BorderStyle = BorderStyle.Fixed3D;            
            flw_InputVariables.Controls.Add(separator);

            //add each control
            foreach (var ctrl in userSelectedCommand.UIControls)
                flw_InputVariables.Controls.Add(ctrl);

            OnResize(EventArgs.Empty);
            SelectedCommand.Shown();
        }

        public void cboSelectedCommand_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void CopyPropertiesTo(object fromObject, object toObject)
        {
            PropertyInfo[] toObjectProperties = toObject.GetType().GetProperties();
            foreach (PropertyInfo propTo in toObjectProperties)
            {
                try
                {
                    PropertyInfo propFrom = fromObject.GetType().GetProperty(propTo.Name);

                    if (propTo.Name == "SelectionName")
                        continue;

                    if (propFrom != null && propFrom.CanWrite)
                        propTo.SetValue(toObject, propFrom.GetValue(fromObject, null), null);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void AfterFormInitialization()
        {
            //force control resizing
            frmCommandEditor_Resize(null, null);
        }
        #endregion Form Events

        #region Save/Close Buttons

        //handles returning DialogResult
        public void uiBtnAdd_Click(object sender, EventArgs e)
        {
            //commit any datagridviews
            foreach (Control ctrl in flw_InputVariables.Controls)
            {
                if (ctrl is DataGridView)
                {
                    DataGridView currentControl = (DataGridView)ctrl;
                    currentControl.EndEdit();
                    currentControl.CurrentCell = null;
                }

                if (ctrl is UIPictureBox)
                {
                    var typedControl = (UIPictureBox)ctrl;
                    dynamic cmd;

                    if (SelectedCommand.CommandName == "SurfaceAutomationCommand")
                    {
                        cmd = (IImageCommands)SelectedCommand;
                        cmd.v_ImageCapture = typedControl.EncodedImage;
                    }
                    else if (SelectedCommand.CommandName == "CaptureImageCommand")
                    {
                        cmd = (IImageCommands)SelectedCommand;
                        cmd.v_ImageCapture = typedControl.EncodedImage;
                    }
                }
            }

            bool success = ValidateInputs();
            if (success)
                DialogResult = DialogResult.OK;
        }

        private bool ValidateInputs()
        {
            bool isAllValid = true;
            dynamic currentControl;
            _errorToolTip.RemoveAll();

            foreach (Control ctrl in flw_InputVariables.Controls)
            {
                if (ctrl.Visible)
                {
                    if (ctrl is UIDataGridView)
                    {
                        currentControl = (UIDataGridView)ctrl;
                        currentControl.BorderColor = Color.Transparent;

                        var validationContext = (CommandControlValidationContext)currentControl.Tag;

                        if (currentControl.Rows.Count == 0 && validationContext.IsRequired)
                        {
                            currentControl.BorderColor = Color.Red;
                            _errorToolTip.SetToolTip(currentControl, "Input is required.");
                            isAllValid = false;
                            continue;
                        }

                        foreach (DataGridViewRow row in currentControl.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                if (currentControl.Columns[0] is DataGridViewCheckBoxColumn)
                                {
                                    if (row.Cells[0].Value.ToString() == "True")
                                    {
                                        foreach (DataGridViewCell cell in row.Cells)
                                        {
                                            if (cell is DataGridViewTextBoxCell && !cell.OwningColumn.ReadOnly)
                                            {
                                                bool isCellValid = ValidateInput(true, cell.Value?.ToString(), currentControl);
                                                if (!isCellValid)
                                                {
                                                    isAllValid = false;
                                                    break;
                                                }
                                            }
                                        }
                                    }                                   
                                }
                                else
                                {
                                    foreach (DataGridViewCell cell in row.Cells)
                                    {
                                        if (cell is DataGridViewTextBoxCell && !cell.OwningColumn.ReadOnly)
                                        {
                                            bool isCellValid = ValidateInput(true, cell.Value?.ToString(), currentControl);

                                            if (!isCellValid)
                                            {
                                                isAllValid = false;
                                                break;
                                            }
                                        }
                                    }
                                }                             
                            }                          
                        }
                    }
                    else if (ctrl is UITextBox)
                    {
                        currentControl = (UITextBox)ctrl;
                        currentControl.BorderColor = Color.Transparent;
                        isAllValid = ValidateInput(isAllValid, currentControl.Text, currentControl);
                    }
                    else if (ctrl is UIComboBox)
                    {
                        currentControl = (UIComboBox)ctrl;
                        currentControl.BorderColor = Color.Transparent;
                        isAllValid = ValidateInput(isAllValid, currentControl.Text, currentControl);
                    }
                    else if(ctrl is UIPictureBox)
                    {
                        currentControl = (UIPictureBox)ctrl;
                        currentControl.BorderColor = Color.Transparent;
                        isAllValid = ValidateInput(isAllValid, currentControl.EncodedImage, currentControl);
                    }
                    else
                        continue;
                }              
            }
            return isAllValid;
        }

        private bool ValidateInput(bool isAllValid, string validatingText, dynamic currentControl)
        {
            var validationContext = (CommandControlValidationContext)currentControl.Tag;
            string errorMessage = "";

            //check whether input is required
            if (string.IsNullOrEmpty(validatingText) && validationContext.IsRequired == true)
            {
                currentControl.BorderColor = Color.Red;
                _errorToolTip.SetToolTip(currentControl, "Input is required.");
                isAllValid = false;
                return isAllValid;
            }
            else if (string.IsNullOrEmpty(validatingText) && validationContext.IsRequired == false)
                return isAllValid;

            if (validationContext.IsImageCapture)
                return isAllValid;

            if (validationContext.IsDropDown)
                return isAllValid;

            foreach(var compType in validationContext.CompatibleTypes) 
            {
                EmitResult result;
                if(currentControl is UITextBox && currentControl.IsEvaluateSnippet)
                    result = ScriptContext.EvaluateSnippet(validatingText);
                else
                    result = ScriptContext.EvaluateInput(compType, validatingText);

                if (result.Success)
                    return isAllValid;
                else
                {
                    var errorMessages = result.Diagnostics.ToList().Where(x => x.DefaultSeverity == DiagnosticSeverity.Error).Select(x => x.ToString()).ToArray();
                    errorMessage = string.Join(Environment.NewLine, errorMessages);
                }
            }
 
            isAllValid = false;
            currentControl.BorderColor = Color.Red;
            _errorToolTip.SetToolTip(currentControl, errorMessage);
            return isAllValid;
        }

        public ToolTip AddValidationErrorToolTip()
        {
            ToolTip errorToolTip = new ToolTip();
            errorToolTip.ToolTipIcon = ToolTipIcon.Error;
            errorToolTip.IsBalloon = true;
            errorToolTip.ShowAlways = true;
            errorToolTip.ToolTipTitle = "Error";
            errorToolTip.AutoPopDelay = 15000;
            return errorToolTip;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        #endregion Save/Close Buttons

        /// <summary>
        /// Delegate for showing message box
        /// </summary>
        /// <param name="message"></param>
        public delegate void ShowMessageDelegate(string message, string title, DialogType dialogType, int closeAfter);
        /// <summary>
        /// Used by the automation engine to show a message to the user on-screen. If UI is not available, a standard messagebox will be invoked instead.
        /// </summary>
        public void ShowMessage(string message, string title, DialogType dialogType, int closeAfter)
        {
            if (InvokeRequired)
            {
                var d = new ShowMessageDelegate(ShowMessage);
                Invoke(d, new object[] { message, title, dialogType, closeAfter });
            }
            else
            {
                var confirmationForm = new frmDialog(message, title, dialogType, closeAfter);
                confirmationForm.ShowDialog();
                confirmationForm.Dispose();
            }
        }

        
    }
}
