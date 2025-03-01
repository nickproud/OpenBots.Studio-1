﻿using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.IO;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.Data
{
	[Serializable]
	[Category("Data Commands")]
	[Description("This command reads all text from a PDF file and saves it into a variable.")]
	public class GetPDFTextCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Source Type")]
		[PropertyUISelectionOption("File Path")]
		[PropertyUISelectionOption("File URL")]
		[Description("Select source type of PDF file.")]
		[SampleUsage("")]
		[Remarks("Select 'File Path' if the file is locally placed or 'File URL' to read a file from a web URL.")]
		public string v_FileSourceType { get; set; }

		[Required]
		[DisplayName("File Path / URL")]
		[Description("Specify the local path or URL to the applicable PDF file.")]
		[SampleUsage("@\"C:\\temp\\myfile.pdf\" || ProjectPath + @\"\\myfile.pdf\" || vFilePath || https://temp.com/myfile.pdf")]
		[Remarks("Providing an invalid File Path/URL will result in an error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_FilePath { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Text Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_OutputUserVariableName { get; set; }

		public GetPDFTextCommand()
		{
			CommonMethods.InitializeDefaultWebProtocol();
			CommandName = "GetPDFTextCommand";
			SelectionName = "Get PDF Text";
			CommandEnabled = true;
			CommandIcon = Resources.command_function;

			v_FileSourceType = "File Path";
			
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			//get variable path or URL to source file
			var vSourceFilePath = (string)await v_FilePath.EvaluateCode(engine);

			if (v_FileSourceType == "File URL")
			{
				//create temp directory
				var tempDir = Folders.GetFolder(FolderType.TempFolder);
				var tempFile = Path.Combine(tempDir, $"{ Guid.NewGuid()}.pdf");

				//check if directory does not exist then create directory
				if (!Directory.Exists(tempDir))
					Directory.CreateDirectory(tempDir);

				// Create webClient to download the file for extraction
				var webclient = new WebClient();
				var uri = new Uri(vSourceFilePath);
				webclient.DownloadFile(uri, tempFile);

				// check if file is downloaded successfully
				if (File.Exists(tempFile))
					vSourceFilePath = tempFile;

				// Free not needed resources
				if (webclient != null)
					webclient.Dispose();
			}

			// Check if file exists before proceeding
			if (!File.Exists(vSourceFilePath))
				throw new FileNotFoundException("Could not find file: " + vSourceFilePath);

			PdfDocument pdfDoc = new PdfDocument(new PdfReader(vSourceFilePath));
			string result = string.Empty;
			for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
			{
				result += PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page));
			}
			pdfDoc.Close();

			result.SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			//create standard group controls
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_FileSourceType", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Extract Text From '{v_FilePath}' - Store Text in '{v_OutputUserVariableName}']";
		}
	}
}