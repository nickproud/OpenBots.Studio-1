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
	[Description("This command moves/copies a file to a specified destination.")]
	public class MoveCopyFileCommand : ScriptCommand
	{
		[Required]
		[DisplayName("File Operation Type")]
		[PropertyUISelectionOption("Move File")]
		[PropertyUISelectionOption("Copy File")]
		[Description("Specify whether you intend to move the file or copy the file.")]
		[SampleUsage("")]
		[Remarks("Moving will remove the file from the original path while Copying will not.")]
		public string v_OperationType { get; set; }

		[Required]
		[DisplayName("Source File Path")]
		[Description("Enter or Select the path to the file.")]
		[SampleUsage("@\"C:\\temp\\myfile.txt\" || ProjectPath + @\"\\myfile.txt\" || vFilePath")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_SourceFilePath { get; set; }

		[Required]
		[DisplayName("Destination File Path")]
		[Description("Enter or Select the new (destination) path to the file.")]
		[SampleUsage("@\"C:\\temp\" || ProjectPath + @\"\\temp\" || vDirectoryPath")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFolderSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_DestinationDirectory { get; set; }

		[Required]
		[DisplayName("Create Folder")]
		[PropertyUISelectionOption("Yes")]
		[PropertyUISelectionOption("No")]
		[Description("Specify whether the directory should be created if it does not already exist.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_CreateDirectory { get; set; }

		[Required]
		[DisplayName("Overwrite File")]
		[PropertyUISelectionOption("Yes")]
		[PropertyUISelectionOption("No")]
		[Description("Specify whether the file should be overwritten if it already exists.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_OverwriteFile { get; set; }

		public MoveCopyFileCommand()
		{
			CommandName = "MoveCopyFileCommand";
			SelectionName = "Move/Copy File";
			CommandEnabled = true;
			CommandIcon = Resources.command_files;

			v_CreateDirectory = "Yes";
			v_OverwriteFile = "Yes";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			//apply variable logic
			var sourceFile = (string)await v_SourceFilePath.EvaluateCode(engine);
			var destinationFolder = (string)await v_DestinationDirectory.EvaluateCode(engine);

            if (!IO.File.Exists(sourceFile))
            {
				throw new IO.FileNotFoundException($"File {sourceFile} does not exist");
            }

			if ((v_CreateDirectory == "Yes") && (!IO.Directory.Exists(destinationFolder)))
			{
				IO.Directory.CreateDirectory(destinationFolder);
			}
			else if ((v_CreateDirectory == "No") && (!IO.Directory.Exists(destinationFolder)))
            {
				throw new IO.DirectoryNotFoundException($"Directory {destinationFolder} does not exist");
            }

			//get source file name and info
			IO.FileInfo sourceFileInfo = new IO.FileInfo(sourceFile);

			//create destination
			var destinationPath = IO.Path.Combine(destinationFolder, sourceFileInfo.Name);

			//delete if it already exists per user
			if (v_OverwriteFile == "Yes")
			{
				IO.File.Delete(destinationPath);
			}

			if (v_OperationType == "Move File")
			{
				//move file
				IO.File.Move(sourceFile, destinationPath);
			}
			else
			{
				//copy file
				IO.File.Copy(sourceFile, destinationPath);
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_OperationType", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SourceFilePath", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DestinationDirectory", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_CreateDirectory", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_OverwriteFile", this, editor));
			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [{v_OperationType} From '{v_SourceFilePath}' to '{v_DestinationDirectory}']";
		}
	}
}