using System;

namespace OpenBots.Core.Server.Models
{
    public class Asset : NamedEntity
    {
        public string Type { get; set; }
        public string TextValue { get; set; }
        public double? NumberValue { get; set; }
        public string JsonValue { get; set; }
        public Guid? FileId { get; set; }
        public Guid? AgentId { get; set; }
        public long? SizeInBytes { get; set; }
    }
}
