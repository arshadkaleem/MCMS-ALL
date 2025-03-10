using MCMS.Infrastructure.Models;
using MCMS.Infrastructure.Repositories.Interfaces;
using MCMS.Web.Models;
using MCMS.Web.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MCMS.Web.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.Departments.GetAllAsync();
            return View(departments);
        }


        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _unitOfWork.Departments.GetDepartmentWithFacultyAsync(id.Value);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }


        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentName,Slug,CollegeType")] Department department)
        {
            department.IsDeleted = false;
            department.CreatedAt = DateTime.Now;
            department.Slug = StringUtils.GenerateSlug(department.DepartmentName);

            if (ModelState.IsValid)
            {                
                await _unitOfWork.Departments.AddAsync(department);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }



        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _unitOfWork.Departments.GetByIdAsync(id.Value);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }


        // POST: Departments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DepartmentId,DepartmentName,Slug,CollegeType")] Department department)
        {
            if (id != department.DepartmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _unitOfWork.Departments.UpdateAsync(department);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _unitOfWork.Departments.GetByIdAsync(id.Value);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            if (department != null)
            {
                // This will perform a soft delete since we're using the IsDeleted flag
                await _unitOfWork.Departments.DeleteAsync(department);
                await _unitOfWork.CompleteAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Departments/Restore/5
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _unitOfWork.Departments.RestoreDepartmentAsync(id.Value);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: Departments/AssignHOD/5
        public async Task<IActionResult> AssignHOD(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _unitOfWork.Departments.GetByIdAsync(id.Value);
            if (department == null)
            {
                return NotFound();
            }

            // Get all faculty members in this department who are eligible to be HOD
            var allFaculty = await _unitOfWork.Faculty.GetAllAsync();
            var facultyList = allFaculty
                .Where(f => f.DepartmentId == id.Value && !f.IsDeleted)
                .ToList();

            var viewModel = new AssignHODViewModel
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
                SelectedFacultyId = department.Hodid,
                FacultyList = facultyList.Select(f => new SelectListItem
                {
                    Value = f.FacultyId.ToString(),
                    Text = $"{f.FirstName} {f.LastName}",
                    Selected = department.Hodid.HasValue && department.Hodid == f.FacultyId
                })
            };

            return View(viewModel);
        }

        // POST: Departments/AssignHOD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignHOD(AssignHODViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(viewModel.DepartmentId);
                if (department == null)
                {
                    return NotFound();
                }

                // If there was a previous HOD, remove their HOD status
                if (department.Hodid.HasValue)
                {
                    var previousHOD = await _unitOfWork.Faculty.GetByIdAsync(department.Hodid.Value);
                    if (previousHOD != null)
                    {
                        previousHOD.IsHod = false;
                        await _unitOfWork.Faculty.UpdateAsync(previousHOD);
                    }
                }

                // Update the department with the new HOD
                department.Hodid = viewModel.SelectedFacultyId;
                department.UpdatedAt = DateTime.Now;
                await _unitOfWork.Departments.UpdateAsync(department);

                // Update the faculty member's IsHOD status
                if (viewModel.SelectedFacultyId.HasValue)
                {
                    var newHOD = await _unitOfWork.Faculty.GetByIdAsync(viewModel.SelectedFacultyId.Value);
                    if (newHOD != null)
                    {
                        newHOD.IsHod = true;
                        await _unitOfWork.Faculty.UpdateAsync(newHOD);
                    }
                }

                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed; reload the form
            //var facultyList = await _unitOfWork.Faculty.GetAllAsync(f => f.DepartmentId == viewModel.DepartmentId && !f.IsDeleted);
            var allFaculty = await _unitOfWork.Faculty.GetAllAsync();
            var facultyList = allFaculty
                .Where(f => f.DepartmentId == viewModel.DepartmentId && !f.IsDeleted)
                .ToList();

            viewModel.FacultyList = facultyList.Select(f => new SelectListItem
            {
                Value = f.FacultyId.ToString(),
                Text = $"{f.FirstName} {f.LastName}",
                Selected = viewModel.SelectedFacultyId.HasValue && viewModel.SelectedFacultyId == f.FacultyId
            });

            return View(viewModel);
        }

    }

}

