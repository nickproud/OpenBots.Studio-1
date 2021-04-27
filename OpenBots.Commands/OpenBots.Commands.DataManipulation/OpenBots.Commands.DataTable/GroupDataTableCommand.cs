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
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OBDataTable = System.Data.DataTable;

namespace OpenBots.Commands.DataTable
{
    [Serializable]
    [Category("DataTable Commands")]
    [Description("This command groups a DataTable by a specified column name/index as a List of DataTables.")]
    public class GroupDataTableCommand : ScriptCommand
    {
		[Required]
		[DisplayName("DataTable")]
		[Description("Enter the DataTable to group.")]
		[SampleUsage("vDataTable")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(OBDataTable) })]
		public string v_DataTable { get; set; }

		[Required]
		[DisplayName("Search Option")]
		[PropertyUISelectionOption("Column Name")]
		[PropertyUISelectionOption("Column Index")]
		[Description("Select whether the Datatable should be grouped by column index or column name.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_Option { get; set; }

		[Required]
		[DisplayName("Search Value")]
		[Description("Enter a valid DataTable index or column name.")]
		[SampleUsage("0 || vIndex || \"Column1\" || vColumnName")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string), typeof(int) })]
		public string v_DataValueIndex { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output DataTable List Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(List<OBDataTable>) })]
		public string v_OutputUserVariableName { get; set; }

		public GroupDataTableCommand()
		{
			CommandName = "GroupDataTableCommand";
			SelectionName = "Group Datatable";
			CommandEnabled = true;
			CommandIcon = Resources.command_spreadsheet;

			v_Option = "Column Index";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			OBDataTable dataTable = (OBDataTable)await v_DataTable.EvaluateCode(engine);
			dynamic valueIndex = await v_DataValueIndex.EvaluateCode(engine);

			string columnName = "";

			if (v_Option == "Column Index")
			{
				int index = (int)valueIndex;
				columnName = dataTable.Columns[index].ColumnName;
			}
			else if (v_Option == "Column Name")
			{
				columnName = (string)valueIndex;
			}

			List<OBDataTable> dataTableList = new List<OBDataTable>();
			dataTableList = (from table in dataTable.AsEnumerable()
						group table by new { placeCol = table[columnName] } into dataTableGroup
						select dataTableGroup.ToList().CopyToDataTable()).ToList();

			dataTableList.SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataTable", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_Option", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataValueIndex", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Group '{v_DataTable}' by Column '{v_DataValueIndex}' - Store DataTable List in '{v_OutputUserVariableName}']";
		}
	}
}
