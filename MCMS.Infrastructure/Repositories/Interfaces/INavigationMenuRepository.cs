using MCMS.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCMS.Infrastructure.Repositories.Interfaces
{
    public interface INavigationMenuRepository : IRepository<NavigationMenu>
    {
        Task<NavigationMenu> GetMenuWithItemsAsync(int menuId);
        Task<IEnumerable<NavigationMenu>> GetAllWithItemsAsync();
        Task<NavigationMenu> GetActiveMenuAsync();
    }
}
