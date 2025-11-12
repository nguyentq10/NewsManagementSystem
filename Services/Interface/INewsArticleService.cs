using Repositories.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface INewsArticleService
    {
        Task<List<NewsArticleSummaryDto>> GetAllActiveNewsAsync();
        Task<NewsArticleDetailDto> GetActiveNewsByIdAsync(int id);
    }
}
