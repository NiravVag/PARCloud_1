using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class ScaleConfiguration : IEntityTypeConfiguration<Scale>
    {
        public void Configure(EntityTypeBuilder<Scale> builder)
        {
            builder.ToTable("Scale");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.TenantId)
               .HasColumnName(@"TenantId");

            builder.Ignore(x => x.Item);
            builder.Ignore(x => x.Location);
        }
    }
}
