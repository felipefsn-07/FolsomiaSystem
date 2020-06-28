using FolsomiaSystem.Domain.Enums;
using System;

namespace FolsomiaSystem.Domain.Entities
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }
        public string MessageLog { get; set; }
        public DateTime DateLog { get; set; }
        public OperationLog OperationLog { get; set; }
        public StatusLog StatusLog { get; set; }

    }
}