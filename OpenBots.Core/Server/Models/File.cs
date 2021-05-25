using System;

namespace OpenBots.Core.Server.Models
{
    public class File : NamedEntity
    {
        public long? Size { get; set; }
        public string StoragePath { get; set; }
        public string FullStoragePath { get; set; }
        public  bool? HasChild { get; set; }
        public string ContentType { get; set; }
        public bool? IsFile { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? StorageDriveId { get; set; }
    }
}