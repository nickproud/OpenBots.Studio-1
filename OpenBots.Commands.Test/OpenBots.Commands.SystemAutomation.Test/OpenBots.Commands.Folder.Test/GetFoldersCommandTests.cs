using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace OpenBots.Commands.Folder.Test
{
    public class GetFoldersCommandTests
    {
        private AutomationEngineInstance _engine;
        private GetFoldersCommand _getFolders;

        [Fact]
        public void GetsFolders()
        {
            _engine = new AutomationEngineInstance(null);
            _getFolders = new GetFoldersCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(List<>));

            Directory.CreateDirectory(Path.Combine(inputPath, @"toGet"));

            _getFolders.v_SourceFolderPath = "{inputPath}";
            _getFolders.v_OutputUserVariableName = "{output}";

            _getFolders.RunCommand(_engine);

            List<string> folderList = (List<string>)"{output}".ConvertUserVariableToObject(_engine, typeof(List<>));

            Assert.Contains(Path.Combine(inputPath, @"toGet"), folderList);
        }

        [Fact]
        public void HandlesInvalidDirectoryPath()
        {
            _engine = new AutomationEngineInstance(null);
            _getFolders = new GetFoldersCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\doesNotExist");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(List<>));

            _getFolders.v_SourceFolderPath = "{inputPath}";
            _getFolders.v_OutputUserVariableName = "{output}";

            Assert.Throws<DirectoryNotFoundException>(() => _getFolders.RunCommand(_engine));
        }
    }
}
