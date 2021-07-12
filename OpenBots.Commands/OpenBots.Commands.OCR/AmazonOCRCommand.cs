using Newtonsoft.Json;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Amazon.Textract;
using Amazon.Textract.Model;
using System.Drawing;
using System.Drawing.Imaging;
using OpenBots.Core.Interfaces;

namespace OpenBots.Commands.OCR
{
	[Serializable]
	[Category("OCR Commands")]
	[Description("This command extracts text from an image file using Amazon OCR.")]
	public class AmazonOCRCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Image Input Type")]
		[PropertyUISelectionOption("Filepath")]
		[PropertyUISelectionOption("Bitmap")]
		[Description("Select the method through which to input your image.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_ImageType { get; set; }

		[Required]
		[DisplayName("Image File Path")]
		[Description("Enter the image filepath.")]
		[SampleUsage("@\"C:\\temp\\myfile.png\" || ProjectPath + @\"\\myfile.png\" || vFilePath")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_FilePath { get; set; }

		[Required]
		[DisplayName("Image Bitmap")]
		[Description("Enter the image Bitmap.")]
		[SampleUsage("vBitmap")]
		[Remarks("Use the Capture Image or Load Image commands to generate an image bitmap.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(Bitmap) })]
		public string v_Bitmap { get; set; }

		[Required]
		[DisplayName("AWS Access Key")]
		[Description("The access key to use when calling AWS textract service.")]
		[SampleUsage("\"myAccessKey\" || vAccessKey")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_AccessKey { get; set; }

		[Required]
		[DisplayName("AWS Secret Key")]
		[Description("The secret key to use when calling AWS textract service.")]
		[SampleUsage("\"mySecretKey\" || vSecretKey")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_SecretKey { get; set; }

		[Required]
		[DisplayName("AWS Region Endpoint")]
		[PropertyUISelectionOption("us-east-2")]
		[PropertyUISelectionOption("us-east-1")]
		[PropertyUISelectionOption("us-west-1")]
		[PropertyUISelectionOption("us-west-2")]
		[PropertyUISelectionOption("af-south-1")]
		[PropertyUISelectionOption("ap-east-1")]
		[PropertyUISelectionOption("ap-south-1")]
		[PropertyUISelectionOption("ap-northeast-3")]
		[PropertyUISelectionOption("ap-northeast-2")]
		[PropertyUISelectionOption("ap-northeast-1")]
		[PropertyUISelectionOption("ap-southeast-1")]
		[PropertyUISelectionOption("ap-southeast-2")]
		[PropertyUISelectionOption("ca-central-1")]
		[PropertyUISelectionOption("cn-north-1")]
		[PropertyUISelectionOption("cn-northwest-1")]
		[PropertyUISelectionOption("eu-central-1")]
		[PropertyUISelectionOption("eu-west-1")]
		[PropertyUISelectionOption("eu-west-2")]
		[PropertyUISelectionOption("eu-west-3")]
		[PropertyUISelectionOption("eu-south-1")]
		[PropertyUISelectionOption("eu-north-1")]
		[PropertyUISelectionOption("me-south-1")]
		[PropertyUISelectionOption("sa-east-1")]
		[PropertyUISelectionOption("us-gov-east-1")]
		[PropertyUISelectionOption("us-gov-west-1")]
		[Description("Select the AWS Region Endpoint to use.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_AWSRegion { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output OCR Result Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_OutputUserVariableName { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private bool _hasRendered;

		[JsonIgnore]
		[Browsable(false)]
		private List<Control> _filepathControls;

		[JsonIgnore]
		[Browsable(false)]
		private List<Control> _bitmapControls;

		public AmazonOCRCommand()
		{
			CommandName = "AmazonOCRCommand";
			SelectionName = "Amazon OCR";
			CommandEnabled = true;
			CommandIcon = Resources.command_camera;
		}

		public override async Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var vAccessKey = (string)await v_AccessKey.EvaluateCode(engine);
			var vSecretKey = (string)await v_SecretKey.EvaluateCode(engine);

			var ocrRequest = new DetectDocumentTextRequest();
			MemoryStream memStream = new MemoryStream();
			if (v_ImageType == "Filepath")
            {
				string vFilePath = (string)await v_FilePath.EvaluateCode(engine);
				FileStream image = File.OpenRead(vFilePath);
				image.CopyTo(memStream);
			}
            else
            {
				Bitmap vBitmap = (Bitmap)await v_Bitmap.EvaluateCode(engine);
				vBitmap.Save(memStream, ImageFormat.Jpeg);
			}
			ocrRequest.Document = new Document();
			ocrRequest.Document.Bytes = memStream;
			AmazonTextractConfig config = new AmazonTextractConfig();
			config.RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(v_AWSRegion);
			AmazonTextractClient client = new AmazonTextractClient(vAccessKey, vSecretKey, config);
			DetectDocumentTextResponse results = client.DetectDocumentText(ocrRequest);
			string readText = "";
			if (results.HttpStatusCode == System.Net.HttpStatusCode.OK)
			{
				foreach (Block block in results.Blocks)
				{
					if (block.BlockType.Value == "LINE")
						readText += block.Text + "\n";
				}
			}
            else
            {
				throw new Exception("Call to AWS textract failed");
            }
			readText.SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_ImageType", this, editor));
			((ComboBox)RenderedControls[1]).SelectedIndexChanged += imageTypeComboBox_SelectedIndexChanged;

			_filepathControls = new List<Control>();
			_filepathControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));
			RenderedControls.AddRange(_filepathControls);

			_bitmapControls = new List<Control>();
			_bitmapControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Bitmap", this, editor));
			RenderedControls.AddRange(_bitmapControls);
			RenderedControls.AddRange(commandControls.CreateDefaultPasswordInputGroupFor("v_AccessKey", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultPasswordInputGroupFor("v_SecretKey", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_AWSRegion", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Image '{v_FilePath}{v_Bitmap}' - Store OCR Result in '{v_OutputUserVariableName}']";
		}

		public override void Shown()
		{
			base.Shown();
			_hasRendered = true;
			if (v_ImageType == null)
			{
				v_ImageType = "Filepath";
				((ComboBox)RenderedControls[1]).Text = v_ImageType;
			}
			imageTypeComboBox_SelectedIndexChanged(null, null);
		}

		private void imageTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (((ComboBox)RenderedControls[1]).Text == "Filepath" && _hasRendered)
			{
				foreach (var ctrl in _filepathControls)
					ctrl.Visible = true;

				foreach (var ctrl in _bitmapControls)
				{
					ctrl.Visible = false;
					if (ctrl is TextBox)
						((TextBox)ctrl).Clear();
				}
			}
			else if (_hasRendered)
			{
				foreach (var ctrl in _filepathControls)
				{
					ctrl.Visible = false;
					if (ctrl is TextBox)
						((TextBox)ctrl).Clear();
				}

				foreach (var ctrl in _bitmapControls)
					ctrl.Visible = true;
			}
		}
	}
}
