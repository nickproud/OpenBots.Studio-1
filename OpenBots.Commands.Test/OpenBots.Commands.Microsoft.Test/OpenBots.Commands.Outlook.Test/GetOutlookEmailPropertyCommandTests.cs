using Microsoft.Office.Interop.Outlook;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System.Collections.Generic;
using Xunit;

namespace OpenBots.Commands.Outlook.Test
{
    public class GetOutlookEmailPropertyCommandTests
    {
        private AutomationEngineInstance _engine;
        private GetOutlookEmailsCommand _getOutlookEmails;
        private GetOutlookEmailPropertyCommand _getOutlookEmailProperty;

        /*
         * Prerequisite: User is signed into openbots.test@outlook.com on local Microsoft Outlook.
        */
        [Theory]
        [InlineData("Subject","subjectProp")]
        [InlineData("Body", "bodyProp\r\n")]
        [InlineData("SenderEmailAddress", "openbots.test@outlook.com")]
        [InlineData("UnRead", "False")]
        [InlineData("Recipients", "openbots.test@outlook.com")]
        [InlineData("Size", "57813")]
        public async void GetsOutlookEmailProperty(string prop, string propValue)
        {
            _engine = new AutomationEngineInstance(null);
            _getOutlookEmails = new GetOutlookEmailsCommand();
            _getOutlookEmailProperty = new GetOutlookEmailPropertyCommand();

            VariableMethods.CreateTestVariable(null, _engine, "emails", typeof(List<>));

            _getOutlookEmails.v_SourceFolder = "TestInput";
            _getOutlookEmails.v_Filter = "[Subject] = 'subjectProp'";
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
            VariableMethods.CreateTestVariable(null, _engine, "property", typeof(string));

            _getOutlookEmailProperty.v_MailItem = "{email}";
            _getOutlookEmailProperty.v_Property = prop;
            _getOutlookEmailProperty.v_OutputUserVariableName = "{property}";

            _getOutlookEmailProperty.RunCommand(_engine);

            Assert.Equal(propValue, (string)await "{property}".EvaluateCode(_engine));
        }
    }
}
