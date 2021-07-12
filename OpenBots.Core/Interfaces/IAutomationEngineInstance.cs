using OpenBots.Core.Command;
using OpenBots.Core.Model.EngineModel;
using OpenBots.Core.Script;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenBots.Core.Interfaces
{
    public interface IAutomationEngineInstance
    {
        EngineContext EngineContext { get; set; }
        ScriptCommand LastExecutedCommand { get; set; }
        List<ScriptError> ErrorsOccured { get; set; }
        string ErrorHandlingAction { get; set; }
        bool ChildScriptFailed { get; set; }
        bool ChildScriptErrorCaught { get; set; }
        bool IsCancellationPending { get; set; }
        bool CurrentLoopCancelled { get; set; }
        bool CurrentLoopContinuing { get; set; }
        string PrivateCommandLog { get; set; }

        event EventHandler<ReportProgressEventArgs> ReportProgressEvent;
        event EventHandler<ScriptFinishedEventArgs> ScriptFinishedEvent;
        event EventHandler<LineNumberChangedEventArgs> LineNumberChangedEvent;

        void ExecuteScriptAsync();
        Task ExecuteCommand(ScriptAction command);
        string GetEngineContext();
        void ReportProgress(string progress, string eventLevel = "Information");
        IAutomationEngineInstance CreateAutomationEngineInstance(EngineContext engineContext);
        void ExecuteScriptSync();
        void PauseScript();
        void ResumeScript();
        void CancelScript();
        void StepOverScript();
        void StepIntoScript();
    }
}
