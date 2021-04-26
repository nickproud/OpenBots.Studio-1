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
using OBFile = System.IO.File;

namespace OpenBots.Commands.TextFile
{
    [Serializable]
	[Category("Text File Commands")]
	[Description("This command writes specified data to an existing or newly created text file.")]
	public class WriteCreateTextFileCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Text File Path")]
		[Description("Enter or select the text file path.")]
		[SampleUsage("@\"C:\\temp\\sample.txt\" || ProjectPath + @\"\\temp\" || vDirectoryPath + @\\text.txt")]
		[Remarks("If the selected text file does not exist, a file with the provided name and location will be created.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFolderSelectionHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_FilePath { get; set; }

		[Required]
		[DisplayName("Text")]
		[Description("Indicate the Text to write.")]
		[SampleUsage("\"Hello World!\" || vText")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_TextToWrite { get; set; }

		[Required]
		[DisplayName("Overwrite Option")]
		[PropertyUISelectionOption("Append")]
		[PropertyUISelectionOption("Overwrite")]
		[Description("Indicate whether this command should append the text to or overwrite all existing text " +
							"in the file")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_Overwrite { get; set; }

		public WriteCreateTextFileCommand()
		{
			CommandName = "WriteCreateTextFileCommand";
			SelectionName = "Write/Create Text File";
			CommandEnabled = true;
			CommandIcon = Resources.command_files;

			v_Overwrite = "Append";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			//convert variables
			var filePath = (string)await v_FilePath.EvaluateCode(engine);
			var outputText = (string)await v_TextToWrite.EvaluateCode(engine);

			//append or overwrite as necessary
			if (v_Overwrite == "Append")
				OBFile.AppendAllText(filePath, outputText);
			else
				OBFile.WriteAllText(filePath, outputText);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TextToWrite", this, editor, 100, 300));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_Overwrite", this, editor));

			return RenderedControls;
		}


		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" ['{v_Overwrite}' to '{v_FilePath}']";
		}
	}
}