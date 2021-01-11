using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using Xunit;

namespace OpenBots.Commands.Variable.Test
{
    public class NewVariableCommandTests
    {
        private AutomationEngineInstance _engine;
        private NewVariableCommand _newVariable;

        [Fact]
        public void CreatesNewVariable()
        {
            _engine = new AutomationEngineInstance(null, null);
            _newVariable = new NewVariableCommand();

            "testValue".StoreInUserVariable(_engine, "{testValue}");

            _newVariable.v_VariableName = "{testVariable}";
            _newVariable.v_Input = "{testValue}";
            _newVariable.v_IfExists = "Do Nothing If Variable Exists";

            _newVariable.RunCommand(_engine);

            Assert.Equal("testValue", "{testVariable}".ConvertUserVariableToString(_engine));
        }

        [Fact]
        public void ErrorIfVariableExists()
        {
            _engine = new AutomationEngineInstance(null, null);
            _newVariable = new NewVariableCommand();

            "testValue".StoreInUserVariable(_engine, "{testValue}");
            "existingValue".StoreInUserVariable(_engine, "{testVariable}");

            _newVariable.v_VariableName = "{testVariable}";
            _newVariable.v_Input = "{testValue}";
            _newVariable.v_IfExists = "Error If Variable Exists";

            Assert.Throws<Exception>(() => _newVariable.RunCommand(_engine));
        }

        [Fact]
        public void ReplaceIfVariableExists()
        {
            _engine = new AutomationEngineInstance(null, null);
            _newVariable = new NewVariableCommand();

            "testValue".StoreInUserVariable(_engine, "{testValue}");
            "existingValue".StoreInUserVariable(_engine, "{testVariable}");

            _newVariable.v_VariableName = "{testVariable}";
            _newVariable.v_Input = "{testValue}";
            _newVariable.v_IfExists = "Replace If Variable Exists";

            _newVariable.RunCommand(_engine);

            Assert.Equal("testValue", "{testVariable}".ConvertUserVariableToString(_engine));
        }
    }
}
