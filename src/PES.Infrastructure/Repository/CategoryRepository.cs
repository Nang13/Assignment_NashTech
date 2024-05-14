using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    }
}