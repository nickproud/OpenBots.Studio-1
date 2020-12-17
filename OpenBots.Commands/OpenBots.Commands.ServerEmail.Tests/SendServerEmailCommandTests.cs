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

        [Fact]
        public void SendServerEmailWithAttachment()
        {
            _engine = new AutomationEngineInstance(null);
            _sendServerEmail = new SendServerEmailCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, @"Resources\");
            string fileName = "testFile.txt";
            string attachment = Path.Combine(filePath, @"Download", fileName);
            string email = filePath + @"Download\" + "One Attachment.msg";

            //Send Server email with no account name (gets default email account)
            _sendServerEmail.v_AccountName = "";
            //TODO: replace email with user's Outlook email
            _sendServerEmail.v_ToRecipients = "nicole.carrero@openbots.ai";
            _sendServerEmail.v_CCRecipients = "";
            _sendServerEmail.v_BCCRecipients = "";
            _sendServerEmail.v_Subject = "One Attachment";
            _sendServerEmail.v_Body = "Test Body";
            _sendServerEmail.v_Attachments = filePath + @"Upload\" + fileName;

            _sendServerEmail.RunCommand(_engine);

            var emailMessage = GetEmail(filePath);

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
            string email = filePath + @"Download\" + "Multiple Attachments.msg";

            _sendServerEmail.v_AccountName = "";
            //TODO: replace email with test Outlook email
            _sendServerEmail.v_ToRecipients = "nicole.carrero@openbots.ai";
            _sendServerEmail.v_CCRecipients = "";
            _sendServerEmail.v_BCCRecipients = "";
            _sendServerEmail.v_Subject = "Multiple Attachments";
            _sendServerEmail.v_Body = "Test Body";
            _sendServerEmail.v_Attachments = filePath + @"Upload\" + fileName1
                + ";" + filePath + @"Upload\" + fileName2;

            _sendServerEmail.RunCommand(_engine);

            var emailMessage = GetEmail(filePath);

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
            string email = filePath + @"Download\" + "No Attachments.msg";

            _sendServerEmail.v_AccountName = "";
            //TODO: replace email with test Outlook email
            _sendServerEmail.v_ToRecipients = "nicole.carrero@openbots.ai";
            _sendServerEmail.v_CCRecipients = "";
            _sendServerEmail.v_BCCRecipients = "";
            _sendServerEmail.v_Subject = "No Attachments";
            _sendServerEmail.v_Body = "Test Body";
            _sendServerEmail.v_Attachments = "";

            _sendServerEmail.RunCommand(_engine);

            var emailMessage = GetEmail(filePath);

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
            string email = filePath + @"Download\" + "Account Name.msg";

            _sendServerEmail.v_AccountName = "Nicole-Accounts";
            //TODO: replace email with test Outlook email
            _sendServerEmail.v_ToRecipients = "nicole.carrero@openbots.ai";
            _sendServerEmail.v_CCRecipients = "";
            _sendServerEmail.v_BCCRecipients = "";
            _sendServerEmail.v_Subject = "Account Name";
            _sendServerEmail.v_Body = "Test Body";
            _sendServerEmail.v_Attachments = "";

            _sendServerEmail.RunCommand(_engine);

            var emailMessage = GetEmail(filePath);

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
            string email = filePath + @"Download\" + "One CC.msg";

            _sendServerEmail.v_AccountName = "";
            //TODO: replace email with test Outlook email
            _sendServerEmail.v_ToRecipients = "nicole.carrero@accelirate.com";
            _sendServerEmail.v_CCRecipients = "nicole.carrero@openbots.ai";
            _sendServerEmail.v_BCCRecipients = "";
            _sendServerEmail.v_Subject = "One CC";
            _sendServerEmail.v_Body = "Test Body";
            _sendServerEmail.v_Attachments = "";

            _sendServerEmail.RunCommand(_engine);

            var emailMessage = GetEmail(filePath);

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
            string email = filePath + @"Download\" + "One BCC.msg";

            _sendServerEmail.v_AccountName = "";
            //TODO: replace email with test Outlook email
            _sendServerEmail.v_ToRecipients = "nicole.carrero@accelirate.com";
            _sendServerEmail.v_CCRecipients = "";
            _sendServerEmail.v_BCCRecipients = "nicole.carrero@openbots.ai";
            _sendServerEmail.v_Subject = "One BCC";
            _sendServerEmail.v_Body = "Test Body";
            _sendServerEmail.v_Attachments = "";

            _sendServerEmail.RunCommand(_engine);

            var emailMessage = GetEmail(filePath);

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
            string email = filePath + @"Download\" + "Multiple CC.msg";

            _sendServerEmail.v_AccountName = "";
            //TODO: replace email with test Outlook email
            _sendServerEmail.v_ToRecipients = "nicole.carrero@accelirate.com";
            _sendServerEmail.v_CCRecipients = "ncarrero18@gmail.com;nicole.carrero@openbots.ai";
            _sendServerEmail.v_BCCRecipients = "";
            _sendServerEmail.v_Subject = "Multiple CC";
            _sendServerEmail.v_Body = "Test Body";
            _sendServerEmail.v_Attachments = "";

            _sendServerEmail.RunCommand(_engine);

            var emailMessage = GetEmail(filePath);

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
            string email = filePath + @"Download\" + "Multiple BCC.msg";

            _sendServerEmail.v_AccountName = "";
            //TODO: replace email with test Outlook email
            _sendServerEmail.v_ToRecipients = "nicole.carrero@accelirate.com";
            _sendServerEmail.v_CCRecipients = "";
            _sendServerEmail.v_BCCRecipients = "ncarrero18@gmail.com;nicole.carrero@openbots.ai";
            _sendServerEmail.v_Subject = "Multiple BCC";
            _sendServerEmail.v_Body = "Test Body";
            _sendServerEmail.v_Attachments = "";

            _sendServerEmail.RunCommand(_engine);

            var emailMessage = GetEmail(filePath);

            Assert.True(File.Exists(email));

            DeleteEmail(emailMessage);
            File.Delete(email);
        }

        [Fact]
        public void SendServerEmailWithNoSubject()
        {
            _engine = new AutomationEngineInstance(null);
            _sendServerEmail = new SendServerEmailCommand();
            _getEmail = new GetOutlookEmailsCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, @"Resources\");
            string email = filePath + @"Download\" + "(no subject).msg";

            _sendServerEmail.v_AccountName = "";
            //TODO: replace email with test Outlook email
            _sendServerEmail.v_ToRecipients = "nicole.carrero@openbots.ai";
            _sendServerEmail.v_CCRecipients = "";
            _sendServerEmail.v_BCCRecipients = "";
            _sendServerEmail.v_Subject = "";
            _sendServerEmail.v_Body = "Test Body";
            _sendServerEmail.v_Attachments = "";

            _sendServerEmail.RunCommand(_engine);

            var emailMessage = GetEmail(filePath);

            //Assert that email was sent successfully
            Assert.True(File.Exists(email));

            DeleteEmail(emailMessage);
            File.Delete(email);
        }

        [Fact]
        public void SendServerEmailWithNoBody()
        {
            _engine = new AutomationEngineInstance(null);
            _sendServerEmail = new SendServerEmailCommand();
            _getEmail = new GetOutlookEmailsCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, @"Resources\");
            string email = filePath + @"Download\" + "No Body.msg";

            _sendServerEmail.v_AccountName = "";
            //TODO: replace email with test Outlook email
            _sendServerEmail.v_ToRecipients = "nicole.carrero@openbots.ai";
            _sendServerEmail.v_CCRecipients = "";
            _sendServerEmail.v_BCCRecipients = "";
            _sendServerEmail.v_Subject = "No Body";
            _sendServerEmail.v_Body = "";
            _sendServerEmail.v_Attachments = "";

            _sendServerEmail.RunCommand(_engine);

            var emailMessage = GetEmail(filePath);

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
            //TODO: replace email with test Outlook email
            _sendServerEmail.v_ToRecipients = "";
            _sendServerEmail.v_CCRecipients = "";
            _sendServerEmail.v_BCCRecipients = "";
            _sendServerEmail.v_Subject = "One BCC";
            _sendServerEmail.v_Body = "Test Body";
            _sendServerEmail.v_Attachments = "";

            Assert.Throws<NullReferenceException>(() => _sendServerEmail.RunCommand(_engine));
        }

        public MailItem GetEmail(string filePath)
        {
            _getEmail = new GetOutlookEmailsCommand();

            //Wait 30 seconds for email to be sent to user
            System.Threading.Thread.Sleep(30000);

            _getEmail.v_SourceFolder = "Inbox";
            _getEmail.v_Filter = "None";
            _getEmail.v_GetUnreadOnly = "Yes";
            _getEmail.v_MarkAsRead = "Yes";
            _getEmail.v_SaveMessagesAndAttachments = "Yes";
            _getEmail.v_MessageDirectory = filePath + @"Download\";
            _getEmail.v_AttachmentDirectory = filePath + @"Download\";
            _getEmail.v_OutputUserVariableName = "{vTestEmail}";

            _getEmail.RunCommand(_engine);

            var emailMessageList = (List<MailItem>)"{vTestEmail}".ConvertUserVariableToObject(_engine);
            var emailMessage = emailMessageList[0];

            return emailMessage;
        }

        public void DeleteEmail(object emailMessage)
        {
            _deleteEmail = new DeleteOutlookEmailCommand();

            _deleteEmail.v_MailItem = "{vMailItem}";
            emailMessage.StoreInUserVariable(_engine, _deleteEmail.v_MailItem);
            _deleteEmail.v_DeleteReadOnly = "Yes";

            _deleteEmail.RunCommand(_engine);
        }
    }
}
