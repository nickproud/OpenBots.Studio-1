using Microsoft.Office.Interop.Outlook;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System.Collections.Generic;
using Xunit;

namespace OpenBots.Commands.Outlook.Test
{
    public class ReplyToOutlookEmailCommandTests
    {
        private AutomationEngineInstance _engine;
        private ReplyToOutlookEmailCommand _replyToOutlookEmail;
        private GetOutlookEmailsCommand _getOutlookEmails;
        private DeleteOutlookEmailCommand _deleteOutlookEmail;
        private SendOutlookEmailCommand _sendOutlookEmail;
        private MoveCopyOutlookEmailCommand _moveCopyOutlookEmail;

        /*
         * Prerequisite: User is signed into openbots.test@outlook.com on local Microsoft Outlook.
        */
        [Fact]
        public async void RepliesToOutlookEmail()
        {
            _engine = new AutomationEngineInstance(null);
            _replyToOutlookEmail = new ReplyToOutlookEmailCommand();
            _getOutlookEmails = new GetOutlookEmailsCommand();

            VariableMethods.CreateTestVariable(null, _engine, "emails", typeof(List<>));

            _getOutlookEmails.v_SourceFolder = "TestInput";
            _getOutlookEmails.v_Filter = "[Subject] = 'toReply'";
            _getOutlookEmails.v_GetUnreadOnly = "No";
            _getOutlookEmails.v_MarkAsRead = "No";
            _getOutlookEmails.v_SaveMessagesAndAttachments = "No";
            _getOutlookEmails.v_MessageDirectory = "";
            _getOutlookEmails.v_AttachmentDirectory = "";
            _getOutlookEmails.v_OutputUserVariableName = "{emails}";

            _getOutlookEmails.RunCommand(_engine);

            var emails = (List<MailItem>)await "{emails}".EvaluateCode(_engine);
            MailItem email = emails[0];
            VariableMethods.CreateTestVariable(email, _engine, "email", typeof(MailItem));

            _replyToOutlookEmail.v_MailItem = "{email}";
            _replyToOutlookEmail.v_OperationType = "Reply";
            _replyToOutlookEmail.v_Body = "replyBody";
            _replyToOutlookEmail.v_BodyType = "Plain";

            _replyToOutlookEmail.RunCommand(_engine);
            
            int attempts = 0;
            do {
                System.Threading.Thread.Sleep(5000);
                _getOutlookEmails.v_SourceFolder = "Inbox";
                _getOutlookEmails.v_Filter = "[Subject] = 'toReply'";
                _getOutlookEmails.v_GetUnreadOnly = "No";
                _getOutlookEmails.v_MarkAsRead = "No";
                _getOutlookEmails.v_SaveMessagesAndAttachments = "No";
                _getOutlookEmails.v_MessageDirectory = "";
                _getOutlookEmails.v_AttachmentDirectory = "";
                _getOutlookEmails.v_OutputUserVariableName = "{emails}";

                _getOutlookEmails.RunCommand(_engine);

                emails = (List<MailItem>)await "{emails}".EvaluateCode(_engine);
                attempts++;
            } while (emails.Count < 1 && attempts < 8);
            email = emails[0];

            Assert.Equal("replyBody \r\n", email.Body);

            resetReplyEmail(_engine);
        }

        private async void resetReplyEmail(AutomationEngineInstance _engine)
        {
            _deleteOutlookEmail = new DeleteOutlookEmailCommand();
            _getOutlookEmails = new GetOutlookEmailsCommand();
            _sendOutlookEmail = new SendOutlookEmailCommand();
            _moveCopyOutlookEmail = new MoveCopyOutlookEmailCommand();

            VariableMethods.CreateTestVariable(null, _engine, "emails", typeof(List<>));

            _getOutlookEmails.v_SourceFolder = "Inbox";
            _getOutlookEmails.v_Filter = "[Subject] = 'toReply'";
            _getOutlookEmails.v_GetUnreadOnly = "No";
            _getOutlookEmails.v_MarkAsRead = "No";
            _getOutlookEmails.v_SaveMessagesAndAttachments = "No";
            _getOutlookEmails.v_MessageDirectory = "";
            _getOutlookEmails.v_AttachmentDirectory = "";
            _getOutlookEmails.v_OutputUserVariableName = "{emails}";

            _getOutlookEmails.RunCommand(_engine);

            List<MailItem> emails = (List<MailItem>)await "{emails}".EvaluateCode(_engine);
            MailItem email = emails[0];
            VariableMethods.CreateTestVariable(email, _engine, "email", typeof(MailItem));

            _deleteOutlookEmail.v_MailItem = "{email}";
            _deleteOutlookEmail.v_DeleteReadOnly = "No";

            _deleteOutlookEmail.RunCommand(_engine);

            _getOutlookEmails.v_SourceFolder = "TestInput";
            _getOutlookEmails.v_Filter = "[Subject] = 'toReply'";
            _getOutlookEmails.v_GetUnreadOnly = "No";
            _getOutlookEmails.v_MarkAsRead = "No";
            _getOutlookEmails.v_SaveMessagesAndAttachments = "No";
            _getOutlookEmails.v_MessageDirectory = "";
            _getOutlookEmails.v_AttachmentDirectory = "";
            _getOutlookEmails.v_OutputUserVariableName = "{emails}";

            _getOutlookEmails.RunCommand(_engine);

            emails = (List<MailItem>)await "{emails}".EvaluateCode(_engine);
            email = emails[0];
            VariableMethods.CreateTestVariable(email, _engine, "email", typeof(MailItem));

            _deleteOutlookEmail.v_MailItem = "{email}";
            _deleteOutlookEmail.v_DeleteReadOnly = "No";

            _deleteOutlookEmail.RunCommand(_engine);

            _sendOutlookEmail.v_Recipients = "openbots.test@outlook.com";
            _sendOutlookEmail.v_Subject = "toReply";
            _sendOutlookEmail.v_Body = "testBody";
            _sendOutlookEmail.v_BodyType = "Plain";

            _sendOutlookEmail.RunCommand(_engine);

            int attempts = 0;
            do {
                System.Threading.Thread.Sleep(5000);
                _getOutlookEmails.v_SourceFolder = "Inbox";
                _getOutlookEmails.v_Filter = "[Subject] = 'toReply'";
                _getOutlookEmails.v_GetUnreadOnly = "No";
                _getOutlookEmails.v_MarkAsRead = "No";
                _getOutlookEmails.v_SaveMessagesAndAttachments = "No";
                _getOutlookEmails.v_MessageDirectory = "";
                _getOutlookEmails.v_AttachmentDirectory = "";
                _getOutlookEmails.v_OutputUserVariableName = "{emails}";

                _getOutlookEmails.RunCommand(_engine);

                emails = (List<MailItem>)await "{emails}".EvaluateCode(_engine);
                attempts++;
            } while (attempts < 5 && emails.Count < 1);
            email = emails[0];
            VariableMethods.CreateTestVariable(email, _engine, "email", typeof(MailItem));

            _moveCopyOutlookEmail.v_MailItem = "{email}";
            _moveCopyOutlookEmail.v_DestinationFolder = "TestInput";
            _moveCopyOutlookEmail.v_OperationType = "Move MailItem";
            _moveCopyOutlookEmail.v_MoveCopyUnreadOnly = "No";

            _moveCopyOutlookEmail.RunCommand(_engine);
        }
    }
}
