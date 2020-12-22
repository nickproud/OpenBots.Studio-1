using Microsoft.Office.Interop.Outlook;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System.Collections.Generic;
using Xunit;

namespace OpenBots.Commands.Outlook.Test
{
    public class ForwardOutlookEmailCommandTests
    {
        private AutomationEngineInstance _engine;
        private GetOutlookEmailsCommand _getOutlookEmails;
        private ForwardOutlookEmailCommand _forwardOutlookEmail;
        private DeleteOutlookEmailCommand _deleteOutlookEmail;

        [Fact]
        public void ForwardsOutlookEmail()
        {
            _engine = new AutomationEngineInstance(null);
            _getOutlookEmails = new GetOutlookEmailsCommand();
            _forwardOutlookEmail = new ForwardOutlookEmailCommand();
            _deleteOutlookEmail = new DeleteOutlookEmailCommand();

            _getOutlookEmails.v_SourceFolder = "TestInput";
            _getOutlookEmails.v_Filter = "[Subject] = 'toForward'";
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
            string forwardAddress = "openbots.test@outlook.com";
            forwardAddress.StoreInUserVariable(_engine, "{forwardEmail}");

            _forwardOutlookEmail.v_MailItem = "{email}";
            _forwardOutlookEmail.v_Recipients = "{forwardEmail}";

            _forwardOutlookEmail.RunCommand(_engine);

            System.Threading.Thread.Sleep(30000);

            _getOutlookEmails.v_SourceFolder = "Inbox";
            _getOutlookEmails.v_Filter = "[Subject] = 'toForward'";
            _getOutlookEmails.v_GetUnreadOnly = "No";
            _getOutlookEmails.v_MarkAsRead = "No";
            _getOutlookEmails.v_SaveMessagesAndAttachments = "No";
            _getOutlookEmails.v_MessageDirectory = "";
            _getOutlookEmails.v_AttachmentDirectory = "";
            _getOutlookEmails.v_OutputUserVariableName = "{emails}";

            _getOutlookEmails.RunCommand(_engine);

            emails = (List<MailItem>)"{emails}".ConvertUserVariableToObject(_engine);
            email = emails[0];

            resetEmail(_engine);

            Assert.Equal("openbots.test@outlook.com", email.Sender.Address);
        }

        private void resetEmail(AutomationEngineInstance _engine)
        {
            _deleteOutlookEmail = new DeleteOutlookEmailCommand();
            _getOutlookEmails = new GetOutlookEmailsCommand();

            _getOutlookEmails.v_SourceFolder = "Inbox";
            _getOutlookEmails.v_Filter = "[Subject] = 'toForward'";
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
