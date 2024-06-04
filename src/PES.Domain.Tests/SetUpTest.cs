using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;
using PES.Application.IService;
using PES.Infrastructure.Data;
using PES.Infrastructure.IRepository;
using PES.Infrastructure.UnitOfWork;
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
        protected readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        protected readonly PlantManagementContext _context;

        public SetUpTest()
        {
            _fixture = new Fixture();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _categoryServiceMock = new Mock<ICategoryService>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _claimServiceMock = new Mock<IClaimsService>();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            var options = new DbContextOptionsBuilder<PlantManagementContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

            _context = new PlantManagementContext(options);

        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
