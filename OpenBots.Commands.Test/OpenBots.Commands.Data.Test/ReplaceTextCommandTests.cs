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
            inputText.CreateTestVariable(_engine, "input");
            oldSubstring.CreateTestVariable(_engine, "old");
            newSubstring.CreateTestVariable(_engine, "new");
            "unassigned".CreateTestVariable(_engine, "output");

            _replaceTextCommand.v_InputText = "{input}";
            _replaceTextCommand.v_OldText = "{old}";
            _replaceTextCommand.v_NewText = "{new}";
            _replaceTextCommand.v_OutputUserVariableName = "{output}";

            _replaceTextCommand.RunCommand(_engine);

            Assert.Equal("Goodbye john", "{output}".ConvertUserVariableToString(_engine));
        }
    }
}
