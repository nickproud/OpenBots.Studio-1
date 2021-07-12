using OpenBots.Core.Enums;
using Serilog.Events;
using System;
using System.Windows.Forms;

namespace OpenBots.Core.Settings
{
    /// <summary>
    /// Defines engine settings which can be managed by the user
    /// </summary>
    [Serializable]
    public class EngineSettings
    {
        public bool ShowDebugWindow { get; set; }
        public bool AutoCloseDebugWindow { get; set; }
        public bool ShowAdvancedDebugOutput { get; set; }
        public bool TrackExecutionMetrics { get; set; }
        public Keys CancellationKey { get; set; }
        public int DelayBetweenCommands { get; set; }
        public SinkType LoggingSinkType { get; set; }
        public string LoggingValue { get; set; }
        public LogEventLevel MinLogLevel { get; set; }        

        public EngineSettings()
        {
            ShowDebugWindow = true;
            AutoCloseDebugWindow = false;
            ShowAdvancedDebugOutput = true;
            TrackExecutionMetrics = true;
            CancellationKey = Keys.Pause;
            DelayBetweenCommands = 250;
            LoggingSinkType = SinkType.File;
            LoggingValue = "";
            MinLogLevel = LogEventLevel.Verbose;
        }        
    }
}
