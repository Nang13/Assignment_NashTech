using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PES.Domain.Entities.Model;

namespace PES.Infrastructure.Data.FluentAPI
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
           builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}