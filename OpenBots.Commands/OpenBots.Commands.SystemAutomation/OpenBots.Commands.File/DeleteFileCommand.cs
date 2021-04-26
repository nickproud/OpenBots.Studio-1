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
using IO = System.IO;

namespace OpenBots.Commands.File
{
	[Serializable]
	[Category("File Operation Commands")]
	[Description("This command deletes a file from a specified destination.")]
	public class DeleteFileCommand : ScriptCommand
	{
		[Required]
		[DisplayName("File Path")]
		[Description("Enter or Select the path to the file.")]
		[SampleUsage("@\"C:\\temp\\myfile.txt\" || ProjectPath + @\"\\myfile.txt\" || vFilePath")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_SourceFilePath { get; set; }

		public DeleteFileCommand()
		{
			CommandName = "DeleteFileCommand";
			SelectionName = "Delete File";
			CommandEnabled = true;
			CommandIcon = Resources.command_files;
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			//apply variable logic
			var sourceFile = (string)await v_SourceFilePath.EvaluateCode(engine);

			if (!IO.File.Exists(sourceFile))
				throw new IO.FileNotFoundException($"{sourceFile} is not a valid file path");

			//delete file
			IO.File.Delete(sourceFile);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SourceFilePath", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Delete '{v_SourceFilePath}']";
		}
	}
}