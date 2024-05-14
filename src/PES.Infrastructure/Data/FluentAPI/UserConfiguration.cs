using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PES.Domain.Entities.Model;

namespace PES.Infrastructure.Data.FluentAPI
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // builder.HasOne(x => x.).WithMany(x => x.MediaRecords).HasForeignKey(x => x.MediaTypeId);
             builder.HasMany(x => x.ProductRatings).WithOne(x => x.User).HasForeignKey(x => x.UserId);
             builder.HasMany(x => x.Orders).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        }
    }
}