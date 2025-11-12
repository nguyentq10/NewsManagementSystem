using Repositories.Context;
using Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        
        private readonly NewsManagementDBContext _context;

       
        private AccountRepository _systemAccountRepository;
        private NewsArticleRepository _newsArticleRepository;
        private CategoryRepository _categoryRepository;
        private TagRepository _tagRepository;

     
        public UnitOfWork() => _context = new NewsManagementDBContext();

      
        public AccountRepository AccountRepository
        {
            get { return _systemAccountRepository ??= new AccountRepository(_context); }
        }

        public NewsArticleRepository NewsArticleRepository
        {
            get { return _newsArticleRepository ??= new NewsArticleRepository(_context); }
        }

        public CategoryRepository CategoryRepository
        {
            get { return _categoryRepository ??= new CategoryRepository(_context); }
        }

        public TagRepository TagRepository
        {
            get { return _tagRepository ??= new TagRepository(_context); }
        }
        public int SaveChangesWithTransaction()
        {
            int result = -1;

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    result = _context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }
            return result;
        }

        public async Task<int> SaveChangesWithTransactionAsync()
        {
            int result = -1;

            using (var dbContextTransaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    result = await _context.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();
                }
                catch (Exception)
                {
                    result = -1;
                    await dbContextTransaction.RollbackAsync();
                }
            }
            return result;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
