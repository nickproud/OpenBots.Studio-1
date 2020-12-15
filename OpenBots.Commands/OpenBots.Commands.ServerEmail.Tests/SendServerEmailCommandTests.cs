using OpenBots.Commands.Outlook;
using OpenBots.Engine;
using System;
using System.IO;
using Xunit;

namespace OpenBots.Commands.ServerEmail.Test
{
    public class SendServerEmailCommandTests
    {
        private AutomationEngineInstance _engine;
        private SendServerEmailCommand _sendServerEmail;
        private GetOutlookEmailsCommand _getEmail;

        [Fact]
        public void SendServerEmailWithAttachment()
        {
            _engine = new AutomationEngineInstance(null);
            _sendServerEmail = new SendServerEmailCommand();
            _getEmail = new GetOutlookEmailsCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, @"Resources\");
            string fileName = "testFile.txt";
            string attachment = Path.Combine(filePath, @"Download\Attachments", fileName);
            string email = filePath + @"Download\Messages\" + "Test Subject.msg";

            //Send email
            _sendServerEmail.v_AccountName = "";
            //TODO: replace email with user's Outlook email
            _sendServerEmail.v_ToRecipients = "nicole.carrero@openbots.ai";
            _sendServerEmail.v_CCRecipients = "";
            _sendServerEmail.v_BCCRecipients = "";
            _sendServerEmail.v_Subject = "Test Subject";
            _sendServerEmail.v_Body = "Test Body";
            _sendServerEmail.v_Attachments = filePath + @"Upload\Attachments\" + fileName;

            _sendServerEmail.RunCommand(_engine);

            //Get email
            _getEmail.v_SourceFolder = "Inbox";
            _getEmail.v_Filter = "None";
            _getEmail.v_GetUnreadOnly = "Yes";
            _getEmail.v_MarkAsRead = "No";
            _getEmail.v_SaveMessagesAndAttachments = "Yes";
            _getEmail.v_MessageDirectory = filePath + @"Download\Messages\";
            _getEmail.v_AttachmentDirectory = filePath + @"Download\Attachments";
            _getEmail.v_OutputUserVariableName = "{vTestEmail}";

            _getEmail.RunCommand(_engine);

            //Assert that email was sent successfully
            Assert.True(File.Exists(email));
            //Assert the attachment exists
            Assert.True(File.Exists(attachment));

            File.Delete(email);
            File.Delete(attachment);
        }

        [Fact]
        public void SendServerEmailWithMultipleAttachments()
        {
            _engine = new AutomationEngineInstance(null);
            _sendServerEmail = new SendServerEmailCommand();
            _getEmail = new GetOutlookEmailsCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, @"Resources\");
            string fileName1 = "testFile.txt";
            string attachment1 = Path.Combine(filePath, @"Download\Attachments", fileName1);
            string fileName2 = "testFile2.txt";
            string attachment2 = Path.Combine(filePath, @"Download\Attachments", fileName2);
            string email = filePath + @"Download\Messages\" + "Test Subject.msg";

            //Send email
            _sendServerEmail.v_AccountName = "";
            //TODO: replace email with user's Outlook email
            _sendServerEmail.v_ToRecipients = "nicole.carrero@openbots.ai";
            _sendServerEmail.v_CCRecipients = "";
            _sendServerEmail.v_BCCRecipients = "";
            _sendServerEmail.v_Subject = "Test Subject";
            _sendServerEmail.v_Body = "Test Body";
            _sendServerEmail.v_Attachments = Path.Combine(filePath, @"Upload\Attachments", fileName1)
                + ";" + Path.Combine(filePath, @"Upload\Attachments", fileName2);

            _sendServerEmail.RunCommand(_engine);

            //Get email
            _getEmail.v_SourceFolder = "Inbox";
            _getEmail.v_Filter = "None";
            _getEmail.v_GetUnreadOnly = "Yes";
            _getEmail.v_MarkAsRead = "No";
            _getEmail.v_SaveMessagesAndAttachments = "Yes";
            _getEmail.v_MessageDirectory = filePath + @"Download\Messages\";
            _getEmail.v_AttachmentDirectory = filePath + @"Download\Attachments";
            _getEmail.v_OutputUserVariableName = "{vTestEmail}";

            _getEmail.RunCommand(_engine);

            //Assert that email was sent successfully
            Assert.True(File.Exists(email));
            //Assert the attachment exists
            Assert.True(File.Exists(attachment1));
            Assert.True(File.Exists(attachment2));

            File.Delete(email);
            File.Delete(attachment1);
            File.Delete(attachment2);
        }

        //SendServerEmailWithNoAttachments
        //SendServerEmailWithAccountName
        //HandlesNonExistentAccountName - throw error
        //SendServerEmailWithNoAccountName
        //SendServerEmailWithCc
        //SendServerEmailWithBcc
        //SendServerEmailWithNoSubject - throw error?
        //SendServerEmailWithNoBody - throw error?
        //HandlesNonExistentRecipient - throw error
    }
}
