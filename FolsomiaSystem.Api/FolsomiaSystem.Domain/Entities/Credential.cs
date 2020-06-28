using System;

namespace FolsomiaSystem.Domain.Entities
{
    public class Credential
    {
        public int Id { get; set; }
		public string Name { get; set; }
		public string Tag { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Content { get; set; }
		public string Hostname { get; set; }
		public string Parent_Password_Cod { get; set; }
		public string Parent_Password { get; set; }
		public string Aditional { get; set; }
		public string Domain { get; set; }
		public string Ip { get; set; }
		public int Port { get; set; }
		public string SO_Version { get; set; }
		public DateTime ExpirationDate { get; set; }
        public bool FromCache { get; set; }
    }
}
