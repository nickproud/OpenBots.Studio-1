using OpenBots.Core.Command;
using OpenBots.Core.Model.EngineModel;
using OpenBots.Core.Script;
using OpenBots.Core.Settings;
using RestSharp;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenBots.Core.Infrastructure
{
    public interface IAutomationEngineInstance
    {
        EngineContext AutomationEngineContext { get; set; }
        //ErrorHandlingCommand ErrorHandler;
        List<ScriptError> ErrorsOccured { get; set; }
        string ErrorHandlingAction { get; set; }
        bool ChildScriptFailed { get; set; }
        bool ChildScriptErrorCaught { get; set; }
        ScriptCommand LastExecutedCommand { get; set; }
        bool IsCancellationPending { get; set; }
        bool CurrentLoopCancelled { get; set; }
        bool CurrentLoopContinuing { get; set; }
        EngineSettings EngineSettings { get; set; }
        string PrivateCommandLog { get; set; }
        string FileName { get; set; }
        bool IsServerExecution { get; set; }
        bool IsServerChildExecution { get; set; }
        List<IRestResponse> ServiceResponses { get; set; }
        bool AutoCalculateVariables { get; set; }
        string TaskResult { get; set; }

        event EventHandler<ReportProgressEventArgs> ReportProgressEvent;
        event EventHandler<ScriptFinishedEventArgs> ScriptFinishedEvent;
        event EventHandler<LineNumberChangedEventArgs> LineNumberChangedEvent;

        void ExecuteScriptAsync();
        void ExecuteScriptAsync(string filePath, string projectPath);
        void ExecuteScriptJson();
        Task ExecuteCommand(ScriptAction command);
        string GetEngineContext();
        void ReportProgress(string progress, LogEventLevel eventLevel = LogEventLevel.Information);
        string GetProjectPath();
        IAutomationEngineInstance CreateAutomationEngineInstance(EngineContext engineContext);
        void ExecuteScriptSync();
    }
}
