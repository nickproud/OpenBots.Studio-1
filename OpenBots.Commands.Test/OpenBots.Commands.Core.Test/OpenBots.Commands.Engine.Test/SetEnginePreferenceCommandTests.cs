using OpenBots.Core.Script;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using Xunit;

namespace OpenBots.Commands.Engine.Test
{
    public class SetEnginePreferenceCommandTests
    {
        private AutomationEngineInstance _engine;
        private SetEnginePreferenceCommand _setEnginePreference;
        private Variable.SetVariableCommand _setVariable;

        [Fact]
        public async void SetsEnginePreference()
        {
            _engine = new AutomationEngineInstance(null);
            _setEnginePreference = new SetEnginePreferenceCommand();

            _setEnginePreference.v_CalculationOption = "Disable Automatic Calculations";

            _setEnginePreference.RunCommand(_engine);

            Assert.False(_engine.AutoCalculateVariables);

            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _setVariable = new Variable.SetVariableCommand();
            _setVariable.v_Input = "1+1";
            _setVariable.v_OutputUserVariableName = "{output}";

            ScriptAction action = new ScriptAction();
            action.ScriptCommand = _setVariable;
            _engine.ExecuteCommand(action);

            Assert.Equal("1+1", (string)await "{output}".EvaluateCode(_engine));
        }
    }
}
