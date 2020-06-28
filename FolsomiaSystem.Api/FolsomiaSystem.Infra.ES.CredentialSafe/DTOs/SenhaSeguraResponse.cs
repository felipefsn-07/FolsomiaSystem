using System;

namespace FolsomiaSystem.Infra.ES.CredentialSafe.DTOs
{
    internal class SenhaSeguraResponse
    {
        public SenhaSeguraInnerResponse Response { get; set; }
        public SenhaSeguraItem Senha { get; set; }
    }
}
