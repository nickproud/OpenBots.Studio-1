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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using IO = System.IO;

namespace OpenBots.Commands.File
{
	[Serializable]
	[Category("File Operation Commands")]
	[Description("This command waits for a file to exist at a specified destination.")]
	public class WaitForFileCommand : ScriptCommand
	{
		[Required]
		[DisplayName("File Path")]
		[Description("Enter or Select the path to the file.")]
		[SampleUsage("@\"C:\\temp\\myfile.txt\" || ProjectPath + @\"\\myfile.txt\" || vFilePath")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_FileName { get; set; }

		[Required]
		[DisplayName("Timeout")]
		[Description("Specify how many seconds to wait for the file to exist.")]
		[SampleUsage("10 || vSeconds")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_WaitTime { get; set; }

		public WaitForFileCommand()
		{
			CommandName = "WaitForFileCommand";
			SelectionName = "Wait For File";
			CommandEnabled = true;
			CommandIcon = Resources.command_files;

		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			//convert items to variables
			var fileName = (string)await v_FileName.EvaluateCode(engine);
			var pauseTime = (int)await v_WaitTime.EvaluateCode(engine);

			//determine when to stop waiting based on user config
			var stopWaiting = DateTime.Now.AddSeconds(pauseTime);

			//initialize flag for file found
			var fileFound = false;

			//while file has not been found
			while (!fileFound)
			{
				//if file exists at the file path
				if (IO.File.Exists(fileName))
					fileFound = true;

				//test if we should exit and throw exception
				if (DateTime.Now > stopWaiting)
					throw new IO.FileNotFoundException("File was not found in time!");

				//put thread to sleep before iterating
				Thread.Sleep(100);
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FileName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_WaitTime", this, editor));
			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Wait '{v_WaitTime}' Seconds for File '{v_FileName}' to Exist]";
		}
	}
}