using System;
using System.Threading.Tasks;

namespace FolsomiaSystem.Application.Interfaces.Providers
{
    public interface ISqlConnectionStringProvider
    {
        Task<string> GetByNameAsync(string name);
        Task<string> GetByNameAsync(string name, bool invalidateCache);
    }
}
