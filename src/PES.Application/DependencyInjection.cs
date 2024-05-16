using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PES.Application.IService;
using PES.Application.Service;

namespace PES.Application
{
    public  static class DependencyInjection
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services) { 
            
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService,  ProductService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();
            
            
            return services;
        }
    }
}