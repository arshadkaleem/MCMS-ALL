using MCMS.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCMS.Infrastructure.Repositories.Interfaces
{
    public interface INavigationItemRepository : IRepository<NavigationItem>
    {
        Task<IEnumerable<NavigationItem>> GetByMenuIdAsync(int menuId);
        Task<IEnumerable<NavigationItem>> GetParentItemsAsync(int menuId);
        Task<IEnumerable<NavigationItem>> GetChildItemsAsync(int parentId);
        Task RemoveAsync(NavigationItem item);
    }
}
