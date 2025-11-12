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
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository(NewsManagementDBContext context) : base(context) { }

        public async Task<Category> GetTrackedByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            return await _context.Categories.AsNoTracking()
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    CategoryDescription = c.CategoryDescription,
                    ParentCategoryID = c.ParentCategoryId,
                    IsActive = c.IsActive
                }).ToListAsync();
        }

        public async Task<CategoryDto> GetCategoryDtoByIdAsync(int id)
        {
            return await _context.Categories.AsNoTracking()
                .Where(c => c.CategoryId == id)
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    CategoryDescription = c.CategoryDescription,
                    ParentCategoryID = c.ParentCategoryId,
                    IsActive = c.IsActive
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CheckIfCategoryHasNewsAsync(int categoryId)
        {
            return await _context.NewsArticles.AnyAsync(n => n.CategoryId == categoryId);
        }
    }
}
