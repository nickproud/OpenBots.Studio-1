using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using Xunit;

namespace OpenBots.Commands.Data.Test
{
    public class ReplaceTextCommandTests
    {
        private ReplaceTextCommand _replaceTextCommand;
        private AutomationEngineInstance _engine;

        [Fact]
        public void ReplacesText()
        {
            _engine = new AutomationEngineInstance(null);
            _replaceTextCommand = new ReplaceTextCommand();

            string inputText = "Hello john";
            string oldSubstring = "Hello";
            string newSubstring = "Goodbye";
            VariableMethods.CreateTestVariable(inputText, _engine, "input", typeof(string));
            VariableMethods.CreateTestVariable(oldSubstring, _engine, "old", typeof(string));
            VariableMethods.CreateTestVariable(newSubstring, _engine, "new", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _replaceTextCommand.v_InputText = "{input}";
            _replaceTextCommand.v_OldText = "{old}";
            _replaceTextCommand.v_NewText = "{new}";
            _replaceTextCommand.v_OutputUserVariableName = "{output}";

            _replaceTextCommand.RunCommand(_engine);

            Assert.Equal("Goodbye john", "{output}".ConvertUserVariableToString(_engine));
        }
    }
}
