using Microsoft.Office.Interop.Excel;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Commands.Microsoft.Library;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Application = Microsoft.Office.Interop.Excel.Application;
using System.Threading.Tasks;
using OpenBots.Core.Model.ApplicationModel;

namespace OpenBots.Commands.Excel
{
	[Serializable]
	[Category("Excel Commands")]
	[Description("This command activates a specific range in an Excel Worksheet.")]

	public class ExcelActivateRangeCommand : ScriptCommand
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
		[DisplayName("Range")]
		[Description("Enter the location of the cell or range to activate.")]
		[SampleUsage("\"A1\" || \"A1:B10\" || \"A1:\" || vRange || vStart+\":+\"vEnd || vStart + \":\"")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_Range { get; set; }

		public ExcelActivateRangeCommand()
		{
			CommandName = "ExcelActivateRangeCommand";
			SelectionName = "Activate Range";
			CommandEnabled = true;
			CommandIcon = Resources.command_excel;
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var excelObject = ((OBAppInstance)await v_InstanceName.EvaluateCode(engine)).Value;
			var vRange = (string)await v_Range.EvaluateCode(engine);

			var excelInstance = (Application)excelObject;
			Worksheet excelSheet = excelInstance.ActiveSheet;
			Range cellRange = excelInstance.GetRange(vRange, excelSheet);	

			excelSheet.Range[cellRange.Address].Select();           
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Range", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Activate '{v_Range}' - Instance Name '{v_InstanceName}']";
		}
	}
}
