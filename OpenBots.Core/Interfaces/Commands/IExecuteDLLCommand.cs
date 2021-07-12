using System.Data;

namespace OpenBots.Core.Interfaces
{
    public interface IExecuteDLLCommand
    {
        string v_FilePath { get; set; }
        string v_ClassName { get; set; }
        string v_MethodName { get; set; }
        DataTable v_MethodParameters { get; set; }
        string v_OutputUserVariableName { get; set; }
    }
}
