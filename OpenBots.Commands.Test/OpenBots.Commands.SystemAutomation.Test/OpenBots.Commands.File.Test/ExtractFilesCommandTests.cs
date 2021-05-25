using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Security;
using System.IO;
using System.Collections.Generic;
using Xunit;
using IO = System.IO;

namespace OpenBots.Commands.File.Test
{
    public class ExtractFilesCommandTests
    {
        private AutomationEngineInstance _engine;
        private ExtractFilesCommand _extractFiles;

        [Fact]
        public void ExtractsFiles()
        {
            _engine = new AutomationEngineInstance(null);
            _extractFiles = new ExtractFilesCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\compressed.zip");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));

            string outputPath = Environment.CurrentDirectory;
            VariableMethods.CreateTestVariable(outputPath, _engine, "outputPath", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(List<>));

            _extractFiles.v_FilePathOrigin = "{inputPath}";
            _extractFiles.v_PathDestination = "{outputPath}";
            _extractFiles.v_OutputUserVariableName = "{output}";

            _extractFiles.RunCommand(_engine);

            Assert.True(IO.File.Exists(Path.Combine(outputPath, @"compressed\compressed.txt")));
            Assert.True(Directory.Exists(Path.Combine(outputPath, @"compressed\emptyFolder")));
            Assert.True(IO.File.Exists(Path.Combine(outputPath, @"compressed\subfolder\subfolder2\deepfile.txt")));

            IO.File.Delete(Path.Combine(outputPath, @"compressed\compressed.txt"));
            Directory.Delete(Path.Combine(outputPath, @"compressed\emptyFolder"));
            IO.File.Delete(Path.Combine(outputPath, @"compressed\subfolder\subfolder2\deepfile.txt"));
            Directory.Delete(Path.Combine(outputPath, @"compressed\subfolder\subfolder2"));
            Directory.Delete(Path.Combine(outputPath, @"compressed\subfolder"));
            Directory.Delete(Path.Combine(outputPath, @"compressed"));
        }
        [Fact]
        public void ExtractsFilesWithPassword()
        {
            _engine = new AutomationEngineInstance(null);
            _extractFiles = new ExtractFilesCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\compressedWithPassword.zip");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));

            string outputPath = Environment.CurrentDirectory;
            VariableMethods.CreateTestVariable(outputPath, _engine, "outputPath", typeof(string));

            string password = "testPassword";
            VariableMethods.CreateTestVariable(password.ConvertStringToSecureString(), _engine, "testPassword", typeof(SecureString));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(List<>));

            _extractFiles.v_FilePathOrigin = "{inputPath}";
            _extractFiles.v_Password = "{testPassword}";
            _extractFiles.v_PathDestination = "{outputPath}";
            _extractFiles.v_OutputUserVariableName = "{output}";

            _extractFiles.RunCommand(_engine);

            Assert.True(IO.File.Exists(Path.Combine(outputPath, @"compressedWithPassword\compressed.txt")));
            Assert.True(Directory.Exists(Path.Combine(outputPath, @"compressedWithPassword\emptyFolder")));
            Assert.True(IO.File.Exists(Path.Combine(outputPath, @"compressedWithPassword\subfolder\subfolder2\deepfile.txt")));

            IO.File.Delete(Path.Combine(outputPath, @"compressedWithPassword\compressed.txt"));
            Directory.Delete(Path.Combine(outputPath, @"compressedWithPassword\emptyFolder"));
            IO.File.Delete(Path.Combine(outputPath, @"compressedWithPassword\subfolder\subfolder2\deepfile.txt"));
            Directory.Delete(Path.Combine(outputPath, @"compressedWithPassword\subfolder\subfolder2"));
            Directory.Delete(Path.Combine(outputPath, @"compressedWithPassword\subfolder"));
            Directory.Delete(Path.Combine(outputPath, @"compressedWithPassword"));
        }

        [Fact]
        public async global::System.Threading.Tasks.Task HandlesNonDirectoryOutputTarget()
        {
            _engine = new AutomationEngineInstance(null);
            _extractFiles = new ExtractFilesCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\compressed.zip");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));

            string outputPath = inputPath;
            VariableMethods.CreateTestVariable(outputPath, _engine, "outputPath", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(List<>));

            _extractFiles.v_FilePathOrigin = "{inputPath}";
            _extractFiles.v_PathDestination = "{outputPath}";
            _extractFiles.v_OutputUserVariableName = "{output}";

            await Assert.ThrowsAsync<DirectoryNotFoundException>(() => _extractFiles.RunCommand(_engine));
        }

        [Fact]
        public async global::System.Threading.Tasks.Task HandlesNonZipInput()
        {
            _engine = new AutomationEngineInstance(null);
            _extractFiles = new ExtractFilesCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\toCompress.txt");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));

            string outputPath = Environment.CurrentDirectory;
            VariableMethods.CreateTestVariable(outputPath, _engine, "outputPath", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(List<>));

            _extractFiles.v_FilePathOrigin = "{inputPath}";
            _extractFiles.v_PathDestination = "{outputPath}";
            _extractFiles.v_OutputUserVariableName = "{output}";

            await Assert.ThrowsAsync<FileNotFoundException>(() => _extractFiles.RunCommand(_engine));
        }
    }
}
