using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.IO;
using Xunit;
using IO = System.IO;

namespace OpenBots.Commands.File.Test
{
    public class DeleteFileCommandTests
    {
        private AutomationEngineInstance _engine;
        private DeleteFileCommand _deleteFile;

        [Fact]
        public void DeletesFile()
        {
            _engine = new AutomationEngineInstance(null);
            _deleteFile = new DeleteFileCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\toDelete.txt");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));

            _deleteFile.v_SourceFilePath = "{inputPath}";

            _deleteFile.RunCommand(_engine);

            Assert.False(IO.File.Exists(inputPath));

            IO.File.Create(Path.Combine(projectDirectory, @"Resources\toDelete.txt"));
        }

        [Fact]
        public void HandlesBadFilepath()
        {
            _engine = new AutomationEngineInstance(null);
            _deleteFile = new DeleteFileCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));

            _deleteFile.v_SourceFilePath = "{inputPath}";

            Assert.Throws<FileNotFoundException>(() => _deleteFile.RunCommand(_engine));
        }
    }
}
