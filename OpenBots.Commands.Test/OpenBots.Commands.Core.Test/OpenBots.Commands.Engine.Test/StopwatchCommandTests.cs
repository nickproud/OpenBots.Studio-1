using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace OpenBots.Commands.Engine.Test
{
    public class StopwatchCommandTests
    {
        private AutomationEngineInstance _engine;
        private StopwatchCommand _stopwatch;
        private readonly ITestOutputHelper output;

        public StopwatchCommandTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async void ExecutesStopwatchActionSequence()
        {
            _engine = new AutomationEngineInstance(null);
            _stopwatch = new StopwatchCommand();

            VariableMethods.CreateTestVariable("ss\\.fff", _engine, "timeFormat", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _stopwatch.v_InstanceName = "testStopwatch";
            _stopwatch.v_StopwatchAction = "Start Stopwatch";
            _stopwatch.v_ToStringFormat = "{timeFormat}";
            _stopwatch.v_OutputUserVariableName = "{output}";
            _stopwatch.RunCommand(_engine);

            Thread.Sleep(1000);

            _stopwatch.v_StopwatchAction = "Stop Stopwatch";
            _stopwatch.RunCommand(_engine);

            _stopwatch.v_StopwatchAction = "Measure Stopwatch";
            _stopwatch.RunCommand(_engine);

            output.WriteLine((string)await "{output}".EvaluateCode(_engine));
            double firstTime = (Double)await "{output}".EvaluateCode(_engine);
            Assert.True(firstTime > 1.0);

            _stopwatch.v_StopwatchAction = "Restart Stopwatch";
            _stopwatch.RunCommand(_engine);

            Thread.Sleep(500);

            _stopwatch.v_StopwatchAction = "Stop Stopwatch";
            _stopwatch.RunCommand(_engine);

            _stopwatch.v_StopwatchAction = "Measure Stopwatch";
            _stopwatch.RunCommand(_engine);

            output.WriteLine((string)await "{output}".EvaluateCode(_engine));
            double secondTime = (Double)await "{output}".EvaluateCode(_engine);
            Assert.True(secondTime > 0.5);

            _stopwatch.v_StopwatchAction = "Reset Stopwatch";
            _stopwatch.RunCommand(_engine);

            _stopwatch.v_StopwatchAction = "Measure Stopwatch";
            _stopwatch.RunCommand(_engine);

            output.WriteLine((string)await "{output}".EvaluateCode(_engine));
            double thirdTime = (Double)await "{output}".EvaluateCode(_engine);
            Assert.Equal(0.0, thirdTime);
        }
    }
}
