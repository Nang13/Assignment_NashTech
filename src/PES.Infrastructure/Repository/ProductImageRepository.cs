using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PES.Infrastructure.Data;
using PES.Infrastructure.IRepository;

namespace PES.Infrastructure.Repository
{
    public class ProductImageRepository : GenericRepository<ProductImage> , IProductImageRepository
    {
        private readonly PlantManagementContext _context;
        public ProductImageRepository(PlantManagementContext context) : base(context)
        {
            _context = context;
        }

        public async ValueTask DeleteRange(List<ProductImage> images)
        {
            _context.ProductImages.RemoveRange(images);
            await _context.SaveChangesAsync();
        }
    }
}