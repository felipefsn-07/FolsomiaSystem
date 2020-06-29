using FolsomiaSystem.Domain.Enums;
using System;

namespace FolsomiaSystem.Domain.Entities
{
    public class FolsomiaSetup
    {
        public int FolsomiaSetupId { get; set; }
        public int MaxTest { get; set; }
        public int MaxConcentration { get; set; }

        public AuditLog AuditLog = new AuditLog
        {
            DateLog = DateTime.UtcNow,
            StatusLog = StatusLog.success
        };
    }
}