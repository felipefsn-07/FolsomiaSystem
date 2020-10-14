using System;
using System.Collections.Generic;
using System.Text;

namespace FolsomiaSystem.Domain.Entities
{
    public class FileToUpload
    {
        public string FileAsBase64 { get; set; }
        public byte[] FileAsByteArray { get; set; }
    }
}
