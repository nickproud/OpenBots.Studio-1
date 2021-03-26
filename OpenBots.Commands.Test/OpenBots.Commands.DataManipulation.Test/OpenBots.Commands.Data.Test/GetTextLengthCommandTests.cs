using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using Xunit;

namespace OpenBots.Commands.Data.Test
{
    public class GetTextLengthCommandTests
    {
        private GetTextLengthCommand _getTextLength;
        private AutomationEngineInstance _engine;

        [Fact]
        public void GetsTextLength()
        {
            _getTextLength = new GetTextLengthCommand();
            _engine = new AutomationEngineInstance(null);

            string textToMeasure = "testText";
            VariableMethods.CreateTestVariable(textToMeasure, _engine, "inputText", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(int));

            _getTextLength.v_InputValue = "{inputText}";
            _getTextLength.v_OutputUserVariableName = "{output}";

            _getTextLength.RunCommand(_engine);

            Assert.Equal(textToMeasure.Length, Int32.Parse(_getTextLength.v_OutputUserVariableName.ConvertUserVariableToString(_engine)));
        }
    }
}
