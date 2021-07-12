using Microsoft.Office.Interop.Excel;
using System;

namespace OpenBots.Commands.Microsoft.Library
{
    public static class ExcelHelper
    {
		//deprecated but may return
		public static string GetAddressOfLastCell(this Application app, Range sourceRange, Range startPoint)
		{
			Range rng = sourceRange.Cells.Find(
				What: "*",
				After: startPoint,
				LookIn: XlFindLookIn.xlFormulas,
				LookAt: XlLookAt.xlPart,
				SearchDirection: XlSearchDirection.xlPrevious,
				MatchCase: false);
			if (rng == null)
				return "";
			return rng.Address[false, false, XlReferenceStyle.xlA1, Type.Missing, Type.Missing];
		}

		public static string GetAddressOfLastCell(this Application app, Worksheet sheet)
        {
			int lastUsedRow = sheet.Cells.Find("*", System.Reflection.Missing.Value,
							   System.Reflection.Missing.Value, System.Reflection.Missing.Value,
							   XlSearchOrder.xlByRows, XlSearchDirection.xlPrevious,
							   false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;
			int lastUsedColumn = sheet.Cells.Find("*", System.Reflection.Missing.Value,
							   System.Reflection.Missing.Value, System.Reflection.Missing.Value,
							   XlSearchOrder.xlByColumns, XlSearchDirection.xlPrevious,
							   false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Column;
			Range rng = sheet.Cells[lastUsedRow, lastUsedColumn] as Range;
			return rng.Address[false, false, XlReferenceStyle.xlA1, Type.Missing, Type.Missing];
		}

		public static Range GetRange(this Application app, string range, Worksheet sheet)
        {
			var splitRange = range.Split(':');
			Range cellRange;

			try
			{
				//Attempt to extract a single cell
				if (splitRange[1] == "")
				{
					var cell = app.GetAddressOfLastCell(sheet);

					if (cell == "")
						throw new Exception("No data found in sheet.");

					cellRange = sheet.Range[splitRange[0], cell];
				}
				else
					cellRange = sheet.Range[splitRange[0], splitRange[1]];
			}
			//Select a cell
			catch (Exception)
			{
				cellRange = sheet.Range[splitRange[0], Type.Missing];
			}

			return cellRange;
		}
	}
}
