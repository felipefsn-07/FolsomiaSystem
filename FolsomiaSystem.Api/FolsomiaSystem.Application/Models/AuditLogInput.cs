using FolsomiaSystem.Domain.Enums;
using System;

namespace FolsomiaSystem.Application.Models
{
    public class AuditLogInput
    {
        public string MessageLog { get; set; }

        public DateTime DateLog { get; set; }

        public Nullable<OperationLog> OperationLog { get; set; }

        public Nullable<StatusLog> StatusLog { get; set; }

    }
}