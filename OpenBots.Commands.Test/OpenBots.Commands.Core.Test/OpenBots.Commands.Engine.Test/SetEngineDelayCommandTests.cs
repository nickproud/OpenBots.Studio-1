using OpenBots.Core.Script;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace OpenBots.Commands.Engine.Test
{
    public class SetEngineDelayCommandTests
    {
        private AutomationEngineInstance _engine;
        private SetEngineDelayCommand _setEngineDelay;
        private Variable.SetVariableCommand _setVariableCommand1;
        private Variable.SetVariableCommand _setVariableCommand2;
        private readonly ITestOutputHelper output;

        public SetEngineDelayCommandTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void SetsEngineDelay()
        {
            _engine = new AutomationEngineInstance(null);
            _setEngineDelay = new SetEngineDelayCommand();

            string newDelay = "1000";
            VariableMethods.CreateTestVariable(newDelay, _engine, "newDelay", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "var1", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "var2", typeof(string));

            _setEngineDelay.v_EngineDelay = "{newDelay}";

            _setEngineDelay.RunCommand(_engine);

            _setVariableCommand1 = new Variable.SetVariableCommand();
            _setVariableCommand1.v_Input = "val1";
            _setVariableCommand1.v_OutputUserVariableName = "{var1}";

            _setVariableCommand2 = new Variable.SetVariableCommand();
            _setVariableCommand2.v_Input = "val2";
            _setVariableCommand2.v_OutputUserVariableName = "{var2}";

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            ScriptAction action1 = new ScriptAction();
            action1.ScriptCommand = _setVariableCommand1;
            _engine.ExecuteCommand(action1);

            output.WriteLine(stopwatch.ElapsedMilliseconds.ToString());
            Assert.True(stopwatch.ElapsedMilliseconds > 1000);

            ScriptAction action2 = new ScriptAction();
            action2.ScriptCommand = _setVariableCommand2;
            _engine.ExecuteCommand(action2);

            output.WriteLine(stopwatch.ElapsedMilliseconds.ToString());
            Assert.True(stopwatch.ElapsedMilliseconds > 2000);
            stopwatch.Stop();

            Assert.Equal(int.Parse(newDelay), _engine.EngineSettings.DelayBetweenCommands);
        }
    }
}
