using System;
using FolsomiaSystem.Domain.Entities;
using FolsomiaSystem.Infra.ES.CredentialSafe.DTOs;

namespace FolsomiaSystem.Infra.ES.CredentialSafe.Mappers
{
    internal static class SenhaSeguraMapper
    {
        public static Credential DtoToDomain(SenhaSeguraItem item)
        {
            return new Credential
            {
                Aditional = item.Adicional,
                Content = item.Conteudo,
                Domain = item.Dominio,
                ExpirationDate = item.DataHora_Expiracao,
                Hostname = item.Hostname,
                Id = item.Id,
                Ip = item.Ip,
                Name = item.Name,
                Parent_Password = item.Senha_Pai,
                Parent_Password_Cod = item.Senha_Pai_Cod,
                Password = item.Senha,
                Port = item.Porta,
                SO_Version = item.Modelo,
                Tag = item.Tag,
                Username = item.Username
            };
        }
    }
}
