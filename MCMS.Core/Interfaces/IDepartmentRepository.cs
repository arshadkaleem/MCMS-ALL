using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCMS.Infrastructure.Models;


namespace MCMS.Core.Interfaces
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        // Additional department-specific methods
        Task<Department> GetDepartmentWithFacultyAsync(int departmentId);
        Task<Department> GetDepartmentWithCoursesAsync(int departmentId);
        Task<bool> RestoreDepartmentAsync(int departmentId);  // For handling soft delete
    }
}
