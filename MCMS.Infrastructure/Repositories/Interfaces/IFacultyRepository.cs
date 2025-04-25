using MCMS.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCMS.Infrastructure.Repositories.Interfaces
{
    public interface IFacultyRepository : IRepository<Faculty>
    {
        Task<IEnumerable<Faculty>> GetAllFacultiesAsync();
        Task<Faculty> GetFacultyWithDepartmentAsync(int facultyId);
        Task<IReadOnlyList<Faculty>> GetFacultyByDepartmentAsync(int departmentId);
        Task<Faculty> GetFacultyWithEducationHistoryAsync(int facultyId);
        Task<Faculty> GetFacultyWithResearchAsync(int facultyId);
        Task<Faculty> GetFacultyWithSubjectsAsync(int facultyId);
        Task<bool> RestoreFacultyAsync(int facultyId);
    }
}
