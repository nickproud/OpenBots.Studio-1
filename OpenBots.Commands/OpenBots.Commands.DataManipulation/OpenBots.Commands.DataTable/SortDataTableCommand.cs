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
using System.Windows.Forms;
using OBDataTable = System.Data.DataTable;

namespace OpenBots.Commands.DataTable
{
    [Serializable]
    [Category("DataTable Commands")]
    [Description("This command sorts a DataTable by a specified column name/index.")]
    public class SortDataTableCommand : ScriptCommand
    {
		[Required]
		[DisplayName("DataTable")]
		[Description("Enter the DataTable to sort.")]
		[SampleUsage("{vDataTable}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(OBDataTable) })]
		public string v_DataTable { get; set; }

		[Required]
		[DisplayName("Search Option")]
		[PropertyUISelectionOption("Column Name")]
		[PropertyUISelectionOption("Column Index")]
		[Description("Select whether the DataRow value should be found by column index or column name.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_Option { get; set; }

		[Required]
		[DisplayName("Search Value")]
		[Description("Enter a valid DataTable index or column name.")]
		[SampleUsage("0 || {vIndex} || Column1 || {vColumnName}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_DataValueIndex { get; set; }

		[Required]
		[DisplayName("Sort Type")]
		[PropertyUISelectionOption("Ascending")]
		[PropertyUISelectionOption("Descending")]
		[Description("Select whether the DataTable should be sorted by ascending or descending order.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_SortType { get; set; }

		[Required]
		[DisplayName("Sort Order")]
		[PropertyUISelectionOption("Alphabetical")]
		[PropertyUISelectionOption("Numeric")]
		[Description("Select whether the DataTable should be sorted alphabetically or numerically.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_SortOrder { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Sorted DataTable Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(OBDataTable) })]
		public string v_OutputUserVariableName { get; set; }

		public SortDataTableCommand()
		{
			CommandName = "SortDataTableCommand";
			SelectionName = "Sort Datatable";
			CommandEnabled = true;
			CommandIcon = Resources.command_spreadsheet;

			v_Option = "Column Index";
			v_SortType = "Ascending";
			v_SortOrder = "Alphabetical";
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			var dataTableVariable = v_DataTable.ConvertUserVariableToObject(engine, nameof(v_DataTable), this);
			OBDataTable dataTable = (OBDataTable)dataTableVariable;

			var valueIndex = v_DataValueIndex.ConvertUserVariableToString(engine);

			string columnName = "";

			if (v_Option == "Column Index")
			{
				int index = int.Parse(valueIndex);
				columnName = dataTable.Columns[index].ColumnName;
			}
			else if (v_Option == "Column Name")
			{
				columnName = valueIndex;
			}

			if (v_SortType == "Ascending")
			{
				if (v_SortOrder == "Alphabetical")
					dataTable = (from row in dataTable.AsEnumerable()
								 orderby row[columnName].ToString() ascending
								 select row).CopyToDataTable();
				else
					dataTable = (from row in dataTable.AsEnumerable()
								 orderby Convert.ToInt32(row[columnName]) ascending
								 select row).CopyToDataTable();
			}
			else if (v_SortType == "Descending")
			{
				if (v_SortOrder == "Alphabetical")
					dataTable = (from row in dataTable.AsEnumerable()
								 orderby row[columnName].ToString() descending
								 select row).CopyToDataTable();
				else
					dataTable = (from row in dataTable.AsEnumerable()
								 orderby Convert.ToInt32(row[columnName]) descending
								 select row).CopyToDataTable();
			}
			dataTable.StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataTable", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_Option", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataValueIndex", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_SortType", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_SortOrder", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Sort '{v_DataTable}' by Column '{v_DataValueIndex}' {v_SortType} - Store Sorted DataTable in '{v_OutputUserVariableName}']";
		}
	}
}
