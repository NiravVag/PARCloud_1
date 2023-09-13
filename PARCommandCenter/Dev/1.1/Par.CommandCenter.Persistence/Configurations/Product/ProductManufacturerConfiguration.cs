using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.Product;

namespace Par.CommandCenter.Persistence.Configurations.Product
{
    public class ProductManufacturerConfiguration : IEntityTypeConfiguration<ProductManufacturer>
    {
        public void Configure(EntityTypeBuilder<ProductManufacturer> builder)
        {
            builder.ToTable("ProductManufacturer");

            builder.HasKey(x => x.Id);
        }
    }

}