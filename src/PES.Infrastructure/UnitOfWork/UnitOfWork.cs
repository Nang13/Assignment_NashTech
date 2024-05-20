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
        private readonly INutrionInfoRepository _nutrionInfoRepository;
        private readonly IImportantInfoRepository _importantInfoRepository;

        private readonly IProductImageRepository _productImageRepository;

        private readonly IOrderRepository _orderRepository;

        private readonly IOrderDetailRepository _orderDetailRepository;
        public UnitOfWork(PlantManagementContext context,
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        IProductRatingRepository productRatingRepository,
        IProductImageRepository productImageRepository,
        IOrderRepository orderRepository,
        IOrderDetailRepository orderDetailRepository,
        INutrionInfoRepository nutrionInfoRepository,
        IImportantInfoRepository importantInfoRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _productRatingRepository = productRatingRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _nutrionInfoRepository = nutrionInfoRepository;
            _importantInfoRepository = importantInfoRepository;
        }
        public ICategoryRepository CategoryRepository => _categoryRepository;

        public IProductRepository ProductRepository => _productRepository;

        public IProductRatingRepository ProductRatingRepository => _productRatingRepository;


        public IProductImageRepository ProductImageRepository => _productImageRepository;

        public IOrderRepository OrderRepository => _orderRepository;

        public IOrderDetailRepository OrderDetailRepository => _orderDetailRepository;

        public INutrionInfoRepository NutrionInfoRepository => _nutrionInfoRepository;

        public IImportantInfoRepository ImportantInfoRepository => _importantInfoRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}