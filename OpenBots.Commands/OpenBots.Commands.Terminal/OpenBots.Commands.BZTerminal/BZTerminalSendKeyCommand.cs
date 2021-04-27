using Newtonsoft.Json;
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
using System.Threading.Tasks;
using System.Windows.Forms;
using static OpenBots.Commands.Terminal.Library.TerminalKeys;

namespace OpenBots.Commands.BZTerminal
{
    [Serializable]
	[Category("BlueZone Terminal Commands")]
	[Description("This command sends advanced keystrokes to a targeted terminal screen.")]
	public class BZTerminalSendKeyKeyCommand : ScriptCommand
	{
		[Required]
		[DisplayName("BZ Terminal Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create BZ Terminal Session** command.")]
		[SampleUsage("MyBZTerminalInstance")]
		[Remarks("Failure to enter the correct instance or failure to first call the **Create BZ Terminal Session** command will cause an error.")]
		[CompatibleTypes(new Type[] { typeof(BZTerminalContext) })]
		public string v_InstanceName { get; set; }

		[DisplayName("Row Position (Optional)")]
		[Description("Input the new vertical position of the terminal. Starts from 1 at the top and increases going down.")]
		[SampleUsage("1 || vRowPosition")]
		[Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by the terminal.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_YMousePosition { get; set; }

		[DisplayName("Column Position (Optional)")]
		[Description("Input the new horizontal position of the terminal. Starts from 1 on the left and increases going right.")]
		[SampleUsage("1 || vColPosition")]
		[Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by the terminal.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_XMousePosition { get; set; }

		[Required]
		[DisplayName("Terminal Key")]
		[Description("Select the key to send to the terminal screen.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_TerminalKey { get; set; }

		public BZTerminalSendKeyKeyCommand()
		{
			CommandName = "BZTerminalSendKeyKeyCommand";
			SelectionName = "BZ Send Key";
			CommandEnabled = true;
			CommandIcon = Resources.command_system;

			v_InstanceName = "DefaultBZTerminal";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			int mouseX = 0, mouseY = 0; 
			if (!string.IsNullOrEmpty(v_XMousePosition))
				mouseX = (int)await v_XMousePosition.EvaluateCode(engine);

			if (!string.IsNullOrEmpty(v_YMousePosition))
				mouseY = (int)await v_YMousePosition.EvaluateCode(engine);

			var terminalContext = (BZTerminalContext)v_InstanceName.GetAppInstance(engine);

			if (terminalContext.BZTerminalObj == null || !terminalContext.BZTerminalObj.Connected)
				throw new Exception($"Terminal Instance {v_InstanceName} is not connected.");

			BZ3270Keys selectedKey = (BZ3270Keys)Enum.Parse(typeof(BZ3270Keys), v_TerminalKey);
			string selectedKeyValue = BZTerminalKeysDict[selectedKey];

			if (!string.IsNullOrEmpty(v_XMousePosition) && !string.IsNullOrEmpty(v_YMousePosition))
				terminalContext.BZTerminalObj.SetCursor(mouseY, mouseX);

			terminalContext.BZTerminalObj.SendKey(selectedKeyValue);
			terminalContext.BZTerminalObj.WaitForReady();
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_YMousePosition", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_XMousePosition", this, editor));

			var terminalKeyNameLabel = commandControls.CreateDefaultLabelFor("v_TerminalKey", this);
			var terminalKeyNameComboBox = (ComboBox)commandControls.CreateDropdownFor("v_TerminalKey", this);
			terminalKeyNameComboBox.DataSource = Enum.GetValues(typeof(BZ3270Keys));

			RenderedControls.Add(terminalKeyNameLabel);
			RenderedControls.Add(terminalKeyNameComboBox);

			return RenderedControls;
		}
	 
		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Terminal Key '{v_TerminalKey}' to Row/Col '{{{v_YMousePosition}, {v_XMousePosition}}}' - Instance Name '{v_InstanceName}']";
		}
	}
}