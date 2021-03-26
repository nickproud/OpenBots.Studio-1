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
using OBFile = System.IO.File;

namespace OpenBots.Commands.TextFile
{
	[Serializable]
	[Category("Text File Commands")]
	[Description("This command reads text data from a text file and stores it in a variable.")]
	public class ReadTextFileCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Text File Path")]
		[Description("Enter or select the path to the text file.")]
		[SampleUsage(@"C:\temp\myfile.txt || {ProjectPath}\myText.txt || {vTextFilePath}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_FilePath { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Text Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_OutputUserVariableName { get; set; }

		public ReadTextFileCommand()
		{
			CommandName = "ReadTextFileCommand";
			SelectionName = "Read Text File";
			CommandEnabled = true;
			CommandIcon = Resources.command_files;

		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			//convert variables
			var filePath = v_FilePath.ConvertUserVariableToString(engine);
			//read text from file
			var textFromFile = OBFile.ReadAllText(filePath);
			//assign text to user variable
			textFromFile.StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));
			RenderedControls.AddRange(
				commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor)
			);

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Read Text From '{v_FilePath}' - Store Text in '{v_OutputUserVariableName}']";
		}
	}
}