using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;
using PES.Application.IService;
using PES.Infrastructure.Data;
using PES.Infrastructure.IRepository;
using PES.Infrastructure.UnitOfWork;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PES.Domain.Tests
{
    public class SetUpTest : IDisposable
    {

        protected readonly Fixture _fixture;
        protected readonly Mock<IUnitOfWork> _unitOfWorkMock;
        protected readonly Mock<ICategoryService> _categoryServiceMock;
        protected readonly Mock<IClaimsService> _claimServiceMock;
        protected readonly Mock<IOrderService> _orderServiceMock;
        protected readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        protected readonly Mock<IProductService> _productServiceMock;

        protected readonly Mock<IUserService> _userServiceMock; 
        protected readonly Mock<ICartService> _cartServiceMock;
        protected readonly Mock<IDatabase> _redisMock;
       // protected readonly Mock<IUnitOfWorkRepository> _
        protected readonly PlantManagementContext _context;

        public SetUpTest()
        {
          
            _userServiceMock = new Mock<IUserService>();
            _cartServiceMock = new Mock<ICartService>();
            _fixture = new Fixture();
            _orderServiceMock = new Mock<IOrderService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _categoryServiceMock = new Mock<ICategoryService>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _claimServiceMock = new Mock<IClaimsService>();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _productServiceMock = new Mock<IProductService> ();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            var options = new DbContextOptionsBuilder<PlantManagementContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

            _context = new PlantManagementContext(options);
            _redisMock = new Mock<IDatabase> ();

        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
