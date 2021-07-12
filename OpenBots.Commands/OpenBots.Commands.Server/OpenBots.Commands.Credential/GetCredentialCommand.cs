using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security;
using System.Windows.Forms;
using OpenBots.Core.Properties;
using System.Threading.Tasks;
using OpenBots.Server.SDK.HelperMethods;
using OpenBots.Commands.Server.Library;

namespace OpenBots.Commands.Credential
{
	[Serializable]
	[Category("Credential Commands")]
	[Description("This command gets a Credential from OpenBots Server.")]
	public class GetCredentialCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Credential Name")]
		[Description("Enter the name of the Credential.")]
		[SampleUsage("\"Name\" || vCredentialName")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_CredentialName { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Username Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_OutputUserVariableName { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Password Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(SecureString) })]
		public string v_OutputUserVariableName2 { get; set; }

		public GetCredentialCommand()
		{
			CommandName = "GetCredentialCommand";
			SelectionName = "Get Credential";
			CommandEnabled = true;
			CommandIcon = Resources.command_asset;

			CommonMethods.InitializeDefaultWebProtocol();
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var vCredentialName = (string)await v_CredentialName.EvaluateCode(engine);

			var userInfo = ServerSessionVariableMethods.GetUserInfo(engine);
			var credential = CredentialMethods.GetCredential(userInfo, vCredentialName);

			if (credential == null)
                throw new Exception($"No Credential was found for '{vCredentialName}'");

            string username = credential.UserName;
            SecureString password = credential.PasswordSecret.ConvertStringToSecureString();

			username.SetVariableValue(engine, v_OutputUserVariableName);
			password.SetVariableValue(engine, v_OutputUserVariableName2);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_CredentialName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName2", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			  return base.GetDisplayValue() + $" ['{v_CredentialName}' - Store Username in '{v_OutputUserVariableName}' and Password in '{v_OutputUserVariableName2}']";
		}       
	}
}