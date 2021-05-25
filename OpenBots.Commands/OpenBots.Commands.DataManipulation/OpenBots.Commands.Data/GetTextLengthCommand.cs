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

namespace OpenBots.Commands.Data
{
	[Serializable]
	[Category("Data Commands")]
	[Description("This command returns the length of a string.")]
	public class GetTextLengthCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Text Data")]
		[Description("Provide a variable or text value.")]
		[SampleUsage("\"Hello World\" || vStringVariable")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_InputValue { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Length Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_OutputUserVariableName { get; set; }

		public GetTextLengthCommand()
		{
			CommandName = "GetTextLengthCommand";
			SelectionName = "Get Text Length";
			CommandEnabled = true;
			CommandIcon = Resources.command_function;
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var stringRequiringLength = (string)await v_InputValue.EvaluateCode(engine);

			var stringLength = stringRequiringLength.Length;

			stringLength.SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputValue", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Of Text '{v_InputValue}' - Store Length in '{v_OutputUserVariableName}']";
		}
	}
}