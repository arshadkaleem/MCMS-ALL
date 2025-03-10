using MCMS.Infrastructure.Data;
using MCMS.Infrastructure.Models;
using MCMS.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCMS.Infrastructure.Repositories.Implementations
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(College365DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetAllIncludingPagesAsync()
        {
            return await _context.Categories
                .Include(c => c.StaticPages.Where(p => !p.IsDeleted))
                .ToListAsync();
        }

        public async Task<Category> GetBySlugAsync(string slug)
        {
            return await _context.Categories
                .Include(c => c.StaticPages.Where(p => !p.IsDeleted))
                .FirstOrDefaultAsync(c => c.Slug == slug) ?? new Category();
        }

        public async Task<bool> SlugExistsAsync(string slug)
        {
            return await _context.Categories.AnyAsync(c => c.Slug == slug);
        }

        public async Task<bool> SlugExistsExceptAsync(string slug, int categoryId)
        {
            return await _context.Categories
                .AnyAsync(c => c.Slug == slug && c.CategoryId != categoryId);
        }

        // Helper method to get the College365DbContext
        private College365DbContext Context => _context as College365DbContext;
    }
}
