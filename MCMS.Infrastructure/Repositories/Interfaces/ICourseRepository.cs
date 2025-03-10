using MCMS.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCMS.Infrastructure.Repositories.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<Course> GetCourseWithSubjectsAsync(int courseId);
        Task<IReadOnlyList<Course>> GetCoursesByDepartmentAsync(int departmentId);
        Task<bool> RestoreCourseAsync(int courseId);
    }
}
