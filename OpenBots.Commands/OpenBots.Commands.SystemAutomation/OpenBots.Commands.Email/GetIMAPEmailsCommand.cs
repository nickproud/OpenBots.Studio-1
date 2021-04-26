using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using Newtonsoft.Json;
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
using System.Linq;
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
	[Description("This command gets selected emails and their attachments using IMAP protocol.")]
	public class GetIMAPEmailsCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Host")]
		[Description("Define the host/service name that the script should use.")]
		[SampleUsage("\"imap.gmail.com\" || vHost")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_IMAPHost { get; set; }

		[Required]
		[DisplayName("Port")]
		[Description("Define the port number that should be used when contacting the IMAP service.")]
		[SampleUsage("\"993\" || vPort")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_IMAPPort { get; set; }

		[Required]
		[DisplayName("Username")]
		[Description("Define the username to use when contacting the IMAP service.")]
		[SampleUsage("\"myRobot\" || vUsername")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_IMAPUserName { get; set; }

		[Required]
		[DisplayName("Password")]
		[Description("Define the password to use when contacting the IMAP service.")]
		[SampleUsage("vPassword")]
		[Remarks("Password input must be a SecureString variable.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(SecureString) })]
		public string v_IMAPPassword { get; set; }

		[Required]
		[DisplayName("Source Mail Folder Name")]
		[Description("Enter the name of the mail folder the emails are located in.")]
		[SampleUsage("\"Inbox\" || vFolderName")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_IMAPSourceFolder { get; set; }

		[Required]
		[DisplayName("Filter")]
		[Description("Enter a valid filter string.")]
		[SampleUsage("\"Hello World\" || \"myRobot@company.com\" || vFilter || \"None\"")]
		[Remarks("*Warning* Using 'None' as the Filter will return every email in the selected Mail Folder.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_IMAPFilter { get; set; }

		[Required]
		[DisplayName("Unread Only")]
		[PropertyUISelectionOption("Yes")]
		[PropertyUISelectionOption("No")]
		[Description("Specify whether to retrieve unread email messages only.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_IMAPGetUnreadOnly { get; set; }

		[Required]
		[DisplayName("Mark As Read")]
		[PropertyUISelectionOption("Yes")]
		[PropertyUISelectionOption("No")]
		[Description("Specify whether to mark retrieved emails as read.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_IMAPMarkAsRead { get; set; }

		[Required]
		[DisplayName("Save MimeMessages and Attachments")]
		[PropertyUISelectionOption("Yes")]
		[PropertyUISelectionOption("No")]
		[Description("Specify whether to save the email attachments to a local directory.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_IMAPSaveMessagesAndAttachments { get; set; }

		[Required]
		[DisplayName("Include Embedded Images")]
		[PropertyUISelectionOption("Yes")]
		[PropertyUISelectionOption("No")]
		[Description("Specify whether to consider images in body as attachments.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_IncludeEmbeddedImagesAsAttachments { get; set; }

		[Required]
		[DisplayName("Output MimeMessage Directory")]
		[Description("Enter or Select the path of the directory to store the messages in.")]
		[SampleUsage("@\"C:\\temp\" || ProjectPath + @\"\\temp\" || vDirectoryPath")]
		[Remarks("This input is optional and will only be used if *Save MimeMessages and Attachments* is set to **Yes**.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFolderSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_IMAPMessageDirectory { get; set; }

		[Required]
		[DisplayName("Output Attachment Directory")]
		[Description("Enter or Select the path to the directory to store the attachments in.")]
		[SampleUsage("@\"C:\\temp\" || ProjectPath + @\"\\temp\" || vDirectoryPath")]
		[Remarks("This input is optional and will only be used if *Save MimeMessages and Attachments* is set to **Yes**.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFolderSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_IMAPAttachmentDirectory { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output MimeMessage List Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(List<MimeMessage>) })]
		public string v_OutputUserVariableName { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private List<Control> _savingControls;

		[JsonIgnore]
		[Browsable(false)]
		private bool _hasRendered;

		public GetIMAPEmailsCommand()
		{
			CommandName = "GetIMAPEmailsCommand";
			SelectionName = "Get IMAP Emails";
			CommandEnabled = true;
			CommandIcon = Resources.command_smtp;

			v_IMAPSourceFolder = "INBOX";
			v_IMAPGetUnreadOnly = "No";
			v_IMAPMarkAsRead = "Yes";
			v_IMAPSaveMessagesAndAttachments = "No";
			v_IncludeEmbeddedImagesAsAttachments = "No";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			string vIMAPHost = (string)await v_IMAPHost.EvaluateCode(engine);
			string vIMAPPort = (string)await v_IMAPPort.EvaluateCode(engine);
			string vIMAPUserName = (string)await v_IMAPUserName.EvaluateCode(engine);
			string vIMAPPassword = ((SecureString)await v_IMAPPassword.EvaluateCode(engine)).ConvertSecureStringToString();
			string vIMAPSourceFolder = (string)await v_IMAPSourceFolder.EvaluateCode(engine);
			string vIMAPFilter = (string)await v_IMAPFilter.EvaluateCode(engine);
			string vIMAPMessageDirectory = (string)await v_IMAPMessageDirectory.EvaluateCode(engine);
			string vIMAPAttachmentDirectory = (string)await v_IMAPAttachmentDirectory.EvaluateCode(engine);

			using (var client = new ImapClient())
			{
				client.ServerCertificateValidationCallback = (sndr, certificate, chain, sslPolicyErrors) => true;
				client.SslProtocols = SslProtocols.None;

				using (var cancel = new CancellationTokenSource())
				{
					try
					{
						client.Connect(vIMAPHost, int.Parse(vIMAPPort), true, cancel.Token); //SSL
					}
					catch (Exception)
					{
						client.Connect(vIMAPHost, int.Parse(vIMAPPort)); //TLS
					}

					client.AuthenticationMechanisms.Remove("XOAUTH2");
					client.Authenticate(vIMAPUserName, vIMAPPassword, cancel.Token);

					IMailFolder toplevel = client.GetFolder(client.PersonalNamespaces[0]);
					IMailFolder foundFolder = FindFolder(toplevel, vIMAPSourceFolder);

					if (foundFolder != null)
						foundFolder.Open(FolderAccess.ReadWrite, cancel.Token);
					else
						throw new Exception("Source Folder not found");

					SearchQuery query;
					if (vIMAPFilter.ToLower() == "none")
						query = SearchQuery.All;
					else if (!string.IsNullOrEmpty(vIMAPFilter.Trim()))
					{
						query = SearchQuery.MessageContains(vIMAPFilter)
							.Or(SearchQuery.SubjectContains(vIMAPFilter))
							.Or(SearchQuery.FromContains(vIMAPFilter))
							.Or(SearchQuery.BccContains(vIMAPFilter))
							.Or(SearchQuery.BodyContains(vIMAPFilter))
							.Or(SearchQuery.CcContains(vIMAPFilter))
							.Or(SearchQuery.ToContains(vIMAPFilter));
					}                   
					else 
						throw new NullReferenceException("Filter not specified");

					if (v_IMAPGetUnreadOnly == "Yes")
						query = query.And(SearchQuery.NotSeen);

					var filteredItems = foundFolder.Search(query, cancel.Token);

					List<MimeMessage> outMail = new List<MimeMessage>();

					foreach (UniqueId uid in filteredItems)
					{
						if (v_IMAPMarkAsRead == "Yes")
							foundFolder.AddFlags(uid, MessageFlags.Seen, true);

						MimeMessage message = foundFolder.GetMessage(uid, cancel.Token);

						if (v_IMAPSaveMessagesAndAttachments == "Yes")                       
							ProcessEmail(message, vIMAPMessageDirectory, vIMAPAttachmentDirectory);

						message.MessageId = $"{vIMAPSourceFolder}#{uid}";
						outMail.Add(message);

					}
					outMail.SetVariableValue(engine, v_OutputUserVariableName);

					client.Disconnect(true, cancel.Token);
					client.ServerCertificateValidationCallback = null;
				}
			}           
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_IMAPHost", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_IMAPPort", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_IMAPUserName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_IMAPPassword", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_IMAPSourceFolder", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_IMAPFilter", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_IMAPGetUnreadOnly", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_IMAPMarkAsRead", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_IMAPSaveMessagesAndAttachments", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));
			((ComboBox)RenderedControls[23]).SelectedIndexChanged += SaveMailItemsComboBox_SelectedIndexChanged;

			_savingControls = new List<Control>();
			_savingControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_IncludeEmbeddedImagesAsAttachments", this, editor));
			_savingControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_IMAPMessageDirectory", this, editor));
			_savingControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_IMAPAttachmentDirectory", this, editor));

			RenderedControls.AddRange(_savingControls);

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [From '{v_IMAPSourceFolder}' - Filter by '{v_IMAPFilter}' - Store MimeMessage List in '{v_OutputUserVariableName}']";
		}

		public override void Shown()
		{
			base.Shown();
			_hasRendered = true;
			SaveMailItemsComboBox_SelectedIndexChanged(this, null);
		}

		public static IMailFolder FindFolder(IMailFolder toplevel, string name)
		{
			var subfolders = toplevel.GetSubfolders().ToList();

			foreach (var subfolder in subfolders)
			{
				if (subfolder.Name == name)
					return subfolder;
			}

			foreach (var subfolder in subfolders)
			{
				var folder = FindFolder(subfolder, name);

				if (folder != null)
					return folder;
			}

			return null;
		}

		private void ProcessEmail(MimeMessage message, string msgDirectory, string attDirectory)
		{
			if (Directory.Exists(msgDirectory))
				message.WriteTo(Path.Combine(msgDirectory, message.Subject + ".eml"));

			if (Directory.Exists(attDirectory))
			{
				
				foreach (var attachment in message.Attachments)
				{
					if (attachment is MessagePart)
					{
						var fileName = attachment.ContentDisposition?.FileName;
						var rfc822 = (MessagePart)attachment;

						if (string.IsNullOrEmpty(fileName))
							fileName = "attached-message.eml";
						if (!message.HtmlBody.Contains(fileName) || v_IncludeEmbeddedImagesAsAttachments.Equals("Yes"))
						{
							using (var stream = OBFile.Create(Path.Combine(attDirectory, fileName)))
								rfc822.Message.WriteTo(stream);
						}
					}
					else
					{
						var part = (MimePart)attachment;
						var fileName = part.FileName;
						if (!message.HtmlBody.Contains(fileName) || v_IncludeEmbeddedImagesAsAttachments.Equals("Yes"))
						{
							using (var stream = OBFile.Create(Path.Combine(attDirectory, fileName)))
								part.Content.DecodeTo(stream);
						}
					}
				}
			}           
		}

		private void SaveMailItemsComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (((ComboBox)RenderedControls[23]).Text == "Yes" && _hasRendered)
			{
				foreach (var ctrl in _savingControls)
					ctrl.Visible = true;
			}
			else if(_hasRendered)
			{
				foreach (var ctrl in _savingControls)
				{
					ctrl.Visible = false;
					if (ctrl is TextBox)
						((TextBox)ctrl).Clear();
				}
			}
		}
	}
}
