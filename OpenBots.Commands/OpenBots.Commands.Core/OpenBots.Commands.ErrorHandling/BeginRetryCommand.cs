using Newtonsoft.Json;
using OpenBots.Commands.If;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Script;
using OpenBots.Core.UI.Controls;
using OpenBots.Core.Utilities.CommandUtilities;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace OpenBots.Commands.ErrorHandling
{
    [Serializable]
	[Category("Error Handling Commands")]
	[Description("This command defines a retry block which will retry the contained commands as long as the condition is not met or " +
		"an error is thrown.")]
	public class BeginRetryCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Number of Retries")]
		[Description("Enter or provide the number of retries.")]
		[SampleUsage("3 || {vRetryCount}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_RetryCount { get; set; }

		[Required]
		[DisplayName("Retry Interval")]
		[Description("Enter or provide the amount of time (in seconds) between each retry.")]
		[SampleUsage("5 || {vRetryInterval}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_RetryInterval { get; set; }

		[Required]
		[DisplayName("Logic Type")]
		[PropertyUISelectionOption("And")]
		[PropertyUISelectionOption("Or")]
		[Description("Select the logic to use when evaluating multiple Ifs.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_LogicType { get; set; }

		[Required]
		[DisplayName("Condition")]
		[Description("Add a condition.")]
		[SampleUsage("")]
		[Remarks("Items in the retry scope will be executed if the condition doesn't satisfy.")]
		[Editor("ShowIfBuilder", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(object), typeof(Bitmap), typeof(DateTime), typeof(string) }, true)]
		public DataTable v_IfConditionsTable { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private DataGridView _ifConditionHelper;

		[JsonIgnore]
		[Browsable(false)]
		private Exception _exception;

		public BeginRetryCommand()
		{
			CommandName = "BeginRetryCommand";
			SelectionName = "Begin Retry";
			CommandEnabled = true;
			CommandIcon = Resources.command_try;

			v_LogicType = "And";
			v_IfConditionsTable = new DataTable();
			v_IfConditionsTable.TableName = DateTime.Now.ToString("MultiIfConditionTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
			v_IfConditionsTable.Columns.Add("Statement");
			v_IfConditionsTable.Columns.Add("CommandData");
		}

		public override void RunCommand(object sender, ScriptAction parentCommand)
		{
			//get engine
			var engine = (IAutomationEngineInstance)sender;
			var retryCommand = (BeginRetryCommand)parentCommand.ScriptCommand;

			int retryCount = int.Parse(retryCommand.v_RetryCount.ConvertUserVariableToString(engine));
			int retryInterval = int.Parse(retryCommand.v_RetryInterval.ConvertUserVariableToString(engine))*1000;
			bool exceptionOccurred;

			for(int startIndex = 0; startIndex < retryCount; startIndex++)
			{
				//reset error flags
				engine.ChildScriptFailed = false;
				engine.ChildScriptErrorCaught = false;

				exceptionOccurred = false;
				engine.ReportProgress($"Begin Retry [{startIndex + 1}/{retryCount}]");

				foreach (var cmd in parentCommand.AdditionalScriptCommands)
				{
					try
					{
						cmd.IsExceptionIgnored = true;						
						engine.ExecuteCommand(cmd);
						if (cmd.ScriptCommand.CommandName == "RunTaskCommand" && engine.ChildScriptFailed && !engine.ChildScriptErrorCaught)
							throw new Exception("Child Script Failed");
					}
					catch (Exception ex)
					{
						_exception = ex;
						exceptionOccurred = true;
						break;
					}
				}
				// If no exception is thrown out and the Condition's satisfied				
				if (!exceptionOccurred && GetConditionResult(engine))
				{
					engine.ErrorsOccured.Clear();
					retryCount = 0;					
				}
				else if((startIndex + 1) < retryCount)
				{
					engine.ErrorsOccured.Clear();
					Thread.Sleep(retryInterval);
				}
                else
                {
					if(!(engine.LastExecutedCommand.CommandName == "RethrowCommand"))
						engine.ErrorsOccured.Clear();	
					
					if (_exception != null)
						throw new Exception("Number of retries has expired! - " + _exception.Message);
					else
						throw new Exception("Number of retries has expired!");
				}
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_RetryCount", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_RetryInterval", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_LogicType", this, editor));

			//create controls
			var controls = commandControls.CreateDefaultDataGridViewGroupFor("v_IfConditionsTable", this, editor);
			_ifConditionHelper = controls[2] as DataGridView;

			//handle helper click
			var helper = controls[1] as CommandItemControl;
			helper.Click += (sender, e) => CreateIfCondition(sender, e, editor, commandControls);

			//add for rendering
			RenderedControls.AddRange(controls);

			//define if condition helper
			_ifConditionHelper.Width = 450;
			_ifConditionHelper.Height = 200;
			_ifConditionHelper.AutoGenerateColumns = false;
			_ifConditionHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			_ifConditionHelper.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Condition", DataPropertyName = "Statement", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
			_ifConditionHelper.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "CommandData", DataPropertyName = "CommandData", ReadOnly = true, Visible = false });
			_ifConditionHelper.Columns.Add(new DataGridViewButtonColumn() { HeaderText = "Edit", UseColumnTextForButtonValue = true, Text = "Edit", Width = 45 });
			_ifConditionHelper.Columns.Add(new DataGridViewButtonColumn() { HeaderText = "Delete", UseColumnTextForButtonValue = true, Text = "Delete", Width = 60 });
			_ifConditionHelper.AllowUserToAddRows = false;
			_ifConditionHelper.AllowUserToDeleteRows = true;
			_ifConditionHelper.CellContentClick += (sender, e) => IfConditionHelper_CellContentClick(sender, e, editor, commandControls);

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Number of Retries '{v_RetryCount}' - Retry Interval '{v_RetryInterval}']";
		}

		private void IfConditionHelper_CellContentClick(object sender, DataGridViewCellEventArgs e, IfrmCommandEditor parentEditor, ICommandControls commandControls)
		{
			var senderGrid = (DataGridView)sender;

			if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
			{
				var buttonSelected = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewButtonCell;
				var selectedRow = v_IfConditionsTable.Rows[e.RowIndex];

				if (buttonSelected.Value.ToString() == "Edit")
				{
					//launch editor
					var statement = selectedRow["Statement"];
					var commandData = selectedRow["CommandData"].ToString();

					var ifCommand = commandControls.CreateBeginIfCommand(commandData);

					var automationCommands = new List<AutomationCommand>() { CommandsHelper.ConvertToAutomationCommand(ifCommand.GetType()) };
					IfrmCommandEditor editor = commandControls.CreateCommandEditorForm(automationCommands, null);
					editor.SelectedCommand = ifCommand;
					editor.EditingCommand = ifCommand;
					editor.OriginalCommand = ifCommand;
					editor.CreationModeInstance = CreationMode.Edit;
					editor.ScriptEngineContext = parentEditor.ScriptEngineContext;
					editor.TypeContext = parentEditor.TypeContext;

					if (((Form)editor).ShowDialog() == DialogResult.OK)
					{
						parentEditor.ScriptEngineContext = editor.ScriptEngineContext;
						parentEditor.TypeContext = editor.TypeContext;

						var editedCommand = editor.SelectedCommand as BeginIfCommand;
						var displayText = editedCommand.GetDisplayValue();
						var serializedData = JsonConvert.SerializeObject(editedCommand);

						selectedRow["Statement"] = displayText;
						selectedRow["CommandData"] = serializedData;
					}
				}
				else if (buttonSelected.Value.ToString() == "Delete")
				{
					//delete
					v_IfConditionsTable.Rows.Remove(selectedRow);
				}
				else
				{
					throw new NotImplementedException("Requested Action is not implemented.");
				}
			}
		}

		private void CreateIfCondition(object sender, EventArgs e, IfrmCommandEditor parentEditor, ICommandControls commandControls)
		{
			var automationCommands = new List<AutomationCommand>() { CommandsHelper.ConvertToAutomationCommand(commandControls.GetCommandType("BeginIfCommand")) };
            IfrmCommandEditor editor = commandControls.CreateCommandEditorForm(automationCommands, null);
			editor.SelectedCommand = commandControls.CreateBeginIfCommand();
			editor.ScriptEngineContext = parentEditor.ScriptEngineContext;
			editor.TypeContext = parentEditor.TypeContext;

			if (((Form)editor).ShowDialog() == DialogResult.OK)
			{
				//get data
				var configuredCommand = editor.SelectedCommand;
				var displayText = configuredCommand.GetDisplayValue();
				var serializedData = JsonConvert.SerializeObject(configuredCommand);
				parentEditor.ScriptEngineContext = editor.ScriptEngineContext;
				parentEditor.TypeContext = editor.TypeContext;

				//add to list
				v_IfConditionsTable.Rows.Add(displayText, serializedData);
			}
		}

		private bool GetConditionResult(IAutomationEngineInstance engine)
		{
			bool isTrueStatement = true;
			foreach (DataRow rw in v_IfConditionsTable.Rows)
			{
				var commandData = rw["CommandData"].ToString();
				var ifCommand = JsonConvert.DeserializeObject<BeginIfCommand>(commandData);
				var statementResult = CommandsHelper.DetermineStatementTruth(engine, ifCommand.v_IfActionType, ifCommand.v_ActionParameterTable);

				if (!statementResult && v_LogicType == "And")
				{
					isTrueStatement = false;
					break;
				}

				if(statementResult && v_LogicType == "Or")
                {
					isTrueStatement = true;
					break;
                }
				else if (v_LogicType == "Or")
				{
					isTrueStatement = false;
				}
			}
			return isTrueStatement;
		}
	}
}
