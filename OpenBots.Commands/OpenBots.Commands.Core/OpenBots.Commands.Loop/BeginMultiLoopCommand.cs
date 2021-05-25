using Newtonsoft.Json;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Script;
using OpenBots.Core.UI.Controls;
using OpenBots.Core.Utilities.CommandUtilities;
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
	public class BeginMultiLoopCommand : ScriptCommand
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
		[DisplayName("Multiple Loop Conditions")]
		[Description("Add new Loop condition(s).")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowLoopBuilder", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(Bitmap), typeof(DateTime), typeof(string), typeof(double), typeof(int), typeof(bool) })]
		public DataTable v_LoopConditionsTable { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private DataGridView _loopConditionHelper;

		public BeginMultiLoopCommand()
		{
			CommandName = "BeginMultiLoopCommand";
			SelectionName = "Begin Multi Loop";
			CommandEnabled = true;
			CommandIcon = Resources.command_startloop;
			ScopeStartCommand = true;

			v_LogicType = "And";
			v_LoopConditionsTable = new DataTable();
			v_LoopConditionsTable.TableName = DateTime.Now.ToString("MultiLoopConditionTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
			v_LoopConditionsTable.Columns.Add("Statement");
			v_LoopConditionsTable.Columns.Add("CommandData");
		}

		public async override Tasks.Task RunCommand(object sender, ScriptAction parentCommand)
		{
			var engine = (IAutomationEngineInstance)sender;
			bool isTrueStatement = await DetermineMultiStatementTruth(engine);
			engine.ReportProgress("Starting Loop");

			while (isTrueStatement)
			{
				foreach (var cmd in parentCommand.AdditionalScriptCommands)
				{
					if (engine.IsCancellationPending)
						return;

					await engine.ExecuteCommand(cmd);

					if (engine.CurrentLoopCancelled)
					{
						engine.ReportProgress("Exiting Loop");
						engine.CurrentLoopCancelled = false;
						return;
					}

					if (engine.CurrentLoopContinuing)
					{
						engine.ReportProgress("Continuing Next Loop");
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
			var controls = commandControls.CreateDefaultDataGridViewGroupFor("v_LoopConditionsTable", this, editor);
			_loopConditionHelper = controls[2] as DataGridView;

			//handle helper click
			var helper = controls[1] as CommandItemControl;
			helper.Click += (sender, e) => CreateLoopCondition(sender, e, editor, commandControls);

			//add for rendering
			RenderedControls.AddRange(controls);

			//define if condition helper
			_loopConditionHelper.Width = 450;
			_loopConditionHelper.Height = 200;
			_loopConditionHelper.AutoGenerateColumns = false;
			_loopConditionHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			_loopConditionHelper.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Condition", DataPropertyName = "Statement", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
			_loopConditionHelper.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "CommandData", DataPropertyName = "CommandData", ReadOnly = true, Visible = false });
			_loopConditionHelper.Columns.Add(new DataGridViewButtonColumn() { HeaderText = "Edit", UseColumnTextForButtonValue = true, Text = "Edit", Width = 45 });
			_loopConditionHelper.Columns.Add(new DataGridViewButtonColumn() { HeaderText = "Delete", UseColumnTextForButtonValue = true, Text = "Delete", Width = 60 });
			_loopConditionHelper.AllowUserToAddRows = false;
			_loopConditionHelper.AllowUserToDeleteRows = true;
			_loopConditionHelper.CellContentClick += (sender, e) => LoopConditionHelper_CellContentClick(sender, e, editor, commandControls);

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			if (v_LoopConditionsTable.Rows.Count == 0)
			{
				return "Loop <Not Configured>";
			}
			else if (v_LogicType == "And")
			{
				var statements = v_LoopConditionsTable.AsEnumerable().Select(f => f.Field<string>("Statement")).ToList();
				return string.Join(" && ", statements);
			}
			else
            {
				var statements = v_LoopConditionsTable.AsEnumerable().Select(f => f.Field<string>("Statement")).ToList();
				return string.Join(" || ", statements);
			}
		}

		private async Tasks.Task<bool> DetermineMultiStatementTruth(IAutomationEngineInstance engine)
		{
			bool isTrueStatement = true;
			foreach (DataRow rw in v_LoopConditionsTable.Rows)
			{
				var commandData = rw["CommandData"].ToString();
				var loopCommand = JsonConvert.DeserializeObject<BeginLoopCommand>(commandData);
				var statementResult = await CommandsHelper.DetermineStatementTruth(engine, loopCommand.v_LoopActionType, loopCommand.v_ActionParameterTable);

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

		private void LoopConditionHelper_CellContentClick(object sender, DataGridViewCellEventArgs e, IfrmCommandEditor parentEditor, ICommandControls commandControls)
		{
			var senderGrid = (DataGridView)sender;

			if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
			{
				var buttonSelected = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewButtonCell;
				var selectedRow = v_LoopConditionsTable.Rows[e.RowIndex];

				if (buttonSelected.Value.ToString() == "Edit")
				{
					//launch editor
					var statement = selectedRow["Statement"];
					var commandData = selectedRow["CommandData"].ToString();
					var loopCommand = JsonConvert.DeserializeObject<BeginLoopCommand>(commandData);

					var automationCommands = new List<AutomationCommand>() { CommandsHelper.ConvertToAutomationCommand(typeof(BeginLoopCommand)) };
					IfrmCommandEditor editor = commandControls.CreateCommandEditorForm(automationCommands, null);
					editor.SelectedCommand = new BeginLoopCommand(); 
					editor.SelectedCommand = loopCommand;
					editor.EditingCommand = loopCommand;
					editor.OriginalCommand = loopCommand;
					editor.CreationModeInstance = CreationMode.Edit;
					editor.ScriptContext = parentEditor.ScriptContext;
					editor.TypeContext = parentEditor.TypeContext;

					if (((Form)editor).ShowDialog() == DialogResult.OK)
					{

						var editedCommand = editor.SelectedCommand as BeginLoopCommand;
						var displayText = editedCommand.GetDisplayValue();
						var serializedData = JsonConvert.SerializeObject(editedCommand);

						selectedRow["Statement"] = displayText;
						selectedRow["CommandData"] = serializedData;
					}
				}
				else if (buttonSelected.Value.ToString() == "Delete")
				{
					//delete
					v_LoopConditionsTable.Rows.Remove(selectedRow);
				}
				else
				{
					throw new NotImplementedException("Requested Action is not implemented.");
				}
			}
		}

		private void CreateLoopCondition(object sender, EventArgs e, IfrmCommandEditor parentEditor, ICommandControls commandControls)
		{
			var automationCommands = new List<AutomationCommand>() { CommandsHelper.ConvertToAutomationCommand(typeof(BeginLoopCommand)) };
			IfrmCommandEditor editor = commandControls.CreateCommandEditorForm(automationCommands, null);
			editor.SelectedCommand = new BeginLoopCommand();
			editor.ScriptContext = parentEditor.ScriptContext;
			editor.TypeContext = parentEditor.TypeContext;

			if (((Form)editor).ShowDialog() == DialogResult.OK)
			{
				//get data
				var configuredCommand = editor.SelectedCommand as BeginLoopCommand;
				var displayText = configuredCommand.GetDisplayValue();
				var serializedData = JsonConvert.SerializeObject(configuredCommand);
				parentEditor.ScriptContext = editor.ScriptContext;
				parentEditor.TypeContext = editor.TypeContext;

				//add to list
				v_LoopConditionsTable.Rows.Add(displayText, serializedData);
			}
		}
	}
}