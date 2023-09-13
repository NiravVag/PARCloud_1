using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.Product;

namespace Par.CommandCenter.Persistence.Configurations.Product
{
    public class ProductUnspscSegmentConfiguration : IEntityTypeConfiguration<ProductUNSPSCSegment>
    {
        public void Configure(EntityTypeBuilder<ProductUNSPSCSegment> builder)
        {
            builder.ToTable("ProductUNSPSCSegment");

            builder.HasKey(x => x.Id);
        }
    }

}