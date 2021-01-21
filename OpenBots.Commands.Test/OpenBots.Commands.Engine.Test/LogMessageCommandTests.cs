using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.IO;
using Xunit;

namespace OpenBots.Commands.Engine.Test
{
    public class LogMessageCommandTests
    {
        private AutomationEngineInstance _engine;
        private LogMessageCommand _logMessage;

        [Theory]
        [InlineData("Verbose", "verboseLog.txt")]
        [InlineData("Debug", "debugLog.txt")]
        [InlineData("Information", "infoLog.txt")]
        [InlineData("Warning", "warnLog.txt")]
        [InlineData("Error", "errLog.txt")]
        [InlineData("Fatal", "fatalLog.txt")]
        public void LogsMessage(string logType, string logFile)
        {
            _engine = new AutomationEngineInstance(null, null);
            _logMessage = new LogMessageCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string inputPath = Path.Combine(projectDirectory, @"Resources\" + logFile);
            inputPath.StoreInUserVariable(_engine, "{inputPath}");

            "testLogData".StoreInUserVariable(_engine, "{logText}");

            _logMessage.v_LogFile = "{inputPath}";
            _logMessage.v_LogText = "{logText}";
            _logMessage.v_LogType = logType;

            _logMessage.RunCommand(_engine);

            string fileText = File.ReadAllText(inputPath);

            Assert.Contains("testLogData", fileText);
            switch (logType)
            {
                case "Verbose":
                    Assert.Contains("[VRB]", fileText);
                    break;
                case "Debug":
                    Assert.Contains("[DBG]", fileText);
                    break;
                case "Information":
                    Assert.Contains("[INF]", fileText);
                    break;
                case "Warning":
                    Assert.Contains("[WRN]", fileText);
                    break;
                case "Error":
                    Assert.Contains("[ERR]", fileText);
                    break;
                case "Fatal":
                    Assert.Contains("[FTL]", fileText);
                    break;
                default:
                    break;
            }

            File.Delete(inputPath);
            File.Create(inputPath);
        }
    }
}
