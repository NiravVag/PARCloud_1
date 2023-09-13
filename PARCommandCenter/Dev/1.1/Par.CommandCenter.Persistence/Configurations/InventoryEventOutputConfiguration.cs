using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class InventoryEventOutputConfiguration : IEntityTypeConfiguration<InventoryEventOutput>
    {
        public void Configure(EntityTypeBuilder<InventoryEventOutput> builder)
        {
            builder.ToTable("InventoryEventOutput");

            builder.HasKey(x => x.Id);
        }
    }
}
