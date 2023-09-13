using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.Interfaces;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class OrderEventOutputOrderConfiguration : IEntityTypeConfiguration<OrderEventOutputOrder>
    {
        public void Configure(EntityTypeBuilder<OrderEventOutputOrder> builder)
        {
            builder.ToTable("OrderEventOutputOrder");

            builder.HasKey(x => x.Id);
        }
    }
}
