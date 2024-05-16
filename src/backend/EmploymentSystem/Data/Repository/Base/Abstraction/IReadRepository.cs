using Data.Entities.Base;
using System.Linq.Expressions;

namespace Data.Repository.Base
{
    public interface IReadRepository<TEntity, TKey>
        where TEntity : IEntity<TKey>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        public IQueryable<TEntity> Table { get; }

        Task<TEntity?> GetAsync(TKey id);
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> expression);
        Task<bool> ExistAsync(Expression<Func<TEntity, bool>> expression);

        Task<IEnumerable<TOutput>> QueryAsync<TOutput>(Func<IQueryable<TEntity>, IQueryable<TOutput>> func);
    }
}
