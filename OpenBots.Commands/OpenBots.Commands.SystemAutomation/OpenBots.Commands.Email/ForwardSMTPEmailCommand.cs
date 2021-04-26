using MailKit.Net.Smtp;
using MimeKit;
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
using System.Security;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.Email
{
	[Serializable]
	[Category("Email Commands")]
	[Description("This command forwards a selected email using SMTP protocol.")]
	public class ForwardSMTPEmailCommand : ScriptCommand
	{
		[Required]
		[DisplayName("MimeMessage")]
		[Description("Enter the MimeMessage to forward.")]
		[SampleUsage("vMimeMessage")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(MimeMessage) })]
		public string v_SMTPMimeMessage { get; set; }

		[Required]
		[DisplayName("Host")]
		[Description("Define the host/service name that the script should use.")]
		[SampleUsage("\"smtp.gmail.com\" || vHost")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_SMTPHost { get; set; }

		[Required]
		[DisplayName("Port")]
		[Description("Define the port number that should be used when contacting the SMTP service.")]
		[SampleUsage("\"465\" || vPort")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_SMTPPort { get; set; }

		[Required]
		[DisplayName("Username")]
		[Description("Define the username to use when contacting the SMTP service.")]
		[SampleUsage("\"myRobot\" || vUsername")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_SMTPUserName { get; set; }

		[Required]
		[DisplayName("Password")]
		[Description("Define the password to use when contacting the SMTP service.")]
		[SampleUsage("vPassword")]
		[Remarks("Password input must be a SecureString variable.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(SecureString) })]
		public string v_SMTPPassword { get; set; }

		[Required]
		[DisplayName("Recipient(s)")]
		[Description("Enter the email address(es) of the recipient(s).")]
		[SampleUsage("new List<string>() { \"test@test.com\", \"test2@test.com\" } || vEmailsList")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(List<string>) })]
		public string v_SMTPRecipients { get; set; }

		[Required]
		[DisplayName("Email Body")]
		[Description("Enter text to be used as the email body.")]
		[SampleUsage("$\"Everything ran ok at {DateTime.Now}\"  || vBody")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_SMTPBody { get; set; }

		public ForwardSMTPEmailCommand()
		{
			CommandName = "ForwardSMTPEmailCommand";
			SelectionName = "Forward SMTP Email";
			CommandEnabled = true;
			CommandIcon = Resources.command_smtp;
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			MimeMessage vMimeMessageToForward = (MimeMessage)await v_SMTPMimeMessage.EvaluateCode(engine);
			string vSMTPHost = (string)await v_SMTPHost.EvaluateCode(engine);
			string vSMTPPort = (string)await v_SMTPPort.EvaluateCode(engine);
			string vSMTPUserName = (string)await v_SMTPUserName.EvaluateCode(engine);
			string vSMTPPassword = ((SecureString)await v_SMTPPassword.EvaluateCode(engine)).ConvertSecureStringToString();
			List<string> vSMTPRecipients = (List<string>)await v_SMTPRecipients.EvaluateCode(engine);
			string vSMTPBody = (string)await v_SMTPBody.EvaluateCode(engine);

			using (var client = new SmtpClient())
			{
				client.ServerCertificateValidationCallback = (sndr, certificate, chain, sslPolicyErrors) => true;
				client.SslProtocols = SslProtocols.None;

				using (var cancel = new CancellationTokenSource())
				{
					try
					{
						client.Connect(vSMTPHost, int.Parse(vSMTPPort), true, cancel.Token); //SSL
					}
					catch (Exception)
					{
						client.Connect(vSMTPHost, int.Parse(vSMTPPort)); //TLS
					}

					client.AuthenticationMechanisms.Remove("XOAUTH2");
					client.Authenticate(vSMTPUserName, vSMTPPassword, cancel.Token);

					//construct a new message
					var message = new MimeMessage();
					message.From.Add(MailboxAddress.Parse(vSMTPUserName));
					message.ReplyTo.Add(MailboxAddress.Parse(vSMTPUserName));

					foreach (var vSMTPToEmail in vSMTPRecipients)
						message.To.Add(MailboxAddress.Parse(vSMTPToEmail));

					message.Subject = "Fwd: " + vMimeMessageToForward.Subject;

					//create a body
					var builder = new BodyBuilder();
					builder.TextBody = vSMTPBody;
					builder.Attachments.Add(new MessagePart { Message = vMimeMessageToForward });
					message.Body = builder.ToMessageBody();

					client.Send(message);
					client.ServerCertificateValidationCallback = null;
				}
			}                    
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SMTPMimeMessage", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SMTPHost", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SMTPPort", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SMTPUserName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SMTPPassword", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SMTPRecipients", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SMTPBody", this, editor, 100, 300));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [MimeMessage '{v_SMTPMimeMessage}' - Forward to '{v_SMTPRecipients}']";
		}
	}
}
