using System;
using System.Threading.Tasks;

namespace FolsomiaSystem.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync();
    }
}
