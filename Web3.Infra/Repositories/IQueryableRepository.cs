using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Web3.Infra.Repositories
{
    public interface IQueryableRepository<TEntity, in TKey>
        where TEntity : class
    {
        Task<TEntity> GetAsync(TKey key);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression);
    }
}