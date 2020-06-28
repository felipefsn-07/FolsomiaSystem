using System;
using System.Threading.Tasks;
using FolsomiaSystem.Domain.Entities;

namespace FolsomiaSystem.Application.Interfaces.ExternalServices
{
    public interface ICredentialSafeExternalService
    {
        Task<Credential> GetByNameAsync(string name);
    }
}
