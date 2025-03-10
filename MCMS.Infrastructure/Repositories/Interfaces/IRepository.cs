using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MCMS.Infrastructure.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}
