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
	[Description("This command sets a stored password in a targeted terminal screen.")]
	public class BZTerminalSetPasswordCommand : ScriptCommand
	{
		[Required]
		[DisplayName("BZ Terminal Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create BZ Terminal Session** command.")]
		[SampleUsage("MyBZTerminalInstance")]
		[Remarks("Failure to enter the correct instance or failure to first call the **Create BZ Terminal Session** command will cause an error.")]
		public string v_InstanceName { get; set; }

		[DisplayName("Row Position (Optional)")]
		[Description("Input the new vertical position of the terminal. Starts from 1 at the top and increases going down.")]
		[SampleUsage("1 || {vRowPosition}")]
		[Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by the terminal.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_YMousePosition { get; set; }

		[DisplayName("Column Position (Optional)")]
		[Description("Input the new horizontal position of the terminal. Starts from 1 on the left and increases going right.")]
		[SampleUsage("1 || {vColPosition}")]
		[Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by the terminal.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_XMousePosition { get; set; }

		[Required]
		[DisplayName("Timeout (Seconds)")]
		[Description("Specify how many seconds to wait before throwing an exception.")]
		[SampleUsage("30 || {vSeconds}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_Timeout { get; set; }

		public BZTerminalSetPasswordCommand()
		{
			CommandName = "BZTerminalSetPasswordCommand";
			SelectionName = "BZ Set Password";
			CommandEnabled = true;
			CommandIcon = Resources.command_system;
			v_InstanceName = "DefaultBZTerminal";
			v_Timeout = "30";
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var mouseX = v_XMousePosition.ConvertUserVariableToString(engine);
			var mouseY = v_YMousePosition.ConvertUserVariableToString(engine);
			var timeout = int.Parse(v_Timeout.ConvertUserVariableToString(engine));
			var terminalContext = (BZTerminalContext)v_InstanceName.GetAppInstance(engine);

			if (terminalContext.BZTerminalObj == null || !terminalContext.BZTerminalObj.Connected)
				throw new Exception($"Terminal Instance {v_InstanceName} is not connected.");

			if (!string.IsNullOrEmpty(mouseX) && !string.IsNullOrEmpty(mouseY))
				terminalContext.BZTerminalObj.SetCursor(int.Parse(mouseY), int.Parse(mouseX));

			terminalContext.BZTerminalObj.SendKey(terminalContext.Password.ConvertSecureStringToString());
			terminalContext.BZTerminalObj.WaitForText(terminalContext.Password.ConvertSecureStringToString(), 1, 1, timeout);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_YMousePosition", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_XMousePosition", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Timeout", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Row/Col '{{{v_YMousePosition}, {v_XMousePosition}}}' - Instance Name '{v_InstanceName}']";
		}     
	}
}