using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using productNamepace = Par.CommandCenter.Domain.Entities.Product;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<productNamepace.Product> 
    {
        public void Configure(EntityTypeBuilder<productNamepace.Product> builder)
        {
            builder.ToTable("Product");

            builder.HasKey(x => x.Id);

            builder.Ignore(x => x.Tenants);

            builder.Ignore(x => x.Items);

            builder.Ignore(x => x.Manufacturer);
        }
    }
}