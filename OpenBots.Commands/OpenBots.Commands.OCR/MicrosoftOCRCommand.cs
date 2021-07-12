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
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading;
using OpenBots.Core.Interfaces;

namespace OpenBots.Commands.OCR
{
	[Serializable]
	[Category("OCR Commands")]
	[Description("This command extracts text from an image file using Microsoft OCR.")]
	public class MicrosoftOCRCommand : ScriptCommand
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
		[DisplayName("Microsoft OCR Service API Key")]
		[Description("The access key to use when calling the Microsoft OCR service.")]
		[SampleUsage("\"myAPIKey\" || vAPIKey")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_APIKey { get; set; }

		[Required]
		[DisplayName("Microsoft OCR Service Endpoint")]
		[Description("The endpoint to target when calling the Microsoft OCR service.")]
		[SampleUsage("\"myEndpoint\" || vEndpoint")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_Endpoint { get; set; }

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

		public MicrosoftOCRCommand()
		{
			CommandName = "MicrosoftOCRCommand";
			SelectionName = "Microsoft OCR";
			CommandEnabled = true;
			CommandIcon = Resources.command_camera;
		}

		public override async Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var vAPIKey = (string)await v_APIKey.EvaluateCode(engine);
			var vEndpoint = (string)await v_Endpoint.EvaluateCode(engine);
			Stream imageStream = null;
			if (v_ImageType == "Filepath")
			{
				imageStream = new FileStream((string)await v_FilePath.EvaluateCode(engine), FileMode.Open);
			}
			else
			{
				Bitmap vBitmap = (Bitmap)await v_Bitmap.EvaluateCode(engine);
				imageStream = new FileStream(engine.EngineContext.ProjectPath + "\\tempOCRBitmap.bmp", FileMode.OpenOrCreate);
				vBitmap.Save(imageStream, ImageFormat.Bmp);
				imageStream.Close();
				imageStream = new FileStream(engine.EngineContext.ProjectPath + "\\tempOCRBitmap.bmp", FileMode.Open);
			}
			ComputerVisionClient client =
			  new ComputerVisionClient(new ApiKeyServiceClientCredentials(vAPIKey))
			  { Endpoint = vEndpoint };
			var textHeaders = await client.ReadInStreamAsync(imageStream);

			string operationLocation = textHeaders.OperationLocation;
			Thread.Sleep(2000);
			
			const int numberOfCharsInOperationId = 36;
			string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

			ReadOperationResult results;
			do
			{
				results = await client.GetReadResultAsync(Guid.Parse(operationId));
			}
			while ((results.Status == OperationStatusCodes.Running ||
				results.Status == OperationStatusCodes.NotStarted));
			string readText = "";
			var textUrlFileResults = results.AnalyzeResult.ReadResults;
			foreach (ReadResult page in textUrlFileResults)
			{
				foreach (Line line in page.Lines)
				{
					readText += line.Text + "\n";
				}
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
			RenderedControls.AddRange(commandControls.CreateDefaultPasswordInputGroupFor("v_APIKey", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Endpoint", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Image '{v_Bitmap}{v_FilePath}' - Store OCR Result in '{v_OutputUserVariableName}']";
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
