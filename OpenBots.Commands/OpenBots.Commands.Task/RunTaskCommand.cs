using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Model.EngineModel;
using OpenBots.Core.Properties;
using OpenBots.Core.Script;
using OpenBots.Core.Utilities.CommonUtilities;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OpenBots.Commands.Task
{
	[Serializable]
	[Category("Task Commands")]
	[Description("This command executes a Task.")]
	public class RunTaskCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Task File Path")]
		[Description("Enter or select a valid path to the Task file.")]
		[SampleUsage(@"C:\temp\mytask.json || {vScriptPath} || {ProjectPath}\mytask.json")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		public string v_TaskPath { get; set; }

		[Required]
		[DisplayName("Assign Variables")]
		[Description("Select to assign variables to the Task.")]
		[SampleUsage("")]
		[Remarks("If selected, variables will be automatically generated from the Task's *Variable Manager*.")]
		public bool v_AssignVariables { get; set; }

		[Required]
		[DisplayName("Task Variables")]
		[Description("Enter a VariableValue for each input variable.")]
		[SampleUsage("Hello World || {vVariableValue}")]
		[Remarks("For inputs, set VariableReturn to *No*. For outputs, set VariableReturn to *Yes*. " +
				 "Failure to assign a VariableReturn value will result in an error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public DataTable v_VariableAssignments { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private CheckBox _passParameters;

		[JsonIgnore]
		[Browsable(false)]
		private DataGridView _assignmentsGridViewHelper;

		[JsonIgnore]
		[Browsable(false)]
		private List<ScriptVariable> _variableList;

		[JsonIgnore]
		[Browsable(false)]
		private List<ScriptVariable> _variableReturnList;

		[JsonIgnore]
		[Browsable(false)]
		private IfrmScriptEngine _childfrmScriptEngine;

		public RunTaskCommand()
		{
			CommandName = "RunTaskCommand";
			SelectionName = "Run Task";
			CommandEnabled = true;
			CommandIcon = Resources.command_start_process;

			v_TaskPath = "{ProjectPath}";

			v_VariableAssignments = new DataTable();
			v_VariableAssignments.Columns.Add("VariableName");
			v_VariableAssignments.Columns.Add("VariableValue");
			v_VariableAssignments.Columns.Add("VariableReturn");
			v_VariableAssignments.TableName = "RunTaskCommandInputParameters" + DateTime.Now.ToString("MMddyyhhmmss");

			_assignmentsGridViewHelper = new DataGridView();
			_assignmentsGridViewHelper.AllowUserToAddRows = false;
			_assignmentsGridViewHelper.AllowUserToDeleteRows = false;
			_assignmentsGridViewHelper.Size = new Size(400, 250);
			_assignmentsGridViewHelper.ColumnHeadersHeight = 30;
			_assignmentsGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			_assignmentsGridViewHelper.DataSource = v_VariableAssignments;
			_assignmentsGridViewHelper.Hide();
		}

		public override void RunCommand(object sender)
		{
			var parentAutomationEngineInstance = (IAutomationEngineInstance)sender;
			if(parentAutomationEngineInstance.AutomationEngineContext.ScriptEngine == null)
			{
				RunServerTask(sender);
				return;
			}

			var childTaskPath = v_TaskPath.ConvertUserVariableToString(parentAutomationEngineInstance);
			if (!File.Exists(childTaskPath))
				throw new FileNotFoundException("Task file was not found");

			IfrmScriptEngine parentfrmScriptEngine = parentAutomationEngineInstance.AutomationEngineContext.ScriptEngine;
			string parentTaskPath = parentAutomationEngineInstance.AutomationEngineContext.ScriptEngine.ScriptEngineContext.FilePath;
			int parentDebugLine = parentAutomationEngineInstance.AutomationEngineContext.ScriptEngine.DebugLineNumber;

			//create variable list
			InitializeVariableLists(parentAutomationEngineInstance);

			string projectPath = parentfrmScriptEngine.ScriptEngineContext.ProjectPath;

			EngineContext childEngineContext = new EngineContext(childTaskPath, projectPath, parentAutomationEngineInstance.AutomationEngineContext.Container, CurrentScriptBuilder,
				parentfrmScriptEngine.ScriptEngineContext.EngineLogger, _variableList, null, parentAutomationEngineInstance.AutomationEngineContext.AppInstances, null);

			_childfrmScriptEngine = parentfrmScriptEngine.CommandControls.CreateScriptEngineForm(childEngineContext, false, parentfrmScriptEngine.IsDebugMode);

			if (parentAutomationEngineInstance.AutomationEngineContext.ScriptEngine.IsScheduledOrAttendedTask)
				_childfrmScriptEngine.IsScheduledOrAttendedTask = true;

			_childfrmScriptEngine.IsChildEngine = true;
			_childfrmScriptEngine.IsHiddenTaskEngine = true;

			if (IsSteppedInto)
			{                
				_childfrmScriptEngine.IsNewTaskSteppedInto = true;
				_childfrmScriptEngine.IsHiddenTaskEngine = false;
			}

			if (CurrentScriptBuilder != null)
				CurrentScriptBuilder.EngineLogger.Information("Executing Child Task: " + Path.GetFileName(childTaskPath));
			else
				Log.Information("Executing Child Task: " + Path.GetFileName(childTaskPath));

			((Form)parentAutomationEngineInstance.AutomationEngineContext.ScriptEngine).Invoke((Action)delegate()
			{
				((Form)parentAutomationEngineInstance.AutomationEngineContext.ScriptEngine).TopMost = false;
			});
			Application.Run((Form)_childfrmScriptEngine);
			
			if (_childfrmScriptEngine.ClosingAllEngines)
			{
				parentAutomationEngineInstance.AutomationEngineContext.ScriptEngine.ClosingAllEngines = true;
				parentAutomationEngineInstance.AutomationEngineContext.ScriptEngine.CloseWhenDone = true;
			}

			// Update Current Engine Context Post Run Task
			_childfrmScriptEngine.UpdateCurrentEngineContext(parentAutomationEngineInstance, _childfrmScriptEngine, _variableReturnList);

			if (CurrentScriptBuilder != null)
				CurrentScriptBuilder.EngineLogger.Information("Resuming Parent Task: " + Path.GetFileName(parentTaskPath));
			else
				Log.Information("Resuming Parent Task: " + Path.GetFileName(parentTaskPath));

			if (parentfrmScriptEngine.IsDebugMode)
			{
				((Form)parentAutomationEngineInstance.AutomationEngineContext.ScriptEngine).Invoke((Action)delegate()
				{
					((Form)parentfrmScriptEngine).TopMost = true;
					parentfrmScriptEngine.IsHiddenTaskEngine = true;

					if ((IsSteppedInto || !_childfrmScriptEngine.IsHiddenTaskEngine) && !_childfrmScriptEngine.IsNewTaskResumed && !_childfrmScriptEngine.IsNewTaskCancelled)
					{
						parentfrmScriptEngine.ScriptEngineContext.ScriptBuilder.CurrentEngine = parentfrmScriptEngine;
						parentfrmScriptEngine.ScriptEngineContext.ScriptBuilder.IsScriptSteppedInto = true;
						parentfrmScriptEngine.IsHiddenTaskEngine = false;

						//toggle running flag to allow for tab selection
						parentfrmScriptEngine.ScriptEngineContext.ScriptBuilder.IsScriptRunning = false;
						parentfrmScriptEngine.ScriptEngineContext.ScriptBuilder.OpenScriptFile(parentTaskPath);
						parentfrmScriptEngine.ScriptEngineContext.ScriptBuilder.IsScriptRunning = true;

						parentfrmScriptEngine.UpdateLineNumber(parentDebugLine + 1);
						parentfrmScriptEngine.AddStatus("Pausing Before Execution");
					}
					else if (_childfrmScriptEngine.IsNewTaskResumed)
					{
						parentfrmScriptEngine.ScriptEngineContext.ScriptBuilder.CurrentEngine = parentfrmScriptEngine;
						parentfrmScriptEngine.IsNewTaskResumed = true;
						parentfrmScriptEngine.IsHiddenTaskEngine = true;
						parentfrmScriptEngine.ScriptEngineContext.ScriptBuilder.IsScriptSteppedInto = false;
						parentfrmScriptEngine.ScriptEngineContext.ScriptBuilder.IsScriptPaused = false;
						parentfrmScriptEngine.ResumeParentTask();
					}
					else if (_childfrmScriptEngine.IsNewTaskCancelled)
						parentfrmScriptEngine.uiBtnCancel_Click(null, null);
					else //child task never stepped into
						parentfrmScriptEngine.IsHiddenTaskEngine = false;
				});
			}
			else
			{
				((Form)parentAutomationEngineInstance.AutomationEngineContext.ScriptEngine).Invoke((Action)delegate()
				{
					((Form)parentfrmScriptEngine).TopMost = true;
				});
			}          
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			//create file path and helpers
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_TaskPath", this));
			var taskPathControl = commandControls.CreateDefaultInputFor("v_TaskPath", this);
			RenderedControls.AddRange(commandControls.CreateUIHelpersFor("v_TaskPath", this, new Control[] { taskPathControl }, editor));
			RenderedControls.Add(taskPathControl);
			taskPathControl.TextChanged += TaskPathControl_TextChanged;

			_passParameters = new CheckBox();
			_passParameters.AutoSize = true;
			_passParameters.Text = "Assign Variables";
			_passParameters.Font = new Font("Segoe UI Light", 12);
			_passParameters.ForeColor = Color.White;
			_passParameters.DataBindings.Add("Checked", this, "v_AssignVariables", false, DataSourceUpdateMode.OnPropertyChanged);
			_passParameters.CheckedChanged += (sender, e) => PassParametersCheckbox_CheckedChanged(sender, e, editor, commandControls);
			commandControls.CreateDefaultToolTipFor("v_AssignVariables", this, _passParameters);
			RenderedControls.Add(_passParameters);

			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_VariableAssignments", this));
			RenderedControls.AddRange(commandControls.CreateUIHelpersFor("v_VariableAssignments", this, new Control[] { _assignmentsGridViewHelper }, editor));
			RenderedControls.Add(_assignmentsGridViewHelper);

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Run '{v_TaskPath}']";
		}

		private void TaskPathControl_TextChanged(object sender, EventArgs e)
		{
			_passParameters.Checked = false;
		}

		private void PassParametersCheckbox_CheckedChanged(object sender, EventArgs e, IfrmCommandEditor editor, ICommandControls commandControls)
		{
			var currentScriptEngine = commandControls.CreateAutomationEngineInstance(null);
			currentScriptEngine.AutomationEngineContext.Variables.AddRange(editor.ScriptEngineContext.Variables);
			currentScriptEngine.AutomationEngineContext.Elements.AddRange(editor.ScriptEngineContext.Elements);

			var startFile = v_TaskPath;
			if (startFile.Contains("{ProjectPath}"))
				startFile = startFile.Replace("{ProjectPath}", editor.ScriptEngineContext.ProjectPath);

			startFile = startFile.ConvertUserVariableToString(currentScriptEngine);
			
			var Sender = (CheckBox)sender;

			_assignmentsGridViewHelper.Visible = Sender.Checked;

			//load variables if selected and file exists
			if (Sender.Checked && File.Exists(startFile))
			{
				_assignmentsGridViewHelper.DataSource = v_VariableAssignments;

				JObject scriptObject = JObject.Parse(File.ReadAllText(startFile));
				var variables = scriptObject["Variables"].ToObject<List<ScriptVariable>>();

				foreach (var variable in variables)
				{
					if (variable.VariableName == "ProjectPath")
						continue;

					DataRow[] foundVariables  = v_VariableAssignments.Select("VariableName = '" + "{" + variable.VariableName + "}" + "'");
					if (foundVariables.Length == 0)
						v_VariableAssignments.Rows.Add("{" + variable.VariableName + "}", variable.VariableValue, "No");                   
				}               

				for (int i = 0; i < _assignmentsGridViewHelper.Rows.Count; i++)
				{
					DataGridViewComboBoxCell returnComboBox = new DataGridViewComboBoxCell();
					returnComboBox.Items.Add("Yes");
					returnComboBox.Items.Add("No");
					_assignmentsGridViewHelper.Rows[i].Cells[2] = returnComboBox;
				}
			}
			else if (!Sender.Checked)
			{
				v_VariableAssignments.Clear();
			}
		}       

		private void RunServerTask(object sender)
		{
			var parentAutomationEngineInstance = (IAutomationEngineInstance)sender;
			string childTaskPath = v_TaskPath.ConvertUserVariableToString(parentAutomationEngineInstance);
			string parentTaskPath = parentAutomationEngineInstance.FileName;

			//create variable list
			InitializeVariableLists(parentAutomationEngineInstance);

			EngineContext childEngineContext = new EngineContext
			{
				FilePath = childTaskPath,
				ProjectPath = parentAutomationEngineInstance.GetProjectPath(),
				EngineLogger = (Logger)Log.Logger,
				Container = parentAutomationEngineInstance.AutomationEngineContext.Container,
			};

			var childAutomationEngineInstance = parentAutomationEngineInstance.CreateAutomationEngineInstance(childEngineContext);
			childAutomationEngineInstance.AutomationEngineContext.Variables = _variableList;
			childAutomationEngineInstance.AutomationEngineContext.AppInstances = parentAutomationEngineInstance.AutomationEngineContext.AppInstances;
			childAutomationEngineInstance.IsServerChildExecution = true;

			Log.Information("Executing Child Task: " + Path.GetFileName(childTaskPath));
			childAutomationEngineInstance.ExecuteScriptSync();

			UpdateCurrentEngineContext(parentAutomationEngineInstance, childAutomationEngineInstance);

			Log.Information("Resuming Parent Task: " + Path.GetFileName(parentTaskPath));
		}

		private void InitializeVariableLists(IAutomationEngineInstance parentAutomationEngineInstance)
		{
			_variableList = new List<ScriptVariable>();
			_variableReturnList = new List<ScriptVariable>();

			foreach (DataRow rw in v_VariableAssignments.Rows)
			{
				var variableName = (string)rw.ItemArray[0];
				object variableValue = null;

				if (((string)rw.ItemArray[1]).StartsWith("{") && ((string)rw.ItemArray[1]).EndsWith("}"))
					variableValue = ((string)rw.ItemArray[1]).ConvertUserVariableToObject(parentAutomationEngineInstance);

				if (variableValue is string || variableValue == null)
					variableValue = ((string)rw.ItemArray[1]).ConvertUserVariableToString(parentAutomationEngineInstance);

				var variableReturn = (string)rw.ItemArray[2];

				_variableList.Add(new ScriptVariable
				{
					VariableName = variableName.Replace("{", "").Replace("}", ""),
					VariableValue = variableValue
				});

				if (variableReturn == "Yes")
				{
					_variableReturnList.Add(new ScriptVariable
					{
						VariableName = variableName.Replace("{", "").Replace("}", ""),
						VariableValue = variableValue
					});
				}
			}
		}

		private void UpdateCurrentEngineContext(IAutomationEngineInstance parentAutomationEngineIntance, IAutomationEngineInstance childAutomationEngineInstance)
		{
			//get new variable list from the new task engine after it finishes running
			var childVariableList = childAutomationEngineInstance.AutomationEngineContext.Variables;
			foreach (var variable in _variableReturnList)
			{
				//check if the variables we wish to return are in the new variable list
				if (childVariableList.Exists(x => x.VariableName == variable.VariableName))
				{
					//if yes, get that variable from the new list
					ScriptVariable newTemp = childVariableList.Where(x => x.VariableName == variable.VariableName).FirstOrDefault();
					//check if that variable previously existed in the current engine
					if (parentAutomationEngineIntance.AutomationEngineContext.Variables.Exists(x => x.VariableName == newTemp.VariableName))
					{
						//if yes, overwrite it
						ScriptVariable currentTemp = parentAutomationEngineIntance.AutomationEngineContext.Variables.Where(x => x.VariableName == newTemp.VariableName).FirstOrDefault();
						parentAutomationEngineIntance.AutomationEngineContext.Variables.Remove(currentTemp);
					}
					//Add to current engine variable list
					parentAutomationEngineIntance.AutomationEngineContext.Variables.Add(newTemp);
				}
			}

			//get updated app instance dictionary after the new engine finishes running
			parentAutomationEngineIntance.AutomationEngineContext.AppInstances = childAutomationEngineInstance.AutomationEngineContext.AppInstances;

			//get errors from new engine (if any)
			var newEngineErrors = childAutomationEngineInstance.ErrorsOccured;
			if (newEngineErrors.Count > 0)
			{
				parentAutomationEngineIntance.ChildScriptFailed = true;
				foreach (var error in newEngineErrors)
				{
					parentAutomationEngineIntance.ErrorsOccured.Add(error);
				}
			}
		}
	}
}
