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
using System.Diagnostics;
using System.Windows.Forms;
using Diagnostics = System.Diagnostics;
namespace OpenBots.Commands.Process
{
    [Serializable]
	[Category("Programs/Process Commands")]
	[Description("This command runs a Powershell script or program and waits for it to exit before proceeding.")]

	public class RunScriptCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Script Path")]
		[Description("Enter a fully qualified path to the script, including the script extension.")]
		[SampleUsage(@"C:\temp\myscript.ps1 || {vScriptPath} || {ProjectPath}\myscript.ps1")]
		[Remarks("This command differs from *Start Process* because this command blocks execution until the script has completed. " +
				 "If you do not want to stop while the script executes, consider using *Start Process* instead.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_ScriptPath { get; set; }

		[DisplayName("Arguments (Optional)")]
		[Description("Enter any arguments as a single string.")]
		[SampleUsage("-message Hello -t 2 || {vArguments}")]
		[Remarks("This input is optional.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_ScriptArgs { get; set; }

		public RunScriptCommand()
		{
			CommandName = "RunPowershellScriptCommand";
			SelectionName = "Run Powershell Script";
			CommandEnabled = true;
			CommandIcon = Resources.command_script;
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			Diagnostics.Process scriptProc = new Diagnostics.Process();

			string scriptPath = v_ScriptPath.ConvertUserVariableToString(engine);
			string scriptArgs = v_ScriptArgs.ConvertUserVariableToString(engine);

			scriptProc.StartInfo = new ProcessStartInfo()
			{
				FileName = "powershell.exe",
				Arguments = $"-NoProfile -ExecutionPolicy unrestricted -file \"{scriptPath}\" " + scriptArgs,
				CreateNoWindow = true,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true
			};

			scriptProc.Start();
			scriptProc.WaitForExit();

			scriptProc.StandardOutput.ReadToEnd();
			scriptProc.StandardError.ReadToEnd();
			scriptProc.Close();
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ScriptPath", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ScriptArgs", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Powershell Script Path '{v_ScriptPath}']";
		}
	}
}
