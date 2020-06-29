using System;
using System.Threading.Tasks;
using FolsomiaSystem.Application.DTOs;
using FolsomiaSystem.Domain.Entities;

namespace FolsomiaSystem.Application.Interfaces.ExternalServices
{
    public interface ICredentialSafeExternalService
    {
        Task<CredentialSafeConfig> Login(AdminUserInputs user);
        Task<CredentialSafeConfig> AlterPassword(AlterAdminUserInputs alterAdminUserInputs);
    }
}
