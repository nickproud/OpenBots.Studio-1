using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Properties;
using OpenBots.Core.Script;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Tasks = System.Threading.Tasks;

namespace OpenBots.Commands.Loop
{
	[Serializable]
	[Category("Loop Commands")]
	[Description("This command repeats the subsequent actions a specified number of times.")]
	public class LoopNumberOfTimesCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Loop Count")]
		[Description("Enter the amount of times you would like to execute the encased commands.")]
		[SampleUsage("5 || vLoopCount")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_LoopParameter { get; set; }

		public LoopNumberOfTimesCommand()
		{
			CommandName = "LoopNumberOfTimesCommand";
			SelectionName = "Loop Number Of Times";
			CommandEnabled = true;
			CommandIcon = Resources.command_startloop;
			ScopeStartCommand = true;
		}

		public async override Tasks.Task RunCommand(object sender, ScriptAction parentCommand)
		{
			LoopNumberOfTimesCommand loopCommand = (LoopNumberOfTimesCommand)parentCommand.ScriptCommand;
			var engine = (IAutomationEngineInstance)sender;
			int  loopTimes = (int)await loopCommand.v_LoopParameter.EvaluateCode(engine);

			for (int i = 0; i < loopTimes; i++)
			{
				engine.ReportProgress("Starting Loop Number " + (i + 1) + "/" + loopTimes + " From Line " + loopCommand.LineNumber);

				foreach (var cmd in parentCommand.AdditionalScriptCommands)
				{
					if (engine.IsCancellationPending)
						return;

					await engine.ExecuteCommand(cmd);

					if (engine.CurrentLoopCancelled)
					{
						engine.ReportProgress("Exiting Loop From Line " + loopCommand.LineNumber);
						engine.CurrentLoopCancelled = false;
						return;
					}

					if (engine.CurrentLoopContinuing)
					{
						engine.ReportProgress("Continuing Next Loop From Line " + loopCommand.LineNumber);
						engine.CurrentLoopContinuing = false;
						break;
					}
				}
				engine.ReportProgress("Finished Loop From Line " + loopCommand.LineNumber);
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_LoopParameter", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Times '{v_LoopParameter}']";
		}
	}
}