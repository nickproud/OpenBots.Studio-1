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
	[Description("This command returns a dictionary value based on a specified key.")]
	public class GetDictionaryValueCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Dictionary")]
		[Description("Specify the dictionary variable to get a value from.")]
		[SampleUsage("vDictionary || new Dictionary<string, int>() {{ \"Hello\", 1 }}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(Dictionary<,>) })]
		public string v_InputDictionary { get; set; }

		[Required]
		[DisplayName("Key")]
		[Description("Specify the key to get the value for.")]
		[SampleUsage("\"SomeKey\" || 1 || vKey")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(object) })]
		public string v_Key { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Value Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(object) })]

		public string v_OutputUserVariableName { get; set; }

		public GetDictionaryValueCommand()
		{
			CommandName = "GetDictionaryValueCommand";
			SelectionName = "Get Dictionary Value";
			CommandEnabled = true;
			CommandIcon = Resources.command_dictionary;
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			dynamic dynamicKey = await v_Key.EvaluateCode(engine);
			dynamic dynamicDict = await v_InputDictionary.EvaluateCode(engine);

			dynamic dynamicValue = dynamicDict[dynamicKey];

			((object)dynamicValue).SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputDictionary", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Key", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [From '{v_InputDictionary}' for Key '{v_Key}' - Store Value in '{v_OutputUserVariableName}']";
		}        
	}
}