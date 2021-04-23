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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.Data
{
	[Serializable]
	[Category("Data Commands")]
	[Description("This command performs a specified operation on a string to modify it.")]
	public class ModifyStringCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Text Data")]
		[Description("Provide a variable or text value.")]
		[SampleUsage("\"A sample text\" || vStringVariable")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_InputText { get; set; }

		[Required]
		[DisplayName("String Function")]
		[PropertyUISelectionOption("To Upper Case")]
		[PropertyUISelectionOption("To Lower Case")]
		[PropertyUISelectionOption("To Base64 String")]
		[PropertyUISelectionOption("From Base64 String")]
		[PropertyUISelectionOption("Trim")]
		[PropertyUISelectionOption("TrimStart")]
		[PropertyUISelectionOption("TrimEnd")]
		[Description("Select a string function to apply to the input text or variable.")]
		[SampleUsage("")]
		[Remarks("Each function, when applied to text data, converts it to a specific format.")]
		public string v_TextOperation { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Text Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_OutputUserVariableName { get; set; }

		public ModifyStringCommand()
		{
			CommandName = "ModifyStringCommand";
			SelectionName = "Modify String";
			CommandEnabled = true;
			CommandIcon = Resources.command_function;

			v_TextOperation = "To Upper Case";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var stringValue = (string)await v_InputText.EvaluateCode(engine);

			switch (v_TextOperation)
			{
				case "To Upper Case":
					stringValue = stringValue.ToUpper();
					break;
				case "To Lower Case":
					stringValue = stringValue.ToLower();
					break;
				case "To Base64 String":
					byte[] textAsBytes = Encoding.ASCII.GetBytes(stringValue);
					stringValue = Convert.ToBase64String(textAsBytes);
					break;
				case "From Base64 String":
					byte[] encodedDataAsBytes = Convert.FromBase64String(stringValue);
					stringValue = Encoding.ASCII.GetString(encodedDataAsBytes);
					break;
				case "Trim":
					stringValue = stringValue.Trim();
					break;
				case "TrimStart":
					stringValue = stringValue.TrimStart();
					break;
				case "TrimEnd":
					stringValue = stringValue.TrimEnd();
					break;
				default:
					throw new NotImplementedException("Conversion Type '" + v_TextOperation + "' not implemented!");
			}

			stringValue.SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputText", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_TextOperation", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Convert '{v_InputText}' {v_TextOperation} - Store Text in '{v_OutputUserVariableName}']";
		}
	}
}