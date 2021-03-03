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
using System.IO;
using CSScriptLibrary;
using Diagnostics = System.Diagnostics;
using OBFile = System.IO.File;
namespace OpenBots.Commands.Process
{
	[Serializable]
	[Category("Programs/Process Commands")]
	[Description("This command runs a C# script and waits for it to exit before proceeding.")]

	public class RunCSharpScriptCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Script Path")]
		[Description("Enter a fully qualified path to the script, including the script extension.")]
		[SampleUsage(@"C:\temp\myscript.ps1 || {vScriptPath} || {ProjectPath}\myscript.ps1")]
		[Remarks("This command differs from *Start Process* because this command blocks execution until the script has completed. " +
				 "If you do not want to stop while the script executes, consider using *Start Process* instead.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		public string v_ScriptPath { get; set; }

		[DisplayName("Arguments (Optional)")]
		[Description("Enter any arguments as a single string.")]
		[SampleUsage("{argument1},{variable2},valueString")]
		[Remarks("This input is passed to your function as an object[].")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_ScriptArgs { get; set; }

		public RunCSharpScriptCommand()
		{
			CommandName = "RunCSharpScriptCommand";
			SelectionName = "Run C# Script";
			CommandEnabled = true;
			CommandIcon = Resources.command_script;
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			string scriptPath = v_ScriptPath.ConvertUserVariableToString(engine);
			string scriptArgs = v_ScriptArgs.ConvertUserVariableToString(engine);

			string code = OBFile.ReadAllText(scriptPath);
			var scriptMethod = CSScript.LoadDelegate<Action<object[]>>(code);

			string[] argVars = v_ScriptArgs.Split(',');
			object[] args = new object[argVars.Length];
			for (int i = 0; i < argVars.Length; i++)
			{
				argVars[i] = argVars[i].Trim();
				if (argVars[i].Contains("{"))
					args[i] = argVars[i].ConvertUserVariableToObject(engine);
				else
					args[i] = argVars[i];
			}

			scriptMethod(args);
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
			return base.GetDisplayValue() + $" [C# Script Path '{v_ScriptPath}']";
		}
	}
}
