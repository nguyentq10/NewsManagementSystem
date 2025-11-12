using Repositories.DTO;
using Repositories.UnitOfWork;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class NewsArticleService : INewsArticleService
    {
        public async Task<List<NewsArticleSummaryDto>> GetAllActiveNewsAsync()
        {
            using (var uow = new UnitOfWork())
            {
                return await uow.NewsArticleRepository.GetActiveNewsSummariesAsync();
            }
        }
        public async Task<NewsArticleDetailDto> GetActiveNewsByIdAsync(int id)
        {
            using (var uow = new UnitOfWork())
            {
                return await uow.NewsArticleRepository.GetActiveNewsDetailByIdAsync(id);
            }
        }
    }
}
