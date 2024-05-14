using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PES.Domain.Entities.Model;

namespace PES.Infrastructure.Data.FluentAPI
{
    public class ProductRatingConfiguration : IEntityTypeConfiguration<ProductRating>
    {
        public void Configure(EntityTypeBuilder<ProductRating> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Product).WithOne(x => x.ProductRating).HasForeignKey<ProductRating>(x => x.ProductId);
        }

    }
}