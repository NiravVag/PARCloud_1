using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.Interfaces;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class InventoryEventOutputTransactionConfiguration : IEntityTypeConfiguration<InventoryEventOutputTransaction>
    {
        public void Configure(EntityTypeBuilder<InventoryEventOutputTransaction> builder)
        {
            builder.ToTable("InventoryEventOutputTransaction");

            builder.HasKey(x => x.Id);
        }
    }
}
