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
            inputPath.StoreInUserVariable(_engine, "{inputPath}");

            Directory.CreateDirectory(inputPath);

            "newName".StoreInUserVariable(_engine, "{newName}");

            _renameFolder.v_SourceFolderPath = "{inputPath}";
            _renameFolder.v_NewName = "{newName}";

            _renameFolder.RunCommand(_engine);

            Assert.True(Directory.Exists(Path.Combine(projectDirectory, @"Resources\newName")));

            resetFolderName();
        }

        [Fact]
        public void HandlesInvalidSourceFolderInput()
        {
            _engine = new AutomationEngineInstance(null);
            _renameFolder = new RenameFolderCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\doesNotExist");
            inputPath.StoreInUserVariable(_engine, "{inputPath}");

            "newName".StoreInUserVariable(_engine, "{newName}");

            _renameFolder.v_SourceFolderPath = "{inputPath}";
            _renameFolder.v_NewName = "{newName}";

            Assert.Throws<DirectoryNotFoundException>(() => _renameFolder.RunCommand(_engine));
        }

        private void resetFolderName()
        {
            _engine = new AutomationEngineInstance(null);
            _renameFolder = new RenameFolderCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\newName");
            inputPath.StoreInUserVariable(_engine, "{inputPath}");

            "toDelete".StoreInUserVariable(_engine, "{newName}");

            _renameFolder.v_SourceFolderPath = "{inputPath}";
            _renameFolder.v_NewName = "{newName}";

            _renameFolder.RunCommand(_engine);
        }
    }
}
