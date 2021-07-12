using Newtonsoft.Json;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
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
using System.Linq;
using System.Windows.Forms;
using Tasks = System.Threading.Tasks;

namespace OpenBots.Commands.Loop
{
	[Serializable]
	[Category("Loop Commands")]
	[Description("This command evaluates a group of specified logical statements and executes the contained commands repeatedly (in loop) " +
		"until the result of the logical statements becomes false.")]
	public class BeginMultiWhileCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Logic Type")]
		[PropertyUISelectionOption("And")]
		[PropertyUISelectionOption("Or")]
		[Description("Select the logic to use when evaluating multiple Ifs.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_LogicType { get; set; }

		[Required]
		[DisplayName("Multiple While Conditions")]
		[Description("Add new While condition(s).")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowLoopBuilder", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(Bitmap), typeof(DateTime), typeof(string), typeof(double), typeof(int), typeof(bool) })]
		public DataTable v_WhileConditionsTable { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private DataGridView _whileConditionHelper;

		public BeginMultiWhileCommand()
		{
			CommandName = "BeginMultiWhileCommand";
			SelectionName = "Multi While";
			CommandEnabled = true;
			CommandIcon = Resources.command_startloop;
			ScopeStartCommand = true;

			v_LogicType = "And";
			v_WhileConditionsTable = new DataTable();
			v_WhileConditionsTable.TableName = DateTime.Now.ToString("MultiWhileConditionTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
			v_WhileConditionsTable.Columns.Add("Statement");
			v_WhileConditionsTable.Columns.Add("CommandData");
		}

		public async override Tasks.Task RunCommand(object sender, ScriptAction parentCommand)
		{
			var engine = (IAutomationEngineInstance)sender;
			bool isTrueStatement = await DetermineMultiStatementTruth(engine);
			engine.ReportProgress("Starting While");

			while (isTrueStatement)
			{
				foreach (var cmd in parentCommand.AdditionalScriptCommands)
				{
					if (engine.IsCancellationPending)
						return;

					await engine.ExecuteCommand(cmd);

					if (engine.CurrentLoopCancelled)
					{
						engine.ReportProgress("Exiting While");
						engine.CurrentLoopCancelled = false;
						return;
					}

					if (engine.CurrentLoopContinuing)
					{
						engine.ReportProgress("Continuing Next While");
						engine.CurrentLoopContinuing = false;
						break;
					}
				}
				isTrueStatement = await DetermineMultiStatementTruth(engine);
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_LogicType", this, editor));

			//create controls
			var controls = commandControls.CreateDefaultDataGridViewGroupFor("v_WhileConditionsTable", this, editor);
			_whileConditionHelper = controls[2] as DataGridView;

			//handle helper click
			var helper = controls[1] as CommandItemControl;
			helper.Click += (sender, e) => CreateWhileCondition(sender, e, editor, commandControls);

			//add for rendering
			RenderedControls.AddRange(controls);

			//define if condition helper
			_whileConditionHelper.Width = 450;
			_whileConditionHelper.Height = 200;
			_whileConditionHelper.AutoGenerateColumns = false;
			_whileConditionHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			_whileConditionHelper.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Condition", DataPropertyName = "Statement", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
			_whileConditionHelper.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "CommandData", DataPropertyName = "CommandData", ReadOnly = true, Visible = false });
			_whileConditionHelper.Columns.Add(new DataGridViewButtonColumn() { HeaderText = "Edit", UseColumnTextForButtonValue = true, Text = "Edit", Width = 45 });
			_whileConditionHelper.Columns.Add(new DataGridViewButtonColumn() { HeaderText = "Delete", UseColumnTextForButtonValue = true, Text = "Delete", Width = 60 });
			_whileConditionHelper.AllowUserToAddRows = false;
			_whileConditionHelper.AllowUserToDeleteRows = true;
			_whileConditionHelper.CellContentClick += (sender, e) => WhileConditionHelper_CellContentClick(sender, e, editor, commandControls);

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			if (v_WhileConditionsTable.Rows.Count == 0)
				return "While <Not Configured>";
			else if (v_LogicType == "And")
			{
				var statements = v_WhileConditionsTable.AsEnumerable().Select(f => f.Field<string>("Statement")).ToList();
				return string.Join(" && ", statements);
			}
			else
			{
				var statements = v_WhileConditionsTable.AsEnumerable().Select(f => f.Field<string>("Statement")).ToList();
				return string.Join(" || ", statements);
			}
		}

		private async Tasks.Task<bool> DetermineMultiStatementTruth(IAutomationEngineInstance engine)
		{
			bool isTrueStatement = true;
			foreach (DataRow rw in v_WhileConditionsTable.Rows)
			{
				var commandData = rw["CommandData"].ToString();
				var whileCommand = JsonConvert.DeserializeObject<BeginWhileCommand>(commandData);
				bool statementResult;

				if (whileCommand.v_Option == "Builder")
					statementResult = await CommandsHelper.DetermineStatementTruth(engine, whileCommand.v_ActionType, whileCommand.v_ActionParameterTable);
				else
					statementResult = (bool)await whileCommand.v_Condition.EvaluateCode(engine);

				if (!statementResult && v_LogicType == "And")
				{
					isTrueStatement = false;
					break;
				}

				if (statementResult && v_LogicType == "Or")
				{
					isTrueStatement = true;
					break;
				}
				else if (v_LogicType == "Or")
					isTrueStatement = false;
			}
			return isTrueStatement;
		}

		private void WhileConditionHelper_CellContentClick(object sender, DataGridViewCellEventArgs e, IfrmCommandEditor parentEditor, ICommandControls commandControls)
		{
			var senderGrid = (DataGridView)sender;

			if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
			{
				var buttonSelected = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewButtonCell;
				var selectedRow = v_WhileConditionsTable.Rows[e.RowIndex];

				if (buttonSelected.Value.ToString() == "Edit")
				{
					//launch editor
					var statement = selectedRow["Statement"];
					var commandData = selectedRow["CommandData"].ToString();
					var whileCommand = JsonConvert.DeserializeObject<BeginWhileCommand>(commandData);

					var automationCommands = new List<AutomationCommand>() { CommandsHelper.ConvertToAutomationCommand(typeof(BeginWhileCommand)) };
					IfrmCommandEditor editor = commandControls.CreateCommandEditorForm(automationCommands, null);
					editor.SelectedCommand = new BeginWhileCommand();
					editor.SelectedCommand = whileCommand;
					editor.EditingCommand = whileCommand;
					editor.OriginalCommand = whileCommand;
					editor.CreationModeInstance = CreationMode.Edit;
					editor.ScriptContext = parentEditor.ScriptContext;
					editor.TypeContext = parentEditor.TypeContext;

					if (((Form)editor).ShowDialog() == DialogResult.OK)
					{

						var editedCommand = editor.SelectedCommand as BeginWhileCommand;
						var displayText = editedCommand.GetDisplayValue();
						var serializedData = JsonConvert.SerializeObject(editedCommand);

						selectedRow["Statement"] = displayText;
						selectedRow["CommandData"] = serializedData;
					}

					commandControls.AddIntellisenseListBoxToCommandForm();
				}
				else if (buttonSelected.Value.ToString() == "Delete")
				{
					//delete
					v_WhileConditionsTable.Rows.Remove(selectedRow);
				}
				else
					throw new NotImplementedException("Requested Action is not implemented.");
			}
		}

		private void CreateWhileCondition(object sender, EventArgs e, IfrmCommandEditor parentEditor, ICommandControls commandControls)
		{
			var automationCommands = new List<AutomationCommand>() { CommandsHelper.ConvertToAutomationCommand(typeof(BeginWhileCommand)) };
			IfrmCommandEditor editor = commandControls.CreateCommandEditorForm(automationCommands, null);
			editor.SelectedCommand = new BeginWhileCommand();
			editor.ScriptContext = parentEditor.ScriptContext;
			editor.TypeContext = parentEditor.TypeContext;

			if (((Form)editor).ShowDialog() == DialogResult.OK)
			{
				//get data
				var configuredCommand = editor.SelectedCommand as BeginWhileCommand;
				var displayText = configuredCommand.GetDisplayValue();
				var serializedData = JsonConvert.SerializeObject(configuredCommand);
				parentEditor.ScriptContext = editor.ScriptContext;
				parentEditor.TypeContext = editor.TypeContext;

				//add to list
				v_WhileConditionsTable.Rows.Add(displayText, serializedData);
			}

			commandControls.AddIntellisenseListBoxToCommandForm();
		}
	}
}