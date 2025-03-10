using MCMS.Infrastructure.Data;
using MCMS.Infrastructure.Models;
using MCMS.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCMS.Infrastructure.Repositories.Implementations
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        private readonly College365DbContext _dbContext;

        public CourseRepository(College365DbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<IReadOnlyList<Course>> GetAllAsync()
        {
            return await _dbContext.Courses
                .Include(c => c.Department)
                .Where(c => !c.IsDeleted)
                .ToListAsync() ?? new List<Course>();
        }

        public async Task<Course> GetCourseWithSubjectsAsync(int courseId)
        {
            return await _dbContext.Courses
                .Include(c => c.Subjects)
                .FirstOrDefaultAsync(c => c.CourseId == courseId && !c.IsDeleted) ?? new Course();
        }

        public async Task<IReadOnlyList<Course>> GetCoursesByDepartmentAsync(int departmentId)
        {
            return await _dbContext.Courses
                .Where(c => c.DepartmentId == departmentId && !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<bool> RestoreCourseAsync(int courseId)
        {
            // Using stored procedure to restore course
            await _dbContext.Database.ExecuteSqlRawAsync("EXEC [dbo].[sp_RestoreCourse] @CourseId",
                new Microsoft.Data.SqlClient.SqlParameter("@CourseId", courseId));
            return true;
        }
    }
}
