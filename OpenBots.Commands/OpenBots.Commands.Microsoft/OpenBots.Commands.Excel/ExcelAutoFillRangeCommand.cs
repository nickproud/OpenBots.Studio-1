using Microsoft.Office.Interop.Excel;
using OpenBots.Commands.Microsoft.Library;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Model.ApplicationModel;
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
	[Description("This command autofills a specified range in an Excel Worksheet.")]
	public class ExcelAutoFillRangeCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Excel Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create Application** command.")]
		[SampleUsage("MyExcelInstance")]
		[Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(OBAppInstance) })]
		public string v_InstanceName { get; set; }

		[Required]
		[DisplayName("Source Range")]
		[Description("Enter the source range.")]
		[SampleUsage("\"A1\" || \"A1:B1\" || \"A1:\" || vRange || vStart + \":\" + vEnd || vStart + \":\"")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_SourceRange { get; set; }

		[Required]
		[DisplayName("AutoFill Range")]
		[Description("Enter the range to autofill. The end cell must be specified.")]
		[SampleUsage("\"A1:B10\" || vRange || vStart + \":\" + vEnd")]
		[Remarks("The source range should be included as part of the autofill range.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_FillRange { get; set; }

		public ExcelAutoFillRangeCommand()
		{
			CommandName = "ExcelAutoFillRangeCommand";
			SelectionName = "AutoFill Range";
			CommandEnabled = true;
			CommandIcon = Resources.command_excel;
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var excelObject = ((OBAppInstance)await v_InstanceName.EvaluateCode(engine)).Value;
			var excelInstance = (Application)excelObject;
			string vSourceRange = (string)await v_SourceRange.EvaluateCode(engine);
			string vFillRange = (string)await v_FillRange.EvaluateCode(engine);

			Worksheet excelSheet = excelInstance.ActiveSheet;
			Range sourceRange = excelInstance.GetRange(vSourceRange, excelSheet);

			var splitRange = vFillRange.Split(':');
			Range fillRange = excelSheet.Range[splitRange[0], splitRange[1]];

			sourceRange.AutoFill(fillRange, XlAutoFillType.xlFillDefault);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SourceRange", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FillRange", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Source Range '{v_SourceRange}' - Fill Range '{v_FillRange}' - Instance Name '{v_InstanceName}']";
		}
	}
}