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
    [Description("This command removes a value from an existing Dictionary variable at a specified key.")]
    public class RemoveDictionaryValueCommand : ScriptCommand
    {
		[Required]
		[DisplayName("Dictionary")]
		[Description("Provide a Dictionary variable.")]
		[SampleUsage("vDictionary || new Dictionary<string, int>() {{ \"Hello\", 1 }}")]
		[Remarks("Any type of variable other than Dictionary will cause error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(Dictionary<,>) })]
		public string v_DictionaryName { get; set; }

		[Required]
		[DisplayName("Key")]
		[Description("Enter key where the value will be removed")]
		[SampleUsage("\"Hello\" || 1 || vKey")]
		[Remarks("Providing a non existing key will produce an exception.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(object) })]
		public string v_Key { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Dictionary Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(Dictionary<,>) })]
		public string v_OutputUserVariableName { get; set; }

		public RemoveDictionaryValueCommand()
		{
			CommandName = "RemoveDictionaryValueCommand";
			SelectionName = "Remove Dictionary Value";
			CommandEnabled = true;
			CommandIcon = Resources.command_dictionary;
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			dynamic dynamicDict = await v_DictionaryName.EvaluateCode(engine);
			dynamic dynamicKey = await v_Key.EvaluateCode(engine);

			dynamicDict[dynamicKey] = null;

			((object)dynamicDict).SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DictionaryName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Key", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [From Dictionary '{v_DictionaryName}' at Key '{v_Key}' - Store Dictionary in '{v_OutputUserVariableName}']";
		}
	}
}
