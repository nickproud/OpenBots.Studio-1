using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.IO;
using Xunit;

namespace OpenBots.Commands.Folder.Test
{
    public class RenameFolderCommandTests
    {
        private AutomationEngineInstance _engine;
        private RenameFolderCommand _renameFolder;

        [Fact]
        public void RenamesFolder()
        {
            _engine = new AutomationEngineInstance(null);
            _renameFolder = new RenameFolderCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\toRename");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));

            Directory.CreateDirectory(inputPath);

            VariableMethods.CreateTestVariable("newName", _engine, "newName", typeof(string));

            _renameFolder.v_SourceFolderPath = "{inputPath}";
            _renameFolder.v_NewName = "{newName}";

            _renameFolder.RunCommand(_engine);

            Assert.True(Directory.Exists(Path.Combine(projectDirectory, @"Resources\newName")));

            resetFolderName();
        }

        [Fact]
        public async global::System.Threading.Tasks.Task HandlesInvalidSourceFolderInput()
        {
            _engine = new AutomationEngineInstance(null);
            _renameFolder = new RenameFolderCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\doesNotExist");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));

            VariableMethods.CreateTestVariable("newName", _engine, "newName", typeof(string));

            _renameFolder.v_SourceFolderPath = "{inputPath}";
            _renameFolder.v_NewName = "{newName}";

            await Assert.ThrowsAsync<DirectoryNotFoundException>(() => _renameFolder.RunCommand(_engine));
        }

        private void resetFolderName()
        {
            _engine = new AutomationEngineInstance(null);
            _renameFolder = new RenameFolderCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\newName");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));

            VariableMethods.CreateTestVariable("toRename", _engine, "newName", typeof(string));

            _renameFolder.v_SourceFolderPath = "{inputPath}";
            _renameFolder.v_NewName = "{newName}";

            _renameFolder.RunCommand(_engine);
        }
    }
}
