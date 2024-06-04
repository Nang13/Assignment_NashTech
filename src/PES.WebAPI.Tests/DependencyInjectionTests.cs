using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PES.Application;
using PES.Infrastructure;
using PES.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PES.Presentation.Tests
{
    public class DependencyInjectionTests
    {
        private readonly ServiceProvider _serviceProvider;
        public DependencyInjectionTests()
        {
            var builder = WebApplication.CreateBuilder();
            var service = new ServiceCollection();
            service.AddApplicationService();
            service.AddInfrastructureServices("mock");
            service.AddInfrastructureService(builder);
            service.AddDbContext<PlantManagementContext>(
                option => option.UseInMemoryDatabase("test"));
            _serviceProvider = service.BuildServiceProvider();
        }
    }
}
