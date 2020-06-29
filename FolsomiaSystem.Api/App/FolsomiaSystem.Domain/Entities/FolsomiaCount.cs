using FolsomiaSystem.Domain.Enums;
using System;

namespace FolsomiaSystem.Domain.Entities
{
    public class FolsomiaCount
    {
        public string IdTest { get; set; }
        public string ImageFolsomiaURL { get; set; }
        public string ImageFolsomiaOutlinedURL { get; set; }
        public decimal TotalCountFolsomia { get; set; }
        public Nullable<BackgroundImage> BackgroundImage { get; set; }
        public AuditLog AuditLog = new AuditLog
        {
            DateLog = DateTime.UtcNow,
            StatusLog = StatusLog.success
        };
    }
}