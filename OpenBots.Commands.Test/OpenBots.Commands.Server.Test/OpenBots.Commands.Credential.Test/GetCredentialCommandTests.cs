using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System.Security;
using Xunit;
using Xunit.Abstractions;
using System.IO;
using System;
using OpenBots.Core.Properties;

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
         * Place the folder into your openbots core resources directory. Ex. OpenBots.Studio\OpenBots.Core\Resources\Credentials
        */
        [Fact]
        public void GetsCredential()
        {
            _engine = new AutomationEngineInstance(null);
            _getCredential = new GetCredentialCommand();

            string credentialName = "CommandTestCreds";

            VariableMethods.CreateTestVariable(credentialName, _engine, "credName", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "username", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "password", typeof(SecureString));

            _getCredential.v_CredentialName = "{credName}";
            _getCredential.v_OutputUserVariableName = "{username}";
            _getCredential.v_OutputUserVariableName2 = "{password}";

            _getCredential.RunCommand(_engine);

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;

            string username = Resources.testCredUsername;
            string plainPassword = Resources.testCredPassword;

            SecureString expectedPass = plainPassword.ConvertStringToSecureString();
            Assert.Equal(username, "{username}".ConvertUserVariableToString(_engine));
            Assert.Equal(expectedPass.ToString(),"{password}".ConvertUserVariableToObject(_engine, typeof(SecureString)).ToString());
        }
    }
}
