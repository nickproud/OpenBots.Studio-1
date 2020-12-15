using System.Data;

namespace OpenBots.Core.Infrastructure
{
    public interface IConditionCommand
    {
        DataTable v_ActionParameterTable { get; set; }
    }
}
