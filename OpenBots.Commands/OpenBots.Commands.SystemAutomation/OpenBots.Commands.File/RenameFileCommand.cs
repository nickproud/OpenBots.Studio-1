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
	[Description("This command renames an existing file.")]
	public class RenameFileCommand : ScriptCommand
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

		[Required]
		[DisplayName("New File Name (with extension)")]
		[Description("Specify new file name with extension.")]
		[SampleUsage("@\"newfile.txt\" || vNewFileName")]
		[Remarks("Changing the file extension will not automatically convert files.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_NewName { get; set; }

		public RenameFileCommand()
		{
			CommandName = "RenameFileCommand";
			SelectionName = "Rename File";
			CommandEnabled = true;
			CommandIcon = Resources.command_files;

		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			//apply variable logic
			var sourceFile = (string)await v_SourceFilePath.EvaluateCode(engine);
			var newFileName = (string)await v_NewName.EvaluateCode(engine);

			if (!IO.File.Exists(sourceFile))
            {
				throw new IO.FileNotFoundException($"File {sourceFile} does not exist");
            }

			//get source file name and info
			IO.FileInfo sourceFileInfo = new IO.FileInfo(sourceFile);

			//create destination
			var destinationPath = IO.Path.Combine(sourceFileInfo.DirectoryName, newFileName);

			//rename file
			IO.File.Move(sourceFile, destinationPath);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SourceFilePath", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_NewName", this, editor));
			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Rename '{v_SourceFilePath}' to '{v_NewName}']";
		}
	}
}