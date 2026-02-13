using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> include = null);

        Task<IEnumerable<T>> GetAllAsyncWithoutInclude();

        Task<IEnumerable<T>> GetAllAsyncWitFillter(List<Expression<Func<T, bool>>>? filters = null);

        Task<IEnumerable<TResult>> GetAllAsyncWitFillterWithSelect<TResult>(List<Expression<Func<T, bool>>>? filters = null,Func<IQueryable<T>, IQueryable<TResult>>? select = null);

        Task<IEnumerable<T>> GetAllFillterIncludeData(Func<IQueryable<T>, IQueryable<T>> include = null, List<Expression<Func<T, bool>>>? filters = null);

        Task<bool> AddAsync(T entity);

        Task<bool> UpdateAsync(T entity);

        void UpdateRange(IEnumerable<T> entities);

        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        void DeleteRange(IEnumerable<T> entities);
    }
}
