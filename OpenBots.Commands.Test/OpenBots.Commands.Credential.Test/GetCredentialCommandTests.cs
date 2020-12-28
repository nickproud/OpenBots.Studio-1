using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System.Security;
using Xunit;
using Xunit.Abstractions;
using System.IO;
using System;

namespace OpenBots.Commands.Credential.Test
{
    public class GetCredentialCommandTests
    {
        private AutomationEngineInstance _engine;
        private GetCredentialCommand _getCredential;
        private readonly ITestOutputHelper output;

        public GetCredentialCommandTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        /*
         * Download the OpenBotsLocalTestData.zip file in OpenBots/COE_Documentation/Studio for credential data.
         * Place the folder into your top level openbots studio directory. Ex. C:\Users\username\source\repos\OpenBots.Studio\
        */
        [Fact]
        public void GetsCredential()
        {
            _engine = new AutomationEngineInstance(null);
            _getCredential = new GetCredentialCommand();

            string credentialName = "CommandTestCreds";

            credentialName.StoreInUserVariable(_engine, "{credName}");

            _getCredential.v_CredentialName = "{credName}";
            _getCredential.v_OutputUserVariableName = "{username}";
            _getCredential.v_OutputUserVariableName2 = "{password}";

            _getCredential.RunCommand(_engine);

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;

            string userFilepath = projectDirectory + @"\OpenBotsLocalTestData\testCredUsername.txt";
            string passwordFilepath = projectDirectory + @"\OpenBotsLocalTestData\testCredPassword.txt";

            string plainPassword = File.ReadAllText(passwordFilepath);
            string username = File.ReadAllText(userFilepath);

            SecureString expectedPass = plainPassword.GetSecureString();
            Assert.Equal(username, "{username}".ConvertUserVariableToString(_engine));
            Assert.Equal(expectedPass.ToString(),"{password}".ConvertUserVariableToObject(_engine).ToString());
        }
    }
}
