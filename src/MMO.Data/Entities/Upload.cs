using System;
using MMO.Base;

namespace MMO.Data.Entities
{
    public abstract class Upload
    {
        public int Id { get; set; }
        public BuildNumber Version { get; set; }
        public DateTime UploadedAt { get; set; }

        public long FileSizeBytes { get; set; }
        public string OriginalFileName { get; set; }
    }
}
