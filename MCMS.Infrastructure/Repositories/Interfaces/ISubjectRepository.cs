using MCMS.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCMS.Infrastructure.Repositories.Interfaces
{
    public interface ISubjectRepository : IRepository<Subject>
    {
        Task<Subject> GetSubjectWithCourseAsync(int subjectId);
        Task<IReadOnlyList<Subject>> GetSubjectsByCourseAsync(int courseId);
        Task<IReadOnlyList<Subject>> GetSubjectsByFacultyAsync(int facultyId);
        Task<IReadOnlyList<Subject>> GetSubjectsBySemesterAsync(string semester);
        Task<bool> RestoreSubjectAsync(int subjectId);
    }
}
