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
    public class GetFoldersCommandTests
    {
        private AutomationEngineInstance _engine;
        private GetFoldersCommand _getFolders;

        [Fact]
        public void GetsFolders()
        {
            _engine = new AutomationEngineInstance(null, null);
            _getFolders = new GetFoldersCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources");
            inputPath.StoreInUserVariable(_engine, "{inputPath}");

            _getFolders.v_SourceFolderPath = "{inputPath}";
            _getFolders.v_OutputUserVariableName = "{output}";

            _getFolders.RunCommand(_engine);

            List<string> folderList = (List<string>)"{output}".ConvertUserVariableToObject(_engine);

            Assert.Contains(Path.Combine(inputPath, @"toDelete"), folderList);
        }

        [Fact]
        public void HandlesInvalidDirectoryPath()
        {
            _engine = new AutomationEngineInstance(null, null);
            _getFolders = new GetFoldersCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\doesNotExist");
            inputPath.StoreInUserVariable(_engine, "{inputPath}");

            _getFolders.v_SourceFolderPath = "{inputPath}";
            _getFolders.v_OutputUserVariableName = "{output}";

            Assert.Throws<DirectoryNotFoundException>(() => _getFolders.RunCommand(_engine));
        }
    }
}
