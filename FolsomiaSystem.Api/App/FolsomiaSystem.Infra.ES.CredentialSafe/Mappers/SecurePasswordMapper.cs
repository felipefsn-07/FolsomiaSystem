using System;
using FolsomiaSystem.Application.DTOs;
using FolsomiaSystem.Domain.Entities;

namespace FolsomiaSystem.Infra.ES.CredentialSafe.Mappers
{
    internal static class SecurePasswordMapper
    {
        public static AdminUser DtoToDomain(AdminUserInputs item)
        {
            return new AdminUser
            {
                UserName = item.Username,
                Password = item.Password,
            };
        }
    }
}
