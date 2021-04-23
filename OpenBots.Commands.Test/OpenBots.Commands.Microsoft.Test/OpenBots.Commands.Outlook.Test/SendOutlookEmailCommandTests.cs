using Microsoft.Office.Interop.Outlook;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System.Collections.Generic;
using Xunit;

namespace OpenBots.Commands.Outlook.Test
{
    public class SendOutlookEmailCommandTests
    {
        private AutomationEngineInstance _engine;
        private SendOutlookEmailCommand _sendOutlookEmail;
        private GetOutlookEmailsCommand _getOutlookEmails;
        private DeleteOutlookEmailCommand _deleteOutlookEmail;

        /*
         * Prerequisite: User is signed into openbots.test@outlook.com on local Microsoft Outlook.
        */
        [Fact]
        public async void SendsOutlookEmail()
        {
            _engine = new AutomationEngineInstance(null);
            _sendOutlookEmail = new SendOutlookEmailCommand();
            _getOutlookEmails = new GetOutlookEmailsCommand();

            _sendOutlookEmail.v_Recipients = "openbots.test@outlook.com";
            _sendOutlookEmail.v_Subject = "testSend";
            _sendOutlookEmail.v_Body = "testBody";
            _sendOutlookEmail.v_BodyType = "Plain";

            _sendOutlookEmail.RunCommand(_engine);

            VariableMethods.CreateTestVariable(null, _engine, "emails", typeof(List<>));
            List<MailItem> emails;
            int attempts = 0;
            do {
                System.Threading.Thread.Sleep(5000);
                _getOutlookEmails.v_SourceFolder = "Inbox";
                _getOutlookEmails.v_Filter = "[Subject] = 'testSend'";
                _getOutlookEmails.v_GetUnreadOnly = "No";
                _getOutlookEmails.v_MarkAsRead = "No";
                _getOutlookEmails.v_SaveMessagesAndAttachments = "No";
                _getOutlookEmails.v_MessageDirectory = "";
                _getOutlookEmails.v_AttachmentDirectory = "";
                _getOutlookEmails.v_OutputUserVariableName = "{emails}";

                _getOutlookEmails.RunCommand(_engine);

                emails = (List<MailItem>)await "{emails}".EvaluateCode(_engine);
                attempts++;
            } while (emails.Count < 1 && attempts < 5);
            MailItem email = emails[0];

            Assert.Equal("testSend", email.Subject);

            resetSendEmail(_engine, email);
        }

        private void resetSendEmail(AutomationEngineInstance _engine, MailItem email)
        {
            _deleteOutlookEmail = new DeleteOutlookEmailCommand();
            VariableMethods.CreateTestVariable(email, _engine, "email", typeof(List<>));

            _deleteOutlookEmail.v_MailItem = "{email}";
            _deleteOutlookEmail.v_DeleteReadOnly = "No";

            _deleteOutlookEmail.RunCommand(_engine);
        }
    }
}
