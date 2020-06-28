using System;

namespace FolsomiaSystem.Infra.ES.CredentialSafe.DTOs
{
    internal class SenhaSeguraInnerResponse
    {
        public int Status { get; set; }
        public string Mensagem { get; set; }
        public bool Erro { get; set; }
        public string Cod_Erro { get; set; }
    }
}
