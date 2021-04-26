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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Data;
using System.Threading.Tasks;

namespace OpenBots.Commands.Excel
{
    [Serializable]
    [Category("Excel Commands")]
    [Description("This command writes an image to a specific cell in an Excel Worksheet.")]
    public class ExcelWriteImageToCell : ScriptCommand
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
		[Description("Enter the location of the cell to write image.")]
		[SampleUsage("\"A1\" || vCellLocation")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_CellLocation { get; set; }

		[Required]
		[DisplayName("Image Path")]
		[Description("Enter or Select the path to the image file.")]
		[SampleUsage("@\"C:\\temp\\myfile.png\" || ProjectPath + @\"\\myfile.png\" || vFilePath")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_ImagePath { get; set; }

		[Required]
		[DisplayName("Image Scale Percentage")]
		[Description("Enter the image scale percentage to enlarge or reduce the size of the rendered image.")]
		[SampleUsage("75 || vImageScalePercentage")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_ImageScalePercentage { get; set; }

		public ExcelWriteImageToCell()
		{
			CommandName = "ExcelWriteImageToCell";
			SelectionName = "Write Image To Cell";
			CommandEnabled = true;
			CommandIcon = Resources.command_spreadsheet;

			v_InstanceName = "DefaultExcel";
			v_CellLocation = "\"A1\"";
			v_ImageScalePercentage = "100";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var excelObject = v_InstanceName.GetAppInstance(engine);
			var excelInstance = (Application)excelObject;
			var vTargetAddress = (string)await v_CellLocation.EvaluateCode(engine);
			var vImagePath = (string)await v_ImagePath.EvaluateCode(engine);
			var vImageScalePercentage = (int)await v_ImageScalePercentage.EvaluateCode(engine);

			if(vImageScalePercentage < 1)
				throw new DataException("Invalid Image Scale Percentage value, it should be greater than 0.");

			Worksheet excelSheet = excelInstance.ActiveSheet;
			Range oRange = excelSheet.Range[vTargetAddress];
			float left = (float)((double)oRange.Left);
			float top = (float)((double)oRange.Top);
			Image img = Image.FromFile(vImagePath);
			img = ScaleByPercent(img, vImageScalePercentage);
			excelSheet.Shapes.AddPicture(vImagePath, MsoTriState.msoFalse, MsoTriState.msoCTrue, left, top, img.Width, img.Height);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_CellLocation", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ImagePath", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ImageScalePercentage", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Write Image '{v_ImagePath}' to Cell '{v_CellLocation}' - Instance Name '{v_InstanceName}']";
		}

		public static Image ScaleByPercent(Image imgPhoto, int Percent)
		{
			float nPercent = ((float)Percent / 100);

			int sourceWidth = (int)imgPhoto.Width;
			int sourceHeight = (int)imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;

			int destX = 0;
			int destY = 0;
			int destWidth = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap bmPhoto = new Bitmap(destWidth, destHeight,
									 PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
									imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

			grPhoto.DrawImage(imgPhoto,
				new System.Drawing.Rectangle(destX, destY, destWidth, destHeight),
				new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
				GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
		}
	}
}
