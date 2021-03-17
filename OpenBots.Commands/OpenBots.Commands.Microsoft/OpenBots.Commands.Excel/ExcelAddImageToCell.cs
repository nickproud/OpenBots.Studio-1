using Newtonsoft.Json;
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
using System.Windows.Forms;
using Application = Microsoft.Office.Interop.Excel.Application;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;

namespace OpenBots.Commands.Excel
{
    [Serializable]
    [Category("Excel Commands")]
    [Description("This command adds an image to excel cell.")]
    public class ExcelAddImageToCell : ScriptCommand
    {
		[Required]
		[DisplayName("Excel Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create Application** command.")]
		[SampleUsage("MyExcelInstance")]
		[Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
		[CompatibleTypes(new Type[] { typeof(Application) })]
		public string v_InstanceName { get; set; }

		[Required]
		[DisplayName("Cell Location")]
		[Description("Enter the location of the cell to add image.")]
		[SampleUsage("A1 || {vCellLocation}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_CellLocation { get; set; }

		[Required]
		[DisplayName("Image Path")]
		[Description("Enter or Select the path to the image file.")]
		[SampleUsage(@"C:\temp\image.png || {vFilePath} || {ProjectPath}\image.png")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_ImagePath { get; set; }

		public ExcelAddImageToCell()
		{
			CommandName = "ExcelAddImageToCell";
			SelectionName = "Add Image To Cell";
			CommandEnabled = true;
			CommandIcon = Resources.command_spreadsheet;

			v_InstanceName = "DefaultExcel";
			v_CellLocation = "A1";
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var excelObject = v_InstanceName.GetAppInstance(engine);
			var vTargetAddress = v_CellLocation.ConvertUserVariableToString(engine);
			var vImagePath = v_ImagePath.ConvertUserVariableToString(engine);
			var excelInstance = (Application)excelObject;

			Worksheet excelSheet = excelInstance.ActiveSheet;
			Range oRange = excelSheet.Range[vTargetAddress];
			float left = (float)((double)oRange.Left);
			float top = (float)((double)oRange.Top);
			System.Drawing.Image img = System.Drawing.Image.FromFile(vImagePath);
			excelSheet.Shapes.AddPicture(vImagePath, MsoTriState.msoFalse, MsoTriState.msoCTrue, left, top, img.Width, img.Height);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_CellLocation", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ImagePath", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Add Image '{v_ImagePath}' to Cell '{v_CellLocation}' - Instance Name '{v_InstanceName}']";
		}
	}
}
