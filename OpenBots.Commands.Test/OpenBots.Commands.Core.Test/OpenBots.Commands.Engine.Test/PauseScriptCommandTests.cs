using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using Xunit;
using Xunit.Abstractions;

namespace OpenBots.Commands.Engine.Test
{
    public class PauseScriptCommandTests
    {
        private AutomationEngineInstance _engine;
        private PauseScriptCommand _pauseScript;
        private readonly ITestOutputHelper output;

        public PauseScriptCommandTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void PausesScript()
        {
            _engine = new AutomationEngineInstance(null);
            _pauseScript = new PauseScriptCommand();

            int pauseLength = 1000;
            VariableMethods.CreateTestVariable(pauseLength.ToString(), _engine, "pauseLength", typeof(string));
            _pauseScript.v_PauseLength = "{pauseLength}";

            DateTime startTime = DateTime.Now;

            _pauseScript.RunCommand(_engine);

            DateTime endTime = DateTime.Now;

            output.WriteLine("Time Elapsed: ", ((endTime.Ticks - startTime.Ticks)/10000).ToString());

            Assert.True((endTime.Ticks - startTime.Ticks)/10000 > pauseLength);
        }
    }
}
