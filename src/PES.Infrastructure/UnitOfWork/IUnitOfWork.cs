using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PES.Infrastructure.IRepository;
using PES.Infrastructure.Repository;

namespace PES.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IProductRatingRepository ProductRatingRepository { get; }


        public IProductImageRepository ProductImageRepository { get; }

        public IOrderRepository OrderRepository { get; }

        public IOrderDetailRepository OrderDetailRepository { get; }


        public Task<int> SaveChangeAsync();
    }
}