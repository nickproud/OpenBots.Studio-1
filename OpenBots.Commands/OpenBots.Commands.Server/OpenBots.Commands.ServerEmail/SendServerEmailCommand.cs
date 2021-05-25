using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Server.HelperMethods;
using OpenBots.Core.Server.Models;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.ServerEmail
{
    [Serializable]
    [Category("Server Email Commands")]
    [Description("This command sends an email with optional attachment(s) in OpenBots Server.")]
    public class SendServerEmailCommand : ScriptCommand
    {
        [DisplayName("Account Name (Optional)")]
        [Description("Define the account name to use when contacting the Server email service.")]
        [SampleUsage("\"myRobot\" || vAccountName")]
        [Remarks("If no account name is specified, the default account will be used.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_AccountName { get; set; }

        [Required]
        [DisplayName("To Recipient(s)")]
        [Description("Enter the email address(es) of the 'To' recipient(s).")]
		[SampleUsage("new List<string>() { \"test@test.com\", \"test2@test.com\" } || vEmailsList")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(List<string>) })]
        public string v_ToRecipients { get; set; }

        [DisplayName("CC Recipient(s) (Optional)")]
        [Description("Enter the email address(es) of the 'CC' recipient(s).")]
		[SampleUsage("new List<string>() { \"test@test.com\", \"test2@test.com\" } || vEmailsList")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(List<string>) })]
        public string v_CCRecipients { get; set; }

        [DisplayName("BCC Recipient(s) (Optional)")]
        [Description("Enter the email address(es) of the BCC recipient(s).")]
		[SampleUsage("new List<string>() { \"test@test.com\", \"test2@test.com\" } || vEmailsList")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(List<string>) })]
        public string v_BCCRecipients { get; set; }

        [Required]
        [DisplayName("Email Subject")]
        [Description("Enter the subject of the email.")]
        [SampleUsage("\"Hello\" || vSubject")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_Subject { get; set; }

        [Required]
        [DisplayName("Email Body")]
        [Description("Enter text to be used as the email body.")]
        [SampleUsage("\"Dear John, ...\" || vBody")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_Body { get; set; }

        [DisplayName("Attachment File Path(s) (Optional)")]
        [Description("Enter the file path(s) of the file(s) to attach.")]
		[SampleUsage("new List<string>() { \"C:\\temp\\myFile1.xlsx\", \"C:\\temp\\myFile2.xlsx\" } || vFileList")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(List<string>) })]
        public string v_Attachments { get; set; }

        public SendServerEmailCommand()
        {
            CommandName = "SendServerEmailCommand";
            SelectionName = "Send Server Email";
            CommandEnabled = true;
            CommandIcon = Resources.command_smtp;

            v_AccountName = null;
            v_Attachments = null;
            CommonMethods.InitializeDefaultWebProtocol();
        }

        public async override Task RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            var vAccountName = (string)await v_AccountName.EvaluateCode(engine);
            var vToRecipients = (List<string>)await v_ToRecipients.EvaluateCode(engine);
            var vSubject = (string)await v_Subject.EvaluateCode(engine);
            var vBody = (string)await v_Body.EvaluateCode(engine);
            
            var toEmailList = GetEmailList(vToRecipients);

            List<string> vCCRecipients = null;
            if (!string.IsNullOrEmpty(v_CCRecipients))
                vCCRecipients = (List<string>)await v_CCRecipients.EvaluateCode(engine);

            var ccEmailList = GetEmailList(vCCRecipients);

            List<string> vBCCRecipients = null;
            if (!string.IsNullOrEmpty(v_BCCRecipients))
                vBCCRecipients = (List<string>)await v_BCCRecipients.EvaluateCode(engine);

            var bccEmailList = GetEmailList(vBCCRecipients);

            var emailMessage = new EmailMessage()
            {
                To = toEmailList,
                CC = ccEmailList,
                BCC = bccEmailList,
                Subject = vSubject,
                Body = vBody
            };

            if (vToRecipients == null)
                throw new NullReferenceException("To Recipient(s) cannot be empty");

            var userInfo = AuthMethods.GetUserInfo();

            List<string> vAttachments = null;
            if (!string.IsNullOrEmpty(v_Attachments))
                vAttachments = (List<string>)await v_Attachments.EvaluateCode(engine);

            ServerEmailMethods.SendServerEmail(userInfo.Token, userInfo.ServerUrl, userInfo.OrganizationId, emailMessage, vAttachments, vAccountName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_AccountName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ToRecipients", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_CCRecipients", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_BCCRecipients", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Subject", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Body", this, editor, 100, 300));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Attachments", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [To '{v_ToRecipients}' - Subject '{v_Subject}']";
        }

        private List<EmailAddress> GetEmailList(List<string> recipients)
        {
            var emailList = new List<EmailAddress>();

            if (recipients != null)
            {
                foreach (var recipient in recipients)
                {
                    var email = new EmailAddress()
                    {
                        Name = recipient,
                        Address = recipient
                    };
                    emailList.Add(email);
                }
            }

            return emailList;
        }
    }
}