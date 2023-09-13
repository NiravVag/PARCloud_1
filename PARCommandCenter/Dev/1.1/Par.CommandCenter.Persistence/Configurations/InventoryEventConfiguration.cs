using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;
using Par.CommandCenter.Domain.Entities.Interfaces;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class InventoryEventConfiguration : IEntityTypeConfiguration<InventoryEvent>
    {
        public void Configure(EntityTypeBuilder<InventoryEvent> builder)
        {
            builder.ToTable("InventoryEvent");

            builder.HasKey(x => x.Id);
        }
    }
}
