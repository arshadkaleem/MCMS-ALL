using MCMS.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCMS.Infrastructure.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetAllIncludingPagesAsync();
        Task<Category> GetBySlugAsync(string slug);
        Task<bool> SlugExistsAsync(string slug);
        Task<bool> SlugExistsExceptAsync(string slug, int categoryId);
    }
}
