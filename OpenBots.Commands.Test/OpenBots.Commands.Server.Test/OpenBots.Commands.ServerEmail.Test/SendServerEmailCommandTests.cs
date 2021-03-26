using Microsoft.Office.Interop.Outlook;
using OpenBots.Commands.Outlook;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace OpenBots.Commands.ServerEmail.Test
{
    public class SendServerEmailCommandTests
    {
        private AutomationEngineInstance _engine;
        private SendServerEmailCommand _sendServerEmail;
        private GetOutlookEmailsCommand _getEmail;
        private DeleteOutlookEmailCommand _deleteEmail;

        /*
        * Prerequisite: User is signed into openbots.test.1@outlook.com on local Microsoft Outlook.
        */

        [Fact]
        public void SendServerEmailWithAttachment()
        {
            _engine = new AutomationEngineInstance(null);
            _sendServerEmail = new SendServerEmailCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, @"Resources\");
            string fileName = "testFile.txt";
            string attachment = Path.Combine(filePath, @"Download", fileName);
            string subject = "One Attachment";
            string email = filePath + @"Download\" + $"{subject}.msg";

            //Send Server email with no account name (gets default email account)
            _sendServerEmail.v_AccountName = "";
            _sendServerEmail.v_ToRecipients = "openbots.test.1@outlook.com";
            _sendServerEmail.v_CCRecipients = "";
            _sendServerEmail.v_BCCRecipients = "";
            _sendServerEmail.v_Subject = subject;
            _sendServerEmail.v_Body = "Test Body";
            _sendServerEmail.v_Attachments = filePath + @"Upload\" + fileName;

            _sendServerEmail.RunCommand(_engine);

            var emailMessage = GetEmail(filePath, subject);

            Assert.True(File.Exists(email));
            Assert.True(File.Exists(attachment));

            DeleteEmail(emailMessage);
            File.Delete(email);
            File.Delete(attachment);
        }

        [Fact]
        public void SendServerEmailWithMultipleAttachments()
        {
            _engine = new AutomationEngineInstance(null);
            _sendServerEmail = new SendServerEmailCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, @"Resources\");
            string fileName1 = "testFile.txt";
            string fileName2 = "testFile2.txt";
            string attachment1 = filePath + @"Download\" + fileName1;
            string attachment2 = filePath + @"Download\" + fileName2;
            string subject = "Multiple Attachments";
            string email = filePath + @"Download\" + $"{subject}.msg";

            _sendServerEmail.v_AccountName = "";
            _sendServerEmail.v_ToRecipients = "openbots.test.1@outlook.com";
            _sendServerEmail.v_CCRecipients = "";
            _sendServerEmail.v_BCCRecipients = "";
            _sendServerEmail.v_Subject = subject;
            _sendServerEmail.v_Body = "Test Body";
            _sendServerEmail.v_Attachments = filePath + @"Upload\" + fileName1
                + ";" + filePath + @"Upload\" + fileName2;

            _sendServerEmail.RunCommand(_engine);

            var emailMessage = GetEmail(filePath, subject);

            Assert.True(File.Exists(email));
            Assert.True(File.Exists(attachment1));
            Assert.True(File.Exists(attachment2));

            DeleteEmail(emailMessage);
            File.Delete(email);
            File.Delete(attachment1);
            File.Delete(attachment2);
        }

        [Fact]
        public void SendServerEmailWithNoAttachments()
        {
            _engine = new AutomationEngineInstance(null);
            _sendServerEmail = new SendServerEmailCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, @"Resources\");
            string subject = "No Attachments";
            string email = filePath + @"Download\" + $"{subject}.msg";

            _sendServerEmail.v_AccountName = "";
            _sendServerEmail.v_ToRecipients = "openbots.test.1@outlook.com";
            _sendServerEmail.v_CCRecipients = "";
            _sendServerEmail.v_BCCRecipients = "";
            _sendServerEmail.v_Subject = subject;
            _sendServerEmail.v_Body = "Test Body";
            _sendServerEmail.v_Attachments = "";

            _sendServerEmail.RunCommand(_engine);

            var emailMessage = GetEmail(filePath, subject);

            Assert.True(File.Exists(email));

            DeleteEmail(emailMessage);
            File.Delete(email);
        }

        [Fact]
        public void SendServerEmailWithAccountName()
        {
            _engine = new AutomationEngineInstance(null);
            _sendServerEmail = new SendServerEmailCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, @"Resources\");
            string subject = "Account Name";
            string email = filePath + @"Download\" + $"{subject}.msg";

            _sendServerEmail.v_AccountName = "Nicole-Accounts";
            _sendServerEmail.v_ToRecipients = "openbots.test.1@outlook.com";
            _sendServerEmail.v_CCRecipients = "";
            _sendServerEmail.v_BCCRecipients = "";
            _sendServerEmail.v_Subject = subject;
            _sendServerEmail.v_Body = "Test Body";
            _sendServerEmail.v_Attachments = "";

            _sendServerEmail.RunCommand(_engine);

            var emailMessage = GetEmail(filePath, subject);

            Assert.True(File.Exists(email));

            DeleteEmail(emailMessage);
            File.Delete(email);
        }
        [Fact]
        public void SendServerEmailWithOneCC()
        {
            _engine = new AutomationEngineInstance(null);
            _sendServerEmail = new SendServerEmailCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, @"Resources\");
            string subject = "One CC";
            string email = filePath + @"Download\" + $"{subject}.msg";

            _sendServerEmail.v_AccountName = "";
            _sendServerEmail.v_ToRecipients = "openbots.test.2@outlook.com";
            _sendServerEmail.v_CCRecipients = "openbots.test.1@outlook.com";
            _sendServerEmail.v_BCCRecipients = "";
            _sendServerEmail.v_Subject = subject;
            _sendServerEmail.v_Body = "Test Body";
            _sendServerEmail.v_Attachments = "";

            _sendServerEmail.RunCommand(_engine);

            var emailMessage = GetEmail(filePath, subject);

            Assert.True(File.Exists(email));

            DeleteEmail(emailMessage);
            File.Delete(email);
        }

        [Fact]
        public void SendServerEmailWithOneBCC()
        {
            _engine = new AutomationEngineInstance(null);
            _sendServerEmail = new SendServerEmailCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, @"Resources\");
            string subject = "One BCC";
            string email = filePath + @"Download\" + $"{subject}.msg";

            _sendServerEmail.v_AccountName = "";
            _sendServerEmail.v_ToRecipients = "openbots.test.2@outlook.com";
            _sendServerEmail.v_CCRecipients = "";
            _sendServerEmail.v_BCCRecipients = "openbots.test.1@outlook.com";
            _sendServerEmail.v_Subject = subject;
            _sendServerEmail.v_Body = "Test Body";
            _sendServerEmail.v_Attachments = "";

            _sendServerEmail.RunCommand(_engine);

            var emailMessage = GetEmail(filePath, subject);

            Assert.True(File.Exists(email));

            DeleteEmail(emailMessage);
            File.Delete(email);
        }

        [Fact]
        public void SendServerEmaiWithMultipleCC()
        {
            _engine = new AutomationEngineInstance(null);
            _sendServerEmail = new SendServerEmailCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, @"Resources\");
            string subject = "Multiple CC";
            string email = filePath + @"Download\" + $"{subject}.msg";

            _sendServerEmail.v_AccountName = "";
            _sendServerEmail.v_ToRecipients = "openbots.test.2@outlook.com";
            _sendServerEmail.v_CCRecipients = "openbots.test@outlook.com;openbots.test.1@outlook.com";
            _sendServerEmail.v_BCCRecipients = "";
            _sendServerEmail.v_Subject = subject;
            _sendServerEmail.v_Body = "Test Body";
            _sendServerEmail.v_Attachments = "";

            _sendServerEmail.RunCommand(_engine);

            var emailMessage = GetEmail(filePath, subject);

            Assert.True(File.Exists(email));

            DeleteEmail(emailMessage);
            File.Delete(email);
        }

        [Fact]
        public void SendServerEmailWithMultipleBCC()
        {
            _engine = new AutomationEngineInstance(null);
            _sendServerEmail = new SendServerEmailCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, @"Resources\");
            string subject = "Multiple BCC";
            string email = filePath + @"Download\" + $"{subject}.msg";

            _sendServerEmail.v_AccountName = "";
            _sendServerEmail.v_ToRecipients = "openbots.test.2@outlook.com";
            _sendServerEmail.v_CCRecipients = "";
            _sendServerEmail.v_BCCRecipients = "openbots.test@outlook.com;openbots.test.1@outlook.com";
            _sendServerEmail.v_Subject = "Multiple BCC";
            _sendServerEmail.v_Body = "Test Body";
            _sendServerEmail.v_Attachments = "";

            _sendServerEmail.RunCommand(_engine);

            var emailMessage = GetEmail(filePath, subject);

            Assert.True(File.Exists(email));

            DeleteEmail(emailMessage);
            File.Delete(email);
        }

        [Fact]
        public void HandlesNonExistentRecipients()
        {
            _engine = new AutomationEngineInstance(null);
            _sendServerEmail = new SendServerEmailCommand();

            _sendServerEmail.v_AccountName = "";
            _sendServerEmail.v_ToRecipients = "";
            _sendServerEmail.v_CCRecipients = "";
            _sendServerEmail.v_BCCRecipients = "";
            _sendServerEmail.v_Subject = "One BCC";
            _sendServerEmail.v_Body = "Test Body";
            _sendServerEmail.v_Attachments = "";

            Assert.Throws<NullReferenceException>(() => _sendServerEmail.RunCommand(_engine));
        }

        public MailItem GetEmail(string filePath, string subject)
        {
            _getEmail = new GetOutlookEmailsCommand();
            VariableMethods.CreateTestVariable(null, _engine, "vTestEmail", typeof(List<>));

            var emailMessageList = new List<MailItem>();
            Application outlookApp = new Application();
            MailItem emailMessage = (MailItem)outlookApp.CreateItem(OlItemType.olMailItem);
            var retryCount = 0;

            while (emailMessageList.Count == 0 && retryCount <= 5)
            {
                System.Threading.Thread.Sleep(5000);

                _getEmail.v_SourceFolder = "Inbox";
                    _getEmail.v_Filter = $"[Subject] = '{subject}'";
                _getEmail.v_GetUnreadOnly = "Yes";
                _getEmail.v_MarkAsRead = "Yes";
                _getEmail.v_SaveMessagesAndAttachments = "Yes";
                _getEmail.v_MessageDirectory = filePath + @"Download\";
                _getEmail.v_AttachmentDirectory = filePath + @"Download\";
                _getEmail.v_OutputUserVariableName = "{vTestEmail}";

                _getEmail.RunCommand(_engine);

                emailMessageList = (List<MailItem>)"{vTestEmail}".ConvertUserVariableToObject(_engine, typeof(List<>));
                if (emailMessageList.Count > 0)
                    emailMessage = emailMessageList[0];

                retryCount++;
            }

            return emailMessage;
        }

        internal void DeleteEmail(object emailMessage)
        {
            _deleteEmail = new DeleteOutlookEmailCommand();

            VariableMethods.CreateTestVariable(null, _engine, "vMailItem", typeof(MailItem));
            _deleteEmail.v_MailItem = "{vMailItem}";
            emailMessage.StoreInUserVariable(_engine, _deleteEmail.v_MailItem, typeof(MailItem));
            _deleteEmail.v_DeleteReadOnly = "Yes";

            _deleteEmail.RunCommand(_engine);
        }
    }
}
