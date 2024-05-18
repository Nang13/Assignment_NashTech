using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PES.Infrastructure.Data.FluentAPI
{
    public class ImportantInfoConfiguration : IEntityTypeConfiguration<ImportantInformation>
    {
        public void Configure(EntityTypeBuilder<ImportantInformation> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Product).WithOne(x => x.ImportantInformation).HasForeignKey<ImportantInformation>(x => x.ProductId);
        }
    }
}
