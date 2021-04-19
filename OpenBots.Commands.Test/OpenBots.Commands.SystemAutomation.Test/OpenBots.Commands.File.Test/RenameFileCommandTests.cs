using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.IO;
using Xunit;
using IO = System.IO;

namespace OpenBots.Commands.File.Test
{
    public class RenameFileCommandTests
    {
        private AutomationEngineInstance _engine;
        private RenameFileCommand _renameFile;

        [Fact]
        public void RenamesFile()
        {
            _engine = new AutomationEngineInstance(null);
            _renameFile = new RenameFileCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\toRename.txt");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));

            string newName = "newname.txt";
            VariableMethods.CreateTestVariable(newName, _engine, "newName", typeof(string));

            _renameFile.v_SourceFilePath = "{inputPath}";
            _renameFile.v_NewName = "{newName}";

            _renameFile.RunCommand(_engine);

            string newFile = Path.Combine(projectDirectory, @"Resources\" + newName);

            Assert.True(IO.File.Exists(newFile));

            resetTestFilename(newFile);
        }

        [Fact]
        public async global::System.Threading.Tasks.Task HandlesInvalidFileInput()
        {
            _engine = new AutomationEngineInstance(null);
            _renameFile = new RenameFileCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\noFile.txt");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));

            string newName = "newname.txt";
            VariableMethods.CreateTestVariable(newName, _engine, "newName", typeof(string));

            _renameFile.v_SourceFilePath = "{inputPath}";
            _renameFile.v_NewName = "{newName}";

            await Assert.ThrowsAsync<FileNotFoundException>(() => _renameFile.RunCommand(_engine));
        }

        private void resetTestFilename(string file)
        {
            _engine = new AutomationEngineInstance(null);
            _renameFile = new RenameFileCommand();

            _renameFile.v_SourceFilePath = file;
            _renameFile.v_NewName = "toRename.txt";

            _renameFile.RunCommand(_engine);
        }
    }
}
