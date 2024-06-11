using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PES.Domain.DTOs.ProductDTO;
using PES.Domain.Enum;
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

        public async Task<bool> CheckProductRating(string userId, Guid ProductId)
        {
            //? check if user by this product before
            //Save orderDetail have userId 
            if (_context.OrderDetails.Where(x => x.ProductId == ProductId && x.CreatedBy == userId).Any())
            {
                //? if buy it before comment or not 
                if (_context.ProductRatings.Where(x => x.UserId == userId && x.ProductId == ProductId).Any())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }

        }

        public async Task ExcuteUpdate(Guid id, Dictionary<string, object?> updateObject)
        {
        
            _context.Products.Where(x => x.Id == id).ExecuteUpdate(updateObject);
        }

        public async Task<List<ProductsResponse>> GetProductByCategoryId(List<Guid> categoryId)
        {
            var result = await _context.Products.Where(x => categoryId.Contains(x.CategoryId)).Include(x => x.Category).Include(x => x.ProductImages).Include(x => x.ProductRating)
                .Select(x => new ProductsResponse(x.Id, x.ProductName, x.ProductRating.Select(x => x.Rating).Average(), x.Created, x.ProductImages.FirstOrDefault().ImageUrl, x.Category.CategoryMain, x.Category.CategoryName, x.Price, x.Description, x.CategoryId, x.Status, (bool)x.IsDeleted))
                .ToListAsync();
            return result;

        }

        public async Task UpdateProduct(Guid ProductID, int Quantity)
        {
            Product product = await _context.Products.FirstOrDefaultAsync(x => x.Id == ProductID);
            if (product.Quantity == Quantity)
            {
                _context.Products.Where(x => x.Id == ProductID).ExecuteUpdate(x => x.SetProperty(x => x.Quantity, product.Quantity - Quantity));
                _context.Products.Where(x => x.Id == ProductID).ExecuteUpdate(x => x.SetProperty(x => x.Status, ProductState.OutOfStock));
            }else
            {
                _context.Products.Where(x => x.Id == ProductID).ExecuteUpdate(x => x.SetProperty(x => x.Quantity, product.Quantity - Quantity));
            }
        }
    }
}