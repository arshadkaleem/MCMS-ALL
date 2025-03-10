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
    public class NavigationMenuRepository : Repository<NavigationMenu>, INavigationMenuRepository
    {
        public NavigationMenuRepository(College365DbContext context) : base(context)
        {
        }

        public async Task<NavigationMenu> GetMenuWithItemsAsync(int menuId)
        {
            // Load the menu first
            var menu = await _context.NavigationMenus
                .FirstOrDefaultAsync(m => m.MenuId == menuId);

            if (menu != null)
            {
                // Then explicitly load the items with their parent-child relationships
                var items = await _context.NavigationItems
                    .Where(i => i.MenuId == menuId)
                    .Include(i => i.Page)
                    .Include(i => i.InverseParent)
                    .ThenInclude(c => c.Page)
                    .ToListAsync();

                // Organize into parent-child hierarchy if needed
                menu.NavigationItems = items.Where(i => i.ParentId == null).ToList();
            }

            return menu;
        }

        public async Task<IEnumerable<NavigationMenu>> GetAllWithItemsAsync()
        {
            return await _context.NavigationMenus
                .Include(m => m.NavigationItems)
                .ToListAsync();
        }

        public async Task<NavigationMenu> GetActiveMenuAsync()
        {
            // Get the active menu first
            var menu = await _context.NavigationMenus
                .FirstOrDefaultAsync(m => m.IsActive == true);

            if (menu != null)
            {
                // Then explicitly load the items with their parent-child relationships
                var items = await _context.NavigationItems
                    .Where(i => i.MenuId == menu.MenuId)
                    .Include(i => i.Page)
                    .Include(i => i.InverseParent   )
                    .ThenInclude(c => c.Page)
                    .ToListAsync();

                // Organize into parent-child hierarchy if needed
                menu.NavigationItems = items.Where(i => i.ParentId == null).ToList();
            }

            return menu;
        }

        private College365DbContext Context => _context as College365DbContext;
    }
}
