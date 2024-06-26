using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PES.Infrastructure.IRepository
{
    public interface IProductImageRepository : IGenericRepository<ProductImage>
    {
     
        public ValueTask DeleteRange(List<ProductImage> images);
    }
}