using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class OrderEventOutputConfiguration : IEntityTypeConfiguration<OrderEventOutput>
    {
        public void Configure(EntityTypeBuilder<OrderEventOutput> builder)
        {
            builder.ToTable("OrderEventOutput");

            builder.HasKey(x => x.Id);
        }
    }
}
