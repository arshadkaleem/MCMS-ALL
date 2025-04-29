using Microsoft.AspNetCore.Mvc;
using MCMS.Infrastructure.Models;   
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using MCMS.Infrastructure.Repositories.Interfaces;
using MCMS.Web.Utils;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoursesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var courses = await _unitOfWork.Courses.GetAllAsync();
            return View(courses);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _unitOfWork.Courses.GetCourseWithSubjectsAsync(id.Value);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public async Task<IActionResult> Create()
        {
            var departments = await _unitOfWork.Departments.GetAllAsync();
            ViewData["DepartmentId"] = new SelectList(departments.OrderBy(d => d.DepartmentName), "DepartmentId", "DepartmentName");
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseName,Slug,DepartmentId,CollegeType,DurationYears,Credits")] Course course)
        {
            course.Slug = StringUtils.GenerateSlug(course.CourseName);
            course.IsDeleted = false;
            course.CreatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {                
                await _unitOfWork.Courses.AddAsync(course);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            var departments = await _unitOfWork.Departments.GetAllAsync();
            ViewData["DepartmentId"] = new SelectList(departments, "DepartmentId", "DepartmentName", course.DepartmentId);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _unitOfWork.Courses.GetByIdAsync(id.Value);
            if (course == null)
            {
                return NotFound();
            }
            var departments = await _unitOfWork.Departments.GetAllAsync();
            ViewData["DepartmentId"] = new SelectList(departments, "DepartmentId", "DepartmentName", course.DepartmentId);
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,CourseName,Slug,DepartmentId,CollegeType,DurationYears,Credits")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _unitOfWork.Courses.UpdateAsync(course);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            var departments = await _unitOfWork.Departments.GetAllAsync();
            ViewData["DepartmentId"] = new SelectList(departments, "DepartmentId", "DepartmentName", course.DepartmentId);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _unitOfWork.Courses.GetByIdAsync(id.Value);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(id);
            if (course != null)
            {
                await _unitOfWork.Courses.DeleteAsync(course);
                await _unitOfWork.CompleteAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Courses/Restore/5
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _unitOfWork.Courses.RestoreCourseAsync(id.Value);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}