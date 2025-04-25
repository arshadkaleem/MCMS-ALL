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
    public class FacultyRepository : Repository<Faculty>, IFacultyRepository
    {
        public FacultyRepository(College365DbContext context) : base(context)
        {
        }

        public async Task<Faculty> GetFacultyWithDepartmentAsync(int facultyId)
        {
            return await _context.Faculties
                .Include(f => f.Department)
                .FirstOrDefaultAsync(f => f.FacultyId == facultyId && !f.IsDeleted) ?? throw new InvalidOperationException("Faculty not found");
        }

        public async Task<IReadOnlyList<Faculty>> GetFacultyByDepartmentAsync(int departmentId)
        {
            return await _context.Faculties
                .Where(f => f.DepartmentId == departmentId && !f.IsDeleted)
                .ToListAsync();
        }

        public async Task<Faculty> GetFacultyWithEducationHistoryAsync(int facultyId)
        {
            return await _context.Faculties
                .Include(f => f.FacultyEducations)
                .FirstOrDefaultAsync(f => f.FacultyId == facultyId && !f.IsDeleted) ?? throw new InvalidOperationException("Faculty not found");
        }

        public async Task<Faculty> GetFacultyWithResearchAsync(int facultyId)
        {
            return await _context.Faculties
                .Include(f => f.FacultyResearches)
                .FirstOrDefaultAsync(f => f.FacultyId == facultyId && !f.IsDeleted) ?? throw new InvalidOperationException("Faculty not found");
        }

        public async Task<Faculty> GetFacultyWithSubjectsAsync(int facultyId)
        {
            return await _context.Faculties
                .Include(f => f.FacultySubjects)
                .ThenInclude(fs => fs.Subject)
                .FirstOrDefaultAsync(f => f.FacultyId == facultyId && !f.IsDeleted) ?? throw new InvalidOperationException("Faculty not found");
        }

        public async Task<bool> RestoreFacultyAsync(int facultyId)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC [dbo].[sp_RestoreFaculty] @FacultyId",
                new Microsoft.Data.SqlClient.SqlParameter("@FacultyId", facultyId));
            return true;
        }

        public async Task<IEnumerable<Faculty>> GetAllFacultiesAsync()
        {
            return await _context.Faculties.Include(f => f.Department).OrderBy(d=> d.Department.DepartmentName).ToListAsync();
        }
    }
}
