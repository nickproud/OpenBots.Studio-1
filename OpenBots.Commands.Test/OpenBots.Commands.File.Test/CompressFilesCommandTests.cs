using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;
using IO = System.IO;

namespace OpenBots.Commands.File.Test
{
    public class CompressFilesCommandTests
    {
        private AutomationEngineInstance _engine;
        private CompressFilesCommand _compressFiles;
        private readonly ITestOutputHelper output;

        public CompressFilesCommandTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void CompressesFiles()
        {
            _engine = new AutomationEngineInstance(null, null);
            _compressFiles = new CompressFilesCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources");
            inputPath.StoreInUserVariable(_engine, "{inputPath}");

            string outputPath = Environment.CurrentDirectory;
            outputPath.StoreInUserVariable(_engine, "{outputPath}");
            
            _compressFiles.v_DirectoryPathOrigin = "{inputPath}";
            _compressFiles.v_PathDestination = "{outputPath}";
            _compressFiles.v_OutputUserVariableName = "{output}";

            _compressFiles.RunCommand(_engine);

            Assert.True(IO.File.Exists(Path.Combine(Environment.CurrentDirectory, @"Resources.zip")));

            IO.File.Delete(Path.Combine(outputPath, @"Resources.zip"));
        }

        [Fact]
        public void CompressesFilesWithPassword()
        {
            _engine = new AutomationEngineInstance(null, null);
            _compressFiles = new CompressFilesCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources");
            inputPath.StoreInUserVariable(_engine, "{inputPath}");

            string outputPath = Environment.CurrentDirectory;
            outputPath.StoreInUserVariable(_engine, "{outputPath}");

            string password = "testPassword";
            password.StoreInUserVariable(_engine, "{testPassword}");

            _compressFiles.v_DirectoryPathOrigin = "{inputPath}";
            _compressFiles.v_Password = "{testPassword}";
            _compressFiles.v_PathDestination = "{outputPath}";
            _compressFiles.v_OutputUserVariableName = "{output}";

            _compressFiles.RunCommand(_engine);

            Assert.True(IO.File.Exists(Path.Combine(Environment.CurrentDirectory, @"Resources.zip")));

            IO.File.Delete(Path.Combine(outputPath, @"Resources.zip"));
        }

        [Fact]
        public void HandlesInvalidPath()
        {
            _engine = new AutomationEngineInstance(null, null);
            _compressFiles = new CompressFilesCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\toCompress.txt");
            inputPath.StoreInUserVariable(_engine, "{inputPath}");

            string outputPath = Environment.CurrentDirectory;
            outputPath.StoreInUserVariable(_engine, "{outputPath}");

            _compressFiles.v_DirectoryPathOrigin = "{inputPath}";
            _compressFiles.v_PathDestination = "{outputPath}";
            _compressFiles.v_OutputUserVariableName = "{output}";

            Assert.Throws<ArgumentException>(() => _compressFiles.RunCommand(_engine));
        }
    }
}
