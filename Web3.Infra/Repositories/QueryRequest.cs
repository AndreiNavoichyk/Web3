using System;
using System.Linq.Expressions;

namespace Web3.Infra.Repositories
{
    public class QueryRequest<TEntity>
        where TEntity : class
    {
        public Expression<Func<TEntity, bool>> Expression { get; set; }
        public int? Take { get; set; }
    }
}
