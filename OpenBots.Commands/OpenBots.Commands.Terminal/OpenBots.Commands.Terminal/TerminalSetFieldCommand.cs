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
	[Description("This command sets text at a targeted terminal screen's field index.")]
	public class TerminalSetFieldCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Terminal Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create Terminal Session** command.")]
		[SampleUsage("MyTerminalInstance")]
		[Remarks("Failure to enter the correct instance or failure to first call the **Create Terminal Session** command will cause an error.")]
		[CompatibleTypes(new Type[] { typeof(OpenEmulator) })]
		public string v_InstanceName { get; set; }

		[Required]
		[DisplayName("Field Index")]
		[Description("Enter the index of the field to set text in.")]
		[SampleUsage("0 || vFieldIndex")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_FieldIndex { get; set; }

		[Required]
		[DisplayName("Text to Set")]
		[Description("Enter the text to be sent to the specified terminal.")]
		[SampleUsage("\"Hello, World!\" || vText")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_TextToSet { get; set; }

		public TerminalSetFieldCommand()
		{
			CommandName = "TerminalSetFieldCommand";
			SelectionName = "Set Field";
			CommandEnabled = true;
			CommandIcon = Resources.command_system;

			v_InstanceName = "DefaultTerminal";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var fieldIndex = (int)await v_FieldIndex.EvaluateCode(engine);
			string textToSend = (string)await v_TextToSet.EvaluateCode(engine);
			var terminalObject = (OpenEmulator)v_InstanceName.GetAppInstance(engine);

			if (terminalObject.TN3270 == null || !terminalObject.TN3270.IsConnected)
				throw new Exception($"Terminal Instance {v_InstanceName} is not connected.");

			var field = terminalObject.TN3270.CurrentScreenXML.Fields[fieldIndex];
			terminalObject.TN3270.SetCursor(field.Location.left, field.Location.top);
			terminalObject.TN3270.SetText(textToSend);

			terminalObject.Redraw();
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FieldIndex", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TextToSet", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Field Index - {v_FieldIndex} - Text '{v_TextToSet}' - Instance Name '{v_InstanceName}']";
		}     
	}
}