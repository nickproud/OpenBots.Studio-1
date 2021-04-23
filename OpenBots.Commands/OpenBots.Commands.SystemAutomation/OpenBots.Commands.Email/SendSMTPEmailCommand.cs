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
using System.IO;
using System.Security;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OBFile = System.IO.File;

namespace OpenBots.Commands.Email
{
	[Serializable]
	[Category("Email Commands")]
	[Description("This command sends an email with optional attachment(s) using SMTP protocol.")]
	public class SendSMTPEmailCommand : ScriptCommand
	{
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
		[DisplayName("Email Subject")]
		[Description("Enter the subject of the email.")]
		[SampleUsage("\"Hello\" || vSubject")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_SMTPSubject { get; set; }

		[Required]
		[DisplayName("Email Body")]
		[Description("Enter text to be used as the email body.")]
		[SampleUsage("\"Dear John, ...\" || vBody")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_SMTPBody { get; set; }

		[DisplayName("Attachment File Path(s) (Optional)")]
		[Description("Enter the file path(s) of the file(s) to attach.")]
		[SampleUsage("new List<string>() { \"C:\\temp\\myFile1.xlsx\", \"C:\\temp\\myFile2.xlsx\" } || vFileList")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(List<string>) })]
		public string v_SMTPAttachments { get; set; }

		public SendSMTPEmailCommand()
		{
			CommandName = "SendSMTPEmailCommand";
			SelectionName = "Send SMTP Email";
			CommandEnabled = true;
			CommandIcon = Resources.command_smtp;

		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			string vSMTPHost = (string)await v_SMTPHost.EvaluateCode(engine);
			string vSMTPPort = (string)await v_SMTPPort.EvaluateCode(engine);
			string vSMTPUserName = (string)await v_SMTPUserName.EvaluateCode(engine);
			string vSMTPPassword = ((SecureString)await v_SMTPPassword.EvaluateCode(engine)).ConvertSecureStringToString();
			List<string> vSMTPRecipients = (List<string>)await v_SMTPRecipients.EvaluateCode(engine);
			string vSMTPSubject = (string)await v_SMTPSubject.EvaluateCode(engine);
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

					foreach (var vSMTPToEmail in vSMTPRecipients)
						message.To.Add(MailboxAddress.Parse(vSMTPToEmail));

					message.Subject = vSMTPSubject;

					//create a body
					var builder = new BodyBuilder();
					builder.TextBody = vSMTPBody;

					if (!string.IsNullOrEmpty(v_SMTPAttachments))
					{
						var vSMTPAttachments = (List<string>)await v_SMTPAttachments.EvaluateCode(engine);

						foreach (var vSMTPattachment in vSMTPAttachments)
						{
							using (MemoryStream memoryStream = new MemoryStream(OBFile.ReadAllBytes(vSMTPattachment)))
								builder.Attachments.Add(vSMTPattachment, memoryStream.ToArray());
						}
					}
					message.Body = builder.ToMessageBody();

					client.Send(message);
					client.ServerCertificateValidationCallback = null;
				}
			}   
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SMTPHost", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SMTPPort", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SMTPUserName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SMTPPassword", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SMTPRecipients", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SMTPSubject", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SMTPBody", this, editor, 100, 300));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SMTPAttachments", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [To '{v_SMTPRecipients}' - Subject '{v_SMTPSubject}']";
		}
	}
}
