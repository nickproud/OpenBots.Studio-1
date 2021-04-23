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

        /*
         * Prerequisite: User is signed into openbots.test@outlook.com on local Microsoft Outlook.
        */
        [Fact]
        public async void ForwardsOutlookEmail()
        {
            _engine = new AutomationEngineInstance(null);
            _getOutlookEmails = new GetOutlookEmailsCommand();
            _forwardOutlookEmail = new ForwardOutlookEmailCommand();
            _deleteOutlookEmail = new DeleteOutlookEmailCommand();

            VariableMethods.CreateTestVariable(null, _engine, "emails", typeof(List<>));

            _getOutlookEmails.v_SourceFolder = "TestInput";
            _getOutlookEmails.v_Filter = "[Subject] = 'toForward'";
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
            string forwardAddress = "openbots.test@outlook.com";
            VariableMethods.CreateTestVariable(forwardAddress, _engine, "forwardEmail", typeof(string));

            _forwardOutlookEmail.v_MailItem = "{email}";
            _forwardOutlookEmail.v_Recipients = "{forwardEmail}";

            _forwardOutlookEmail.RunCommand(_engine);

            int attempts = 0;
            do
            {
                System.Threading.Thread.Sleep(5000);

                _getOutlookEmails.v_SourceFolder = "Inbox";
                _getOutlookEmails.v_Filter = "[Subject] = 'toForward'";
                _getOutlookEmails.v_GetUnreadOnly = "No";
                _getOutlookEmails.v_MarkAsRead = "No";
                _getOutlookEmails.v_SaveMessagesAndAttachments = "No";
                _getOutlookEmails.v_MessageDirectory = "";
                _getOutlookEmails.v_AttachmentDirectory = "";
                _getOutlookEmails.v_OutputUserVariableName = "{emails}";

                _getOutlookEmails.RunCommand(_engine);

                emails = (List<MailItem>)await "{emails}".EvaluateCode(_engine);
                attempts++;
            } while (emails.Count < 1 & attempts < 5);
            email = emails[0];

            Assert.Equal("openbots.test@outlook.com", email.Sender.Address);
            Assert.Equal("FW: toForward", email.Subject);

            resetEmail(_engine, email);
        }

        private void resetEmail(AutomationEngineInstance _engine, MailItem email)
        {
            _deleteOutlookEmail = new DeleteOutlookEmailCommand();

            VariableMethods.CreateTestVariable(email, _engine, "email", typeof(MailItem));

            _deleteOutlookEmail.v_MailItem = "{email}";
            _deleteOutlookEmail.v_DeleteReadOnly = "No";

            _deleteOutlookEmail.RunCommand(_engine);
        }
    }
}
