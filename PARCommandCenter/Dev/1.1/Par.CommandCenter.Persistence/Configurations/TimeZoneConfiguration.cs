using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class TimeZoneConfiguration : IEntityTypeConfiguration<TimeZone>
    {
        public void Configure(EntityTypeBuilder<TimeZone> builder)
        {
            builder.ToTable("TimeZone");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
               .HasColumnName(@"Name");
        }
    }
}
