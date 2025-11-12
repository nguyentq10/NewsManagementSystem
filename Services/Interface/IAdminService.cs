using Repositories;
using Repositories.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IAdminService
    {
        Task<List<AccountDTO>> GetAccountsAsync(string searchTerm);
        Task<AccountDTO> GetAccountByIdAsync(int id);
        Task<(AccountDTO, string)> CreateAccountAsync(AccountCreateDto dto);
        Task<(AccountDTO, string)> UpdateAccountAsync(int id, AccountUpdateDto dto);
        Task<(bool, string)> DeleteAccountAsync(int id);
        Task<List<NewsStatisticDto>> GetStatisticsReportAsync(DateTime startDate, DateTime endDate);
    }
}
