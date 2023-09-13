using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class InventoryEventTypeConfiguration : IEntityTypeConfiguration<InventoryEventType>
    {
        public void Configure(EntityTypeBuilder<InventoryEventType> builder)
        {
            builder.ToTable("InventoryEventType");

            builder.HasKey(x => x.Id);
        }
    }
}
