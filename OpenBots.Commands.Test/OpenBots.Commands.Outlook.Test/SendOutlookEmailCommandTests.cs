using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using OpenBots.Engine;
using OpenBots.Core.Utilities.CommonUtilities;
using Microsoft.Office.Interop.Outlook;

namespace OpenBots.Commands.Outlook.Test
{
    public class SendOutlookEmailCommandTests
    {
        private AutomationEngineInstance _engine;
        private SendOutlookEmailCommand _sendOutlookEmail;
        private GetOutlookEmailsCommand _getOutlookEmails;
        private DeleteOutlookEmailCommand _deleteOutlookEmail;

        [Fact]
        public void SendsOutlookEmail()
        {
            _engine = new AutomationEngineInstance(null);
            _sendOutlookEmail = new SendOutlookEmailCommand();
            _getOutlookEmails = new GetOutlookEmailsCommand();

            _sendOutlookEmail.v_Recipients = "openbots.test@outlook.com";
            _sendOutlookEmail.v_Subject = "testSend";
            _sendOutlookEmail.v_Body = "testBody";
            _sendOutlookEmail.v_BodyType = "Plain";

            _sendOutlookEmail.RunCommand(_engine);

            System.Threading.Thread.Sleep(30000);

            _getOutlookEmails.v_SourceFolder = "Inbox";
            _getOutlookEmails.v_Filter = "[Subject] = 'testSend'";
            _getOutlookEmails.v_GetUnreadOnly = "No";
            _getOutlookEmails.v_MarkAsRead = "No";
            _getOutlookEmails.v_SaveMessagesAndAttachments = "No";
            _getOutlookEmails.v_MessageDirectory = "";
            _getOutlookEmails.v_AttachmentDirectory = "";
            _getOutlookEmails.v_OutputUserVariableName = "{emails}";

            _getOutlookEmails.RunCommand(_engine);

            var emails = (List<MailItem>)"{emails}".ConvertUserVariableToObject(_engine);
            MailItem email = emails[0];

            Assert.Equal("testSend", email.Subject);

            resetSendEmail(_engine, email);
        }

        private void resetSendEmail(AutomationEngineInstance _engine, MailItem email)
        {
            _deleteOutlookEmail = new DeleteOutlookEmailCommand();
            email.StoreInUserVariable(_engine, "{email}");

            _deleteOutlookEmail.v_MailItem = "{email}";
            _deleteOutlookEmail.v_DeleteReadOnly = "No";

            _deleteOutlookEmail.RunCommand(_engine);
        }
    }
}
