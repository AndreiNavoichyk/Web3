using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web3.Infra.Repositories
{
    public abstract class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class
    {
        public abstract Task<TEntity> GetAsync(TKey key);

        public async Task<IList<TEntity>> GetAllAsync(QueryRequest<TEntity> queryRequest)
        {
            var entities = await GetAllInternalAsync();

            if (queryRequest.Expression != null)
            {
                entities = entities.Where(queryRequest.Expression);
            }

            if (queryRequest.Take.HasValue)
            {
                entities = entities.Take(queryRequest.Take.Value);
            }

            return entities.ToList();
        }

        protected abstract Task<IQueryable<TEntity>> GetAllInternalAsync();

        public abstract Task AddAsync(TEntity entity);

        public abstract Task<TEntity> UpdateAsync(TEntity entity);

        public abstract Task DeleteAsync(TEntity entity);
    }
}