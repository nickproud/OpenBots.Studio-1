using System;

namespace OpenBots.Core.Server.Models
{
    public class Automation : NamedEntity
    {
        public Guid? FileId { get; set; }
        public string OriginalPackageName { get; set; }
        public string AutomationEngine { get; set; }
        public double? AverageSuccessfulExecutionInMinutes { get; set; }
        public double? AverageUnSuccessfulExecutionInMinutes { get; set; }
    }
}
