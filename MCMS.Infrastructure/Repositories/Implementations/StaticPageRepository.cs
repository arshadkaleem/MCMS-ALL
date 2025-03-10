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
    public class StaticPageRepository : Repository<StaticPage>, IStaticPageRepository
    {
        public StaticPageRepository(College365DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<StaticPage>> GetAllActiveAsync()
        {
            return await _context.StaticPages
                .Where(p => !p.IsDeleted)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<StaticPage>> GetAllByCategoryIdAsync(int categoryId)
        {
            return await _context.StaticPages
                .Where(p => p.CategoryId == categoryId && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<StaticPage>> GetAllPublishedAsync()
        {
            return await _context.StaticPages
                .Where(p => p.Published && !p.IsDeleted)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<StaticPage> GetBySlugAsync(string slug)
        {
            return await _context.StaticPages
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Slug == slug && !p.IsDeleted);
        }

        public async Task<StaticPage> GetWithCategoryAsync(int pageId)
        {
            return await _context.StaticPages
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.PageId == pageId && !p.IsDeleted);
        }

        public async Task<bool> SlugExistsAsync(string slug)
        {
            return await _context.StaticPages.AnyAsync(p => p.Slug == slug && !p.IsDeleted);
        }

        public async Task<bool> SlugExistsExceptAsync(string slug, int pageId)
        {
            return await _context.StaticPages
                .AnyAsync(p => p.Slug == slug && p.PageId != pageId && !p.IsDeleted);
        }

        public async Task SoftDeleteAsync(int pageId)
        {
            var page = await _context.StaticPages.FindAsync(pageId);
            if (page != null)
            {
                page.IsDeleted = true;
                page.DeletedAt = DateTime.Now;
            }
        }

        public async Task RestoreAsync(int pageId)
        {
            var page = await _context.StaticPages.FindAsync(pageId);
            if (page != null)
            {
                page.IsDeleted = false;
                page.DeletedAt = null;
            }
        }

        // Helper method to get the College365DbContext
        private College365DbContext Context => _context as College365DbContext;
    }
}
