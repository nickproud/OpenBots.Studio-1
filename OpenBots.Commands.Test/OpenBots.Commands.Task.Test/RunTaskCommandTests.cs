using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using OpenBots.Engine;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Core.Script;
using OpenBots.Core.Command;

namespace OpenBots.Commands.Task.Test
{
    public class RunTaskCommandTests
    {
        private AutomationEngineInstance _engine;
        private RunTaskCommand _runTask;
        private Script _script;
        private readonly ITestOutputHelper output;

        public RunTaskCommandTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void RunsTask()
        {
            _engine = new AutomationEngineInstance(null);
            _runTask = new RunTaskCommand();
            _script = new Script();

            List<ScriptVariable> variables = new List<ScriptVariable>();
            ScriptVariable var1 = new ScriptVariable();
            var1.VariableName = "input";
            var1.VariableValue = "inputValue";
            variables.Add(var1);

            //_runTask.v_TaskPath = "";
            //_runTask.v_AssignArguments = true;
            //_runTask.v_ArgumentAssignments = "";

            output.WriteLine(variables[0].VariableValue.ToString());

            _script.Variables = variables;

            List<ScriptAction> commands = new List<ScriptAction>();
            ScriptAction com1 = new ScriptAction();
            com1.ScriptCommand = _runTask;
            commands.Add(com1);

            _script.Commands = commands;

            
        }
    }
}
