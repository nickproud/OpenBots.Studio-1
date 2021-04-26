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
using System.Threading.Tasks;
using System.Windows.Forms;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace OpenBots.Commands.Excel
{
	[Serializable]
	[Category("Excel Commands")]
	[Description("This command renames a specific Worksheet in an Excel Workbook.")]
	public class ExcelRenameSheetCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Excel Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create Application** command.")]
		[SampleUsage("MyExcelInstance")]
		[Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
		[CompatibleTypes(new Type[] { typeof(Application) })]
		public string v_InstanceName { get; set; }

		[Required]
		[DisplayName("Original Worksheet Name")]
		[Description("Specify the original name of the Worksheet to rename.")]
		[SampleUsage("\"Sheet1\" || vSheet")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_OriginalSheetName { get; set; }

		[Required]
		[DisplayName("New Worksheet Name")]
		[Description("Specify the new name of the Worksheet.")]
		[SampleUsage("\"Sheet1\" || vSheet")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_NewSheetName { get; set; }

		public ExcelRenameSheetCommand()
		{
			CommandName = "ExcelRenameSheetCommand";
			SelectionName = "Rename Sheet";
			CommandEnabled = true;
			CommandIcon = Resources.command_spreadsheet;

			v_InstanceName = "DefaultExcel";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			string vSheetToRename = (string)await v_OriginalSheetName.EvaluateCode(engine);
			string vNewSheetName = (string)await v_NewSheetName.EvaluateCode(engine);

			var excelObject = v_InstanceName.GetAppInstance(engine);
			var excelInstance = (Application)excelObject;
			var workSheet = excelInstance.Sheets[vSheetToRename] as Worksheet;
			workSheet.Name = vNewSheetName;
			workSheet.Select();
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_OriginalSheetName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_NewSheetName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Sheet '{v_OriginalSheetName}' to '{v_NewSheetName}'- Instance Name '{v_InstanceName}']";
		}
	}
}