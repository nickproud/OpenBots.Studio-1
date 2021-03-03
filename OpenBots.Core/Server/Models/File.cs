using System;

namespace OpenBots.Core.Server.Models
{
    public class File : NamedEntity
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public long? Size { get; set; }
        public string StoragePath { get; set; }
        public string FullStoragePath { get; set; }
        public  bool? HasChild { get; set; }
        public string ContentType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsFile { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? StorageDriveId { get; set; }
    }
}
