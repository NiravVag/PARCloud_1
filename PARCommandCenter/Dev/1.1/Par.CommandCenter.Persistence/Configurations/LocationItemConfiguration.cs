using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class LocationItemConfiguration : IEntityTypeConfiguration<LocationItem>
    {
        public void Configure(EntityTypeBuilder<LocationItem> builder)
        {
            builder.ToTable("LocationItem");

            builder.HasKey(x => x.Id);
        }
    }
}
