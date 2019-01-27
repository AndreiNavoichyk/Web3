using System.Threading.Tasks;

namespace Web3.Infra.Repositories
{
    public interface IRepository<TEntity, in TKey> : IQueryableRepository<TEntity, TKey>
        where TEntity : class
    {
        Task AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}