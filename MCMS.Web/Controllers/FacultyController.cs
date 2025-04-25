using Microsoft.AspNetCore.Mvc;
using MCMS.Infrastructure.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using MCMS.Infrastructure.Repositories.Interfaces;
using MCMS.Web.Models.Dtos;
using MCMS.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace MCMS.Web.Controllers
{
    public class FacultyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        public FacultyController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // GET: Faculty
        public async Task<IActionResult> Index()
        {
            var faculty = await _unitOfWork.Faculty.GetAllFacultiesAsync();
            return View(faculty);
        }

        // GET: Faculty/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = await _unitOfWork.Faculty.GetFacultyWithDepartmentAsync(id.Value);
            if (faculty == null)
            {
                return NotFound();
            }

            return View(faculty);
        }

        // GET: Faculty/Create
        [Authorize(Roles = "Admin, DepartmentHOD")]
        public async Task<IActionResult> Create()
        {
            var departments = await _unitOfWork.Departments.GetAllAsync();
            ViewData["DepartmentId"] = new SelectList(departments.OrderBy(d => d.DepartmentName), "DepartmentId", "DepartmentName");
            return View();
        }

        // POST: Faculty/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, DepartmentHOD")]
        public async Task<IActionResult> Create(FacultyCreateDto model)
        {
            if (ModelState.IsValid)
            {
                // Begin transaction
                await _unitOfWork.BeginTransactionAsync();

                try
                {
                    // 1. Create the identity user
                    var user = new IdentityUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        EmailConfirmed = true
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        // 2. Assign the "Faculty" role to the user
                        await _userManager.AddToRoleAsync(user, "Faculty");

                        if (User.IsInRole("Admin"))
                        {
                            // User is an admin, can add hod
                            await _userManager.AddToRoleAsync(user, "DepartmentHOD");
                        }
                        if (User.IsInRole("DepartmentHOD"))
                        {
                            var departmentHOD = await _unitOfWork.Faculty.GetAllAsync();
                            var currentUser = await _userManager.GetUserAsync(User);
                            model.DepartmentId = departmentHOD.FirstOrDefault(d => d.UserId == currentUser.Id).DepartmentId;
                        }


                        // 3. Create the faculty record with only required fields
                        var faculty = new Faculty
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            Post = model.Post,
                            DepartmentId = model.DepartmentId,
                            JoiningDate = DateOnly.FromDateTime(model.JoiningDate), // Convert DateTime to DateOnly model.JoiningDate,
                            IsHod = model.IsHOD,
                            CreatedAt = DateTime.Now,
                            UserId = user.Id,
                            IsDeleted = false
                        };

                        await _unitOfWork.Faculty.AddAsync(faculty);
                        await _unitOfWork.CompleteAsync(); // Save to get the Faculty ID

                        // Handle HOD assignment if needed
                        if (model.IsHOD)
                        {
                            var department = await _unitOfWork.Departments.GetByIdAsync(model.DepartmentId);

                            // If there's another HOD for this department, remove their HOD status
                            if (department.Hodid.HasValue)
                            {
                                var existingHod = await _unitOfWork.Faculty.GetByIdAsync(department.Hodid.Value);
                                if (existingHod != null)
                                {
                                    existingHod.IsHod = false;
                                    await _unitOfWork.Faculty.UpdateAsync(existingHod);
                                }
                            }

                            // Update the department with the new HOD
                            department.Hodid = faculty.FacultyId;
                            department.UpdatedAt = DateTime.Now;
                            await _unitOfWork.Departments.UpdateAsync(department);
                            await _unitOfWork.CompleteAsync();
                        }

                        await _unitOfWork.CommitTransactionAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        await _unitOfWork.RollbackTransactionAsync();
                    }
                }
                catch (Exception ex)
                {
                    var departmentsV = await _unitOfWork.Departments.GetAllAsync();
                    ViewData["DepartmentId"] = new SelectList(departmentsV.OrderBy(d => d.DepartmentName), "DepartmentId", "DepartmentName");
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the faculty member.");
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                    await _unitOfWork.RollbackTransactionAsync();
                }
            }

            // If we got this far, something failed; reload the form
            var departments = await _unitOfWork.Departments.GetAllAsync();
            model.Departments = new SelectList(departments, "DepartmentId", "DepartmentName", model.DepartmentId);
            return View(model);
        }



       
        // GET: Faculty/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = await _unitOfWork.Faculty.GetByIdAsync(id.Value);
            if (faculty == null)
            {
                return NotFound();
            }
            var departments = await _unitOfWork.Departments.GetAllAsync();
            ViewData["DepartmentId"] = new SelectList(departments, "DepartmentId", "DepartmentName", faculty.DepartmentId);
            return View(faculty);
        }

        // POST: Faculty/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FacultyId,FirstName,LastName,Email,PhoneNumber,Post,DepartmentId,Address,City,State,ZipCode,JoiningDate,IsHOD")] Faculty faculty)
        {
            if (id != faculty.FacultyId)
            {
                return NotFound();
            }

            ModelState.Remove("Department");

            if (ModelState.IsValid)
            {
                await _unitOfWork.Faculty.UpdateAsync(faculty);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            var departments = await _unitOfWork.Departments.GetAllAsync();
            ViewData["DepartmentId"] = new SelectList(departments, "DepartmentId", "DepartmentName", faculty.DepartmentId);
            return View(faculty);
        }

        // GET: Faculty/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = await _unitOfWork.Faculty.GetFacultyWithDepartmentAsync(id.Value);
            if (faculty == null)
            {
                return NotFound();
            }

            return View(faculty);
        }

        // POST: Faculty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faculty = await _unitOfWork.Faculty.GetByIdAsync(id);
            if (faculty != null)
            {
                await _unitOfWork.Faculty.DeleteAsync(faculty);
                await _unitOfWork.CompleteAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Faculty/Restore/5
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _unitOfWork.Faculty.RestoreFacultyAsync(id.Value);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}