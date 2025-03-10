using MCMS.Infrastructure.Models;
using MCMS.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;

namespace MCMS.Web.Controllers
{
    public class StaticPagesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StaticPagesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: StaticPages
        public async Task<IActionResult> Index()
        {
            var pages = await _unitOfWork.StaticPages.GetAllActiveAsync();
            return View(pages);
        }

        // GET: StaticPages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _unitOfWork.StaticPages.GetWithCategoryAsync(id.Value);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        // GET: StaticPages/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: StaticPages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StaticPage page)
        {
            // Always regenerate the slug from the title
            page.Slug = GenerateSlug(page.Title);

            // Check if slug exists
            if (await _unitOfWork.StaticPages.SlugExistsAsync(page.Slug))
            {
                ModelState.AddModelError("Slug", "This slug is already in use. Please choose another one.");
            }

            // Check if CategoryId is valid
            if (page.CategoryId <= 0)
            {
                ModelState.AddModelError("CategoryId", "Please select a category.");
            }
            else
            {
                // If CategoryId is valid, clear any model errors for Category
                ModelState.Remove("Category");
            }

            if (ModelState.IsValid)
            {
                page.CreatedAt = DateTime.Now;
                page.UpdatedAt = DateTime.Now;
                page.IsDeleted = false;

                await _unitOfWork.StaticPages.AddAsync(page);
                await _unitOfWork.CompleteAsync();

                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed, so redisplay form
            var categories = await _unitOfWork.Categories.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName", page.CategoryId);

            return View(page);
        }

        // GET: StaticPages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _unitOfWork.StaticPages.GetByIdAsync(id.Value);
            if (page == null)
            {
                return NotFound();
            }

            var categories = await _unitOfWork.Categories.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName", page.CategoryId);
            return View(page);
        }

        // POST: StaticPages/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PageId,Title,Slug,PageContent,MetaTitle,MetaDescription,Published,CreatedAt,CategoryId,IsDeleted")] StaticPage page)
        {
            if (id != page.PageId)
            {
                return NotFound();
            }

            // Check if CategoryId is valid
            if (page.CategoryId <= 0)
            {
                ModelState.AddModelError("CategoryId", "Please select a category.");
            }
            else
            {
                // If CategoryId is valid, clear any model errors for Category
                ModelState.Remove("Category");
            }

            if (string.IsNullOrEmpty(page.Slug))
            {
                page.Slug = GenerateSlug(page.Title);
            }

            if (await _unitOfWork.StaticPages.SlugExistsExceptAsync(page.Slug, page.PageId))
            {
                ModelState.AddModelError("Slug", "This slug is already in use. Please choose another one.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    page.UpdatedAt = DateTime.Now;
                    await _unitOfWork.StaticPages.UpdateAsync(page);
                    await _unitOfWork.CompleteAsync();
                }
                catch (Exception)
                {
                    if (!await PageExists(page.PageId))
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

            var categories = await _unitOfWork.Categories.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName", page.CategoryId);
            return View(page);
        }

        // GET: StaticPages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _unitOfWork.StaticPages.GetWithCategoryAsync(id.Value);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        // POST: StaticPages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.StaticPages.SoftDeleteAsync(id);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: StaticPages/Restore/5
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _unitOfWork.StaticPages.RestoreAsync(id.Value);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: StaticPages/Preview/my-page
        public async Task<IActionResult> Preview(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var page = await _unitOfWork.StaticPages.GetBySlugAsync(slug);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        // GET: StaticPages/Page/my-page
        [Route("page/{slug}")]
        public async Task<IActionResult> Page(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var page = await _unitOfWork.StaticPages.GetBySlugAsync(slug);
            if (page == null || !page.Published)
            {
                return NotFound();
            }

            return View(page);
        }

        private async Task<bool> PageExists(int id)
        {
            var page = await _unitOfWork.StaticPages.GetByIdAsync(id);
            return page != null;
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
