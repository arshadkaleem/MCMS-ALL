using MCMS.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCMS.Infrastructure.Repositories.Interfaces
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<Department> GetDepartmentWithFacultyAsync(int departmentId);
        Task<Department> GetDepartmentWithCoursesAsync(int departmentId);
        Task<bool> RestoreDepartmentAsync(int departmentId);
    }
}
