using Microsoft.Office.Interop.Outlook;
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

namespace OpenBots.Commands.Dictionary
{
    [Serializable]
	[Category("Dictionary Commands")]
	[Description("This command returns a value from a KeyValuePair.")]
	class GetValueFromKeyValuePairCommand : ScriptCommand
	{
		[Required]
		[DisplayName("KeyValuePair")]
		[Description("Specify the KeyValuePair variable to get a value from.")]
		[SampleUsage("vKeyValuePair")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(KeyValuePair<,>) })]
		public string v_InputKeyValuePair { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Value Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(object) })]
		public string v_OutputUserVariableName { get; set; }

		public GetValueFromKeyValuePairCommand()
		{
			CommandName = "GetValueFromKeyValuePairCommand";
			SelectionName = "Get Value From KeyValuePair";
			CommandEnabled = true;
			CommandIcon = Resources.command_dictionary;
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			dynamic dynamicKVPair = await v_InputKeyValuePair.EvaluateCode(engine);
			dynamic dynamicValue = dynamicKVPair.Value;

			((object)dynamicValue).SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputKeyValuePair", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Store Value From '{v_InputKeyValuePair}' in '{v_OutputUserVariableName}']";
		}
	}
}
