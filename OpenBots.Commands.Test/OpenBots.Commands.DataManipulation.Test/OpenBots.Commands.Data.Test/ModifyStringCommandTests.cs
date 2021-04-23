using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using Xunit;

namespace OpenBots.Commands.Data.Test
{
    public class ModifyStringCommandTests
    {
        private ModifyStringCommand _modifyString;
        private AutomationEngineInstance _engine;

        [Theory]
        [InlineData("lowercase","To Upper Case","LOWERCASE")]
        [InlineData("UPPERcase","To Lower Case","uppercase")]
        [InlineData("test string","To Base64 String", "dGVzdCBzdHJpbmc=")]
        [InlineData("dGVzdCBzdHJpbmc=", "From Base64 String","test string")]
        public async void ModifiesString(string input, string operation, string expectedOutput)
        {
            _modifyString = new ModifyStringCommand();
            _engine = new AutomationEngineInstance(null);

            VariableMethods.CreateTestVariable(input, _engine, "input", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _modifyString.v_InputText = "{input}";
            _modifyString.v_TextOperation = operation;
            _modifyString.v_OutputUserVariableName = "{output}";

            _modifyString.RunCommand(_engine);

            Assert.Equal(expectedOutput, (string)await _modifyString.v_OutputUserVariableName.EvaluateCode(_engine));
        }
    }
}
