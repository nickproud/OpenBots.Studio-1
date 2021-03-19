using OpenBots.Commands.Terminal.Library;
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

namespace OpenBots.Commands.BZTerminal
{
    [Serializable]
	[Category("BlueZone Terminal Commands")]
	[Description("This command waits until the terminal screen is ready for keyboard input.")]
	public class BZTerminalWaitReadyCommand : ScriptCommand
	{
		[Required]
		[DisplayName("BZ Terminal Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create BZ Terminal Session** command.")]
		[SampleUsage("MyBZTerminalInstance")]
		[Remarks("Failure to enter the correct instance or failure to first call the **Create BZ Terminal Session** command will cause an error.")]
		[CompatibleTypes(new Type[] { typeof(BZTerminalContext) })]
		public string v_InstanceName { get; set; }

		[Required]
		[DisplayName("Timeout (Seconds)")]
		[Description("Specify how many seconds to wait before throwing an exception.")]
		[SampleUsage("30 || {vSeconds}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_Timeout { get; set; }

		[Required]
		[DisplayName("Extra Wait (Seconds)")]
		[Description("Specify the number of seconds to validate for a keyboard unlocked status.")]
		[SampleUsage("1 || {vSeconds}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_ExtraWait { get; set; }

		public BZTerminalWaitReadyCommand()
		{
			CommandName = "BZTerminalWaitReadyCommand";
			SelectionName = "BZ Wait Ready";
			CommandEnabled = true;
			CommandIcon = Resources.command_system;
			v_InstanceName = "DefaultBZTerminal";
			v_Timeout = "30";
			v_ExtraWait = "1";
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var timeout = int.Parse(v_Timeout.ConvertUserVariableToString(engine));
			var extrawait = int.Parse(v_ExtraWait.ConvertUserVariableToString(engine)) * 1000;
			var terminalContext = (BZTerminalContext)v_InstanceName.GetAppInstance(engine);

			if (terminalContext.BZTerminalObj == null || !terminalContext.BZTerminalObj.Connected)
				throw new Exception($"Terminal Instance {v_InstanceName} is not connected.");

			terminalContext.BZTerminalObj.WaitReady(timeout, extrawait);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Timeout", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ExtraWait", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Instance Name '{v_InstanceName}']";
		}     
	}
}