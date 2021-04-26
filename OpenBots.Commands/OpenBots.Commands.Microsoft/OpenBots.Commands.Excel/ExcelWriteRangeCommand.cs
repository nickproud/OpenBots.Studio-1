using Microsoft.Office.Interop.Excel;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application = Microsoft.Office.Interop.Excel.Application;
using DataTable = System.Data.DataTable;

namespace OpenBots.Commands.Excel
{
	[Serializable]
	[Category("Excel Commands")]
	[Description("This command writes a DataTable to an Excel Worksheet starting from a specific cell address.")]
	public class ExcelWriteRangeCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Excel Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create Application** command.")]
		[SampleUsage("MyExcelInstance")]
		[Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
		[CompatibleTypes(new Type[] { typeof(Application) })]
		public string v_InstanceName { get; set; }

		[Required]
		[DisplayName("DataTable")]
		[Description("Enter the DataTable to write to the Worksheet.")]
		[SampleUsage("vDataTable")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(DataTable) })]
		public string v_DataTableToSet { get; set; }

		[Required]
		[DisplayName("Cell Location")]
		[Description("Enter the location of the cell to set the DataTable at.")]
		[SampleUsage("\"A1\" || vCellLocation")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_CellLocation { get; set; }

		[Required]
		[DisplayName("Add Headers")]
		[PropertyUISelectionOption("Yes")]
		[PropertyUISelectionOption("No")]
		[Description("When selected, the column headers from the specified DataTable are also written.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_AddHeaders { get; set; }

		public ExcelWriteRangeCommand()
		{
			CommandName = "ExcelWriteRangeCommand";
			SelectionName = "Write Range";
			CommandEnabled = true;
			CommandIcon = Resources.command_spreadsheet;

			v_InstanceName = "DefaultExcel";
			v_AddHeaders = "Yes";
			v_CellLocation = "\"A1\"";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var vTargetAddress = (string)await v_CellLocation.EvaluateCode(engine);
			var excelObject = v_InstanceName.GetAppInstance(engine);

			var excelInstance = (Application)excelObject;
			var excelSheet = (Worksheet)excelInstance.ActiveSheet;

			DataTable Dt = (DataTable)await v_DataTableToSet.EvaluateCode(engine);
			if (string.IsNullOrEmpty(vTargetAddress) || vTargetAddress.Contains(":")) 
				throw new Exception("Cell Location is invalid or empty");

			if (v_AddHeaders == "Yes")
			{
				DataRow firstRow = Dt.NewRow();
				List<string> names = new List<string>();
				foreach (DataColumn column in Dt.Columns)
				{
					names.Add(column.ColumnName);
				}
				firstRow.ItemArray = names.ToArray();
				Dt.Rows.InsertAt(firstRow, 0);
			}

			var dataTableList = ToChunks(Dt.AsEnumerable(), 3000);
			string movingRange = vTargetAddress;

			foreach (var listItem in dataTableList)
            {
				DataTable dataTable = listItem.AsEnumerable().CopyToDataTable();

				int colStartIndex = NumberFromExcelColumn(Regex.Replace(movingRange, @"[\d-]", string.Empty));
				int colEndIndex = colStartIndex + dataTable.Columns.Count - 1;
				string colEndAddress = ExcelColumnFromNumber(colEndIndex);
				colEndAddress = colEndAddress + ((dataTable.Rows.Count) + Convert.ToInt32(Regex.Match(movingRange, @"\d+").Value) - 1).ToString();

				String excelRangeStr = String.Format(movingRange + ":" + colEndAddress);
				Range excelRange = excelSheet.get_Range(excelRangeStr);

				int rowCount = dataTable.Rows.Count;
				int columnCount = dataTable.Columns.Count;

				object[,] objectArr = new object[rowCount, columnCount];

				for (int i = 0; i < rowCount; i++)
				{
					for (int j = 0; j < columnCount; j++)
					{
						objectArr[i, j] = dataTable.Rows[i][j];
					}
				}

				excelRange.Value = objectArr;

				movingRange = Regex.Replace(movingRange, @"[\d-]", string.Empty) + (Convert.ToInt32(Regex.Match(movingRange, @"\d+").Value) + 3000);
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataTableToSet", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_CellLocation", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_AddHeaders", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Write '{v_DataTableToSet}' to Cell '{v_CellLocation}' - Instance Name '{v_InstanceName}']";
		}

		public static int NumberFromExcelColumn(string column)
		{
			int retVal = 0;
			string col = column.ToUpper();
			for (int iChar = col.Length - 1; iChar >= 0; iChar--)
			{
				char colPiece = col[iChar];
				int colNum = colPiece - 64;
				retVal = retVal + colNum * (int)Math.Pow(26, col.Length - (iChar + 1));
			}
			return retVal;
		}
		public static string ExcelColumnFromNumber(int column)
		{
			string columnString = "";
			decimal columnNumber = column;
			while (columnNumber > 0)
			{
				decimal currentLetterNumber = (columnNumber - 1) % 26;
				char currentLetter = (char)(currentLetterNumber + 65);
				columnString = currentLetter + columnString;
				columnNumber = (columnNumber - (currentLetterNumber + 1)) / 26;
			}
			return columnString;
		}
		public static IEnumerable<IEnumerable<T>> ToChunks<T>(IEnumerable<T> enumerable, int chunkSize)
		{
			int itemsReturned = 0;
			var list = enumerable.ToList(); // Prevent multiple execution of IEnumerable.
			int count = list.Count;
			while (itemsReturned < count)
			{
				int currentChunkSize = Math.Min(chunkSize, count - itemsReturned);
				yield return list.GetRange(itemsReturned, currentChunkSize);
				itemsReturned += currentChunkSize;
			}
		}
	}
}