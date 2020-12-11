using OpenBots.Core.Utilities.CommonUtilities;
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

        [Fact]
        public void SendServerEmailWithAttachment()
        {
            _engine = new AutomationEngineInstance(null);
            _sendServerEmail = new SendServerEmailCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, @"Resources\");

            _sendServerEmail.v_AccountName = "";
            _sendServerEmail.v_ToRecipients = "ncarrero18@gmail.com";
            _sendServerEmail.v_CCRecipients = "";
            _sendServerEmail.v_BCCRecipients = "";
            _sendServerEmail.v_Subject = "Test Subject";
            _sendServerEmail.v_Body = "Test Body";
            _sendServerEmail.v_Attachments = filePath;

            _sendServerEmail.RunCommand(_engine);

            string outputEmail = "{output}".ConvertUserVariableToString(_engine);
            //Assert.True(File.Exists(filePath + @"Download\newtest.txt"));
            
        }

        //SendServerEmailWithMultipleAttachments
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
