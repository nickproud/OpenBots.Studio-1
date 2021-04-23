using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using Xunit;
using System.Security;

namespace OpenBots.Commands.Credential.Test
{
    public class UpdateCredentialCommandTests
    {
        private AutomationEngineInstance _engine;
        private UpdateCredentialCommand _updateCredential;
        private GetCredentialCommand _getCredential;

        [Fact]
        public async void UpdatesCredential()
        {
            _engine = new AutomationEngineInstance(null);
            _updateCredential = new UpdateCredentialCommand();
            _getCredential = new GetCredentialCommand();

            string[] oldCreds = resetCredential();

            string credentialName = "UpdateTestCreds";
            string newUsername = "newTestUser";
            string newPassword = "newTestPassword";

            VariableMethods.CreateTestVariable(credentialName, _engine, "credName", typeof(string));
            VariableMethods.CreateTestVariable(newUsername, _engine, "username", typeof(string));
            VariableMethods.CreateTestVariable(newPassword, _engine, "password", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "storedUsername", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "storedPassword", typeof(SecureString));

            _updateCredential.v_CredentialName = "{credName}";
            _updateCredential.v_CredentialUsername = "{username}";
            _updateCredential.v_CredentialPassword = "{password}";

            _updateCredential.RunCommand(_engine);

            _getCredential.v_CredentialName = "{credName}";
            _getCredential.v_OutputUserVariableName = "{storedUsername}";
            _getCredential.v_OutputUserVariableName2 = "{storedPassword}";

            _getCredential.RunCommand(_engine);


            Assert.Equal(newUsername, (string)await "{storedUsername}".EvaluateCode(_engine));
            Assert.Equal(newPassword.ConvertStringToSecureString().ToString(), (await "{storedPassword}".EvaluateCode(_engine)).ToString());
        }

        public string[] resetCredential()
        {
            _engine = new AutomationEngineInstance(null);
            _updateCredential = new UpdateCredentialCommand();

            string credentialName = "UpdateTestCreds";
            string username = "testUser";
            string password = "testPassword";

            _updateCredential.v_CredentialName = credentialName;
            _updateCredential.v_CredentialUsername = username;
            _updateCredential.v_CredentialPassword = password;

            _updateCredential.RunCommand(_engine);

            string[] userPass = { username, password };
            return userPass;
        }
    }
}
