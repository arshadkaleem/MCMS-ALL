using MCMS.Infrastructure.Models;
using MCMS.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace MCMS.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return View(categories);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _unitOfWork.Categories.GetByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,Slug")] Category category)
        {
            if (string.IsNullOrEmpty(category.Slug))
            {
                category.Slug = GenerateSlug(category.CategoryName);
            }

            if (await _unitOfWork.Categories.SlugExistsAsync(category.Slug))
            {
                ModelState.AddModelError("Slug", "This slug is already in use. Please choose another one.");
            }

            if (ModelState.IsValid)
            {
                category.CreatedAt = DateTime.Now;
                category.UpdatedAt = DateTime.Now;

                await _unitOfWork.Categories.AddAsync(category);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _unitOfWork.Categories.GetByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,Slug,CreatedAt")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(category.Slug))
            {
                category.Slug = GenerateSlug(category.CategoryName);
            }

            if (await _unitOfWork.Categories.SlugExistsExceptAsync(category.Slug, category.CategoryId))
            {
                ModelState.AddModelError("Slug", "This slug is already in use. Please choose another one.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    category.UpdatedAt = DateTime.Now;
                    await _unitOfWork.Categories.UpdateAsync(category);
                    await _unitOfWork.CompleteAsync();
                }
                catch (Exception)
                {
                    if (!await CategoryExists(category.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _unitOfWork.Categories.GetByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            // Check if category has any static pages
            var pages = await _unitOfWork.StaticPages.GetAllByCategoryIdAsync(id.Value);
            if (pages.Any())
            {
                ViewBag.HasPages = true;
                ViewBag.PagesCount = pages.Count();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            // Check if category has any static pages
            var pages = await _unitOfWork.StaticPages.GetAllByCategoryIdAsync(id);
            if (pages.Any())
            {
                TempData["Error"] = "Cannot delete category because it contains pages. Please remove or reassign the pages first.";
                return RedirectToAction(nameof(Index));
            }

            await _unitOfWork.Categories.DeleteAsync(category);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CategoryExists(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            return category != null;
        }

        private string GenerateSlug(string title)
        {
            // Remove special characters
            string slug = Regex.Replace(title.ToLower(), @"[^a-z0-9\s-]", "");
            // Replace spaces with hyphens
            slug = Regex.Replace(slug, @"\s+", "-");
            // Remove multiple hyphens
            slug = Regex.Replace(slug, @"-+", "-");
            // Trim hyphens from beginning and end
            slug = slug.Trim('-');
            return slug;
        }
    }
}
