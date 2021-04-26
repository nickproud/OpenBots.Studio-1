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
using Tasks = System.Threading.Tasks;

namespace OpenBots.Commands.Engine
{
	[Serializable]
	[Category("Engine Commands")]
	[Description("This command sets delays between the execution of commands in a running instance.")]
	public class SetEngineDelayCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Command Delay Time (Milliseconds)")]
		[Description("Select or provide a specific amount of time in milliseconds.")]
		[SampleUsage("1000 || vTime")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_EngineDelay { get; set; }

		public SetEngineDelayCommand()
		{
			CommandName = "SetEngineDelayCommand";
			SelectionName = "Set Engine Delay";
			CommandEnabled = true;
			CommandIcon = Resources.command_pause;

			v_EngineDelay = "250";
		}

		public async override Tasks.Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var delay = (int)await v_EngineDelay.EvaluateCode(engine);

			//update delay setting
			engine.EngineSettings.DelayBetweenCommands = delay;
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_EngineDelay", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Set Delay of '{v_EngineDelay} ms' Between Commands]";
		}
	}
}