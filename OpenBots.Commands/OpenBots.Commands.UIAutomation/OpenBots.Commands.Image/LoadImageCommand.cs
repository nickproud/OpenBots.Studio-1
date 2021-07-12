using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using OBImage = System.Drawing.Image;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.Image
{
	[Serializable]
	[Category("Image Commands")]
	[Description("This command loads an image from a file and stores it to a Bitmap.")]
	public class LoadImageCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Image Filepath")]
		[Description("The filepath to the image that will be loaded.")]
		[SampleUsage("@\"C:\\temp\\myfile.png\" || ProjectPath + @\"\\myfile.png\" || vFilePath")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_Filepath { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Image Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(Bitmap) })]
		public string v_OutputUserVariableName { get; set; }

		public LoadImageCommand()
		{
			CommandName = "LoadImageCommand";
			SelectionName = "Load Image";
			CommandEnabled = true;
			CommandIcon = Resources.command_camera;

		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			string vFilepath = (string)await v_Filepath.EvaluateCode(engine);
			OBImage image = OBImage.FromFile(vFilepath);
			MemoryStream bitmapStream = new MemoryStream();
			image.Save(bitmapStream, ImageFormat.Bmp);
			Bitmap bitmap = new Bitmap(bitmapStream);
			bitmap.SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Filepath", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));
			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Load image from {v_Filepath} - Store Image in '{v_OutputUserVariableName}']";
		}
	}
}
