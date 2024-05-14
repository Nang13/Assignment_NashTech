using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PES.Infrastructure.Data;
using PES.Infrastructure.IRepository;

namespace PES.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PlantManagementContext _context;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductRatingRepository _productRatingRepository;

        private readonly IProductInCategoryRepository _productInCategoryRepository;

        private readonly IProductImageRepository _productImageRepository;

        private readonly IOrderRepository _orderRepository;

        private readonly IOrderDetailRepository _orderDetailRepository;
        public UnitOfWork(PlantManagementContext context,
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        IProductRatingRepository productRatingRepository,
        IProductInCategoryRepository productInCategoryRepository,
        IProductImageRepository productImageRepository,
        IOrderRepository orderRepository,
        IOrderDetailRepository orderDetailRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _productImageRepository =productImageRepository;
            _productRatingRepository = productRatingRepository;
            _productInCategoryRepository = productInCategoryRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }
        public ICategoryRepository CategoryRepository => _categoryRepository;

        public IProductRepository ProductRepository => _productRepository;

        public IProductRatingRepository ProductRatingRepository => _productRatingRepository;

        public IProductInCategoryRepository ProductInCategoryRepository => _productInCategoryRepository ;

        public IProductImageRepository ProductImageRepository => _productImageRepository;

        public IOrderRepository OrderRepository => _orderRepository;

        public IOrderDetailRepository OrderDetailRepository => _orderDetailRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}