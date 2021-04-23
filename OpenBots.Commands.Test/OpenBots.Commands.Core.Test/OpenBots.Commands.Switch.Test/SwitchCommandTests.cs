using OpenBots.Core.Script;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using Xunit;
using Xunit.Abstractions;

namespace OpenBots.Commands.Switch.Test
{
    public class SwitchCommandTests
    {
        private AutomationEngineInstance _engine;
        private BeginSwitchCommand _beginSwitch;
        private CaseCommand _case1;
        private CaseCommand _defaultCase;
        private EndSwitchCommand _endSwitch;
        private ScriptAction _parentAction;
        private Variable.SetVariableCommand _setVariableCase1;
        private Variable.SetVariableCommand _setVariableDefaultCase;
        private readonly ITestOutputHelper output;

        public SwitchCommandTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData("case1")]
        [InlineData("noCase")]
        public async void ProcessesSwitch(string caseString)
        {
            _engine = new AutomationEngineInstance(null);
            _beginSwitch = new BeginSwitchCommand();
            _case1 = new CaseCommand();
            _defaultCase = new CaseCommand();
            _endSwitch = new EndSwitchCommand();
            _parentAction = new ScriptAction();
            _setVariableCase1 = new Variable.SetVariableCommand();
            _setVariableDefaultCase = new Variable.SetVariableCommand();

            VariableMethods.CreateTestVariable(caseString, _engine, "caseInput", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "switchOutput", typeof(string));

            _beginSwitch.v_SwitchValue = "{caseInput}";

            _case1.v_CaseValue = "case1";
            _defaultCase.v_CaseValue = "Default";

            _setVariableCase1.v_Input = "case1Set";
            _setVariableCase1.v_OutputUserVariableName = "{switchOutput}";

            _setVariableDefaultCase.v_Input = "defaultCaseSet";
            _setVariableDefaultCase.v_OutputUserVariableName = "{switchOutput}";

            _parentAction.ScriptCommand = _beginSwitch;
            _parentAction.AddAdditionalAction(_case1);
            _parentAction.AddAdditionalAction(_setVariableCase1);
            _parentAction.AddAdditionalAction(_defaultCase);
            _parentAction.AddAdditionalAction(_setVariableDefaultCase);
            _parentAction.AddAdditionalAction(_endSwitch);

            _engine.ExecuteCommand(_parentAction);

            if (caseString.Equals("case1"))
            {
                Assert.Equal("case1Set", (string)await "{switchOutput}".EvaluateCode(_engine));
            }
            else if (caseString.Equals("noCase"))
            {
                Assert.Equal("defaultCaseSet", (string)await "{switchOutput}".EvaluateCode(_engine));
            }
        }
    }
}
