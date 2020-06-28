using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FolsomiaSystem.Domain
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity obj);
        Task AddAsync(IEnumerable<TEntity> objs);
        void Update(TEntity obj);
        void Remove(TEntity obj);
        void Remove(IEnumerable<TEntity> objs);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> where);
        Task<IEnumerable<TEntity>> GetAsync();
        ValueTask<TEntity> GetAsync(int id);

        Task<TEntity> GetFristAsync();
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> where);
        Task<IPagedList<TEntity>> GetAsync<TKey>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TKey>> order, int pageNumber, int pageSize);
        Task<IPagedList<TEntity>> GetAsync<TKey>(Expression<Func<TEntity, TKey>> order, int pageNumber, int pageSize);

    }
}
