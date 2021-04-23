using Microsoft.Office.Interop.Outlook;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System.Collections.Generic;
using Xunit;

namespace OpenBots.Commands.Outlook.Test
{
    public class DeleteOutlookEmailCommandTests
    {
        private AutomationEngineInstance _engine;
        private DeleteOutlookEmailCommand _deleteOutlookEmail;
        private GetOutlookEmailsCommand _getOutlookEmails;
        private SendOutlookEmailCommand _sendOutlookEmail;

        /*
         * Prerequisite: User is signed into openbots.test@outlook.com on local Microsoft Outlook.
        */
        [Fact]
        public async void DeletesOutlookEmail()
        {
            _engine = new AutomationEngineInstance(null);
            _deleteOutlookEmail = new DeleteOutlookEmailCommand();
            _getOutlookEmails = new GetOutlookEmailsCommand();

            VariableMethods.CreateTestVariable(null, _engine, "emails", typeof(List<>));

            _getOutlookEmails.v_SourceFolder = "Inbox";
            _getOutlookEmails.v_Filter = "[Subject] = 'toDelete'";
            _getOutlookEmails.v_GetUnreadOnly = "No";
            _getOutlookEmails.v_MarkAsRead = "No";
            _getOutlookEmails.v_SaveMessagesAndAttachments = "No";
            _getOutlookEmails.v_MessageDirectory = "";
            _getOutlookEmails.v_AttachmentDirectory = "";
            _getOutlookEmails.v_OutputUserVariableName = "{emails}";

            _getOutlookEmails.RunCommand(_engine);

            var emails = (List<MailItem>)await "{emails}".EvaluateCode(_engine);
            if(emails.Count == 0)
            {
                throw new System.ArgumentException("Test email 'toDelete' was not found");
            }
            MailItem email = emails[0];
            VariableMethods.CreateTestVariable(email, _engine, "email", typeof(MailItem));

            _deleteOutlookEmail.v_MailItem = "{email}";
            _deleteOutlookEmail.v_DeleteReadOnly = "No";
            _deleteOutlookEmail.RunCommand(_engine);

            _getOutlookEmails.v_SourceFolder = "Inbox";
            _getOutlookEmails.v_Filter = "[Subject] = 'toDelete'";
            _getOutlookEmails.v_GetUnreadOnly = "No";
            _getOutlookEmails.v_MarkAsRead = "No";
            _getOutlookEmails.v_SaveMessagesAndAttachments = "No";
            _getOutlookEmails.v_MessageDirectory = "";
            _getOutlookEmails.v_AttachmentDirectory = "";
            _getOutlookEmails.v_OutputUserVariableName = "{emails}";

            _getOutlookEmails.RunCommand(_engine);
            List<MailItem> postEmails = (List<MailItem>)await "{emails}".EvaluateCode(_engine);
            resetEmail(_engine);
            Assert.Empty(postEmails);
        }

        internal void resetEmail(AutomationEngineInstance _engine)
        {
            _sendOutlookEmail = new SendOutlookEmailCommand();

            _sendOutlookEmail.v_Recipients = "openbots.test@outlook.com";
            _sendOutlookEmail.v_Subject = "toDelete";
            _sendOutlookEmail.v_Body = "testBody";
            _sendOutlookEmail.v_BodyType = "Plain";

            _sendOutlookEmail.RunCommand(_engine);
        }
    }
}
