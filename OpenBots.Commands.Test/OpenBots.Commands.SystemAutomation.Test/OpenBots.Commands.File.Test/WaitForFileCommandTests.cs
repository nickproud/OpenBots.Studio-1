using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.IO;
using Xunit;
using IO = System.IO;

namespace OpenBots.Commands.File.Test
{
    public class WaitForFileCommandTests
    {
        private AutomationEngineInstance _engine;
        private WaitForFileCommand _waitForFile;

        [Fact]
        public void WaitsForFile()
        {
            _engine = new AutomationEngineInstance(null);
            _waitForFile = new WaitForFileCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\toWaitFor.txt");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));

            string waitTime = "5";
            VariableMethods.CreateTestVariable(waitTime, _engine, "waitTime", typeof(string));

            _waitForFile.v_FileName = "{inputPath}";
            _waitForFile.v_WaitTime = "{waitTime}";

            _waitForFile.RunCommand(_engine);

            Assert.True(IO.File.Exists(inputPath));
        }

        [Fact]
        public async global::System.Threading.Tasks.Task FailsWhenFileNotFound()
        {
            _engine = new AutomationEngineInstance(null);
            _waitForFile = new WaitForFileCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\nofile.txt");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));

            string waitTime = "2";
            VariableMethods.CreateTestVariable(waitTime, _engine, "waitTime", typeof(string));

            _waitForFile.v_FileName = "{inputPath}";
            _waitForFile.v_WaitTime = "{waitTime}";

            await Assert.ThrowsAsync<FileNotFoundException>(() => _waitForFile.RunCommand(_engine));
        }
    }
}
