using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class InventoryEventHandlerLocationConfiguration : IEntityTypeConfiguration<InventoryEventHandlerLocation>
    {
        public void Configure(EntityTypeBuilder<InventoryEventHandlerLocation> builder)
        {
            builder.ToTable("InventoryEventHandlerLocation");

            builder.HasKey(x => x.Id);
        }
    }
}
