using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace PES.Application
{
    public  static class DependencyInjection
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services) { 
            return services;
        }
    }
}