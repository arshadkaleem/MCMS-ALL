using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MCMS.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        // Get methods
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // Add methods
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

        // Update methods
        Task UpdateAsync(T entity);

        // Delete methods (soft delete when applicable)
        Task DeleteAsync(T entity);

        // Count
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        // Exists
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}
