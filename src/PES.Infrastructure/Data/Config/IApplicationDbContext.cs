using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PES.Domain.Entities.Model;

namespace PES.Infrastructure.Data.Config
{
      public interface IApplicationDbContext
    {
        DbSet<ProductRating> ProductRatings { get; }

        DbSet<ProductImage> ProductImages { get; }

        DbSet<Product> Products { get; }

        DbSet<Order> Orders { get; }

        DbSet<OrderDetail> OrderDetails { get; }
        DbSet<Category> Categories { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);




    }
}