using System;

namespace FolsomiaSystem.Infra.ES.CredentialSafe.DTOs
{
    public class SenhaSeguraItem
    {
        public int Id { get; set; }
		public string Name { get; set; }
		public string Tag { get; set; }
		public string Username { get; set; }
		public string Senha { get; set; }
		public string Conteudo { get; set; }
		public string Hostname { get; set; }
		public string Senha_Pai_Cod { get; set; }
		public string Senha_Pai { get; set; }
		public string Adicional { get; set; }
		public string Dominio { get; set; }
		public string Ip { get; set; }
		public int Porta { get; set; }
		public string Modelo { get; set; }
		public DateTime DataHora_Expiracao { get; set; }
    }
}
