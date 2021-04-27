using Microsoft.Office.Interop.Excel;
using OpenBots.Commands.Microsoft.Library;
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
using Application = Microsoft.Office.Interop.Excel.Application;
using DataTable = System.Data.DataTable;

namespace OpenBots.Commands.Excel
{
	[Serializable]
	[Category("Excel Commands")]
	[Description("This command reads an Excel Config Worksheet and stores it in a Dictionary.")]
	public class LoadDictionaryCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Excel Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create Application** command.")]
		[SampleUsage("MyExcelInstance")]
		[Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
		[CompatibleTypes(new Type[] { typeof(Application) })]
		public string v_InstanceName { get; set; }

		[Required]
		[DisplayName("Key Column Name")]
		[Description("Enter the name of the column to be loaded as Dictionary Keys.")]
		[SampleUsage("\"Name\" || vKeyColumn")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_KeyColumn { get; set; }

		[Required]
		[DisplayName("Value Column Name")]
		[Description("Enter the name of the column to be loaded as Dictionary Values.")]
		[SampleUsage("\"Value\" || vValueColumn")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_ValueColumn { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Dictionary Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(Dictionary<,>) })]
		public string v_OutputUserVariableName { get; set; }

		public LoadDictionaryCommand()
		{
			CommandName = "LoadDictionaryCommand";
			SelectionName = "Load Dictionary";
			CommandEnabled = true;
			CommandIcon = Resources.command_spreadsheet;

			v_InstanceName = "DefaultExcel";
			v_KeyColumn = "\"Name\"";
			v_ValueColumn = "\"Value\"";
		}
		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			
			var vKeyColumn = (string)await v_KeyColumn.EvaluateCode(engine);
			var vValueColumn = (string)await v_ValueColumn.EvaluateCode(engine);

			var excelObject = v_InstanceName.GetAppInstance(engine);
			var excelInstance = (Application)excelObject;

			Worksheet excelSheet = excelInstance.ActiveSheet;

			Range sourceRange = excelSheet.UsedRange;
			var last = excelInstance.GetAddressOfLastCell(excelSheet);
			Range cellValue = excelSheet.Range["A1", last];
			
			int rw = cellValue.Rows.Count;
			int cl = 2;
			int rCnt;
			int cCnt;

			DataTable DT = new DataTable();

			for (rCnt = 2; rCnt <= rw; rCnt++)
			{
				DataRow newRow = DT.NewRow();
				for (cCnt = 1; cCnt <= cl; cCnt++)
				{
					if (((cellValue.Cells[rCnt, cCnt] as Range).Value2) != null)
					{
						if (!DT.Columns.Contains(cCnt.ToString()))
						{
							DT.Columns.Add(cCnt.ToString());
						}
						newRow[cCnt.ToString()] = ((cellValue.Cells[rCnt, cCnt] as Range).Value2).ToString();
					}
				}
				DT.Rows.Add(newRow);
			}

			string cKeyName = ((cellValue.Cells[1, 1] as Range).Value2).ToString();
			DT.Columns[0].ColumnName = cKeyName;
			string cValueName = ((cellValue.Cells[1, 2] as Range).Value2).ToString();
			DT.Columns[1].ColumnName = cValueName;

			var dictlist = DT.AsEnumerable().Select(x => new
			{
				keys = (string)x[vKeyColumn],
				values = (string)x[vValueColumn]
			}).ToList();

			Dictionary<string, string> outputDictionary = new Dictionary<string, string>();
			foreach (var dict in dictlist)
			{
				outputDictionary.Add(dict.keys, dict.values);
			}

			outputDictionary.SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_KeyColumn", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ValueColumn", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Load Keys '{v_KeyColumn}' and Values '{v_ValueColumn}' - Store Dictionary in '{v_OutputUserVariableName}' - Instance Name '{v_InstanceName}']";
		}
	}
}
