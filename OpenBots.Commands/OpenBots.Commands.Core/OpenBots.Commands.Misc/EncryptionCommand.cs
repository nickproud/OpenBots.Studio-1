using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommandUtilities;
using OpenBots.Core.Utilities.CommonUtilities;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Tasks = System.Threading.Tasks;

namespace OpenBots.Commands.Misc
{
	[Serializable]
	[Category("Misc Commands")]
	[Description("This command encrypts or decrypts text.")]
	public class EncryptionCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Encryption Action")]
		[PropertyUISelectionOption("Encrypt")]
		[PropertyUISelectionOption("Decrypt")]
		[Description("Select the appropriate action to take.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_EncryptionType { get; set; }

		[Required]
		[DisplayName("Text")]
		[Description("Select or provide the text to encrypt/decrypt.")]
		[SampleUsage("\"Hello\" || vText")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_InputValue { get; set; }

		[Required]
		[DisplayName("Pass Phrase")]
		[Description("Select or provide a pass phrase for encryption/decryption.")]
		[SampleUsage("\"OPENBOTS\" || vPassPhrase")]
		[Remarks("If decrypting, provide the pass phrase used to encypt the original text.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_PassPhrase { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Result Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_OutputUserVariableName { get; set; }

		public EncryptionCommand()
		{
			CommandName = "EncryptionCommand";
			SelectionName = "Encryption Command";
			CommandEnabled = true;
			CommandIcon = Resources.command_password;

			v_EncryptionType = "Encrypt";
		}

		public async override Tasks.Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			var variableInput = (string)await v_InputValue.EvaluateCode(engine);
			var passphrase = (string)await v_PassPhrase.EvaluateCode(engine);

			string resultData = "";
			if (v_EncryptionType == "Encrypt")
				resultData = EncryptionServices.EncryptString(variableInput, passphrase);
			else if (v_EncryptionType == "Decrypt")
				resultData = EncryptionServices.DecryptString(variableInput, passphrase);

			resultData.SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_EncryptionType", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputValue", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultPasswordInputGroupFor("v_PassPhrase", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [{v_EncryptionType} '{v_InputValue}' - Store Result in '{v_OutputUserVariableName}']";
		}
	}
}