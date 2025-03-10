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
    public class NavigationItemRepository : Repository<NavigationItem>, INavigationItemRepository
    {
        public NavigationItemRepository(College365DbContext context) : base(context)
        {
        }


        public async Task<IEnumerable<NavigationItem>> GetByMenuIdAsync(int menuId)
        {
            return await _context.NavigationItems
                .Include(i => i.PageId != null ? i.Page : null)
                .Where(i => i.MenuId == menuId)
                .OrderBy(i => i.OrderIndex)
                .ToListAsync();
        }
        //public async Task<IEnumerable<NavigationItem>> GetByMenuIdAsync(int menuId)
        //{
        //    return await _context.NavigationItems
        //        .Include(i => i.Page)
        //        .Where(i => i.MenuId == menuId)
        //        .OrderBy(i => i.OrderIndex)
        //        .ToListAsync();
        //}

        public async Task<IEnumerable<NavigationItem>> GetParentItemsAsync(int menuId)
        {
            return await _context.NavigationItems
                .Include(i => i.Page)
                .Where(i => i.MenuId == menuId && i.ParentId == null)
                .OrderBy(i => i.OrderIndex)
                .ToListAsync();
        }

        public async Task<IEnumerable<NavigationItem>> GetChildItemsAsync(int parentId)
        {
            return await _context.NavigationItems
                .Include(i => i.Page)
                .Where(i => i.ParentId == parentId)
                .OrderBy(i => i.OrderIndex)
                .ToListAsync();
        }

        public async Task RemoveAsync(NavigationItem item)
        {
            _context.NavigationItems.Remove(item);
        }

        private College365DbContext Context => _context as College365DbContext;
    }
}
