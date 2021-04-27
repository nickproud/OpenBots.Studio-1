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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.Terminal
{
    [Serializable]
	[Category("Terminal Commands")]
	[Description("This command sets the cursor in a targeted terminal screen.")]
	public class TerminalSetCursorCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Terminal Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create Terminal Session** command.")]
		[SampleUsage("MyTerminalInstance")]
		[Remarks("Failure to enter the correct instance or failure to first call the **Create Terminal Session** command will cause an error.")]
		[CompatibleTypes(new Type[] { typeof(OpenEmulator) })]
		public string v_InstanceName { get; set; }

		[Required]
		[DisplayName("Row Position")]
		[Description("Input the new vertical position of the terminal. Starts from 0 at the top and increases going down.")]
		[SampleUsage("0 || vRowPosition")]
		[Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by the terminal.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_YMousePosition { get; set; }

		[Required]
		[DisplayName("Column Position")]
		[Description("Input the new horizontal position of the terminal. Starts from 0 on the left and increases going right.")]
		[SampleUsage("0 || vColPosition")]
		[Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by the terminal.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_XMousePosition { get; set; }

		public TerminalSetCursorCommand()
		{
			CommandName = "TerminalSetCursorCommand";
			SelectionName = "Set Cursor";
			CommandEnabled = true;
			CommandIcon = Resources.command_system;
			v_InstanceName = "DefaultTerminal";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var mouseX = (int)await v_XMousePosition.EvaluateCode(engine);
			var mouseY = (int)await v_YMousePosition.EvaluateCode(engine);
			var terminalObject = (OpenEmulator)v_InstanceName.GetAppInstance(engine);

			if (terminalObject.TN3270 == null || !terminalObject.TN3270.IsConnected)
				throw new Exception($"Terminal Instance {v_InstanceName} is not connected.");

			terminalObject.TN3270.SetCursor(mouseX, mouseY);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_YMousePosition", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_XMousePosition", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Row/Col '{{{v_YMousePosition}, {v_XMousePosition}}}' - Instance Name '{v_InstanceName}']";
		}     
	}
}