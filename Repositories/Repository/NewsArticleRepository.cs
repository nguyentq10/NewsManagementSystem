using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using Repositories.Context;
using Repositories.DTO;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class NewsArticleRepository : GenericRepository<NewsArticle>
    {
        public NewsArticleRepository(NewsManagementDBContext context) : base(context)
        {
        }

        public async Task<List<NewsArticleSummaryDto>> GetActiveNewsSummariesAsync()
        {
            return await _context.NewsArticles
                .AsNoTracking()
                .Where(n => n.NewsStatus == true) 
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .Select(n => new NewsArticleSummaryDto
                {
                    NewsArticleId = n.NewsArticleId,
                    NewsTitle = n.NewsTitle,
                    Headline = n.Headline,
                    CreatedDate = n.CreatedDate,
                    NewsSource = n.NewsSource,
                    CategoryName = n.Category.CategoryName,
                    Tags = n.Tags.Select(t => t.TagName).ToList()
                })
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }
        public async Task<NewsArticle> GetTrackedByIdAsync(int id)
        {
            
            return await _context.NewsArticles
                .Include(n => n.Tags)
                .FirstOrDefaultAsync(n => n.NewsArticleId == id);
        }
        public async Task<NewsArticleDetailDto> GetActiveNewsDetailByIdAsync(int id)
        {
            return await _context.NewsArticles
                .AsNoTracking()
                .Where(n => n.NewsArticleId == id && n.NewsStatus == true) 
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .Select(n => new NewsArticleDetailDto
                {
                    NewsArticleId = n.NewsArticleId,
                    NewsTitle = n.NewsTitle,
                    Headline = n.Headline,
                    NewsContent = n.NewsContent,
                    NewsSource = n.NewsSource,
                    CreatedDate = n.CreatedDate,
                    CategoryName = n.Category.CategoryName,
                    Tags = n.Tags.Select(t => t.TagName).ToList()
                })
                .FirstOrDefaultAsync();
        }
        public async Task<bool> CheckIfAccountHasNewsAsync(int accountId)
        {
            return await _context.NewsArticles.AnyAsync(n => n.CreatedById == accountId);
        }
        public async Task<List<NewsStatisticDto>> GetStatisticsReportAsync(DateTime startDate, DateTime endDate)
        {
            
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            startDate = startDate.Date;

            return await _context.NewsArticles
                .AsNoTracking()
                .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
                .Include(n => n.Category) 
                .Select(n => new NewsStatisticDto
                {
                    NewsArticleId = n.NewsArticleId,
                    NewsTitle = n.NewsTitle,
                    CreatedDate = n.CreatedDate,
                    CreatedByAccountName = n.CreatedBy.AccountName,
                    CategoryName = n.Category.CategoryName
                })
                .OrderByDescending(n => n.CreatedDate) 
                .ToListAsync();
        }
        public async Task<List<NewsArticleDto>> GetNewsAsync(int staffId = 0)
        {
           
            IQueryable<NewsArticle> query = _context.NewsArticles.AsNoTracking();

          
            if (staffId > 0)
            {
             
                query = query.Where(n => n.CreatedById == staffId);
            }
            return await query
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .OrderByDescending(n => n.CreatedDate)
                .Select(n => new NewsArticleDto
                {
                    NewsArticleId = n.NewsArticleId,
                    NewsTitle = n.NewsTitle,
                    Headline = n.Headline,
                    CreatedDate = n.CreatedDate,
                    NewsStatus = n.NewsStatus,
                    CategoryName = n.Category.CategoryName,
                    CreatedBy = n.CreatedBy.AccountName,
                    Tags = n.Tags.Select(t => t.TagName).ToList()
                }).ToListAsync();
        }
        public async Task<NewsArticleDto> GetNewsDetailDtoAsync(int id)
        {
            return await _context.NewsArticles.AsNoTracking()
                .Where(n => n.NewsArticleId == id)
                .Include(n => n.Category)
                .Include(n => n.CreatedBy) // SỬA: Dùng 'CreatedBy'
                .Include(n => n.Tags)     // SỬA: Dùng 'Tags'
                .Select(n => new NewsArticleDto
                {
                    NewsArticleId = n.NewsArticleId,
                    NewsTitle = n.NewsTitle,
                    Headline = n.Headline,
                    CreatedDate = n.CreatedDate,
                    NewsStatus = n.NewsStatus,
                    CategoryName = n.Category.CategoryName,
                    CreatedBy = n.CreatedBy.AccountName, // SỬA: Dùng 'CreatedBy'
                    // SỬA: Select TagName trực tiếp từ collection 'Tags'
                    Tags = n.Tags.Select(t => t.TagName).ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}
