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
using Diagnostics = System.Diagnostics;
using OBFile = System.IO.File;

namespace OpenBots.Commands.Process
{
    [Serializable]
	[Category("Programs/Process Commands")]
	[Description("This command stops a program or process.")]
	public class StopProcessCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Program Name or Path")]
		[Description("Provide a valid program name or enter a full path to the script/executable including the extension.")]
		[SampleUsage("\"notepad\" || \"excel\" || vAppName || @\"C:\\temp\\myapp.exe || ProjectPath + @\"\\myapp.exe\" || vAppPath")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_ProgramName { get; set; }

		[Required]
		[DisplayName("Stop Option")]
		[PropertyUISelectionOption("Close")]
		[PropertyUISelectionOption("Kill")]
		[Description("Indicate whether the program should be closed or killed.")]
		[SampleUsage("")]
		[Remarks("*Close* will close any open process windows while *Kill* will close all processes, including background ones.")]
		public string v_StopOption { get; set; }

		public StopProcessCommand()
		{
			CommandName = "StopProcessCommand";
			SelectionName = "Stop Process";
			CommandEnabled = true;
			CommandIcon = Resources.command_stop_process;

			v_StopOption = "Kill";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			string vProgramName = (string)await v_ProgramName.EvaluateCode(engine);

			if (OBFile.Exists(vProgramName))
				vProgramName = Path.GetFileNameWithoutExtension(vProgramName);

			var processes = Diagnostics.Process.GetProcessesByName(vProgramName);

			foreach (var prc in processes)
			{
				if (v_StopOption == "Close")
					prc.CloseMainWindow();
				else if (v_StopOption == "Kill")
					prc.Kill();
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ProgramName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_StopOption", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [{v_StopOption} Process '{v_ProgramName}']";
		}
	}
}