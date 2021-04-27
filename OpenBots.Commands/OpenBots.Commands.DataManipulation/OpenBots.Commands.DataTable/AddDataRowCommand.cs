using Newtonsoft.Json;
using OpenBots.Commands.DataTable.Forms;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.UI.Controls;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OBDataTable = System.Data.DataTable;

namespace OpenBots.Commands.DataTable
{
	[Serializable]
	[Category("DataTable Commands")]
	[Description("This command adds a DataRow to a DataTable.")]
	public class AddDataRowCommand : ScriptCommand
	{
		[Required]
		[DisplayName("DataTable")]
		[Description("Enter an existing DataTable to add a DataRow to.")]
		[SampleUsage("vDataTable")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(OBDataTable) })]
		public string v_DataTable { get; set; }

		[Required]
		[DisplayName("Data")]
		[Description("Enter Column Names and Data for each column in the DataRow.")]
		[SampleUsage("[ \"First Name\" | \"John\" ] || [ vColumn | vData ]")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string), typeof(object) })]
		public OBDataTable v_DataRowDataTable { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output DataTable Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(OBDataTable) })]
		public string v_OutputUserVariableName { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private List<CreateDataTableCommand> _dataTableCreationCommands;

		public AddDataRowCommand()
		{
			CommandName = "AddDataRowCommand";
			SelectionName = "Add DataRow";
			CommandEnabled = true;
			CommandIcon = Resources.command_spreadsheet;

			//initialize data table
			v_DataRowDataTable = new OBDataTable
			{
				TableName = "AddDataDataTable" + DateTime.Now.ToString("MMddyy.hhmmss")
			};

			v_DataRowDataTable.Columns.Add("Column Name");
			v_DataRowDataTable.Columns.Add("Data");
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			OBDataTable Dt = (OBDataTable)await v_DataTable.EvaluateCode(engine);
			var newRow = Dt.NewRow();

			foreach (DataRow rw in v_DataRowDataTable.Rows)
			{
				var columnName = (string)await rw.Field<string>("Column Name").EvaluateCode(engine);
				var data = await rw.Field<string>("Data").EvaluateCode(engine);
				newRow.SetField(columnName, data);
			}
			Dt.Rows.Add(newRow);

			Dt.SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataTable", this, editor));

			CommandItemControl loadSchemaControl = new CommandItemControl();
			loadSchemaControl.ForeColor = Color.White;
			loadSchemaControl.Font = new Font("Segoe UI Semilight", 10);
			loadSchemaControl.CommandDisplay = "Load Column Names From Existing DataTable";
			loadSchemaControl.CommandImage = Resources.command_spreadsheet;
			loadSchemaControl.Click += LoadSchemaControl_Click;

			var dataRowDataControls = new List<Control>();
			dataRowDataControls.Add(commandControls.CreateDefaultLabelFor("v_DataRowDataTable", this));
			var gridview = commandControls.CreateDefaultDataGridViewFor("v_DataRowDataTable", this);
			dataRowDataControls.AddRange(commandControls.CreateUIHelpersFor("v_DataRowDataTable", this, new Control[] { gridview }, editor));
			dataRowDataControls.Add(loadSchemaControl);
			dataRowDataControls.Add(gridview);

			RenderedControls.AddRange(dataRowDataControls);

			_dataTableCreationCommands = editor.ConfiguredCommands.Where(f => f is CreateDataTableCommand)
																 .Select(f => (CreateDataTableCommand)f)
																 .ToList();

			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Add {v_DataRowDataTable.Rows.Count} Field(s) to '{v_DataTable}' - Store DataTable in '{v_OutputUserVariableName}']";
		}

		private void LoadSchemaControl_Click(object sender, EventArgs e)
		{
			frmDataTableVariableSelector selectionForm = new frmDataTableVariableSelector();
			selectionForm.Text = "Load Schema";
			selectionForm.lblHeader.Text = "Select a DataTable from the list";
			foreach (var item in _dataTableCreationCommands)
			{
				selectionForm.lstVariables.Items.Add(item.v_OutputUserVariableName);
			}

			var result = selectionForm.ShowDialog();

			if (result == DialogResult.OK)
			{
				var tableName = selectionForm.lstVariables.SelectedItem.ToString();
				var schema = _dataTableCreationCommands.Where(f => f.v_OutputUserVariableName == tableName).FirstOrDefault();

				v_DataRowDataTable.Rows.Clear();

				foreach (DataRow rw in schema.v_ColumnNameDataTable.Rows)
				{
					v_DataRowDataTable.Rows.Add(rw.Field<string>("Column Name"), "");
				}
			}

			selectionForm.Dispose();
		}
	}
}