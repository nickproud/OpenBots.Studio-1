using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using OpenBots.Core.Properties;
using System.Threading.Tasks;
using System.Security;
using OpenBots.Commands.Server.HelperMethods;

namespace OpenBots.Commands.Credential
{
	[Serializable]
	[Category("Credential Commands")]
	[Description("This command updates a Credential in OpenBots Server.")]
	public class UpdateCredentialCommand : ScriptCommand
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
		[DisplayName("Credential Username")]
		[Description("Enter the Credential username.")]
		[SampleUsage("\"john@openbots.com\" || vCredentialUsername")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_CredentialUsername { get; set; }

		[Required]
		[DisplayName("Credential Password")]
		[Description("Enter the Credential password.")]
		[SampleUsage("vPassword")]
		[Remarks("Password input must be a SecureString variable.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(SecureString) })]
		public string v_CredentialPassword { get; set; }



		public UpdateCredentialCommand()
		{
			CommandName = "UpdateCredentialCommand";
			SelectionName = "Update Credential";
			CommandEnabled = true;
			CommandIcon = Resources.command_asset;

			CommonMethods.InitializeDefaultWebProtocol();
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var vCredentialName = (string)await v_CredentialName.EvaluateCode(engine);
			var vCredentialUsername = (string)await v_CredentialUsername.EvaluateCode(engine);
			var vCredentialPassword = ((SecureString)await v_CredentialPassword.EvaluateCode(engine)).ConvertSecureStringToString();

			var userInfo = AuthMethods.GetUserInfo();
			var credential = CredentialMethods.GetCredential(userInfo.Token, userInfo.ServerUrl, userInfo.OrganizationId, vCredentialName);

			if (credential == null)
				throw new Exception($"No Credential was found for '{vCredentialName}'");

            credential.UserName = vCredentialUsername;
            credential.PasswordSecret = vCredentialPassword;

            CredentialMethods.PutCredential(userInfo.Token, userInfo.ServerUrl, userInfo.OrganizationId, credential);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_CredentialName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_CredentialUsername", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_CredentialPassword", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" ['{v_CredentialName}']";
		}       
	}
}