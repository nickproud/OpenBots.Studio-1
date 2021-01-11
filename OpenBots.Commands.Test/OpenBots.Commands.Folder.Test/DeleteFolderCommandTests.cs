using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.IO;
using Xunit;

namespace OpenBots.Commands.Folder.Test
{
    public class DeleteFolderCommandTests
    {
        private AutomationEngineInstance _engine;
        private DeleteFolderCommand _deleteFolder;

        [Fact]
        public void DeletesFolder()
        {
            _engine = new AutomationEngineInstance(null, null);
            _deleteFolder = new DeleteFolderCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\toDelete");
            inputPath.StoreInUserVariable(_engine, "{inputPath}");

            _deleteFolder.v_SourceFolderPath = "{inputPath}";

            _deleteFolder.RunCommand(_engine);

            Assert.False(Directory.Exists(inputPath));

            Directory.CreateDirectory(inputPath);
            File.Create(Path.Combine(inputPath + @"toDelete.txt"));
        }
    }
}
