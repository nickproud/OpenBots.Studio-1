using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.IO;
using Xunit;

namespace OpenBots.Commands.Folder.Test
{
    public class MoveCopyFolderCommandTests
    {
        private AutomationEngineInstance _engine;
        private MoveCopyFolderCommand _moveCopyFolder;

        [Theory]
        [InlineData("Move Folder")]
        [InlineData("Copy Folder")]
        public void MovesCopiesFolder(string operation)
        {
            _engine = new AutomationEngineInstance(null);
            _moveCopyFolder = new MoveCopyFolderCommand();

            string folder = "";
            if(operation.Equals("Move Folder"))
            {
                folder = "toMove";
            }
            else
            {
                folder = "toCopy";
            }

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\" + folder);
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));

            Directory.CreateDirectory(inputPath);

            string destinationPath = Path.Combine(projectDirectory, @"Resources\moveCopyDestination");
            VariableMethods.CreateTestVariable(destinationPath, _engine, "destinationPath", typeof(string));

            _moveCopyFolder.v_OperationType = operation;
            _moveCopyFolder.v_SourceFolderPath = "{inputPath}";
            _moveCopyFolder.v_DestinationDirectory = "{destinationPath}";
            _moveCopyFolder.v_CreateDirectory = "Yes";
            _moveCopyFolder.v_DeleteExisting = "Yes";

            _moveCopyFolder.RunCommand(_engine);

            Assert.True(Directory.Exists(Path.Combine(destinationPath, folder)));
            if (operation.Equals("Move Folder"))
            {
                Assert.False(Directory.Exists(inputPath));
                Directory.CreateDirectory(inputPath);
            }
            else
            {
                Assert.True(Directory.Exists(inputPath));
            }

            Directory.Delete(Path.Combine(destinationPath, folder), true);
        }

        [Fact]
        public async global::System.Threading.Tasks.Task HandlesInvalidSourceFolderInput()
        {
            _engine = new AutomationEngineInstance(null);
            _moveCopyFolder = new MoveCopyFolderCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\doesNotExist");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));

            string destinationPath = Path.Combine(projectDirectory, @"Resources\moveCopyDestination");
            VariableMethods.CreateTestVariable(destinationPath, _engine, "destinationPath", typeof(string));

            _moveCopyFolder.v_OperationType = "Move Folder";
            _moveCopyFolder.v_SourceFolderPath = "{inputPath}";
            _moveCopyFolder.v_DestinationDirectory = "{destinationPath}";
            _moveCopyFolder.v_CreateDirectory = "Yes";
            _moveCopyFolder.v_DeleteExisting = "Yes";

            await Assert.ThrowsAsync<DirectoryNotFoundException>(() => _moveCopyFolder.RunCommand(_engine));
        }
    }
}
