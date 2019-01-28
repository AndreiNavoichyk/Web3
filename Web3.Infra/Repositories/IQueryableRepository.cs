using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web3.Infra.Repositories
{
    public interface IQueryableRepository<TEntity, in TKey>
        where TEntity : class
    {
        Task<TEntity> GetAsync(TKey key);
        Task<IList<TEntity>> GetAllAsync(QueryRequest<TEntity> queryRequest);
    }
}