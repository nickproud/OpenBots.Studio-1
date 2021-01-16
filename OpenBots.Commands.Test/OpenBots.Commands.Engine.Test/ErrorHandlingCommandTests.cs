using OpenBots.Core.Script;
using OpenBots.Engine;
using System;
using System.IO;
using Xunit;

namespace OpenBots.Commands.Engine.Test
{
    public class ErrorHandlingCommandTests
    {
        private AutomationEngineInstance _engine;
        private ErrorHandlingCommand _errorHandling;
        private TextFile.ReadTextFileCommand _readTextFile;

        [Theory]
        [InlineData("Stop Processing")]
        [InlineData("Continue Processing")]
        public void ChangesEngineErrorHandling(string handleAction)
        {
            _engine = new AutomationEngineInstance(null, null);
            _errorHandling = new ErrorHandlingCommand();

            _errorHandling.v_ErrorHandlingAction = handleAction;

            _errorHandling.RunCommand(_engine);

            _readTextFile = new TextFile.ReadTextFileCommand();
            
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, @"Resources\doesNotExist.txt");
            
            _readTextFile.v_OutputUserVariableName = "{test}";
            _readTextFile.v_FilePath = "filePath";

            ScriptAction action = new ScriptAction();
            action.ScriptCommand = _readTextFile;

            if(handleAction.Equals("Stop Processing"))
            {
                Assert.Throws<FileNotFoundException>(() => _engine.ExecuteCommand(action));
            }
            else
            {
                _engine.ExecuteCommand(action);
            }

            Assert.Equal(handleAction, _engine.ErrorHandlingAction);
        }
    }
}
