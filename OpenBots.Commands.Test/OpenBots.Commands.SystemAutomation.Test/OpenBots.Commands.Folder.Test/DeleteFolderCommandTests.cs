using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.IO;
using Xunit;
using OBIO = System.IO;
namespace OpenBots.Commands.Folder.Test
{
    public class DeleteFolderCommandTests
    {
        private AutomationEngineInstance _engine;
        private DeleteFolderCommand _deleteFolder;

        [Fact]
        public void DeletesFolder()
        {
            _engine = new AutomationEngineInstance(null);
            _deleteFolder = new DeleteFolderCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\toDelete");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));

            Directory.CreateDirectory(inputPath);
            
            _deleteFolder.v_SourceFolderPath = "{inputPath}";

            _deleteFolder.RunCommand(_engine);

            Assert.False(Directory.Exists(inputPath));

            OBIO.Directory.CreateDirectory(inputPath);
            OBIO.File.Create(Path.Combine(inputPath + @"\toDelete.txt"));
        }
    }
}
