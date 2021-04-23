using Microsoft.Office.Interop.Outlook;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System.Collections.Generic;
using Xunit;

namespace OpenBots.Commands.Outlook.Test
{
    public class GetOutlookEmailsCommandTests
    {
        private AutomationEngineInstance _engine;
        private GetOutlookEmailsCommand _getOutlookEmails;

        /*
         * Prerequisite: User is signed into openbots.test@outlook.com on local Microsoft Outlook.
        */
        [Fact]
        public async void GetsOutlookEmails()
        {
            _engine = new AutomationEngineInstance(null);
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
            MailItem email = emails[0];
            Assert.Equal("testBody \r\n", email.Body);
        }
    }
}
