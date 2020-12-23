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
        public void GetsOutlookEmailProperty(string prop, string propValue)
        {
            _engine = new AutomationEngineInstance(null);
            _getOutlookEmails = new GetOutlookEmailsCommand();
            _getOutlookEmailProperty = new GetOutlookEmailPropertyCommand();

            _getOutlookEmails.v_SourceFolder = "TestInput";
            _getOutlookEmails.v_Filter = "[Subject] = 'subjectProp'";
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

            _getOutlookEmailProperty.v_MailItem = "{email}";
            _getOutlookEmailProperty.v_Property = prop;
            _getOutlookEmailProperty.v_OutputUserVariableName = "{property}";

            _getOutlookEmailProperty.RunCommand(_engine);

            Assert.Equal(propValue, "{property}".ConvertUserVariableToString(_engine));
        }
    }
}
