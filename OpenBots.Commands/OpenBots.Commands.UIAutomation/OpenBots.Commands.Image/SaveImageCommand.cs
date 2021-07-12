using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.Image
{
	[Serializable]
	[Category("Image Commands")]
	[Description("This command Saves an image from a file and stores it to a Bitmap.")]
	public class SaveImageCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Image Location")]
		[Description("Enter or Select the path of the folder to save the image to.")]
		[SampleUsage("@\"C:\\temp\\\" || ProjectPath + @\"\\\" || vFolderPath")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFolderSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_Filepath { get; set; }

		[Required]
		[DisplayName("Image File Name")]
		[Description("Enter or Select the name of the image file.")]
		[SampleUsage("\"savedImage.png\" || vFilename")]
		[Remarks("Supported File Extensions: .jpeg, .png, .bmp, .tiff, .gif ")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_Filename { get; set; }

		[Required]
		[DisplayName("Image")]
		[Description("Provide the image to be saved.")]
		[SampleUsage("vBitmap")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(Bitmap) })]
		public string v_Image { get; set; }

		public SaveImageCommand()
		{
			CommandName = "SaveImageCommand";
			SelectionName = "Save Image";
			CommandEnabled = true;
			CommandIcon = Resources.command_camera;

		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			string vFilepath = (string)await v_Filepath.EvaluateCode(engine);
			Bitmap image = (Bitmap)await v_Image.EvaluateCode(engine);
			string vFilename = (string)await v_Filename.EvaluateCode(engine);
			string fullpath = Path.Combine(vFilepath, vFilename);
			string extension = Path.GetExtension(fullpath).ToLower();
            switch (extension)
			{
				case ".jpeg":
					image.Save(fullpath, ImageFormat.Jpeg);
					break;
				case ".jpg":
					image.Save(fullpath, ImageFormat.Jpeg);
					break;
				case ".png":
					image.Save(fullpath, ImageFormat.Png);
					break;
				case ".bmp":
					image.Save(fullpath, ImageFormat.Bmp);
					break;
				case ".tiff":
					image.Save(fullpath, ImageFormat.Tiff);
					break;
				case ".gif":
					image.Save(fullpath, ImageFormat.Gif);
					break;
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Filepath", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Filename", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Image", this, editor));
			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Save Image {v_Image} at '{v_Filepath}\\{v_Filename}']";
		}
	}
}
