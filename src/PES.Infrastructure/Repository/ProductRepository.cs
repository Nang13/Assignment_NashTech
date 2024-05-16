using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PES.Infrastructure.Common;
using PES.Infrastructure.Data;
using PES.Infrastructure.IRepository;

namespace PES.Infrastructure.Repository
{
    public class ProductRepository : GenericRepository<Product> , IProductRepository
    {
        private readonly PlantManagementContext _context;


        public ProductRepository(PlantManagementContext context) : base(context)
        {
            _context = context;
        }

        public   async Task ExcuteUpdate(Guid id, Dictionary<string, object?> updateObject)
        {
            _context.Products.Where(x => x.Id == id).ExecuteUpdate(updateObject);
        }
    }
}