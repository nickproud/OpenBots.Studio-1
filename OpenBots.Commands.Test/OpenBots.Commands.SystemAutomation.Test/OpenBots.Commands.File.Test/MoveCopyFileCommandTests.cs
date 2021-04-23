using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.IO;
using Xunit;
using IO = System.IO;

namespace OpenBots.Commands.File.Test
{
    public class MoveCopyFileCommandTests
    {
        private AutomationEngineInstance _engine;
        private MoveCopyFileCommand _moveCopyFile;

        [Theory]
        [InlineData("Move File")]
        [InlineData("Copy File")]
        public void CopiesAndMovesFile(string operation)
        {
            _engine = new AutomationEngineInstance(null);
            _moveCopyFile = new MoveCopyFileCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\toCompress.txt");
            string destinationPath = Path.Combine(projectDirectory, @"Resources\moveCopyDestination");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));
            VariableMethods.CreateTestVariable(destinationPath, _engine, "destinationPath", typeof(string));

            _moveCopyFile.v_OperationType = operation;
            _moveCopyFile.v_SourceFilePath = "{inputPath}";
            _moveCopyFile.v_DestinationDirectory = "{destinationPath}";
            _moveCopyFile.v_CreateDirectory = "Yes";
            _moveCopyFile.v_OverwriteFile = "Yes";

            _moveCopyFile.RunCommand(_engine);

            Assert.True(IO.File.Exists(Path.Combine(destinationPath, @"toCompress.txt")));

            if (operation.Equals("Move File")){
                resetMoveTest(Path.Combine(projectDirectory, @"Resources"), Path.Combine(destinationPath, @"toCompress.txt"));
            }
            else
            {
                IO.File.Delete(Path.Combine(destinationPath, @"toCompress.txt"));
            }
        }

        [Fact]
        public async global::System.Threading.Tasks.Task HandlesInvalidFilepath()
        {
            _engine = new AutomationEngineInstance(null);
            _moveCopyFile = new MoveCopyFileCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\nofile.txt");
            string destinationPath = Path.Combine(projectDirectory, @"Resources\moveCopyDestination");
            VariableMethods.CreateTestVariable(inputPath, _engine, "inputPath", typeof(string));
            VariableMethods.CreateTestVariable(destinationPath, _engine, "destinationPath", typeof(string));

            _moveCopyFile.v_OperationType = "Copy File";
            _moveCopyFile.v_SourceFilePath = "{inputPath}";
            _moveCopyFile.v_DestinationDirectory = "{destinationPath}";
            _moveCopyFile.v_CreateDirectory = "Yes";
            _moveCopyFile.v_OverwriteFile = "Yes";

            await Assert.ThrowsAsync<FileNotFoundException>(() => _moveCopyFile.RunCommand(_engine));
        }

        private void resetMoveTest(string initialDirectory, string movedFile)
        {
            _engine = new AutomationEngineInstance(null);
            _moveCopyFile = new MoveCopyFileCommand();

            _moveCopyFile.v_OperationType = "Move File";
            _moveCopyFile.v_SourceFilePath = movedFile;
            _moveCopyFile.v_DestinationDirectory = initialDirectory;
            _moveCopyFile.v_CreateDirectory = "No";
            _moveCopyFile.v_OverwriteFile = "No";

            _moveCopyFile.RunCommand(_engine);

            IO.File.Delete(movedFile);
        }
    }
}
