using MCMS.Infrastructure.Models;
using MCMS.Infrastructure.Repositories.Interfaces;
using MCMS.Web.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MCMS.Web.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubjectsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Subjects
        public async Task<IActionResult> Index()
        {
            var subjects = await _unitOfWork.Subjects.GetAllAsync();
            return View(subjects);
        }

        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _unitOfWork.Subjects.GetSubjectWithCourseAsync(id.Value);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        public async Task<IActionResult> Create()
        {
            var courses = await _unitOfWork.Courses.GetAllAsync();
            var departments = await _unitOfWork.Departments.GetAllAsync();
            ViewData["CourseId"] = new SelectList(courses, "CourseId", "CourseName");
            ViewData["DepartmentId"] = new SelectList(departments, "DepartmentId", "DepartmentName");
            return View();
        }

        // POST: Subjects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubjectName,Slug,CourseId,DepartmentId,Credits,Semester")] Subject subject)
        {
            subject.Slug = StringUtils.GenerateSlug(subject.SubjectName);
            if (ModelState.IsValid)
            {
                await _unitOfWork.Subjects.AddAsync(subject);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            var courses = await _unitOfWork.Courses.GetAllAsync();
            var departments = await _unitOfWork.Departments.GetAllAsync();
            ViewData["CourseId"] = new SelectList(courses, "CourseId", "CourseName", subject.CourseId);
            ViewData["DepartmentId"] = new SelectList(departments, "DepartmentId", "DepartmentName", subject.DepartmentId);
            return View(subject);
        }

        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _unitOfWork.Subjects.GetByIdAsync(id.Value);
            if (subject == null)
            {
                return NotFound();
            }
            var courses = await _unitOfWork.Courses.GetAllAsync();
            var departments = await _unitOfWork.Departments.GetAllAsync();
            ViewData["CourseId"] = new SelectList(courses, "CourseId", "CourseName", subject.CourseId);
            ViewData["DepartmentId"] = new SelectList(departments, "DepartmentId", "DepartmentName", subject.DepartmentId);
            return View(subject);
        }

        // POST: Subjects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubjectId,SubjectName,Slug,CourseId,DepartmentId,Credits,Semester")] Subject subject)
        {
            if (id != subject.SubjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _unitOfWork.Subjects.UpdateAsync(subject);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            var courses = await _unitOfWork.Courses.GetAllAsync();
            var departments = await _unitOfWork.Departments.GetAllAsync();
            ViewData["CourseId"] = new SelectList(courses, "CourseId", "CourseName", subject.CourseId);
            ViewData["DepartmentId"] = new SelectList(departments, "DepartmentId", "DepartmentName", subject.DepartmentId);
            return View(subject);
        }

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _unitOfWork.Subjects.GetSubjectWithCourseAsync(id.Value);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _unitOfWork.Subjects.GetByIdAsync(id);
            if (subject != null)
            {
                await _unitOfWork.Subjects.DeleteAsync(subject);
                await _unitOfWork.CompleteAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Subjects/Restore/5
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _unitOfWork.Subjects.RestoreSubjectAsync(id.Value);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
