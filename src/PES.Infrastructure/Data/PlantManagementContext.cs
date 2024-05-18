using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PES.Domain.Entities.Model;
using PES.Infrastructure.Data.Config;


namespace PES.Infrastructure.Data
{
    public class PlantManagementContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public PlantManagementContext(DbContextOptions<PlantManagementContext> options) : base(options) { }

        public DbSet<ProductRating> ProductRatings => Set<ProductRating>();

        public DbSet<ProductImage> ProductImages => Set<ProductImage>();

        public DbSet<Product> Products => Set<Product>();

        public DbSet<Order> Orders => Set<Order>();

        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();

        public DbSet<Category> Categories => Set<Category>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}