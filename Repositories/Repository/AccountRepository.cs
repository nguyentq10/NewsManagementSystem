using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using Repositories.Context;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class AccountRepository : GenericRepository<SystemAccount>
    {
        public AccountRepository(NewsManagementDBContext context) : base(context)
        {
        }

        public async Task<SystemAccount?> GetOne(string email, string password)
        {
            return await _context.SystemAccounts
                .Where(acc => acc.AccountEmail == email && acc.AccountPassword == password)
                .FirstOrDefaultAsync();
        }
        public async Task<List<AccountDTO>> GetAccountsAsync(string searchTerm)
        {
            var query = _context.SystemAccounts.AsNoTracking();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(a => a.AccountName.Contains(searchTerm) ||
                                         a.AccountEmail.Contains(searchTerm));
            }

            return await query.Select(a => new AccountDTO
            {
                AccountId = a.AccountId,
                AccountName = a.AccountName,
                AccountEmail = a.AccountEmail,
                AccountRole = a.AccountRole, 
                RoleName = a.AccountRole == 1 ? "Staff" : "Lecturer"
            }).ToListAsync();
        }
        public async Task<AccountDTO> GetAccountDetailEmailAsync(string email)
        {
            return await _context.SystemAccounts.AsNoTracking()
                .Where(a => a.AccountEmail == email)
                .Select(a => new AccountDTO
                {
                    AccountId = a.AccountId,
                    AccountName = a.AccountName,
                    AccountEmail = a.AccountEmail,
                    AccountRole = a.AccountRole,
                    RoleName = a.AccountRole == 1 ? "Staff" : "Lecturer"
                }).FirstOrDefaultAsync();
        }
        public async Task<AccountDTO> GetAccountDetailByIdAsync(int id)
        {
            return await _context.SystemAccounts.AsNoTracking()
                .Where(a => a.AccountId == id)
                .Select(a => new AccountDTO
                {
                    AccountId = a.AccountId,
                    AccountName = a.AccountName,
                    AccountEmail = a.AccountEmail,
                    AccountRole = a.AccountRole,
                    RoleName = a.AccountRole == 1 ? "Staff" : "Lecturer"
                }).FirstOrDefaultAsync();
        }
        public async Task<SystemAccount> GetTrackedByIdAsync(int id)
        {
            return await _context.SystemAccounts.FindAsync(id);
        }
        public async Task<SystemAccount> GetByEmailAsync(string email)
        {
           
            return await _context.SystemAccounts
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(a => a.AccountEmail == email);
        }
    }
}
