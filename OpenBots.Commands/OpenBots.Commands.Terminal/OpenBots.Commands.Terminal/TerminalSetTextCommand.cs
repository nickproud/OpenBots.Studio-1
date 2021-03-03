using OpenBots.Commands.Terminal.Forms;
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

namespace OpenBots.Commands.Terminal
{
    [Serializable]
	[Category("Terminal Commands")]
	[Description("This command sets text in a targeted terminal screen.")]
	public class TerminalSetTextCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Terminal Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create Terminal Session** command.")]
		[SampleUsage("MyTerminalInstance")]
		[Remarks("Failure to enter the correct instance or failure to first call the **Create Terminal Session** command will cause an error.")]
		public string v_InstanceName { get; set; }

		[DisplayName("Row Position (Optional)")]
		[Description("Input the new vertical position of the terminal. Starts from 0 at the top and increases going down.")]
		[SampleUsage("0 || {vRowPosition}")]
		[Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by the terminal.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_YMousePosition { get; set; }

		[DisplayName("Column Position (Optional)")]
		[Description("Input the new horizontal position of the terminal. Starts from 0 on the left and increases going right.")]
		[SampleUsage("0 || {vColPosition}")]
		[Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by the terminal.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_XMousePosition { get; set; }

		[Required]
		[DisplayName("Text to Set")]
		[Description("Enter the text to be sent to the specified terminal.")]
		[SampleUsage("Hello, World! || {vText}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_TextToSet { get; set; }

		[Required]
		[DisplayName("Timeout (Seconds)")]
		[Description("Specify how many seconds to wait before throwing an exception.")]
		[SampleUsage("30 || {vSeconds}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_Timeout { get; set; }

		public TerminalSetTextCommand()
		{
			CommandName = "TerminalSetTextCommand";
			SelectionName = "Set Text";
			CommandEnabled = true;
			CommandIcon = Resources.command_system;
			v_InstanceName = "DefaultTerminal";
			v_Timeout = "30";
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var mouseX = v_XMousePosition.ConvertUserVariableToString(engine);
			var mouseY = v_YMousePosition.ConvertUserVariableToString(engine);
			string textToSend = v_TextToSet.ConvertUserVariableToString(engine);
			var timeout = int.Parse(v_Timeout.ConvertUserVariableToString(engine));
			var terminalObject = (OpenEmulator)v_InstanceName.GetAppInstance(engine);

			if (terminalObject.TN3270 == null || !terminalObject.TN3270.IsConnected)
				throw new Exception($"Terminal Instance {v_InstanceName} is not connected.");

			if (!string.IsNullOrEmpty(mouseX) && !string.IsNullOrEmpty(mouseY))
				terminalObject.TN3270.SetCursor(int.Parse(mouseX), int.Parse(mouseY));

			terminalObject.TN3270.SetText(textToSend);
			terminalObject.TN3270.WaitForTextOnScreen(timeout, textToSend);
			terminalObject.Redraw();
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_YMousePosition", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_XMousePosition", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TextToSet", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Timeout", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Text '{v_TextToSet}' at Row/Col '{{{v_YMousePosition}, {v_XMousePosition}}}' - Instance Name '{v_InstanceName}']";
		}     
	}
}