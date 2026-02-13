using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        // GetAllAsync method with optional include
        public async Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
            {
                query = include(query);
            }

            var result = await query.ToListAsync();
            return result;
        }

        public async Task<IEnumerable<T>> GetAllAsyncWithoutInclude()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsyncWitFillter(List<Expression<Func<T, bool>>>? filters = null)
        {
            var entities = _dbSet.AsQueryable();

            foreach (var filter in filters)
            {
                if (filter != null)
                {
                    entities = entities.Where(filter);

                }
            }

            return await entities.ToListAsync();
        }

        public async Task<IEnumerable<TResult>> GetAllAsyncWitFillterWithSelect<TResult>(List<Expression<Func<T, bool>>>? filters = null,Func<IQueryable<T>, IQueryable<TResult>>? select = null)
        {
            var entities = _dbSet.AsQueryable();

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    if (filter != null)
                        entities = entities.Where(filter);
                }
            }

            if (select != null)
                return await select(entities).ToListAsync();

            // في حال لم يتم تمرير select
            return (IEnumerable<TResult>)await entities.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllFillterIncludeData(Func<IQueryable<T>, IQueryable<T>> include = null, List<Expression<Func<T, bool>>>? filters = null)
        {

            IQueryable<T> query = _dbSet;

            foreach (var filter in filters)
            {
                if (filter != null)
                {
                    query = query.Where(filter);

                }
            }



            if (include != null)
            {
                query = include(query);
            }

            var result = await query.ToListAsync();

            return result;
        }

        public async Task<bool> AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
