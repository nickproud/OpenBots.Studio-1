using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace OpenBots.Commands.File.Test
{
    public class GetFilesCommandTests
    {
        private AutomationEngineInstance _engine;
        private GetFilesCommand _getFiles;
        private readonly ITestOutputHelper output;

        public GetFilesCommandTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void GetsFiles()
        {
            _engine = new AutomationEngineInstance(null);
            _getFiles = new GetFilesCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(List<>));

            _getFiles.v_SourceFolderPath = "{inputPath}";
            _getFiles.v_OutputUserVariableName = "{output}";

            _getFiles.RunCommand(_engine);

            List<string> fileList = (List<string>)"{output}".ConvertUserVariableToObject(_engine, typeof(List<>));

            List<string> filenames = new List<string>();

            foreach(string file in fileList)
            {
                output.WriteLine(file);
                string[] splitPath = file.Split('\\');
                string filename = splitPath[splitPath.Length - 1];
                filenames.Add(filename);
            }

            Assert.Contains("compressed.zip", filenames);
            Assert.Contains("toCompress.txt", filenames);
        }

        [Fact]
        public void HandlesInvalidFilepath()
        {
            _engine = new AutomationEngineInstance(null);
            _getFiles = new GetFilesCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\toDelete.txt");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(List<>));

            _getFiles.v_SourceFolderPath = "{inputPath}";
            _getFiles.v_OutputUserVariableName = "{output}";

            Assert.Throws<DirectoryNotFoundException>(() => _getFiles.RunCommand(_engine));
        }
    }
}
