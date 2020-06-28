using System;
using System.Threading.Tasks;
using FolsomiaSystem.Domain.Entities;

namespace FolsomiaSystem.Application.Interfaces.Providers
{
    public interface ICredentialSafeConfigProvider
    {
        Task<CredentialSafeConfig> GetByNameAsync(string name);
    }
}
