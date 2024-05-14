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
        public ProductImageRepository(PlantManagementContext context) : base(context)
        {
            
        }
    }
}