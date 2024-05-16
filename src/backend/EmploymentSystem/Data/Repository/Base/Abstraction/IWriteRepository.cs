using Data.Entities.Base;

namespace Data.Repository.Base
{
    public interface IWriteRepository<TEntity, TKey> : IReadRepository<TEntity, TKey>
        where TEntity : IEntity<TKey>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> UpdateRangeAsync(List<TEntity> entities);
        Task<bool> DeleteAsync(TEntity entity);
    }
}
