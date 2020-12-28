using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System.Security;
using Xunit;
using System.IO;

namespace OpenBots.Commands.Credential.Tests
{
    public class GetCredentialCommandTests
    {
        private AutomationEngineInstance _engine;
        private GetCredentialCommand _getCredential;

        /*
         * Download the OpenBotsLocalTestData.zip file in OpenBots/COE_Documentation/Studio for credential data.
         * Place the folder into your local C drive for the tests to run properly.
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

            string userFilepath = @"C:\OpenBotsLocalTestData\testCredUsername.txt";
            string passwordFilepath = @"C:\OpenBotsLocalTestData\testCredPassword.txt";

            string plainPassword = File.ReadAllText(passwordFilepath);
            string username = File.ReadAllText(userFilepath);

            SecureString expectedPass = plainPassword.GetSecureString();
            Assert.Equal(username, "{username}".ConvertUserVariableToString(_engine));
            Assert.Equal(expectedPass.ToString(),"{password}".ConvertUserVariableToObject(_engine).ToString());
        }
    }
}
