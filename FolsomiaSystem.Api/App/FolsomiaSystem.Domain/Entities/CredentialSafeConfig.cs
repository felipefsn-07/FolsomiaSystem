using System;

namespace FolsomiaSystem.Domain.Entities
{
    public class CredentialSafeConfig
    {
        public int DefaultCacheExpirationInMinutes { get; set; }
        public string Token { get; set; }

        public  AuditLog  AuditLog{ get; set; }
    }
}
