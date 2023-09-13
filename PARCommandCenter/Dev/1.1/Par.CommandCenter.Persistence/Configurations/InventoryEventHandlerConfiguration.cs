using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class InventoryEventHandlerConfiguration : IEntityTypeConfiguration<InventoryEventHandler>
    {
        public void Configure(EntityTypeBuilder<InventoryEventHandler> builder)
        {
            builder.ToTable("InventoryEventHandler");

            builder.HasKey(x => x.Id);
        }
    }
}
