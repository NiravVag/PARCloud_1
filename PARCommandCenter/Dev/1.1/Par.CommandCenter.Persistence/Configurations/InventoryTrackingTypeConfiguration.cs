using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class InventoryTrackingTypeConfiguration : IEntityTypeConfiguration<InventoryTrackingType>
    {
        public void Configure(EntityTypeBuilder<InventoryTrackingType> builder)
        {
            builder.ToTable("InventoryTrackingType");

            builder.HasKey(x => x.Id);
        }
    }
}
