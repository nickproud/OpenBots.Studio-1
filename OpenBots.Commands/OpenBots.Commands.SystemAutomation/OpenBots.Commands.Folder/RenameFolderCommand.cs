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
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.Folder
{
	[Serializable]
	[Category("Folder Operation Commands")]
	[Description("This command renames an existing folder.")]
	public class RenameFolderCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Folder Path")]
		[Description("Enter or Select the path to the folder.")]
		[SampleUsage("@\"C:\\temp\" || ProjectPath + @\"\\temp\" || vDirectoryPath")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFolderSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_SourceFolderPath { get; set; }

		[Required]
		[DisplayName("New Folder Name")]
		[Description("Specify the new folder name.")]
		[SampleUsage("\"New Folder Name\" || vNewFolderName")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_NewName { get; set; }

		public RenameFolderCommand()
		{
			CommandName = "RenameFolderCommand";
			SelectionName = "Rename Folder";
			CommandEnabled = true;
			CommandIcon = Resources.command_folders;
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			//apply variable logic
			var sourceFolder = (string)await v_SourceFolderPath.EvaluateCode(engine);
			var newFolderName = (string)await v_NewName.EvaluateCode(engine);

			if (!Directory.Exists(sourceFolder))
				throw new DirectoryNotFoundException($"Directory {sourceFolder} does not exist");

			//get source folder name and info
			DirectoryInfo sourceFolderInfo = new DirectoryInfo(sourceFolder);

			//create destination
			var destinationPath = Path.Combine(sourceFolderInfo.Parent.FullName, newFolderName);

			//rename folder
			Directory.Move(sourceFolder, destinationPath);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SourceFolderPath", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_NewName", this, editor));
			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Rename '{v_SourceFolderPath}' to '{v_NewName}']";
		}
	}
}