using MCMS.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCMS.Infrastructure.Repositories.Interfaces
{
    public interface IStaticPageRepository : IRepository<StaticPage>
    {
        Task<IEnumerable<StaticPage>> GetAllActiveAsync();
        Task<IEnumerable<StaticPage>> GetAllByCategoryIdAsync(int categoryId);
        Task<IEnumerable<StaticPage>> GetAllPublishedAsync();
        Task<StaticPage> GetBySlugAsync(string slug);
        Task<StaticPage> GetWithCategoryAsync(int pageId);
        Task<bool> SlugExistsAsync(string slug);
        Task<bool> SlugExistsExceptAsync(string slug, int pageId);
        Task SoftDeleteAsync(int pageId);
        Task RestoreAsync(int pageId);
    }
}
