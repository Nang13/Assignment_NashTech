using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PES.Domain.DTOs.ProductDTO;
using PES.Infrastructure.Common;
using PES.Infrastructure.Data;
using PES.Infrastructure.IRepository;

namespace PES.Infrastructure.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly PlantManagementContext _context;


        public ProductRepository(PlantManagementContext context) : base(context)
        {
            _context = context;
        }

        public async Task ExcuteUpdate(Guid id, Dictionary<string, object?> updateObject)
        {
            _context.Products.Where(x => x.Id == id).ExecuteUpdate(updateObject);
        }

        public async Task<List<ProductsResponse>> GetProductByCategoryId(List<Guid> categoryId)
        {
            var result = await _context.Products.Where(x => categoryId.Contains(x.CategoryId)).Include(x => x.Category).Include(x => x.ProductImages)
                .Select(x => new ProductsResponse(x.Id, x.ProductName, x.Created, x.ProductImages.FirstOrDefault().ImageUrl, x.Category.CategoryMain, x.Category.CategoryName, x.Price, x.Description,x.CategoryId))
                .ToListAsync();
            return result;

        }
    }
}