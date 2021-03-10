using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using OpenBots.Engine;
using OpenBots.Core.Utilities.CommonUtilities;

namespace OpenBots.Commands.Variable.Test
{
    public class SetVariableCommandTests
    {
        private AutomationEngineInstance _engine;
        private SetVariableCommand _setVariable;

        [Fact]
        public void SetsVariable()
        {
            _engine = new AutomationEngineInstance(null);
            _setVariable = new SetVariableCommand();

            "valueToSet".CreateTestVariable(_engine, "testValue", typeof(string));
            "undefined".CreateTestVariable(_engine, "setVariable", typeof(string));

            _setVariable.v_Input = "{testValue}";
            _setVariable.v_OutputUserVariableName = "{setVariable}";

            _setVariable.RunCommand(_engine);

            Assert.Equal("valueToSet", "{setVariable}".ConvertUserVariableToString(_engine));
        }

        [Fact]
        public void SetsVariableWithMath()
        {
            _engine = new AutomationEngineInstance(null);
            _setVariable = new SetVariableCommand();

            "1".CreateTestVariable(_engine, "testValue", typeof(string));
            "undefined".CreateTestVariable(_engine, "setVariable", typeof(string));

            _setVariable.v_Input = "{testValue}+1";
            _setVariable.v_OutputUserVariableName = "{setVariable}";

            _setVariable.RunCommand(_engine);

            Assert.Equal("2", "{setVariable}".ConvertUserVariableToString(_engine));
        }
    }
}
