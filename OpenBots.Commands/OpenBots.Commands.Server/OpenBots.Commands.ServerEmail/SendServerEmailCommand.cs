using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Server.API_Methods;
using OpenBots.Core.Server.Models;
using OpenBots.Core.Utilities.CommonUtilities;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        [SampleUsage("myRobot || {vAccountName}")]
        [Remarks("If no account name is specified, the default account will be used.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_AccountName { get; set; }

        [Required]
        [DisplayName("To Recipient(s)")]
        [Description("Enter the email address(es) of the 'To' recipient(s).")]
        [SampleUsage("test@test.com || test@test.com;test2@test.com || {vEmail} || {vEmail1};{vEmail2} || {vEmails}")]
        [Remarks("Multiple recipient email addresses should be delimited by a semicolon (;).")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_ToRecipients { get; set; }

        [DisplayName("CC Recipient(s) (Optional)")]
        [Description("Enter the email address(es) of the 'CC' recipient(s).")]
        [SampleUsage("test@test.com || test@test.com;test2@test.com || {vEmail} || {vEmail1};{vEmail2} || {vEmails}")]
        [Remarks("Multiple recipient email addresses should be delimited by a semicolon (;).")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_CCRecipients { get; set; }

        [DisplayName("BCC Recipient(s) (Optional)")]
        [Description("Enter the email address(es) of the BCC recipient(s).")]
        [SampleUsage("test@test.com || test@test.com;test2@test.com || {vEmail} || {vEmail1};{vEmail2} || {vEmails}")]
        [Remarks("Multiple recipient email addresses should be delimited by a semicolon (;).")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_BCCRecipients { get; set; }

        [Required]
        [DisplayName("Email Subject")]
        [Description("Enter the subject of the email.")]
        [SampleUsage("Hello || {vSubject}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_Subject { get; set; }

        [Required]
        [DisplayName("Email Body")]
        [Description("Enter text to be used as the email body.")]
        [SampleUsage("Dear John, ... || {vBody}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_Body { get; set; }

        [DisplayName("Attachment File Path(s) (Optional)")]
        [Description("Enter the file path(s) of the file(s) to attach.")]
        [SampleUsage(@"C:\temp\myFile.xlsx || {vFile} || C:\temp\myFile1.xlsx;C:\temp\myFile2.xlsx || {vFile1};{vFile2} || {vFiles}")]
        [Remarks("This input is optional. Multiple attachments should be delimited by a semicolon (;).")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
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

        public override void RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            var vAccountName = v_AccountName.ConvertUserVariableToString(engine);
            var vToRecipients = v_ToRecipients.ConvertUserVariableToString(engine);
            var vCCRecipients = v_CCRecipients.ConvertUserVariableToString(engine);
            var vBCCRecipients = v_BCCRecipients.ConvertUserVariableToString(engine);
            var vSubject = v_Subject.ConvertUserVariableToString(engine);
            var vBody = v_Body.ConvertUserVariableToString(engine);
            var vAttachments = v_Attachments.ConvertUserVariableToString(engine);

            var toEmailList = ServerEmailMethods.GetEmailList(vToRecipients);
            var ccEmailList = ServerEmailMethods.GetEmailList(vCCRecipients);
            var bccEmailList = ServerEmailMethods.GetEmailList(vBCCRecipients);

            var emailMessage = new EmailMessage()
            {
                To = toEmailList,
                CC = ccEmailList,
                BCC = bccEmailList,
                Subject = vSubject,
                Body = vBody
            };

            if (string.IsNullOrEmpty(vToRecipients))
                throw new NullReferenceException("To Recipient(s) cannot be empty");

            var client = AuthMethods.GetAuthToken();
            ServerEmailMethods.SendServerEmail(client, emailMessage, vAttachments, vAccountName);
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
    }
}