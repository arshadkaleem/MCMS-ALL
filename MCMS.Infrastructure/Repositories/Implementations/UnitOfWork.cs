using MCMS.Infrastructure.Data;
using MCMS.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCMS.Infrastructure.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly College365DbContext _context;
        private IDbContextTransaction _transaction;


        private IDepartmentRepository _departmentRepository;
        private IFacultyRepository _facultyRepository;
        private ICourseRepository _courseRepository;
        private ISubjectRepository _subjectRepository;
        private ICategoryRepository _categoryRepository;
        private IStaticPageRepository _staticPageRepository;
        private INavigationMenuRepository _navigationMenuRepository;  
        private INavigationItemRepository _navigationItemRepository;  
        private bool disposed = false;

        public UnitOfWork(College365DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IDepartmentRepository Departments =>
            _departmentRepository ??= new DepartmentRepository(_context);

        public IFacultyRepository Faculty =>
            _facultyRepository ??= new FacultyRepository(_context);

        public ICourseRepository Courses =>
            _courseRepository ??= new CourseRepository(_context);

        public ISubjectRepository Subjects =>
            _subjectRepository ??= new SubjectRepository(_context);

        // static repository
        public ICategoryRepository Categories =>  
            _categoryRepository ??= new CategoryRepository(_context);
        public IStaticPageRepository StaticPages =>  
            _staticPageRepository ??= new StaticPageRepository(_context);


        public INavigationMenuRepository NavigationMenus =>
       _navigationMenuRepository ??= new NavigationMenuRepository(_context);

        public INavigationItemRepository NavigationItems =>
            _navigationItemRepository ??= new NavigationItemRepository(_context);


        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }


        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _transaction.CommitAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }



        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
