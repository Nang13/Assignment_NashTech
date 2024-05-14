using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PES.Domain.Entities.Model;
using PES.Infrastructure.Data;
using PES.Infrastructure.Data.Config;
using PES.Infrastructure.IRepository;
using PES.Infrastructure.Repository;
using PES.Infrastructure.UnitOfWork;


namespace PES.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {

            //? Add UnitOfWork
           // services.AddScoped<IGenericRepository, GenericRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork.UnitOfWork>();
           
           
           
           
            //? ADD DI with Database Set up
            services.AddDbContextPool<PlantManagementContext>(options =>
                options.UseNpgsql(connectionString));
            services.AddScoped<ApplicationDbContextInitializer>();
            services.AddIdentityCore<ApplicationUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<PlantManagementContext>()
                    .AddApiEndpoints();
            services.AddSingleton(TimeProvider.System);
            services.AddDataProtection();
            return services;
        }
    }
}