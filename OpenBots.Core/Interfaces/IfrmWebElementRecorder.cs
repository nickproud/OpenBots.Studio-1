using System.Data;

namespace OpenBots.Core.Interfaces
{
    public interface IfrmWebElementRecorder
    {
        IScriptContext ScriptContext { get; set; }
        DataTable SearchParameters { get; set; }
        string LastItemClicked { get; set; }
        string StartURL { get; set; }
        bool IsRecordingSequence { get; set; }
        void CheckBox_StopOnClick(bool check);
    }
}
