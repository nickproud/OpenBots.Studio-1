using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xunit;
using OpenBots.Engine;
using OpenBots.Core.Utilities.CommonUtilities;

namespace OpenBots.Commands.Folder.Test
{
    public class RenameFolderCommandTests
    {
        private AutomationEngineInstance _engine;
        private RenameFolderCommand _renameFolder;

        [Fact]
        public void RenamesFolder()
        {
            _engine = new AutomationEngineInstance(null, null);
            _renameFolder = new RenameFolderCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\toDelete");
            inputPath.StoreInUserVariable(_engine, "{inputPath}");

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
            _engine = new AutomationEngineInstance(null, null);
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
            _engine = new AutomationEngineInstance(null, null);
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
