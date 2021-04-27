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
	[Description("This command returns a substring from a specified string.")]
	public class SubstringCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Text Data")]
		[Description("Provide a variable or text value.")]
		[SampleUsage("\"Hello World\" || vTextData")]
		[Remarks("Providing data of a type other than a 'String' will result in an error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_InputText { get; set; }

		[Required]
		[DisplayName("Starting Index")]
		[Description("Indicate the starting position within the text.")]
		[SampleUsage("0 || 1 || vStartingIndex")]
		[Remarks("0 for beginning, 1 for first character, n for nth character")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_StartIndex { get; set; }

		[DisplayName("Substring Length (Optional)")]
		[Description("Indicate number of characters to extract.")]
		[SampleUsage("1 || vSubstringLength")]
		[Remarks("1 for 1 position after start index, etc.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_StringLength { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Substring Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_OutputUserVariableName { get; set; }

		public SubstringCommand()
		{
			CommandName = "SubstringCommand";
			SelectionName = "Substring";
			CommandEnabled = true;
			CommandIcon = Resources.command_string;
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var inputText = (string)await v_InputText.EvaluateCode(engine);
			var startIndex = (int)await v_StartIndex.EvaluateCode(engine);

			int length = -1;

			if(!string.IsNullOrEmpty(v_StringLength))
				length = (int)await v_StringLength.EvaluateCode(engine);

			//apply substring
			if (length > -1)
				inputText = inputText.Substring(startIndex, length);
			else
				inputText = inputText.Substring(startIndex);

			inputText.SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputText", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_StartIndex", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_StringLength", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Get Substring From '{v_InputText}' - " +
				$"Store Substring in '{v_OutputUserVariableName}']";
		}
	}
}