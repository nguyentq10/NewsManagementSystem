using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Repository;  
namespace Repositories.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        AccountRepository AccountRepository { get; }
        NewsArticleRepository NewsArticleRepository { get; }
        CategoryRepository CategoryRepository { get; }
        TagRepository TagRepository { get; }

       
        int SaveChangesWithTransaction();
        Task<int> SaveChangesWithTransactionAsync();
    }

}
