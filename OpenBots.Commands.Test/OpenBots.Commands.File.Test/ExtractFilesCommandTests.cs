using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.IO;
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
            _engine = new AutomationEngineInstance(null, null);
            _extractFiles = new ExtractFilesCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\compressed.zip");
            inputPath.StoreInUserVariable(_engine, "{inputPath}");

            string outputPath = Environment.CurrentDirectory;
            outputPath.StoreInUserVariable(_engine, "{outputPath}");

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
            _engine = new AutomationEngineInstance(null, null);
            _extractFiles = new ExtractFilesCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\compressedWithPassword.zip");
            inputPath.StoreInUserVariable(_engine, "{inputPath}");

            string outputPath = Environment.CurrentDirectory;
            outputPath.StoreInUserVariable(_engine, "{outputPath}");

            string password = "testPassword";
            password.StoreInUserVariable(_engine, "{testPassword}");

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
        public void HandlesNonDirectoryOutputTarget()
        {
            _engine = new AutomationEngineInstance(null, null);
            _extractFiles = new ExtractFilesCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\compressed.zip");
            inputPath.StoreInUserVariable(_engine, "{inputPath}");

            string outputPath = inputPath;
            outputPath.StoreInUserVariable(_engine, "{outputPath}");

            _extractFiles.v_FilePathOrigin = "{inputPath}";
            _extractFiles.v_PathDestination = "{outputPath}";
            _extractFiles.v_OutputUserVariableName = "{output}";

            Assert.Throws<DirectoryNotFoundException>(() => _extractFiles.RunCommand(_engine));
        }

        [Fact]
        public void HandlesNonZipInput()
        {
            _engine = new AutomationEngineInstance(null, null);
            _extractFiles = new ExtractFilesCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\toCompress.txt");
            inputPath.StoreInUserVariable(_engine, "{inputPath}");

            string outputPath = Environment.CurrentDirectory;
            outputPath.StoreInUserVariable(_engine, "{outputPath}");

            _extractFiles.v_FilePathOrigin = "{inputPath}";
            _extractFiles.v_PathDestination = "{outputPath}";
            _extractFiles.v_OutputUserVariableName = "{output}";

            Assert.Throws<FileNotFoundException>(() => _extractFiles.RunCommand(_engine));
        }
    }
}
