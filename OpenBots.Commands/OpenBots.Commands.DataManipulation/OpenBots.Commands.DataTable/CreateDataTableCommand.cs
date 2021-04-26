using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using OBDataTable = System.Data.DataTable;

namespace OpenBots.Commands.DataTable
{
	[Serializable]
	[Category("DataTable Commands")]
	[Description("This command creates a DataTable with the Column Names provided.")]

	public class CreateDataTableCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Column Names")]
		[Description("Enter the Column Names required for each column of data.")]
		[SampleUsage("\"MyColumn\" || vColumn")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public OBDataTable v_ColumnNameDataTable { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output DataTable Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(OBDataTable) })]
		public string v_OutputUserVariableName { get; set; }

		public CreateDataTableCommand()
		{
			CommandName = "CreateDataTableCommand";
			SelectionName = "Create DataTable";
			CommandEnabled = true;
			CommandIcon = Resources.command_spreadsheet;

			//initialize data table
			v_ColumnNameDataTable = new OBDataTable
			{
				TableName = "ColumnNamesDataTable" + DateTime.Now.ToString("MMddyy.hhmmss")
			};

			v_ColumnNameDataTable.Columns.Add("Column Name");
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			OBDataTable Dt = new OBDataTable();

			foreach(DataRow rwColumnName in v_ColumnNameDataTable.Rows)
			{
				string columnName = (string)await rwColumnName.Field<string>("Column Name").EvaluateCode(engine);
				Dt.Columns.Add(columnName);
			}

			Dt.SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultDataGridViewGroupFor("v_ColumnNameDataTable", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [With {v_ColumnNameDataTable.Rows.Count} Column(s) - Store DataTable in '{v_OutputUserVariableName}']";
		}
	}
}