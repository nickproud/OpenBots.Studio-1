using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using Xunit;

namespace OpenBots.Commands.Data.Test
{
    public class GetWordCountCommandTests
    {
        private GetWordCountCommand _getWordCount;
        private AutomationEngineInstance _engine;

        [Fact]
        public async void GetsWordCount()
        {
            _getWordCount = new GetWordCountCommand();
            _engine = new AutomationEngineInstance(null);

            string input = "Test input sentence";
            VariableMethods.CreateTestVariable(input, _engine, "input", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(int));

            _getWordCount.v_InputValue = "{input}";
            _getWordCount.v_OutputUserVariableName = "{output}";

            _getWordCount.RunCommand(_engine);

            Assert.Equal(3, (Int32)await _getWordCount.v_OutputUserVariableName.EvaluateCode(_engine));
        }
    }
}
