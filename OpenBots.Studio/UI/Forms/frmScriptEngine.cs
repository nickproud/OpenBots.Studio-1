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
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Model.EngineModel;
using OpenBots.Core.Project;
using OpenBots.Core.Script;
using OpenBots.Core.Settings;
using OpenBots.Core.UI.DTOs;
using OpenBots.Core.UI.Forms;
using OpenBots.Core.User32;
using OpenBots.Core.Utilities.FormsUtilities;
using OpenBots.Engine;
using OpenBots.Nuget;
using OpenBots.Properties;
using OpenBots.UI.Forms.ScriptBuilder_Forms;
using OpenBots.UI.Forms.Supplement_Forms;
using OpenBots.Utilities;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OpenBots.UI.Forms
{
    public partial class frmScriptEngine : UIForm, IfrmScriptEngine
    {
        #region Form Variables
        public EngineContext EngineContext { get; set; } = new EngineContext();
        public IAutomationEngineInstance EngineInstance { get; set; }
        public bool IsNewTaskSteppedInto { get; set; }
        public bool IsNewTaskResumed { get; set; }
        public bool IsNewTaskCancelled { get; set; }
        public bool IsHiddenTaskEngine { get; set; }
        public int DebugLineNumber { get; set; }
        public bool CloseWhenDone { get; set; }
        public bool ClosingAllEngines { get; set; }   
        
        private string _configPath;
        private bool _isParentScheduledTask;
        #endregion

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams mdiCp = base.CreateParams;
                mdiCp.ClassStyle = mdiCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return mdiCp;
            }
        }

        //events and methods
        #region Form Events/Methods
        public frmScriptEngine(EngineContext engineContext, bool blnCloseWhenDone = false)
        {
            InitializeComponent();

            EngineContext = engineContext;
            engineContext.ScriptEngine = this;
            CloseWhenDone = blnCloseWhenDone;      
        }

        //Used for Scheduled/Attended Tasks
        public frmScriptEngine(string pathToConfig, Logger engineLogger)
        {
            InitializeComponent();
            
            uiBtnScheduleManagement.Visible = true;
          
            _configPath = pathToConfig;
            _isParentScheduledTask = true;

            var scriptProject = Project.OpenProject(_configPath);
            string mainFileName = scriptProject.Main;
            EngineContext.ProjectPath = new DirectoryInfo(_configPath).Parent.FullName;
            string mainFilePath = Directory.GetFiles(EngineContext.ProjectPath, mainFileName, SearchOption.AllDirectories).FirstOrDefault();

            if (mainFilePath == null)
                throw new FileNotFoundException("Main script not found");

            EngineContext.EngineLogger = engineLogger;
            EngineContext.IsDebugMode = false;
            EngineContext.IsScheduledOrAttendedTask = true;
            CloseWhenDone = true;

            //set file
            EngineContext.FilePath = mainFilePath;   
        }

        public IfrmScriptEngine CreateScriptEngineForm(EngineContext engineContext, bool blnCloseWhenDone)
        {
            return new frmScriptEngine(engineContext, blnCloseWhenDone);
        }

        private void frmProcessingStatus_Load(object sender, EventArgs e)
        {
            //get engine settings
            EngineContext.EngineSettings = new ApplicationSettings().GetOrCreateApplicationSettings().EngineSettings;

            //add hooks for hot key cancellation
            GlobalHook.HookStopped += new EventHandler(OnHookStopped);
            GlobalHook.StartEngineCancellationHook(EngineContext.EngineSettings.CancellationKey);
            GlobalHook.KeyDownEvent += GlobalHook_KeyDownEvent;

            if (_isParentScheduledTask)
            {
                List<string> assemblyList = NugetPackageManager.LoadPackageAssemblies(_configPath, true);
                Dictionary<string, List<Type>> groupedTypes = new Dictionary<string, List<Type>>();
                Dictionary<string, List<AssemblyReference>> allNamespaces = new Dictionary<string, List<AssemblyReference>>();
                var builder = AppDomainSetupManager.LoadBuilder(assemblyList, groupedTypes, allNamespaces, ScriptDefaultNamespaces.DefaultNamespaces);// EngineContext.ImportedNamespaces);
                EngineContext.Container = builder.Build();
            }

            //move engine form to bottom right and bring to front
            if (EngineContext.EngineSettings.ShowDebugWindow || EngineContext.IsDebugMode)
            {
                BringToFront();
                MoveFormToBottomRight(this);
            }
            else
                MoveFormToBottomRight(this);

            if (!EngineContext.IsDebugMode && !EngineContext.EngineSettings.ShowDebugWindow)
                FormsHelper.HideForm(this);

            //if listbox should be shown
            if (EngineContext.EngineSettings.ShowAdvancedDebugOutput || EngineContext.IsDebugMode)
            {
                lstSteppingCommands.Show();
                lblMainLogo.Show();
                pbBotIcon.Hide();
                lblAction.Hide();
            }
            else
            {
                lstSteppingCommands.Hide();
                lblMainLogo.Hide();
                pbBotIcon.Show();
                lblAction.Show();
            }

            //start running
            EngineInstance = new AutomationEngineInstance(EngineContext);

            if (IsNewTaskSteppedInto)
            {
                EngineInstance.PauseScript();
                uiBtnPause.Image = Resources.engine_resume;
                uiBtnPause.DisplayText = "Resume";
                uiBtnStepOver.Visible = true;
                uiBtnStepInto.Visible = true;

                EngineContext.ScriptBuilder.CurrentEngine = this;

                //toggle running flag to allow for tab selection
                EngineContext.ScriptBuilder.IsScriptRunning = false;
                ((frmScriptBuilder)EngineContext.ScriptBuilder).OpenOpenBotsFile(EngineContext.FilePath, true);
                EngineContext.ScriptBuilder.IsScriptRunning = true;
            }

            EngineInstance.ReportProgressEvent += Engine_ReportProgress;
            EngineInstance.ScriptFinishedEvent += Engine_ScriptFinishedEvent;
            EngineInstance.LineNumberChangedEvent += EngineInstance_LineNumberChangedEvent;
            EngineInstance.EngineContext.ScriptEngine = this;

            EngineInstance.ExecuteScriptAsync();
        }

        /// <summary>
        /// Triggers the automation engine to stop based on a hooked key press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHookStopped(object sender, EventArgs e)
        {
            if (EngineInstance != null)
                uiBtnCancel_Click(null, null);

            GlobalHook.HookStopped -= OnHookStopped;
        }

        private void GlobalHook_KeyDownEvent(object sender, KeyDownEventArgs e)
        {
            if (e.Key == GlobalHook.StopHookKey)
            {
                GlobalHook.KeyDownEvent -= GlobalHook_KeyDownEvent;
                GlobalHook.StopHook();
            }
        }
        #endregion

        //engine event handlers and methods
        #region Engine Event Handlers
        /// <summary>
        /// Handles Progress Updates raised by Automation Engine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Engine_ReportProgress(object sender, ReportProgressEventArgs e)
        {
            AddStatus(e.ProgressUpdate, e.LoggerColor);
        }

        /// <summary>
        /// Handles Script Finished Event raised by Automation Engine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Engine_ScriptFinishedEvent(object sender, ScriptFinishedEventArgs e)
        {
            if (EngineContext.IsChildEngine)
                CloseWhenDone = true;

            switch (e.Result)
            {
                case ScriptFinishedResult.Successful:
                    AddStatus("Script Completed Successfully");
                    UpdateUI("debug info (success)");
                    break;
                case ScriptFinishedResult.Error:
                    AddStatus($"Error: {e.Error}", Color.Red);
                    AddStatus("Script Completed With Errors!");
                    UpdateUI("debug info (error)");

                    if (_isParentScheduledTask)
                        CloseWhenDone = false;
                    break;
                case ScriptFinishedResult.Cancelled:
                    AddStatus("Script Cancelled By User");
                    UpdateUI("debug info (cancelled)");

                    if (_isParentScheduledTask)
                        CloseWhenDone = false;
                    break;
                default:
                    break;
            }

            AddStatus("Total Execution Time: " + e.ExecutionTime.ToString());

            try
            {
                UpdateLineNumber(0);
            }
            catch(Exception) 
            { 
                /*Attemting to reset the debug line to 0 will occasionally produce an exception 
                 if the engine is improperly closed or interrupted during execution.*/
            }
            
            if(CloseWhenDone)
                ((frmScriptEngine)EngineInstance.EngineContext.ScriptEngine).Invoke((Action)delegate () { Close(); });
        }

        private void EngineInstance_LineNumberChangedEvent(object sender, LineNumberChangedEventArgs e)
        {
            UpdateLineNumber(e.CurrentLineNumber);
        }

        private void lstSteppingCommands_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index != -1)
            {
                SteppingCommandsItem item = lstSteppingCommands.Items[e.Index] as SteppingCommandsItem;

                if (item != null)
                {
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    {
                        e = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index,
                                                  e.State ^ DrawItemState.Selected,
                                                  e.ForeColor, item.Color);

                        e.DrawBackground();
                        e.Graphics.DrawString(item.Text, e.Font, Brushes.White, e.Bounds);
                    }
                    else
                    {
                        e.DrawBackground();
                        e.Graphics.DrawString(item.Text, e.Font, new SolidBrush(item.Color), e.Bounds);
                    }

                    e.DrawFocusRectangle();
                }
            }
        }
        #endregion

        //delegates to marshal changes to UI
        #region Engine Delegates
        /// <summary>
        /// Delegate for adding progress reports
        /// </summary>
        /// <param name="message">The progress report string from Automation Engine</param>
        public delegate void AddStatusDelegate(string text, Color? statusColor = null);
        /// <summary>
        /// Adds a status to the listbox for debugging and display purposes
        /// </summary>
        /// <param name="text"></param>
        public void AddStatus(string text, Color? statusColor = null)
        {
            if (InvokeRequired)
            {
                var d = new AddStatusDelegate(AddStatus);
                Invoke(d, new object[] { text, statusColor });
            }
            else
            {
                if (text == "Pausing Before Execution" && !uiBtnStepOver.Visible)
                {                  
                    uiBtnPause_Click(null, null);
                    uiBtnStepOver.Visible = true;
                    uiBtnStepInto.Visible = true;
                    
                    if (IsHiddenTaskEngine)
                    {
                        //toggle running flag to allow for tab selection
                        EngineContext.ScriptBuilder.IsScriptRunning = false;
                        ((frmScriptBuilder)EngineContext.ScriptBuilder).OpenOpenBotsFile(EngineContext.FilePath, true);
                        EngineContext.ScriptBuilder.IsScriptRunning = true;

                        EngineContext.ScriptBuilder.CurrentEngine = this;
                        IsNewTaskSteppedInto = true;
                        IsHiddenTaskEngine = false;
                        EngineContext.ScriptBuilder.IsScriptPaused = false;                       
                    }

                    EngineContext.ScriptBuilder.IsScriptPaused = false;
                    UpdateLineNumber(DebugLineNumber);
                }
                else if (text == "Pausing Before Exception")
                {
                    uiBtnPause_Click(null, null);
                    uiBtnStepOver.Visible = true;
                    uiBtnStepInto.Visible = true;
                    
                    if (IsHiddenTaskEngine)
                    {
                        EngineContext.ScriptBuilder.CurrentEngine = this;

                        //toggle running flag to allow for tab selection
                        EngineContext.ScriptBuilder.IsScriptRunning = false;
                        ((frmScriptBuilder)EngineContext.ScriptBuilder).OpenOpenBotsFile(EngineContext.FilePath, true);
                        EngineContext.ScriptBuilder.IsScriptRunning = true;

                        IsNewTaskSteppedInto = true;
                        IsHiddenTaskEngine = false;
                    }

                    EngineContext.ScriptBuilder.IsScriptPaused = false;
                    UpdateLineNumber(DebugLineNumber);
                }
                else
                {
                    //update status
                    lblAction.Text = text + "..";
                    SteppingCommandsItem commandsItem = new SteppingCommandsItem
                    {
                        Text = DateTime.Now.ToString("MM/dd/yy hh:mm:ss.fff") + " | " + text,
                        Color = statusColor ?? SystemColors.Highlight
                    };

                    lstSteppingCommands.Items.Add(commandsItem);
                    lstSteppingCommands.SelectedIndex = lstSteppingCommands.Items.Count - 1;
                }
            }
        }

        /// <summary>
        /// Delegate for updating UI after Automation Engine finishes
        /// </summary>
        /// <param name="mainLogoText"></param>
        public delegate void UpdateUIDelegate(string mainLogoText);
        /// <summary>
        /// Standard UI updates after automation is finished running
        /// </summary>
        /// <param name="mainLogoText"></param>
        private void UpdateUI(string mainLogoText)
        {
            if (InvokeRequired)
            {
                var d = new UpdateUIDelegate(UpdateUI);
                Invoke(d, new object[] { mainLogoText });
            }
            else
            {
                //set main logo text
                lblMainLogo.Text = mainLogoText;

                //hide and change buttons not required
                uiBtnPause.Visible = false;
                uiBtnStepOver.Visible = false;
                uiBtnStepInto.Visible = false;
                uiBtnCancel.Visible = true;
                uiBtnCancel.DisplayText = "Close";

                if ((!EngineContext.EngineSettings.ShowAdvancedDebugOutput) && (mainLogoText.Contains("(error)")))
                    pbBotIcon.Image = Resources.error;

                if (mainLogoText.Contains("(error)"))
                {
                    Theme.BgGradientStartColor = Color.OrangeRed;
                    Theme.BgGradientEndColor = Color.OrangeRed;
                    Invalidate();
                }
                else if (mainLogoText.Contains("(success)")) 
                {
                    Theme.BgGradientStartColor = Color.Green;
                    Theme.BgGradientEndColor = Color.Green;
                    Invalidate();                  
                }

                //begin auto close
                if (EngineContext.EngineSettings.AutoCloseDebugWindow)
                    tmrNotify.Enabled = true;
            }
        }

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

        /// <summary>
        /// Delegate for showing engine context form
        /// </summary>
        /// <param name="message"></param>
        public delegate void ShowEngineContextDelegate(string context, int closeAfter);
        /// <summary>
        /// Used by the automation engine to show the engine context data
        /// </summary>
        public void ShowEngineContext(string context, int closeAfter)
        {
            if (InvokeRequired)
            {
                var d = new ShowEngineContextDelegate(ShowEngineContext);
                Invoke(d, new object[] { context, closeAfter });
            }
            else
            {
                var contextForm = new frmEngineContextViewer(context, closeAfter);
                contextForm.ShowDialog();
                contextForm.Dispose();
            }
        }

        public delegate List<ScriptVariable> ShowHTMLInputDelegate(string htmlTemplate);
        public List<ScriptVariable> ShowHTMLInput(string htmlTemplate)
        {
            if (InvokeRequired)
            {
                var d = new ShowHTMLInputDelegate(ShowHTMLInput);
                Invoke(d, new object[] { htmlTemplate });
                return null;
            }
            else
            {
                var inputForm = new frmHTMLDisplayForm
                {
                    TemplateHTML = htmlTemplate
                };
                
                inputForm.ShowDialog();

                if (inputForm.DialogResult == DialogResult.OK)
                {
                    var variables = inputForm.GetVariablesFromHTML("input");
                    variables.AddRange(inputForm.GetVariablesFromHTML("select"));

                    inputForm.Dispose();
                    return variables;
                }
                else
                {
                    inputForm.Dispose();
                    return null;
                }                                  
            }
        }

        public delegate void UpdateLineNumberDelegate(int lineNumber);
        public void UpdateLineNumber(int lineNumber)
        {
            if (InvokeRequired)
            {
                var d = new UpdateLineNumberDelegate(UpdateLineNumber);
                Invoke(d, new object[] { lineNumber });
            }
            else
            {
                DebugLineNumber = lineNumber;

                if (EngineContext.ScriptBuilder != null && !IsHiddenTaskEngine)
                    EngineContext.ScriptBuilder.DebugLine = lineNumber;
            }
        }
        #endregion

        //various small UI methods
        #region UI Elements
        private void lblClose_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void lblClose_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void autoCloseTimer_Tick(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

        public delegate void uiBtnCancel_ClickDelegate(object sender, EventArgs e);
        public void uiBtnCancel_Click(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                var d = new uiBtnCancel_ClickDelegate(uiBtnCancel_Click);
                Invoke(d, new object[] { sender, e });
            }
            else
            {
                if (uiBtnCancel.DisplayText == "Close")
                {
                    UpdateLineNumber(0);
                    ClosingAllEngines = true;
                    Close();
                    Dispose();
                    return;
                }

                if (IsNewTaskSteppedInto || IsHiddenTaskEngine)
                {
                    IsNewTaskResumed = false;
                    IsNewTaskCancelled = true;
                }

                uiBtnPause.Visible = false;
                uiBtnCancel.Visible = false;
                uiBtnStepInto.Visible = false;
                uiBtnStepOver.Visible = false;
                lblKillProcNote.Text = "Cancelling...";               
                EngineInstance.ResumeScript();

                SteppingCommandsItem commandsItem = new SteppingCommandsItem
                {
                    Text = "[User Requested Cancellation]",
                    Color = Color.Black
                };

                lstSteppingCommands.Items.Add(commandsItem);
                lstSteppingCommands.SelectedIndex = lstSteppingCommands.Items.Count - 1;
                lblMainLogo.Text = "debug info (cancelling)";
                EngineInstance.CancelScript();
            }          
        }

        public delegate void uiBtnPause_ClickDelegate(object sender, EventArgs e);           
        public void uiBtnPause_Click(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                var d = new uiBtnPause_ClickDelegate(uiBtnPause_Click);
                Invoke(d, new object[] { sender, e });
            }
            else
            {
                if (uiBtnPause.DisplayText == "Pause")
                {
                    SteppingCommandsItem commandsItem = new SteppingCommandsItem
                    {
                        Text = "[User Requested Pause]",
                        Color = Color.Red
                    };

                    lstSteppingCommands.Items.Add(commandsItem);
                    uiBtnPause.Image = Resources.engine_resume;
                    uiBtnPause.DisplayText = "Resume";
                    EngineInstance.PauseScript();
                }
                else if (uiBtnPause.DisplayText == "Resume")
                {
                    SteppingCommandsItem commandsItem = new SteppingCommandsItem
                    {
                        Text = "[User Requested Resume]",
                        Color = Color.Green
                    };

                    lstSteppingCommands.Items.Add(commandsItem);
                    uiBtnPause.Image = Resources.engine_pause;
                    uiBtnPause.DisplayText = "Pause";
                    uiBtnStepOver.Visible = false;
                    uiBtnStepInto.Visible = false;

                    if (EngineContext.ScriptBuilder != null)
                    {
                        EngineContext.ScriptBuilder.IsScriptSteppedOver = false;
                        EngineContext.ScriptBuilder.IsScriptSteppedInto = false;
                    }

                    if (IsNewTaskSteppedInto || !IsHiddenTaskEngine)
                        IsNewTaskResumed = true;

                    EngineInstance.ResumeScript();
                }

                lstSteppingCommands.SelectedIndex = lstSteppingCommands.Items.Count - 1;
            }   
        }

        public delegate void ResumeParentTaskDelegate();
        public void ResumeParentTask()
        {
            if (InvokeRequired)
            {
                var d = new ResumeParentTaskDelegate(ResumeParentTask);
                Invoke(d, new object[] { });
            }
            else
            {
                uiBtnPause.Image = Resources.engine_pause;
                uiBtnPause.DisplayText = "Pause";
                uiBtnStepOver.Visible = false;
                uiBtnStepInto.Visible = false;

                if (EngineContext.ScriptBuilder != null)
                {
                    EngineContext.ScriptBuilder.IsScriptSteppedOver = false;
                    EngineContext.ScriptBuilder.IsScriptSteppedInto = false;
                }

                if (IsNewTaskSteppedInto || !IsHiddenTaskEngine)
                    IsNewTaskResumed = true;

                EngineInstance.ResumeScript();
            }
        }

        public void uiBtnStepOver_Click(object sender, EventArgs e)
        {
            if (EngineContext.ScriptBuilder != null)
                EngineContext.ScriptBuilder.IsScriptSteppedOver = true;

            if (IsNewTaskSteppedInto)
                IsNewTaskResumed = false;

            EngineInstance.StepOverScript();
        }

        public void uiBtnStepInto_Click(object sender, EventArgs e)
        {
            if (EngineContext.ScriptBuilder != null)
                EngineContext.ScriptBuilder.IsScriptSteppedInto = true;

            if (IsNewTaskSteppedInto)
                IsNewTaskResumed = false;

            EngineInstance.StepIntoScript();
        }

        private void pbBotIcon_Click(object sender, EventArgs e)
        {
            //show debug if user clicks
            lblMainLogo.Show();
            lstSteppingCommands.Visible = !lstSteppingCommands.Visible;
        }

        private void lstSteppingCommands_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(((SteppingCommandsItem)lstSteppingCommands.SelectedItem).Text, "Item Status");
        }

        private void uiBtnScheduleManagement_Click(object sender, EventArgs e)
        {
            frmScheduleManagement scheduleManager = new frmScheduleManagement();
            scheduleManager.Show();
        }
        #endregion UI Elements       

        private void frmScriptEngine_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalHook.KeyDownEvent -= GlobalHook_KeyDownEvent;

            if (EngineContext.ScriptBuilder != null && !EngineContext.IsChildEngine)
                EngineContext.ScriptBuilder.IsScriptRunning = false;
        }
    }
}