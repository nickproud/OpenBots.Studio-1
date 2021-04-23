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

namespace OpenBots.Commands.Outlook
{
	[Serializable]
	[Category("Outlook Commands")]
	[Description("This command forwards a selected email in Outlook.")]

	public class ForwardOutlookEmailCommand : ScriptCommand
	{

		[Required]
		[DisplayName("MailItem")]
		[Description("Enter the MailItem to forward.")]
		[SampleUsage("vMailItem")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(MailItem)})]
		public string v_MailItem { get; set; }

		[Required]
		[DisplayName("Recipient(s)")]
		[Description("Enter the email address(es) of the recipient(s).")]
		[SampleUsage("new List<string>() { \"test@test.com\", \"test2@test.com\" } || vEmailsList")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(List<string>) })]
		public string v_Recipients { get; set; }

		public ForwardOutlookEmailCommand()
		{
			CommandName = "ForwardOutlookEmailCommand";
			SelectionName = "Forward Outlook Email";
			CommandEnabled = true;
			CommandIcon = Resources.command_smtp;

		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			MailItem vMailItem = (MailItem)await v_MailItem.EvaluateCode(engine);
  
			var vRecipients = (List<string>)await v_Recipients.EvaluateCode(engine);

			MailItem newMail = vMailItem.Forward();

			foreach (var recipient in vRecipients)
				newMail.Recipients.Add(recipient);

			newMail.Recipients.ResolveAll();
			newMail.Send();         
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_MailItem", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Recipients", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [MailItem '{v_MailItem}' - Forward to '{v_Recipients}']";
		}
	}
}
