using System;

namespace Vivarni.Domain.Core.Files
{
    public class FileData
    {
        public Guid FileGuid { get; set; }
        public string Filename { get; set; }
        public string MimeType { get; set; }
        public long ByteCount { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}
