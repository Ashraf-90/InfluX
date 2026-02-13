using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Domain.Abstractions;

namespace Infrastructure.Persistence.Reposities
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDBContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        // ---------------------------------------------
        // Get All (With Optional Include)
        // ---------------------------------------------
        public async Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
                query = include(query);

            return await query.ToListAsync();
        }

        // ---------------------------------------------
        // Get All Without Include
        // ---------------------------------------------
        public async Task<IEnumerable<T>> GetAllAsyncWithoutInclude()
        {
            return await _dbSet.ToListAsync();
        }

        // ---------------------------------------------
        // Get All With Filters
        // ---------------------------------------------
        public async Task<IEnumerable<T>> GetAllAsyncWitFillter(List<Expression<Func<T, bool>>>? filters = null)
        {
            IQueryable<T> query = _dbSet;

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    if (filter != null)
                        query = query.Where(filter);
                }
            }

            return await query.ToListAsync();
        }

        // ---------------------------------------------
        // Get All With Filters + Select
        // ---------------------------------------------
        public async Task<IEnumerable<TResult>> GetAllAsyncWitFillterWithSelect<TResult>(
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IQueryable<TResult>>? select = null)
        {
            IQueryable<T> query = _dbSet;

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    if (filter != null)
                        query = query.Where(filter);
                }
            }

            if (select != null)
                return await select(query).ToListAsync();

            return (IEnumerable<TResult>)await query.ToListAsync();
        }

        // ---------------------------------------------
        // Get With Filters + Include
        // ---------------------------------------------
        public async Task<IEnumerable<T>> GetAllFillterIncludeData(
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            List<Expression<Func<T, bool>>>? filters = null)
        {
            IQueryable<T> query = _dbSet;

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    if (filter != null)
                        query = query.Where(filter);
                }
            }

            if (include != null)
                query = include(query);

            return await query.ToListAsync();
        }

        // ---------------------------------------------
        // Add
        // ---------------------------------------------
        public async Task<bool> AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // ---------------------------------------------
        // Update
        // ---------------------------------------------
        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        // ---------------------------------------------
        // Count
        // ---------------------------------------------
        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        // ---------------------------------------------
        // Delete (Hard Delete - rarely used now)
        // ---------------------------------------------
        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
