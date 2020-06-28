using System;

namespace FolsomiaSystem.Domain.Entities
{
    public class CredentialSafeConfig
    {
        public string BaseUrl { get; set; }
        public int DefaultCacheExpirationInMinutes { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string TokenKey { get; set; }
        public string TokenSecret { get; set; }
    }
}
