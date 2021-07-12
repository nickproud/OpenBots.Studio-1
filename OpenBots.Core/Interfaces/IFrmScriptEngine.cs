using OpenBots.Core.Enums;
using OpenBots.Core.Model.EngineModel;
using OpenBots.Core.Script;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace OpenBots.Core.Interfaces
{
    public interface IfrmScriptEngine
    {
        EngineContext EngineContext { get; set; }  
        IAutomationEngineInstance EngineInstance { get; set; }
        bool IsNewTaskSteppedInto { get; set; }
        bool IsNewTaskResumed { get; set; }
        bool IsNewTaskCancelled { get; set; }
        bool IsHiddenTaskEngine { get; set; }
        int DebugLineNumber { get; set; }
        bool CloseWhenDone { get; set; }
        bool ClosingAllEngines { get; set; }

        IfrmScriptEngine CreateScriptEngineForm(EngineContext engineContext, bool blnCloseWhenDone);
        void ShowMessage(string message, string title, DialogType dialogType, int closeAfter);
        void ShowEngineContext(string context, int closeAfter);
        List<ScriptVariable> ShowHTMLInput(string htmlTemplate);
        void AddStatus(string text, Color? statusColor = null);
        void uiBtnPause_Click(object sender, EventArgs e);
        void uiBtnCancel_Click(object sender, EventArgs e);
        void UpdateLineNumber(int lineNumber);
        void ResumeParentTask();
    }
}
