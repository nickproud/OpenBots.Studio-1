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
            string inputPath = Path.Combine(projectDirectory, @"Resources\toCompress.txt");
            inputPath.CreateTestVariable(_engine, "inputPath");

            string waitTime = "5";
            waitTime.CreateTestVariable(_engine, "waitTime");

            _waitForFile.v_FileName = "{inputPath}";
            _waitForFile.v_WaitTime = "{waitTime}";

            _waitForFile.RunCommand(_engine);

            Assert.True(IO.File.Exists(inputPath));
        }

        [Fact]
        public void FailsWhenFileNotFound()
        {
            _engine = new AutomationEngineInstance(null);
            _waitForFile = new WaitForFileCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\nofile.txt");
            inputPath.CreateTestVariable(_engine, "inputPath");

            string waitTime = "2";
            waitTime.CreateTestVariable(_engine, "waitTime");

            _waitForFile.v_FileName = "{inputPath}";
            _waitForFile.v_WaitTime = "{waitTime}";

            Assert.Throws<FileNotFoundException>(() => _waitForFile.RunCommand(_engine));
        }
    }
}
