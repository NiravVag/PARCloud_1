using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.Product;

namespace Par.CommandCenter.Persistence.Configurations.Product
{
    public class ProductUnspscCommodityConfiguration : IEntityTypeConfiguration<ProductUNSPSCCommodity>
    {
        public void Configure(EntityTypeBuilder<ProductUNSPSCCommodity> builder)
        {
            builder.ToTable("ProductUNSPSCCommodity");

            builder.HasKey(x => x.Id);
        }
    }

}