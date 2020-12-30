using Microsoft.Office.Interop.Outlook;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System.Collections.Generic;
using Xunit;

namespace OpenBots.Commands.Outlook.Test
{
    public class MoveCopyOutlookEmailCommandTests
    {
        private AutomationEngineInstance _engine;
        private GetOutlookEmailsCommand _getOutlookEmails;
        private MoveCopyOutlookEmailCommand _moveCopyOutlookEmail;
        private DeleteOutlookEmailCommand _deleteOutlookEmail;
        private SendOutlookEmailCommand _sendOutlookEmail;

        /*
         * Prerequisite: User is signed into openbots.test@outlook.com on local Microsoft Outlook.
        */
        [Fact]
        public void CopiesOutlookEmail()
        {
            _engine = new AutomationEngineInstance(null, null);
            _getOutlookEmails = new GetOutlookEmailsCommand();
            _moveCopyOutlookEmail = new MoveCopyOutlookEmailCommand();

            _getOutlookEmails.v_SourceFolder = "TestInput";
            _getOutlookEmails.v_Filter = "[Subject] = 'toCopy'";
            _getOutlookEmails.v_GetUnreadOnly = "No";
            _getOutlookEmails.v_MarkAsRead = "No";
            _getOutlookEmails.v_SaveMessagesAndAttachments = "No";
            _getOutlookEmails.v_MessageDirectory = "";
            _getOutlookEmails.v_AttachmentDirectory = "";
            _getOutlookEmails.v_OutputUserVariableName = "{emails}";

            _getOutlookEmails.RunCommand(_engine);

            var emails = (List<MailItem>)"{emails}".ConvertUserVariableToObject(_engine);
            MailItem originalEmail = emails[0];
            originalEmail.StoreInUserVariable(_engine, "{originalEmail}");
            string destFolder = "MovedMail";
            destFolder.StoreInUserVariable(_engine, "{destFolder}");

            _moveCopyOutlookEmail.v_MailItem = "{originalEmail}";
            _moveCopyOutlookEmail.v_DestinationFolder = "{destFolder}";
            _moveCopyOutlookEmail.v_OperationType = "Copy MailItem";
            _moveCopyOutlookEmail.v_MoveCopyUnreadOnly = "No";

            _moveCopyOutlookEmail.RunCommand(_engine);

            _getOutlookEmails.v_SourceFolder = "{destFolder}";
            _getOutlookEmails.v_Filter = "[Subject] = 'toCopy'";
            _getOutlookEmails.v_GetUnreadOnly = "No";
            _getOutlookEmails.v_MarkAsRead = "No";
            _getOutlookEmails.v_SaveMessagesAndAttachments = "No";
            _getOutlookEmails.v_MessageDirectory = "";
            _getOutlookEmails.v_AttachmentDirectory = "";
            _getOutlookEmails.v_OutputUserVariableName = "{emails}";

            _getOutlookEmails.RunCommand(_engine);

            emails = (List<MailItem>)"{emails}".ConvertUserVariableToObject(_engine);
            MailItem copyEmail = emails[0];

            Assert.Equal("toCopy", originalEmail.Subject);
            Assert.Equal("toCopy", copyEmail.Subject);

            resetCopyEmail(_engine);
        }

        [Fact]
        public void MovesOutlookEmail()
        {
            _engine = new AutomationEngineInstance(null, null);
            _getOutlookEmails = new GetOutlookEmailsCommand();
            _moveCopyOutlookEmail = new MoveCopyOutlookEmailCommand();

            _getOutlookEmails.v_SourceFolder = "Inbox";
            _getOutlookEmails.v_Filter = "[Subject] = 'toMove'";
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
            string destFolder = "MovedMail";
            destFolder.StoreInUserVariable(_engine, "{destFolder}");

            _moveCopyOutlookEmail.v_MailItem = "{email}";
            _moveCopyOutlookEmail.v_DestinationFolder = "{destFolder}";
            _moveCopyOutlookEmail.v_OperationType = "Move MailItem";
            _moveCopyOutlookEmail.v_MoveCopyUnreadOnly = "No";

            _moveCopyOutlookEmail.RunCommand(_engine);

            _getOutlookEmails.v_SourceFolder = "{destFolder}";
            _getOutlookEmails.v_Filter = "[Subject] = 'toMove'";
            _getOutlookEmails.v_GetUnreadOnly = "No";
            _getOutlookEmails.v_MarkAsRead = "No";
            _getOutlookEmails.v_SaveMessagesAndAttachments = "No";
            _getOutlookEmails.v_MessageDirectory = "";
            _getOutlookEmails.v_AttachmentDirectory = "";
            _getOutlookEmails.v_OutputUserVariableName = "{emails}";

            _getOutlookEmails.RunCommand(_engine);

            emails = (List<MailItem>)"{emails}".ConvertUserVariableToObject(_engine);
            email = emails[0];

            resetMoveEmail(_engine);

            Assert.Equal("toMove", email.Subject);
        }

        private void resetCopyEmail(AutomationEngineInstance _engine)
        {
            _deleteOutlookEmail = new DeleteOutlookEmailCommand();
            _getOutlookEmails = new GetOutlookEmailsCommand();

            _getOutlookEmails.v_SourceFolder = "MovedMail";
            _getOutlookEmails.v_Filter = "[Subject] = 'toCopy'";
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

        private void resetMoveEmail(AutomationEngineInstance _engine)
        {
            _deleteOutlookEmail = new DeleteOutlookEmailCommand();
            _getOutlookEmails = new GetOutlookEmailsCommand();
            _sendOutlookEmail = new SendOutlookEmailCommand();

            _getOutlookEmails.v_SourceFolder = "MovedMail";
            _getOutlookEmails.v_Filter = "[Subject] = 'toMove'";
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

            _sendOutlookEmail.v_Recipients = "openbots.test@outlook.com";
            _sendOutlookEmail.v_Subject = "toMove";
            _sendOutlookEmail.v_Body = "testBody";
            _sendOutlookEmail.v_BodyType = "Plain";

            _sendOutlookEmail.RunCommand(_engine);
        }
    }
}
