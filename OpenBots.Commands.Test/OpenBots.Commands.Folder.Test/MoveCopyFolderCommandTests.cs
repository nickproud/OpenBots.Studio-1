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
            _engine = new AutomationEngineInstance(null, null);
            _moveCopyFolder = new MoveCopyFolderCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\toMoveCopy");
            inputPath.StoreInUserVariable(_engine, "{inputPath}");

            string destinationPath = Path.Combine(projectDirectory, @"Resources\moveCopyDestination");
            destinationPath.StoreInUserVariable(_engine, "{destinationPath}");

            _moveCopyFolder.v_OperationType = operation;
            _moveCopyFolder.v_SourceFolderPath = "{inputPath}";
            _moveCopyFolder.v_DestinationDirectory = "{destinationPath}";
            _moveCopyFolder.v_CreateDirectory = "Yes";
            _moveCopyFolder.v_DeleteExisting = "Yes";

            _moveCopyFolder.RunCommand(_engine);

            Assert.True(Directory.Exists(Path.Combine(destinationPath, @"toMoveCopy")));
            if (operation.Equals("Move Folder"))
            {
                Assert.False(Directory.Exists(inputPath));
                Directory.CreateDirectory(inputPath);
            }
            else
            {
                Assert.True(Directory.Exists(inputPath));
            }

            Directory.Delete(Path.Combine(destinationPath, @"toMoveCopy"));
        }

        [Fact]
        public void HandlesInvalidSourceFolderInput()
        {
            _engine = new AutomationEngineInstance(null, null);
            _moveCopyFolder = new MoveCopyFolderCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\doesNotExist");
            inputPath.StoreInUserVariable(_engine, "{inputPath}");

            string destinationPath = Path.Combine(projectDirectory, @"Resources\moveCopyDestination");
            destinationPath.StoreInUserVariable(_engine, "{destinationPath}");

            _moveCopyFolder.v_OperationType = "Move Folder";
            _moveCopyFolder.v_SourceFolderPath = "{inputPath}";
            _moveCopyFolder.v_DestinationDirectory = "{destinationPath}";
            _moveCopyFolder.v_CreateDirectory = "Yes";
            _moveCopyFolder.v_DeleteExisting = "Yes";

            Assert.Throws<DirectoryNotFoundException>(() => _moveCopyFolder.RunCommand(_engine));
        }
    }
}
