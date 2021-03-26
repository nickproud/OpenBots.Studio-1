using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Security;
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
            _engine = new AutomationEngineInstance(null);
            _compressFiles = new CompressFilesCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\toCompress");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));

            string outputPath = Environment.CurrentDirectory;
            VariableMethods.CreateTestVariable(outputPath, _engine, "outputPath", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));
            
            _compressFiles.v_DirectoryPathOrigin = "{inputPath}";
            _compressFiles.v_PathDestination = "{outputPath}";
            _compressFiles.v_OutputUserVariableName = "{output}";

            _compressFiles.RunCommand(_engine);

            Assert.True(IO.File.Exists(Path.Combine(Environment.CurrentDirectory, @"toCompress.zip")));

            IO.File.Delete(Path.Combine(outputPath, @"toCompress.zip"));
        }

        [Fact]
        public void CompressesFilesWithPassword()
        {
            _engine = new AutomationEngineInstance(null);
            _compressFiles = new CompressFilesCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\toCompressWithPassword");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));

            string outputPath = Environment.CurrentDirectory;
            VariableMethods.CreateTestVariable(outputPath, _engine, "outputPath", typeof(string));

            string password = "testPassword";
            VariableMethods.CreateTestVariable(password.ConvertStringToSecureString(), _engine, "testPassword", typeof(SecureString));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _compressFiles.v_DirectoryPathOrigin = "{inputPath}";
            _compressFiles.v_Password = "{testPassword}";
            _compressFiles.v_PathDestination = "{outputPath}";
            _compressFiles.v_OutputUserVariableName = "{output}";

            _compressFiles.RunCommand(_engine);

            Assert.True(IO.File.Exists(Path.Combine(Environment.CurrentDirectory, @"toCompressWithPassword.zip")));

            IO.File.Delete(Path.Combine(outputPath, @"toCompressWithPassword.zip"));
        }

        [Fact]
        public void HandlesInvalidPath()
        {
            _engine = new AutomationEngineInstance(null);
            _compressFiles = new CompressFilesCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\writetest.txt");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));

            string outputPath = Environment.CurrentDirectory;
            VariableMethods.CreateTestVariable(outputPath, _engine, "outputPath", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _compressFiles.v_DirectoryPathOrigin = "{inputPath}";
            _compressFiles.v_PathDestination = "{outputPath}";
            _compressFiles.v_OutputUserVariableName = "{output}";

            Assert.Throws<ArgumentException>(() => _compressFiles.RunCommand(_engine));
        }
    }
}
