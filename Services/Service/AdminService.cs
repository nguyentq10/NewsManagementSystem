using Repositories;
using Repositories.DTO;
using Repositories.Models;
using Repositories.UnitOfWork;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class AdminService : IAdminService
    {
        

        public async Task<(AccountDTO, string)> CreateAccountAsync(AccountCreateDto dto)
        {
            using (var uow = new UnitOfWork())
            {
                // 1. Kiểm tra Email
                var existing = await uow.AccountRepository.GetByEmailAsync(dto.AccountEmail);
                if (existing != null)
                {
                    return (null, "This email is already in use.");
                }

                var entity = new SystemAccount
                {
                    AccountName = dto.AccountName,
                    AccountEmail = dto.AccountEmail,
                    AccountPassword = dto.AccountPassword,
                    AccountRole = dto.AccountRole
                };

                // 2. SỬ DỤNG GenericRepository.Add (đã được kế thừa)
                uow.AccountRepository.Add(entity);

                // 3. Save
                await uow.SaveChangesWithTransactionAsync();

                // 4. Map DTO trả về
                var resultDto = new AccountDTO
                {
                    AccountId = entity.AccountId,
                    AccountName = entity.AccountName,
                    AccountEmail = entity.AccountEmail,
                    AccountRole = entity.AccountRole,
                    RoleName = entity.AccountRole == 1 ? "Staff" : "Lecturer"
                };

                return (resultDto, null);
            }
        }

        public async Task<(AccountDTO, string)> UpdateAccountAsync(int id, AccountUpdateDto dto)
        {
            using (var uow = new UnitOfWork())
            {
                var entity = await uow.AccountRepository.GetTrackedByIdAsync(id);
                if (entity == null)
                {
                    return (null, "Account not found.");
                }

                entity.AccountName = dto.AccountName;
                entity.AccountRole = dto.AccountRole;

             
                uow.AccountRepository.Modify(entity);

             
                await uow.SaveChangesWithTransactionAsync();

                var resultDto = await uow.AccountRepository.GetAccountDetailByIdAsync(id);
                return (resultDto, null);
            }
        }

        public async Task<(bool, string)> DeleteAccountAsync(int id)
        {
            using (var uow = new UnitOfWork())
            {
              
                bool hasNews = await uow.NewsArticleRepository.CheckIfAccountHasNewsAsync(id);
                if (hasNews)
                {
                    return (false, "Cannot delete this account as it has created news articles.");
                }

               
                var entity = await uow.AccountRepository.GetTrackedByIdAsync(id);
                if (entity == null)
                {
                    return (false, "Account not found.");
                }

           
                uow.AccountRepository.Delete(entity);

                await uow.SaveChangesWithTransactionAsync();

                return (true, null);
            }
        }

        public async Task<List<AccountDTO>> GetAccountsAsync(string searchTerm)
        {
            using (var uow = new UnitOfWork())
            {
                return await uow.AccountRepository.GetAccountsAsync(searchTerm);
            }
        }

        public async Task<AccountDTO> GetAccountByIdAsync(int id)
        {
            using (var uow = new UnitOfWork())
            {
                return await uow.AccountRepository.GetAccountDetailByIdAsync(id);
            }
        }

        public async Task<List<NewsStatisticDto>> GetStatisticsReportAsync(DateTime startDate, DateTime endDate)
        {
            using (var uow = new UnitOfWork())
            {
                return await uow.NewsArticleRepository.GetStatisticsReportAsync(startDate, endDate);
            }
        }
    }
}
