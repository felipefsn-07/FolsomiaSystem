using System;
using System.Threading.Tasks;

namespace FolsomiaSystem.Domain
{
    public interface ICache<TEntity> where TEntity : class
    {
        Task<TEntity> GetAsync(string key);
        Task AddAsync(string key, TEntity obj, DateTime expirationDate);
        void Remove(string key);
    }
}
