using System;

namespace OpenBots.Core.Server.Models
{
    public class AutomationParameter : NamedEntity
    {
        public string DataType { get; set; }
        public string Value { get; set; }
        public Guid? AutomationId { get; set; }
    }
}
