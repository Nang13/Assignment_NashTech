using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PES.Domain.DTOs.Category;
using PES.Domain.Entities.Model;
using PES.Infrastructure.Data;
using PES.Infrastructure.IRepository;

namespace PES.Infrastructure.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly PlantManagementContext _context;
        public CategoryRepository(PlantManagementContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<CategoryResponse>> GetAllParentCategory()
        {
            var categories = await _context.Categories.ToListAsync();

            // Group by CategoryMain and select the first item from each group
            var groupedCategories = categories
                .Where(x => x.CategoryLeft == 1)
                .GroupBy(c => c.CategoryMain)
                .Select(g => g.First())
                .Select(c => new CategoryResponse
                {
                    CategoryId = c.Id,
                    CategoryName = c.CategoryMain
                }).ToList();

            return groupedCategories;
        }

        public async Task UpdateCategoryParent(Guid categoryId, int rightValue)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);
            _context.Categories.Where(x => x.CategoryMain == category.CategoryMain && x.CategoryRight >= rightValue).ExecuteUpdate(x => x.SetProperty(b => b.CategoryRight, b => b.CategoryRight + 2));
            _context.Categories.Where(x => x.CategoryMain == category.CategoryMain && x.CategoryLeft > rightValue).ExecuteUpdate(x => x.SetProperty(b => b.CategoryLeft, b => b.CategoryRight + 2));
        }

         public async Task DeleteAndUpdateSubCategories(string CategoryMain, int leftValue, int rightValue, int width)
        {
            _context.Categories.Where(x => x.CategoryMain == CategoryMain && x.CategoryLeft >= leftValue && x.CategoryRight <= rightValue).ExecuteDelete();
            //? update position after delete
            _context.Categories.Where(x => x.CategoryMain == CategoryMain && x.CategoryRight > rightValue).ExecuteUpdate(x => x.SetProperty(x => x.CategoryRight, x => x.CategoryRight - width));
            _context.Categories.Where(x => x.CategoryMain == CategoryMain && x.CategoryLeft > rightValue).ExecuteUpdate(x => x.SetProperty(x => x.CategoryLeft, x => x.CategoryLeft - width));

        }
    }
}