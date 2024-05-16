using Data.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository.Base
{
    public class WriteRepository<TEntity, TKey> : ReadRepository<TEntity, TKey>, IWriteRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private readonly DbContext _context;
        private readonly bool _autoCommit;

        public WriteRepository(DbContext context, bool autoCommit = true) : base(context)
        {
            _context = context;
            _autoCommit = autoCommit;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var result = await _context.Set<TEntity>().AddAsync(entity);

            if (_autoCommit)
            {
                await _context.SaveChangesAsync();
            }

            return result.Entity;
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);

            if (_autoCommit)
            {
                await _context.SaveChangesAsync();
            }

            return true;
        }
        public async Task<bool> UpdateRangeAsync(List<TEntity> entities)
        {
            _context.Set<TEntity>().UpdateRange(entities);

            if (_autoCommit)
            {
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);

            if (_autoCommit)
            {
                await _context.SaveChangesAsync();
            }

            return true;
        }

    }
}
