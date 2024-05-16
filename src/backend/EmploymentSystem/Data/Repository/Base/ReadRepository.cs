using Data.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repository.Base
{
    public class ReadRepository<TEntity, TKey> : IReadRepository<TEntity, TKey>
            where TEntity : class, IEntity<TKey>
            where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private readonly DbContext _context;

        public ReadRepository(DbContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> Table => _context.Set<TEntity>();

        public async Task<TEntity?> GetAsync(TKey id)
        {
            var query = from row in _context.Set<TEntity>().Where(e => e.Id.Equals(id))
                        select row;

            return await query.FirstOrDefaultAsync();
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            var query = from row in _context.Set<TEntity>().Where(expression)
                        select row;

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> expression)
        {
            var query = from row in _context.Set<TEntity>().Where(expression)
                        select row;

            return await query.AnyAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> expression)
        {
            var selector = expression.Compile();

            var query = from row in _context.Set<TEntity>().Where(expression)
                        select row;

            return await query.CountAsync();
        }

        public async Task<IEnumerable<TOutput>> QueryAsync<TOutput>(Func<IQueryable<TEntity>, IQueryable<TOutput>> func)
        {
            var query = func(_context.Set<TEntity>());

            return await query.ToListAsync();
        }
    }
}
