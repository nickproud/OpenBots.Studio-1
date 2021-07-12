using System.Data;

namespace OpenBots.Core.Interfaces
{
    public interface ISendAdvancedKeystrokesCommand
    {
        string v_WindowName { get; set; }
        DataTable v_KeyActions { get; set; }
        string v_KeyUpDefault { get; set; }
    }
}
