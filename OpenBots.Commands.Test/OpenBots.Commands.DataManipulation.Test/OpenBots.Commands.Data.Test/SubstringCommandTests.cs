using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using Xunit;

namespace OpenBots.Commands.Data.Test
{
    public class SubstringCommandTests
    {
        private AutomationEngineInstance _engine;
        private SubstringCommand _substringCommand;

        [Fact]
        public async void CreatesSubstring()
        {
            _engine = new AutomationEngineInstance(null);
            _substringCommand = new SubstringCommand();

            string input = "test text";
            string startIndex = "5";
            string length = "4";
            VariableMethods.CreateTestVariable(input, _engine, "input", typeof(string));
            VariableMethods.CreateTestVariable(startIndex, _engine, "start", typeof(string));
            VariableMethods.CreateTestVariable(length, _engine, "length", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _substringCommand.v_InputText = "{input}";
            _substringCommand.v_StartIndex = "{start}";
            _substringCommand.v_StringLength = "{length}";
            _substringCommand.v_OutputUserVariableName = "{output}";

            _substringCommand.RunCommand(_engine);

            Assert.Equal("text", (string)await "{output}".EvaluateCode(_engine));
        }
    }
}
