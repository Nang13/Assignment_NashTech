using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PES.Infrastructure.IRepository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task ExcuteUpdate(Guid id ,Dictionary<string, object?> updateObject);
    }
}