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

        [Fact]
        public void ReplysToOutlookEmail()
        {
            _engine = new AutomationEngineInstance(null);
            _replyToOutlookEmail = new ReplyToOutlookEmailCommand();
            _getOutlookEmails = new GetOutlookEmailsCommand();

            _getOutlookEmails.v_SourceFolder = "TestInput";
            _getOutlookEmails.v_Filter = "[Subject] = 'toReply'";
            _getOutlookEmails.v_GetUnreadOnly = "No";
            _getOutlookEmails.v_MarkAsRead = "No";
            _getOutlookEmails.v_SaveMessagesAndAttachments = "No";
            _getOutlookEmails.v_MessageDirectory = "";
            _getOutlookEmails.v_AttachmentDirectory = "";
            _getOutlookEmails.v_OutputUserVariableName = "{emails}";

            _getOutlookEmails.RunCommand(_engine);

            var emails = (List<MailItem>)"{emails}".ConvertUserVariableToObject(_engine);
            MailItem email = emails[0];
            email.StoreInUserVariable(_engine, "{email}");

            _replyToOutlookEmail.v_MailItem = "{email}";
            _replyToOutlookEmail.v_OperationType = "Reply";
            _replyToOutlookEmail.v_Body = "replyBody";
            _replyToOutlookEmail.v_BodyType = "Plain";

            _replyToOutlookEmail.RunCommand(_engine);
            System.Threading.Thread.Sleep(30000);

            _getOutlookEmails.v_SourceFolder = "Inbox";
            _getOutlookEmails.v_Filter = "[Subject] = 'toReply'";
            _getOutlookEmails.v_GetUnreadOnly = "No";
            _getOutlookEmails.v_MarkAsRead = "No";
            _getOutlookEmails.v_SaveMessagesAndAttachments = "No";
            _getOutlookEmails.v_MessageDirectory = "";
            _getOutlookEmails.v_AttachmentDirectory = "";
            _getOutlookEmails.v_OutputUserVariableName = "{emails}";

            _getOutlookEmails.RunCommand(_engine);

            emails = (List<MailItem>)"{emails}".ConvertUserVariableToObject(_engine);
            email = emails[0];

            resetReplyEmail(_engine);

            Assert.Equal("replyBody \r\n", email.Body);
        }

        private void resetReplyEmail(AutomationEngineInstance _engine)
        {
            _deleteOutlookEmail = new DeleteOutlookEmailCommand();
            _getOutlookEmails = new GetOutlookEmailsCommand();

            _getOutlookEmails.v_SourceFolder = "Inbox";
            _getOutlookEmails.v_Filter = "[Subject] = 'toReply'";
            _getOutlookEmails.v_GetUnreadOnly = "No";
            _getOutlookEmails.v_MarkAsRead = "No";
            _getOutlookEmails.v_SaveMessagesAndAttachments = "No";
            _getOutlookEmails.v_MessageDirectory = "";
            _getOutlookEmails.v_AttachmentDirectory = "";
            _getOutlookEmails.v_OutputUserVariableName = "{emails}";

            _getOutlookEmails.RunCommand(_engine);

            var emails = (List<MailItem>)"{emails}".ConvertUserVariableToObject(_engine);
            MailItem email = emails[0];
            email.StoreInUserVariable(_engine, "{email}");

            _deleteOutlookEmail.v_MailItem = "{email}";
            _deleteOutlookEmail.v_DeleteReadOnly = "No";

            _deleteOutlookEmail.RunCommand(_engine);
        }
    }
}
