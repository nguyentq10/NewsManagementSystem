using Repositories;
using Repositories.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IStaffService
    {
        // === Profile ===
        Task<AccountDTO> GetProfileAsync(int staffId);
        Task<(AccountDTO, string)> UpdateProfileAsync(int staffId, ProfileUpdateDto dto);

        // === Category ===
        Task<List<CategoryDto>> GetCategoriesAsync();
        Task<(CategoryDto, string)> CreateCategoryAsync(CategoryCreateUpdateDto dto);
        Task<(CategoryDto, string)> UpdateCategoryAsync(int id, CategoryCreateUpdateDto dto);
        Task<(bool, string)> DeleteCategoryAsync(int id);

        // === News Article ===
        Task<List<NewsArticleDto>> GetNewsAsync(int staffId, bool historyOnly);
        Task<(NewsArticleDto, string)> CreateNewsAsync(NewsArticleCreateUpdateDto dto, int staffId);
        Task<(NewsArticleDto, string)> UpdateNewsAsync(int id, NewsArticleCreateUpdateDto dto, int staffId);
        Task<(bool, string)> DeleteNewsAsync(int id);
    }
}
