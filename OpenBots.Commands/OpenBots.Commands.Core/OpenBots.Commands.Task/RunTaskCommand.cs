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
using Tasks = System.Threading.Tasks;

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
		[SampleUsage("@\"C:\\temp\\myfile.obscript\" || ProjectPath + @\"\\myfile.obscript\" || vFilePath")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_TaskPath { get; set; }

		[Required]
		[DisplayName("Assign Arguments")]
		[Description("Select to assign arguments to the Task.")]
		[SampleUsage("")]
		[Remarks("If selected, arguments will be automatically generated from the Task's *Argument Manager*.")]
		public bool v_AssignArguments { get; set; }

		[DisplayName("Task Arguments (Optional)")]
		[Description("Enter an ArgumentValue for each input argument.")]
		[SampleUsage("\"test\" || vMyVar || new List<string>() { \"Hello\", \"World\" }")]
		[Remarks("For inputs, set ArgumentDirection to *In*. For outputs, set ArgumentDirection to *Out*. " +
				 "Failure to assign an ArgumentDirection value will result in an error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(object) })]
		public DataTable v_ArgumentAssignments { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private CheckBox _passParameters;

		[JsonIgnore]
		[Browsable(false)]
		private DataGridView _assignmentsGridViewHelper;

		[JsonIgnore]
		[Browsable(false)]
		private List<ScriptArgument> _argumentList;
		 
		[JsonIgnore]
		[Browsable(false)]
		private IfrmScriptEngine _childfrmScriptEngine;

		public RunTaskCommand()
		{
			CommandName = "RunTaskCommand";
			SelectionName = "Run Task";
			CommandEnabled = true;
			CommandIcon = Resources.command_start_process;

			v_TaskPath = "ProjectPath";

			v_ArgumentAssignments = new DataTable();
			v_ArgumentAssignments.Columns.Add("ArgumentName");
			v_ArgumentAssignments.Columns.Add("ArgumentType");
			v_ArgumentAssignments.Columns.Add("ArgumentValue");
			v_ArgumentAssignments.Columns.Add("ArgumentDirection");
			v_ArgumentAssignments.TableName = "RunTaskCommandInputParameters" + DateTime.Now.ToString("MMddyyhhmmss");
			v_ArgumentAssignments.Columns[1].DataType = typeof(Type);			
		}

		public async override Tasks.Task RunCommand(object sender)
		{
			var parentAutomationEngineInstance = (IAutomationEngineInstance)sender;
			if(parentAutomationEngineInstance.AutomationEngineContext.ScriptEngine == null)
			{
				RunServerTask(sender);
				return;
			}

			var childTaskPath = (string)await v_TaskPath.EvaluateCode(parentAutomationEngineInstance);
			if (!File.Exists(childTaskPath))
				throw new FileNotFoundException("Task file was not found");

			IfrmScriptEngine parentfrmScriptEngine = parentAutomationEngineInstance.AutomationEngineContext.ScriptEngine;
			string parentTaskPath = parentAutomationEngineInstance.AutomationEngineContext.ScriptEngine.ScriptEngineContext.FilePath;
			int parentDebugLine = parentAutomationEngineInstance.AutomationEngineContext.ScriptEngine.DebugLineNumber;

			//create argument list
			await InitializeArgumentLists(parentAutomationEngineInstance);

			string projectPath = parentfrmScriptEngine.ScriptEngineContext.ProjectPath;

			EngineContext childEngineContext = new EngineContext(childTaskPath, projectPath, parentAutomationEngineInstance.AutomationEngineContext.Container, CurrentScriptBuilder,
				parentfrmScriptEngine.ScriptEngineContext.EngineLogger, null, _argumentList, null, parentAutomationEngineInstance.AutomationEngineContext.AppInstances, null, null, 1,
				parentfrmScriptEngine.ScriptEngineContext.IsDebugMode, true);

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
			_childfrmScriptEngine.UpdateCurrentEngineContext(parentAutomationEngineInstance, _childfrmScriptEngine, _argumentList);

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
						parentfrmScriptEngine.ScriptEngineContext.ScriptBuilder.OpenScriptFile(parentTaskPath, true);
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

					if (_childfrmScriptEngine.IsNewTaskCancelled)
						parentfrmScriptEngine.uiBtnCancel_Click(null, null);
				});
			}

			if (_childfrmScriptEngine != null)
            {
				((Form)_childfrmScriptEngine).Dispose();
				_childfrmScriptEngine = null;
				GC.Collect();
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
			_passParameters.Text = "Assign Arguments";
			_passParameters.Font = new Font("Segoe UI Light", 12);
			_passParameters.ForeColor = Color.White;
			_passParameters.DataBindings.Add("Checked", this, "v_AssignArguments", false, DataSourceUpdateMode.OnPropertyChanged);
			_passParameters.CheckedChanged += (sender, e) => PassParametersCheckbox_CheckedChanged(sender, e, editor, commandControls);
			commandControls.CreateDefaultToolTipFor("v_AssignArguments", this, _passParameters);
			RenderedControls.Add(_passParameters);

			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_ArgumentAssignments", this));
			_assignmentsGridViewHelper = commandControls.CreateDefaultDataGridViewFor("v_ArgumentAssignments", this);
			_assignmentsGridViewHelper.AllowUserToAddRows = false;
			_assignmentsGridViewHelper.AllowUserToDeleteRows = false;
			//refresh gridview
            _assignmentsGridViewHelper.MouseEnter += (sender, e) => PassParametersCheckbox_CheckedChanged(_passParameters, null, editor, commandControls, true);

			if (!_passParameters.Checked)
				_assignmentsGridViewHelper.Hide();

			RenderedControls.AddRange(commandControls.CreateUIHelpersFor("v_ArgumentAssignments", this, new Control[] { _assignmentsGridViewHelper }, editor));
			RenderedControls.Add(_assignmentsGridViewHelper);

			return RenderedControls;
		}

        public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Run '{v_TaskPath}']";
		}

		private void TaskPathControl_TextChanged(object sender, EventArgs e)
		{
			v_TaskPath = ((TextBox)sender).Text;
			_passParameters.Checked = false;
		}

		private async void PassParametersCheckbox_CheckedChanged(object sender, EventArgs e, IfrmCommandEditor editor, ICommandControls commandControls, bool isMouseEnter = false)
		{			
			var assignArgCheckBox = (CheckBox)sender;
			_assignmentsGridViewHelper.Visible = assignArgCheckBox.Checked;

			//load arguments if selected and file exists
			if (assignArgCheckBox.Checked)
			{
				var currentScriptEngine = commandControls.CreateAutomationEngineInstance(editor.ScriptEngineContext);

				currentScriptEngine.AutomationEngineContext.Variables.Where(x => x.VariableName == "ProjectPath").FirstOrDefault().VariableValue = "@\"" + editor.ScriptEngineContext.ProjectPath + '"';
				foreach (var var in currentScriptEngine.AutomationEngineContext.Variables)
					await VariableMethods.InstantiateVariable(var.VariableName, (string)var.VariableValue, var.VariableType, currentScriptEngine);

				foreach (var arg in currentScriptEngine.AutomationEngineContext.Arguments)
					await VariableMethods.InstantiateVariable(arg.ArgumentName, (string)arg.ArgumentValue, arg.ArgumentType, currentScriptEngine);

				string startFile = "";

				try
                {
					startFile = (string)await v_TaskPath.EvaluateCode(currentScriptEngine);
				}
                catch (Exception)
                {
					return;
                }

				if (!isMouseEnter && File.Exists(startFile))
                {
					_assignmentsGridViewHelper.DataSource = v_ArgumentAssignments;
					DataTable vArgumentAssignmentsCopy = v_ArgumentAssignments.Copy();
					v_ArgumentAssignments.Clear();

					JObject scriptObject = JObject.Parse(File.ReadAllText(startFile));
					var arguments = scriptObject["Arguments"].ToObject<List<ScriptArgument>>();

					foreach (var argument in arguments)
					{
						if (argument.ArgumentName == "ProjectPath")
							continue;

						DataRow foundArguments = vArgumentAssignmentsCopy.Select("ArgumentName = '" + argument.ArgumentName + "'").FirstOrDefault();
						if (foundArguments != null)
						{
							var foundArgumentValue = foundArguments[2];
							v_ArgumentAssignments.Rows.Add(argument.ArgumentName, argument.ArgumentType, foundArgumentValue, argument.Direction.ToString());
						}
						else
							v_ArgumentAssignments.Rows.Add(argument.ArgumentName, argument.ArgumentType, argument.ArgumentValue, argument.Direction.ToString());
					}
				}
				
                for (int i = 0; i < _assignmentsGridViewHelper.Rows.Count; i++)
				{
					DataGridViewComboBoxCell typeComboBox = new DataGridViewComboBoxCell();
					typeComboBox.Items.Add(v_ArgumentAssignments.Rows[i].ItemArray[1]);
					typeComboBox.Tag = v_ArgumentAssignments.Rows[i].ItemArray[1];
					_assignmentsGridViewHelper.Rows[i].Cells[1] = typeComboBox;
					_assignmentsGridViewHelper.Rows[i].Cells[1].ReadOnly = true;

					DataGridViewComboBoxCell returnComboBox = new DataGridViewComboBoxCell();
					returnComboBox.Items.Add("In");
					returnComboBox.Items.Add("Out");
					returnComboBox.Items.Add("InOut");
					_assignmentsGridViewHelper.Rows[i].Cells[3] = returnComboBox;
					_assignmentsGridViewHelper.Rows[i].Cells[3].ReadOnly = true;					
				}
			}
			else if (!assignArgCheckBox.Checked)
			{
				v_ArgumentAssignments.Clear();
			}
		}       

		private async void RunServerTask(object sender)
		{
			var parentAutomationEngineInstance = (IAutomationEngineInstance)sender;
			string childTaskPath = (string)await v_TaskPath.EvaluateCode(parentAutomationEngineInstance);
			string parentTaskPath = parentAutomationEngineInstance.FileName;

			//create argument list
			await InitializeArgumentLists(parentAutomationEngineInstance);

			object engineLogger = Log.Logger;

			if(parentAutomationEngineInstance.AutomationEngineContext.IsTest == true)
				engineLogger = null;

			EngineContext childEngineContext = new EngineContext
			{
				FilePath = childTaskPath,
				ProjectPath = parentAutomationEngineInstance.GetProjectPath(),
				EngineLogger = (Logger)engineLogger,
				Container = parentAutomationEngineInstance.AutomationEngineContext.Container,
			};

			var childAutomationEngineInstance = parentAutomationEngineInstance.CreateAutomationEngineInstance(childEngineContext);

			childAutomationEngineInstance.AutomationEngineContext.IsTest = parentAutomationEngineInstance.AutomationEngineContext.IsTest;
			childAutomationEngineInstance.AutomationEngineContext.Arguments = _argumentList;
			childAutomationEngineInstance.AutomationEngineContext.AppInstances = parentAutomationEngineInstance.AutomationEngineContext.AppInstances;
			childAutomationEngineInstance.IsServerChildExecution = true;

			Log.Information("Executing Child Task: " + Path.GetFileName(childTaskPath));
			childAutomationEngineInstance.ExecuteScriptSync();

			UpdateCurrentEngineContext(parentAutomationEngineInstance, childAutomationEngineInstance);

			Log.Information("Resuming Parent Task: " + Path.GetFileName(parentTaskPath));
		}

		private async Tasks.Task InitializeArgumentLists(IAutomationEngineInstance parentAutomationEngineInstance)
		{
			_argumentList = new List<ScriptArgument>();

			foreach (DataRow rw in v_ArgumentAssignments.Rows)
			{
				var argumentName = (string)rw.ItemArray[0];
				var argumentType = (Type)rw.ItemArray[1];
				object argumentValue = null;
				var argumentDirection = (string)rw.ItemArray[3];

				if (argumentDirection == "In" || argumentDirection == "InOut")
                {
					argumentValue = await ((string)rw.ItemArray[2]).EvaluateCode(parentAutomationEngineInstance);

					_argumentList.Add(new ScriptArgument
					{
						ArgumentName = argumentName,
						Direction = (ScriptArgumentDirection)Enum.Parse(typeof(ScriptArgumentDirection), argumentDirection), 
						ArgumentValue = argumentValue,
						ArgumentType = argumentType
					});
				}
					
                if (argumentDirection == "Out" || argumentDirection == "InOut")
                {
					//verify whether the assigned variable/argument exists
					await ((string)rw.ItemArray[2]).EvaluateCode(parentAutomationEngineInstance);

					var existingArg = _argumentList.Where(x => x.ArgumentName == argumentName).FirstOrDefault();
					if (existingArg != null)
						existingArg.AssignedVariable = (string)rw.ItemArray[2];
                    else
                    {
						_argumentList.Add(new ScriptArgument
						{
							ArgumentName = argumentName,
							Direction = (ScriptArgumentDirection)Enum.Parse(typeof(ScriptArgumentDirection), argumentDirection),
							AssignedVariable = (string)rw.ItemArray[2],
							ArgumentType = argumentType
						});
					}					
                }
            }
		}

		private void UpdateCurrentEngineContext(IAutomationEngineInstance parentAutomationEngineIntance, IAutomationEngineInstance childAutomationEngineInstance)
		{
			var parentVariableList = parentAutomationEngineIntance.AutomationEngineContext.Variables;
			var parentArgumentList = parentAutomationEngineIntance.AutomationEngineContext.Arguments;

			//get new argument list from the new task engine after it finishes running
			var childArgumentList = childAutomationEngineInstance.AutomationEngineContext.Arguments;
			foreach(var argument in _argumentList)
            {
				if ((argument.Direction == ScriptArgumentDirection.Out || argument.Direction == ScriptArgumentDirection.InOut) 
					&& !string.IsNullOrEmpty(argument.AssignedVariable))
                {
					var assignedParentVariable = parentVariableList.Where(v => v.VariableName == argument.AssignedVariable).FirstOrDefault();
					var assignedParentArgument = parentArgumentList.Where(a => a.ArgumentName == argument.AssignedVariable).FirstOrDefault();
					if (assignedParentVariable != null)
					{
						var newVarValue = childArgumentList.Where(a => a.ArgumentName == argument.ArgumentName).First().ArgumentValue;
						newVarValue.SetVariableValue(parentAutomationEngineIntance, assignedParentVariable.VariableName);
					}
					else if (assignedParentArgument != null)
					{
						var newArgValue = childArgumentList.Where(a => a.ArgumentName == argument.ArgumentName).First().ArgumentValue;
						newArgValue.SetVariableValue(parentAutomationEngineIntance, assignedParentArgument.ArgumentName);
					}
					else
                    {
						throw new ArgumentException($"Unable to assign the value of '{argument.ArgumentName}' to '{argument.AssignedVariable}' " +
													 "because no variable/argument with this name exists.");
                    }
                }
            }

			//get updated app instance dictionary after the new engine finishes running
			parentAutomationEngineIntance.AutomationEngineContext.AppInstances = childAutomationEngineInstance.AutomationEngineContext.AppInstances;

			//get errors from new engine (if any)
			var newEngineErrors = childAutomationEngineInstance.ErrorsOccured;
			if (newEngineErrors.Count > 0 && v_ErrorHandling != "Ignore Error")
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
