using System.Data;
using System.Windows.Forms;

namespace OpenBots.Core.Interfaces
{
    public interface IfrmAdvancedUIElementRecorder
    {
        DataTable SearchParameters { get; set; }
        string LastItemClicked { get; set; }
        bool IsRecordingSequence { get; set; }
        string WindowName { get; set; }
        CheckBox chkStopOnClick { get; set; }
    }
}
