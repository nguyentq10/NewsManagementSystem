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
    public class StaffService : IStaffService
    {
        // === PROFILE ===

        public async Task<AccountDTO> GetProfileAsync(int staffId)
        {
            using (var uow = new UnitOfWork())
            {
                return await uow.AccountRepository.GetAccountDetailByIdAsync(staffId);
            }
        }

        public async Task<(AccountDTO, string)> UpdateProfileAsync(int staffId, ProfileUpdateDto dto)
        {
            using (var uow = new UnitOfWork())
            {
                var entity = await uow.AccountRepository.GetTrackedByIdAsync(staffId);
                if (entity == null)
                {
                    return (null, "Account not found.");
                }

                entity.AccountName = dto.AccountName;

                uow.AccountRepository.Modify(entity); 
                await uow.SaveChangesWithTransactionAsync();

                var resultDto = await uow.AccountRepository.GetAccountDetailByIdAsync(staffId);
                return (resultDto, null);
            }
        }

        // === CATEGORY ===

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            using (var uow = new UnitOfWork())
            {
                return await uow.CategoryRepository.GetAllCategoriesAsync();
            }
        }

        public async Task<(CategoryDto, string)> CreateCategoryAsync(CategoryCreateUpdateDto dto)
        {
            using (var uow = new UnitOfWork())
            {
                var entity = new Category
                {
                    CategoryName = dto.CategoryName,
                    CategoryDescription = dto.CategoryDescription,
                    ParentCategoryId = dto.ParentCategoryID,
                    IsActive = dto.IsActive
                };

                uow.CategoryRepository.Add(entity); 
                await uow.SaveChangesWithTransactionAsync();

                var resultDto = await uow.CategoryRepository.GetCategoryDtoByIdAsync(entity.CategoryId);
                return (resultDto, null);
            }
        }

        public async Task<(CategoryDto, string)> UpdateCategoryAsync(int id, CategoryCreateUpdateDto dto)
        {
            using (var uow = new UnitOfWork())
            {
                var entity = await uow.CategoryRepository.GetTrackedByIdAsync(id);
                if (entity == null)
                {
                    return (null, "Category not found.");
                }

                entity.CategoryName = dto.CategoryName;
                entity.CategoryDescription = dto.CategoryDescription;
                entity.ParentCategoryId = dto.ParentCategoryID;
                entity.IsActive = dto.IsActive;

                uow.CategoryRepository.Modify(entity); 
                await uow.SaveChangesWithTransactionAsync();

                var resultDto = await uow.CategoryRepository.GetCategoryDtoByIdAsync(id);
                return (resultDto, null);
            }
        }

        public async Task<(bool, string)> DeleteCategoryAsync(int id)
        {
            using (var uow = new UnitOfWork())
            {
                
                bool hasNews = await uow.CategoryRepository.CheckIfCategoryHasNewsAsync(id);
                if (hasNews)
                {
                    return (false, "Cannot delete category. It is associated with news articles.");
                }

                var entity = await uow.CategoryRepository.GetTrackedByIdAsync(id);
                if (entity == null)
                {
                    return (false, "Category not found.");
                }

                uow.CategoryRepository.Delete(entity); 
                await uow.SaveChangesWithTransactionAsync();
                return (true, null);
            }
        }

        // === NEWS ARTICLE ===

        public async Task<List<NewsArticleDto>> GetNewsAsync(int staffId, bool historyOnly)
        {
            using (var uow = new UnitOfWork())
            {
                
                int idToQuery = historyOnly ? staffId : 0;
                return await uow.NewsArticleRepository.GetNewsAsync(idToQuery);
            }
        }

        public async Task<(NewsArticleDto, string)> CreateNewsAsync(NewsArticleCreateUpdateDto dto, int staffId)
        {
            using (var uow = new UnitOfWork())
            {
                var tagsList = new List<Tag>();
                foreach (var tagName in dto.Tags.Distinct())
                {
                    var tag = await uow.TagRepository.GetOrCreateTagAsync(tagName);
                    tagsList.Add(tag);
                }

                var entity = new NewsArticle
                {
                    NewsTitle = dto.NewsTitle,
                    Headline = dto.Headline,
                    NewsContent = dto.NewsContent,
                    NewsSource = dto.NewsSource,
                    CategoryId = dto.CategoryID,
                    NewsStatus = dto.NewsStatus,
                    CreatedById = staffId, 
                    CreatedDate = DateTime.UtcNow 
                };

             
                foreach (var tag in tagsList)
                {
                    entity.Tags.Add(tag); 
                }

                uow.NewsArticleRepository.Add(entity); 

                await uow.SaveChangesWithTransactionAsync();

                var resultDto = await uow.NewsArticleRepository.GetNewsDetailDtoAsync(entity.NewsArticleId);
                return (resultDto, null);
            }
        }

        public async Task<(NewsArticleDto, string)> UpdateNewsAsync(int id, NewsArticleCreateUpdateDto dto, int staffId)
        {
            using (var uow = new UnitOfWork())
            {
                var entity = await uow.NewsArticleRepository.GetTrackedByIdAsync(id);
                if (entity == null)
                {
                    return (null, "News article not found.");
                }


                var tagsList = new List<Tag>();
                foreach (var tagName in dto.Tags.Distinct())
                {
                    var tag = await uow.TagRepository.GetOrCreateTagAsync(tagName);
                    tagsList.Add(tag);
                }

                entity.NewsTitle = dto.NewsTitle;
                entity.Headline = dto.Headline;
                entity.NewsContent = dto.NewsContent;
                entity.NewsSource = dto.NewsSource;
                entity.CategoryId = dto.CategoryID;
                entity.NewsStatus = dto.NewsStatus;
                entity.UpdatedById = staffId; 
                entity.ModifiedDate = DateTime.UtcNow;

                entity.Tags.Clear(); 
                foreach (var tag in tagsList)
                {
                    entity.Tags.Add(tag); 
                }

                uow.NewsArticleRepository.Modify(entity);

                await uow.SaveChangesWithTransactionAsync();

                var resultDto = await uow.NewsArticleRepository.GetNewsDetailDtoAsync(id);
                return (resultDto, null);
            }
        }

        public async Task<(bool, string)> DeleteNewsAsync(int id)
        {
            using (var uow = new UnitOfWork())
            {
                var entity = await uow.NewsArticleRepository.GetTrackedByIdAsync(id);
                if (entity == null)
                {
                    return (false, "News article not found.");
                }

                uow.NewsArticleRepository.Delete(entity); 
                await uow.SaveChangesWithTransactionAsync();
                return (true, null);
            }
        }
    }
}
