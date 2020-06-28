using System;
using System.Threading.Tasks;
using FolsomiaSystem.Domain.Entities;

namespace FolsomiaSystem.Application.Interfaces.Repositories
{
    public interface ICredentialSafeRepository
    {
        Task<Credential> GetByNameAsync(string name);
        Task<Credential> GetByNameAsync(string name, bool invalidateCache);
    }
}
