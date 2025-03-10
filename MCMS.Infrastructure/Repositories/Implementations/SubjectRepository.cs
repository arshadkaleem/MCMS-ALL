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
    public class SubjectRepository : Repository<Subject>, ISubjectRepository
    {
        private readonly College365DbContext _dbContext;

        public SubjectRepository(College365DbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Subject> GetSubjectWithCourseAsync(int subjectId)
        {
            return await _dbContext.Subjects
                .Include(s => s.Course)
                .FirstOrDefaultAsync(s => s.SubjectId == subjectId && !s.IsDeleted) ?? new Subject();
        }

        public async Task<IReadOnlyList<Subject>> GetSubjectsByCourseAsync(int courseId)
        {
            return await _dbContext.Subjects
                .Where(s => s.CourseId == courseId && !s.IsDeleted)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Subject>> GetSubjectsByFacultyAsync(int facultyId)
        {
            return await _dbContext.Subjects
                .Where(s => s.FacultySubjects.Any(fs => fs.FacultyId == facultyId && !fs.IsDeleted) && !s.IsDeleted)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Subject>> GetSubjectsBySemesterAsync(string semester)
        {
            return await _dbContext.Subjects
                .Where(s => s.Semester == semester && !s.IsDeleted)
                .ToListAsync();
        }

        public async Task<bool> RestoreSubjectAsync(int subjectId)
        {
            // Using stored procedure to restore subject
            await _dbContext.Database.ExecuteSqlRawAsync("EXEC [dbo].[sp_RestoreSubject] @SubjectId",
                new Microsoft.Data.SqlClient.SqlParameter("@SubjectId", subjectId));
            return true;
        }
    }
}
