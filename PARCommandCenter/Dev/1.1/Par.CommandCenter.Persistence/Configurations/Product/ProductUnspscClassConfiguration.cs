using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.Product;

namespace Par.CommandCenter.Persistence.Configurations.Product
{

    public class ProductUnspscClassConfiguration : IEntityTypeConfiguration<ProductUNSPSCClass>
    {
        public void Configure(EntityTypeBuilder<ProductUNSPSCClass> builder)
        {
            builder.ToTable("ProductUNSPSCClass");

            builder.HasKey(x => x.Id);
        }
    }
}
