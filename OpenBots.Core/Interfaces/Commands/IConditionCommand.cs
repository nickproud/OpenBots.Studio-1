using System.Data;

namespace OpenBots.Core.Interfaces
{
    public interface IConditionCommand
    {
        DataTable v_ActionParameterTable { get; set; }
    }
}
