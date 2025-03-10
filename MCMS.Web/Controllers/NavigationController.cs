using MCMS.Infrastructure.Models;
using MCMS.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

public class NavigationController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public NavigationController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: Navigation
    public async Task<IActionResult> Index()
    {
        var menus = await _unitOfWork.NavigationMenus.GetAllAsync();
        return View(menus);
    }

    // GET: Navigation/CreateMenu
    public IActionResult CreateMenu()
    {
        return View();
    }

    // POST: Navigation/CreateMenu
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateMenu(NavigationMenu menu)
    {
        if (ModelState.IsValid)
        {
            menu.CreatedAt = DateTime.Now;

            // If this is set as active, deactivate other menus
            if (menu.IsActive == true)
            {
                var activeMenus = await _unitOfWork.NavigationMenus.GetAllAsync();
                foreach (var existingMenu in activeMenus)
                {
                    existingMenu.IsActive = false;
                    await _unitOfWork.NavigationMenus.UpdateAsync(existingMenu);
                }
            }

            await _unitOfWork.NavigationMenus.AddAsync(menu);
            await _unitOfWork.CompleteAsync();

            TempData["Message"] = "Menu created successfully!";
            return RedirectToAction(nameof(EditMenu), new { id = menu.MenuId });
        }

        return View(menu);
    }

    // GET: Navigation/EditMenu/5
    public async Task<IActionResult> EditMenu(int id)
    {
        var menu = await _unitOfWork.NavigationMenus.GetByIdAsync(id);
        if (menu == null)
        {
            return NotFound();
        }

        try
        {
            // Get menu items for this menu - handle potential NULL values
            var menuItems = await _unitOfWork.NavigationItems.GetByMenuIdAsync(id);
            ViewBag.MenuItems = menuItems ?? new List<NavigationItem>();

            // Get available pages for adding to menu
            var pages = await _unitOfWork.StaticPages.GetAllPublishedAsync();
            ViewBag.AvailablePages = pages ?? new List<StaticPage>();

            // Get categories for adding to menu
            var categories = await _unitOfWork.Categories.GetAllAsync();
            ViewBag.Categories = categories ?? new List<Category>();
        }
        catch (Exception ex)
        {
            // Log the exception
            ViewBag.ErrorMessage = "Error loading menu data: " + ex.Message;
            ViewBag.MenuItems = new List<NavigationItem>();
            ViewBag.AvailablePages = new List<StaticPage>();
            ViewBag.Categories = new List<Category>();
        }

        return View(menu);
    }

    // POST: Navigation/UpdateMenu
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateMenu(NavigationMenu menu)
    {
        if (ModelState.IsValid)
        {
            // If this is set as active, deactivate other menus
            if (menu.IsActive == true)
            {
                var activeMenus = await _unitOfWork.NavigationMenus.GetAllAsync();
                foreach (var existingMenu in activeMenus.Where(m => m.MenuId != menu.MenuId))
                {
                    existingMenu.IsActive = false;
                    await _unitOfWork.NavigationMenus.UpdateAsync(existingMenu);
                }
            }

            await _unitOfWork.NavigationMenus.UpdateAsync(menu);
            await _unitOfWork.CompleteAsync();

            TempData["Message"] = "Menu settings updated successfully!";
            return RedirectToAction(nameof(EditMenu), new { id = menu.MenuId });
        }

        return View("EditMenu", menu);
    }

    // POST: Navigation/AddMenuItems
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddMenuItems(int menuId, string itemType, List<int> selectedPages = null, List<int> selectedCategories = null)
    {
        if (itemType == "Page" && selectedPages != null && selectedPages.Any())
        {
            int order = await GetNextOrderIndex(menuId);

            foreach (var pageId in selectedPages)
            {
                var page = await _unitOfWork.StaticPages.GetByIdAsync(pageId);
                if (page != null)
                {
                    var menuItem = new NavigationItem
                    {
                        MenuId = menuId,
                        Title = page.Title,
                        PageId = pageId,
                        OrderIndex = order++,
                        RoleAccess = "Public",
                        CreatedAt = DateTime.Now
                    };

                    await _unitOfWork.NavigationItems.AddAsync(menuItem);
                }
            }

            await _unitOfWork.CompleteAsync();
            TempData["Message"] = "Pages added to menu successfully!";
        }
        else if (itemType == "Category" && selectedCategories != null && selectedCategories.Any())
        {
            int order = await GetNextOrderIndex(menuId);

            foreach (var categoryId in selectedCategories)
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
                if (category != null)
                {
                    var menuItem = new NavigationItem
                    {
                        MenuId = menuId,
                        Title = category.CategoryName,
                        Slug = $"category/{category.Slug}",
                        OrderIndex = order++,
                        RoleAccess = "Public",
                        CreatedAt = DateTime.Now
                    };

                    await _unitOfWork.NavigationItems.AddAsync(menuItem);
                }
            }

            await _unitOfWork.CompleteAsync();
            TempData["Message"] = "Categories added to menu successfully!";
        }

        return RedirectToAction(nameof(EditMenu), new { id = menuId });
    }

    // POST: Navigation/AddCustomLink
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddCustomLink(int menuId, string url, string linkText)
    {
        if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(linkText))
        {
            int order = await GetNextOrderIndex(menuId);

            var menuItem = new NavigationItem
            {
                MenuId = menuId,
                Title = linkText,
                ExternalUrl = url,
                OrderIndex = order,
                RoleAccess = "Public",
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.NavigationItems.AddAsync(menuItem);
            await _unitOfWork.CompleteAsync();

            TempData["Message"] = "Custom link added to menu successfully!";
        }

        return RedirectToAction(nameof(EditMenu), new { id = menuId });
    }

    // POST: Navigation/SaveMenuStructure
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveMenuStructure(int menuId, string menuStructure)
    {
        if (!string.IsNullOrEmpty(menuStructure))
        {
            try
            {
                // Deserialize the JSON structure
                var menuItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MenuItemStructure>>(menuStructure);

                // Process the structure
                await ProcessMenuStructure(menuItems, null);

                await _unitOfWork.CompleteAsync();

                TempData["Message"] = "Menu structure saved successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error saving menu structure: {ex.Message}";
            }
        }

        return RedirectToAction(nameof(EditMenu), new { id = menuId });
    }

    // GET: Navigation/DeleteMenuItem/5
    public async Task<IActionResult> DeleteMenuItem(int id)
    {
        var menuItem = await _unitOfWork.NavigationItems.GetByIdAsync(id);
        if (menuItem == null)
        {
            return NotFound();
        }

        int menuId = menuItem.MenuId;

        // Delete the menu item and its children
        var children = await _unitOfWork.NavigationItems.GetChildItemsAsync(id);
        foreach (var child in children)
        {
            await _unitOfWork.NavigationItems.RemoveAsync(child);
        }

        await _unitOfWork.NavigationItems.RemoveAsync(menuItem);
        await _unitOfWork.CompleteAsync();

        TempData["Message"] = "Menu item deleted successfully!";
        return RedirectToAction(nameof(EditMenu), new { id = menuId });
    }

    // POST: Navigation/UpdateMenuItem
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateMenuItem(NavigationItem item)
    {
        var menuItem = await _unitOfWork.NavigationItems.GetByIdAsync(item.ItemId);
        if (menuItem == null)
        {
            return NotFound();
        }

        // Update only the editable properties
        menuItem.Title = item.Title;
        if (!string.IsNullOrEmpty(item.ExternalUrl))
        {
            menuItem.ExternalUrl = item.ExternalUrl;
        }
        menuItem.RoleAccess = item.RoleAccess;

        await _unitOfWork.NavigationItems.UpdateAsync(menuItem);
        await _unitOfWork.CompleteAsync();

        TempData["Message"] = "Menu item updated successfully!";
        return RedirectToAction(nameof(EditMenu), new { id = menuItem.MenuId });
    }

    // Helper methods
    private async Task<int> GetNextOrderIndex(int menuId)
    {
        var items = await _unitOfWork.NavigationItems.GetByMenuIdAsync(menuId);
        return items.Any() ? items.Max(i => i.OrderIndex) + 1 : 0;
    }

    private async Task ProcessMenuStructure(List<MenuItemStructure> items, int? parentId)
    {
        if (items == null) return;

        int order = 0;
        foreach (var item in items)
        {
            var menuItem = await _unitOfWork.NavigationItems.GetByIdAsync(item.id);
            if (menuItem != null)
            {
                menuItem.ParentId = parentId;
                menuItem.OrderIndex = order++;
                await _unitOfWork.NavigationItems.UpdateAsync(menuItem);

                // Process children
                if (item.children != null && item.children.Any())
                {
                    await ProcessMenuStructure(item.children, menuItem.ItemId);
                }
            }
        }
    }
}

// Helper class for JSON deserialization
public class MenuItemStructure
{
    public int id { get; set; }
    public List<MenuItemStructure> children { get; set; }
}