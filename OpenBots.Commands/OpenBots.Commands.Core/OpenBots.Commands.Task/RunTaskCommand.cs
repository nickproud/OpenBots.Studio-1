using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
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
			if(parentAutomationEngineInstance.EngineContext.ScriptEngine == null)
			{
				RunServerTask(sender);
				return;
			}

			var childTaskPath = (string)await v_TaskPath.EvaluateCode(parentAutomationEngineInstance);
			if (!File.Exists(childTaskPath))
				throw new FileNotFoundException("Task file was not found");

			IfrmScriptEngine parentfrmScriptEngine = parentAutomationEngineInstance.EngineContext.ScriptEngine;
			string parentTaskPath = parentAutomationEngineInstance.EngineContext.ScriptEngine.EngineContext.FilePath;
			int parentDebugLine = parentAutomationEngineInstance.EngineContext.ScriptEngine.DebugLineNumber;

			//create argument list
			await InitializeArgumentLists(parentAutomationEngineInstance);

			string projectPath = parentfrmScriptEngine.EngineContext.ProjectPath;

			EngineContext childEngineContext = new EngineContext(childTaskPath, projectPath, parentAutomationEngineInstance.EngineContext.Container, CurrentScriptBuilder,
				parentfrmScriptEngine.EngineContext.EngineLogger, null, _argumentList, null, null, parentfrmScriptEngine.EngineContext.SessionVariables, null, 1,
				parentfrmScriptEngine.EngineContext.IsDebugMode, true);

			IfrmScriptEngine childfrmScriptEngine = parentfrmScriptEngine.CreateScriptEngineForm(childEngineContext, false);

			if (parentAutomationEngineInstance.EngineContext.IsScheduledOrAttendedTask)
				childfrmScriptEngine.EngineContext.IsScheduledOrAttendedTask = true;

			childfrmScriptEngine.IsHiddenTaskEngine = true;

			if (IsSteppedInto)
			{                
				childfrmScriptEngine.IsNewTaskSteppedInto = true;
				childfrmScriptEngine.IsHiddenTaskEngine = false;
			}

			if (CurrentScriptBuilder != null)
				CurrentScriptBuilder.EngineLogger.Information("Executing Child Task: " + Path.GetFileName(childTaskPath));
			else
				Log.Information("Executing Child Task: " + Path.GetFileName(childTaskPath));

			((Form)parentAutomationEngineInstance.EngineContext.ScriptEngine).Invoke((Action)delegate()
			{
				((Form)parentAutomationEngineInstance.EngineContext.ScriptEngine).TopMost = false;
			});
			Application.Run((Form)childfrmScriptEngine);
			
			if (childfrmScriptEngine.ClosingAllEngines)
			{
				parentAutomationEngineInstance.EngineContext.ScriptEngine.ClosingAllEngines = true;
				parentAutomationEngineInstance.EngineContext.ScriptEngine.CloseWhenDone = true;
			}

			// Update Current Engine Context Post Run Task
			UpdateCurrentEngineContext(parentAutomationEngineInstance, childfrmScriptEngine.EngineInstance);

			if (CurrentScriptBuilder != null)
				CurrentScriptBuilder.EngineLogger.Information("Resuming Parent Task: " + Path.GetFileName(parentTaskPath));
			else
				Log.Information("Resuming Parent Task: " + Path.GetFileName(parentTaskPath));

			if (parentfrmScriptEngine.EngineContext.IsDebugMode)
			{
				((Form)parentAutomationEngineInstance.EngineContext.ScriptEngine).Invoke((Action)delegate()
				{
					((Form)parentfrmScriptEngine).TopMost = true;
					parentfrmScriptEngine.IsHiddenTaskEngine = true;

					if ((IsSteppedInto || !childfrmScriptEngine.IsHiddenTaskEngine) && !childfrmScriptEngine.IsNewTaskResumed && !childfrmScriptEngine.IsNewTaskCancelled)
					{
						parentfrmScriptEngine.EngineContext.ScriptBuilder.CurrentEngine = parentfrmScriptEngine;
						parentfrmScriptEngine.EngineContext.ScriptBuilder.IsScriptSteppedInto = true;
						parentfrmScriptEngine.IsHiddenTaskEngine = false;

						//toggle running flag to allow for tab selection
						parentfrmScriptEngine.EngineContext.ScriptBuilder.IsScriptRunning = false;
						parentfrmScriptEngine.EngineContext.ScriptBuilder.OpenScriptFile(parentTaskPath, true);
						parentfrmScriptEngine.EngineContext.ScriptBuilder.IsScriptRunning = true;

						parentfrmScriptEngine.UpdateLineNumber(parentDebugLine + 1);
						parentfrmScriptEngine.AddStatus("Pausing Before Execution");
					}
					else if (childfrmScriptEngine.IsNewTaskResumed)
					{
						parentfrmScriptEngine.EngineContext.ScriptBuilder.CurrentEngine = parentfrmScriptEngine;
						parentfrmScriptEngine.IsNewTaskResumed = true;
						parentfrmScriptEngine.IsHiddenTaskEngine = true;
						parentfrmScriptEngine.EngineContext.ScriptBuilder.IsScriptSteppedInto = false;
						parentfrmScriptEngine.EngineContext.ScriptBuilder.IsScriptPaused = false;
						parentfrmScriptEngine.ResumeParentTask();
					}
					else if (childfrmScriptEngine.IsNewTaskCancelled)
						parentfrmScriptEngine.uiBtnCancel_Click(null, null);
					else //child task never stepped into
						parentfrmScriptEngine.IsHiddenTaskEngine = false;
				});
			}
			else
			{
				((Form)parentAutomationEngineInstance.EngineContext.ScriptEngine).Invoke((Action)delegate()
				{
					((Form)parentfrmScriptEngine).TopMost = true;

					if (childfrmScriptEngine.IsNewTaskCancelled)
						parentfrmScriptEngine.uiBtnCancel_Click(null, null);
				});
			}

			if (childfrmScriptEngine != null)
            {
				((Form)childfrmScriptEngine).Dispose();
				childfrmScriptEngine = null;
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
			_passParameters.CheckedChanged += async (sender, e) => await PassParametersCheckbox_CheckedChanged(sender, e, editor);
			commandControls.CreateDefaultToolTipFor("v_AssignArguments", this, _passParameters);
			RenderedControls.Add(_passParameters);

			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_ArgumentAssignments", this));
			_assignmentsGridViewHelper = commandControls.CreateDefaultDataGridViewFor("v_ArgumentAssignments", this);
			_assignmentsGridViewHelper.AllowUserToAddRows = false;
			_assignmentsGridViewHelper.AllowUserToDeleteRows = false;
			//refresh gridview
            _assignmentsGridViewHelper.MouseEnter += async (sender, e) => await PassParametersCheckbox_CheckedChanged(_passParameters, null, editor, true);

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

		private async Tasks.Task PassParametersCheckbox_CheckedChanged(object sender, EventArgs e, IfrmCommandEditor editor, bool isMouseEnter = false)
		{			
			var assignArgCheckBox = (CheckBox)sender;
			_assignmentsGridViewHelper.Visible = assignArgCheckBox.Checked;
			
			//load arguments if selected and file exists
			if (assignArgCheckBox.Checked)
			{
				string startFile = "";

				if (!isMouseEnter)
                {
					var engineContext = new EngineContext(editor.ScriptContext, editor.ProjectPath);

					foreach (var var in engineContext.Variables)
						await VariableMethods.InstantiateVariable(var.VariableName, (string)var.VariableValue, var.VariableType, engineContext);

					foreach (var arg in engineContext.Arguments)
						await VariableMethods.InstantiateVariable(arg.ArgumentName, (string)arg.ArgumentValue, arg.ArgumentType, engineContext);

					try
					{
						startFile = (string)await VariableMethods.EvaluateCode(v_TaskPath, engineContext);
					}
					catch (Exception)
					{
						return;
					}
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

					DataGridViewComboBoxCell returnComboBox = new DataGridViewComboBoxCell();
					returnComboBox.Items.Add("In");
					returnComboBox.Items.Add("Out");
					returnComboBox.Items.Add("InOut");
					_assignmentsGridViewHelper.Rows[i].Cells[3] = returnComboBox;					
				}

				if (_assignmentsGridViewHelper.Columns.Count > 0)
                {
					_assignmentsGridViewHelper.Columns[0].ReadOnly = true;
					_assignmentsGridViewHelper.Columns[1].ReadOnly = true;
					_assignmentsGridViewHelper.Columns[3].ReadOnly = true;
				}
			}
			else if (!assignArgCheckBox.Checked)
				v_ArgumentAssignments.Clear();
		}       

		private async void RunServerTask(object sender)
		{
			var parentAutomationEngineInstance = (IAutomationEngineInstance)sender;
			string childTaskPath = (string)await v_TaskPath.EvaluateCode(parentAutomationEngineInstance);
			string parentTaskPath = parentAutomationEngineInstance.EngineContext.FilePath;

			//create argument list
			await InitializeArgumentLists(parentAutomationEngineInstance);

			object engineLogger = Log.Logger;

			if(parentAutomationEngineInstance.EngineContext.IsTest == true)
				engineLogger = null;

			EngineContext childEngineContext = new EngineContext
			{
				IsChildEngine = true,
				FilePath = childTaskPath,
				ProjectPath = parentAutomationEngineInstance.EngineContext.ProjectPath,
				EngineLogger = (Logger)engineLogger,
				Container = parentAutomationEngineInstance.EngineContext.Container,
			};

			var childAutomationEngineInstance = parentAutomationEngineInstance.CreateAutomationEngineInstance(childEngineContext);

			childAutomationEngineInstance.EngineContext.IsTest = parentAutomationEngineInstance.EngineContext.IsTest;
			childAutomationEngineInstance.EngineContext.Arguments = _argumentList;
			childAutomationEngineInstance.EngineContext.IsServerChildExecution = true;

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
			parentAutomationEngineIntance.EngineContext.SessionVariables = childAutomationEngineInstance.EngineContext.SessionVariables;

			var parentVariableList = parentAutomationEngineIntance.EngineContext.Variables;
			var parentArgumentList = parentAutomationEngineIntance.EngineContext.Arguments;

			//get new argument list from the new task engine after it finishes running
			var childArgumentList = childAutomationEngineInstance.EngineContext.Arguments;

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

			//get errors from new engine (if any)
			var newEngineErrors = childAutomationEngineInstance.ErrorsOccured;

			if (newEngineErrors.Count > 0 && v_ErrorHandling != "Ignore Error")
			{
				parentAutomationEngineIntance.ChildScriptFailed = true;

				foreach (var error in newEngineErrors)
					parentAutomationEngineIntance.ErrorsOccured.Add(error);
			}
		}
	}
}
