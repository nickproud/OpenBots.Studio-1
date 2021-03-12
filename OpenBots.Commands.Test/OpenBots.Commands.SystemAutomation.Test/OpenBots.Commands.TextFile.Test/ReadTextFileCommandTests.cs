using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.IO;
using Xunit;
using OBIO = System.IO;

namespace OpenBots.Commands.TextFile.Test
{
    public class ReadTextFileCommandTests
    {
        private ReadTextFileCommand _readTextFile;
        private AutomationEngineInstance _engine;
        
        [Fact]
        public void ReadsTextFromFile()
        {
            _engine = new AutomationEngineInstance(null);
            _readTextFile = new ReadTextFileCommand();
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, @"Resources\readtest.txt");
            VariableMethods.CreateTestVariable(filePath, _engine, "filepath", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "test", typeof(string));

            _readTextFile.v_OutputUserVariableName = "{test}";
            _readTextFile.v_FilePath = "{filepath}";
            _readTextFile.RunCommand(_engine);
            var textFromFile = OBIO.File.ReadAllText(filePath);
            var storedText = _readTextFile.v_OutputUserVariableName.ConvertUserVariableToString(_engine);

            Assert.Equal(textFromFile, storedText);

        }
        [Fact]
        public void HandlesNonexistentFile()
        {
            _engine = new AutomationEngineInstance(null);
            _readTextFile = new ReadTextFileCommand();
            VariableMethods.CreateTestVariable(null, _engine, "test", typeof(string));
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            _readTextFile.v_FilePath = Path.Combine(projectDirectory, @"Resources\doesNotExist.txt");
            _readTextFile.v_OutputUserVariableName = "{test}";

            Assert.Throws<FileNotFoundException>(() => _readTextFile.RunCommand(_engine));

        }
    }
}
