using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.Product;

namespace Par.CommandCenter.Persistence.Configurations.Product
{
    public class ProductUnspscFamilyConfiguration : IEntityTypeConfiguration<ProductUNSPSCFamily>
    {
        public void Configure(EntityTypeBuilder<ProductUNSPSCFamily> builder)
        {
            builder.ToTable("ProductUNSPSCFamily");

            builder.HasKey(x => x.Id);
        }
    }

}