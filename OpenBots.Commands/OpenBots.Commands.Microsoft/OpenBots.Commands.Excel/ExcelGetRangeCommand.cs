using Microsoft.Office.Interop.Excel;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Commands.Microsoft.Library;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Windows.Forms;
using Application = Microsoft.Office.Interop.Excel.Application;
using DataTable = System.Data.DataTable;

namespace OpenBots.Commands.Excel
{
	[Serializable]
	[Category("Excel Commands")]
	[Description("This command gets the range from an Excel Worksheet and stores it in a DataTable.")]
	public class ExcelGetRangeCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Excel Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create Application** command.")]
		[SampleUsage("MyExcelInstance")]
		[Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
		[CompatibleTypes(new Type[] { typeof(Application) })]
		public string v_InstanceName { get; set; }

		[Required]
		[DisplayName("Range")]
		[Description("Enter the location of the range to extract.")]
		[SampleUsage("A1:B10 || A1: || {vRange} || {vStart}:{vEnd} || {vStart}:")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_Range { get; set; }   

		[Required]
		[DisplayName("Add Headers")]
		[PropertyUISelectionOption("Yes")]
		[PropertyUISelectionOption("No")]
		[Description("When selected, the column headers from the specified spreadsheet range are also extracted.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_AddHeaders { get; set; }

		[Required]
		[DisplayName("Read Formulas")]
		[PropertyUISelectionOption("Yes")]
		[PropertyUISelectionOption("No")]
		[Description("When selected, formulas will be extracted rather than their calculated values.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_Formulas { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output DataTable Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(DataTable) })]
		public string v_OutputUserVariableName { get; set; }

		public ExcelGetRangeCommand()
		{
			CommandName = "ExcelGetRangeCommand";
			SelectionName = "Get Range";
			CommandEnabled = true;
			CommandIcon = Resources.command_spreadsheet;

			v_InstanceName = "DefaultExcel";
			v_AddHeaders = "Yes";
			v_Formulas = "No";
			v_Range = "A1:";
		}

		public override void RunCommand(object sender)
		{         
			var engine = (IAutomationEngineInstance)sender;
			var excelObject = v_InstanceName.GetAppInstance(engine);
			var vRange = v_Range.ConvertUserVariableToString(engine);
			var excelInstance = (Application)excelObject;

			Worksheet excelSheet = excelInstance.ActiveSheet;
			//Extract a range of cells
			var splitRange = vRange.Split(':');
			Range cellRange;
			Range sourceRange = excelSheet.UsedRange;
			

			//Attempt to extract a single cell

			if (splitRange[1] == "")
            {
				var cell = excelInstance.GetAddressOfLastCell(excelSheet);

				if (cell == "")
					throw new Exception("No data found in sheet.");
				cellRange = excelSheet.Range[splitRange[0], cell];
			}
			else
            {
				cellRange = excelSheet.Range[splitRange[0], splitRange[1]];
			}

			int rowStart =1, rowEnd = 0, columnEnd = cellRange.Columns.Count, movingRangeSize = 3000;

			using (DataTable DT = new DataTable())
			{
				do
				{
					Range movingRange;
					object[,] rangeData;

					rowEnd = rowEnd + movingRangeSize;

					if (rowEnd < cellRange.Rows.Count)
						movingRange = excelSheet.Range[cellRange.Cells[rowStart, 1], cellRange.Cells[rowEnd, columnEnd]];
					else
						movingRange = excelSheet.Range[cellRange.Cells[rowStart, 1], cellRange.Cells[cellRange.Rows.Count, columnEnd]];

					rowStart = rowEnd + 1;


					int rw = movingRange.Rows.Count;
					int cl = movingRange.Columns.Count;

					if (v_Formulas == "Yes")
						rangeData = movingRange.Formula;
					else
						rangeData = movingRange.Value;

					int rCnt, cCnt;

					int startRow;
					if (v_AddHeaders == "Yes" && rowEnd == movingRangeSize)
						startRow = 2;
					else
						startRow = 1;

					for (rCnt = startRow; rCnt <= rw; rCnt++)
					{
						DataRow newRow = DT.NewRow();
						for (cCnt = 1; cCnt <= cl; cCnt++)
						{
							string colName = $"Column{cCnt - 1}";
							if (!DT.Columns.Contains(colName))
								DT.Columns.Add(colName);

							var cellValue = rangeData[rCnt, cCnt];

							if (cellValue == null)
								newRow[colName] = "";
							else
								newRow[colName] = cellValue.ToString();
						}
						DT.Rows.Add(newRow);
					}
				}
				while (rowEnd < cellRange.Rows.Count);

				if (v_AddHeaders == "Yes")
				{
					//Set column names
					for (int cCnt = 1; cCnt <= cellRange.Columns.Count; cCnt++)
					{
						var cellValue = (cellRange.Cells[1, cCnt] as Range).Value;
						if (cellValue != null)
							DT.Columns[cCnt - 1].ColumnName = cellValue.ToString();
					}
				}

				DT.StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Range", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_AddHeaders", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_Formulas", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Get Range '{v_Range}' - Store DataTable in '{v_OutputUserVariableName}' - Instance Name '{v_InstanceName}']";
		}
	}
}