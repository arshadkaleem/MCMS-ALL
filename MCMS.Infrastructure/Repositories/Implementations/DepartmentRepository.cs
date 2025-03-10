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
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        private readonly College365DbContext _dbContext;

        public DepartmentRepository(College365DbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Department> GetDepartmentWithFacultyAsync(int departmentId)
        {
            var department = await _dbContext.Departments
                            .Include(d => d.Faculties)
                            .FirstOrDefaultAsync(d => d.DepartmentId == departmentId && !d.IsDeleted);
            return department ?? throw new InvalidOperationException("Department not found");
        }

        public async Task<Department> GetDepartmentWithCoursesAsync(int departmentId)
        {
            var department = await _dbContext.Departments
                .Include(d => d.Courses)
                .FirstOrDefaultAsync(d => d.DepartmentId == departmentId && !d.IsDeleted);
            return department ?? throw new InvalidOperationException("Department not found");
        }

        public async Task<bool> RestoreDepartmentAsync(int departmentId)
        {
            // Using stored procedure to restore department
            await _dbContext.Database.ExecuteSqlRawAsync("EXEC [dbo].[sp_RestoreDepartment] @DepartmentId",
                new Microsoft.Data.SqlClient.SqlParameter("@DepartmentId", departmentId));
            return true;
        }
    }
}
