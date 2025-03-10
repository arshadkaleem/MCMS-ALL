using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCMS.Infrastructure.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDepartmentRepository Departments { get; }
        IFacultyRepository Faculty { get; }
        ICourseRepository Courses { get; }
        ISubjectRepository Subjects { get; }

        // static pages repository
        ICategoryRepository Categories { get; }

        IStaticPageRepository StaticPages { get; }

        INavigationMenuRepository NavigationMenus { get; }  // Add this line
        INavigationItemRepository NavigationItems { get; }  // Add this line



        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
