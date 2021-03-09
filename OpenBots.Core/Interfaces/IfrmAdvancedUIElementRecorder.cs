using System.Data;
using System.Windows.Forms;

namespace OpenBots.Core.Infrastructure
{
    public interface IfrmAdvancedUIElementRecorder
    {
        DataTable SearchParameters { get; set; }
        string LastItemClicked { get; set; }
        bool IsRecordingSequence { get; set; }
        bool IsCommandItemSelected { get; set; }
        string WindowName { get; set; }
        CheckBox chkStopOnClick { get; set; }
    }
}
