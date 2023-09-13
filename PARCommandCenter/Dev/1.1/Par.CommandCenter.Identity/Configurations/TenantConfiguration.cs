using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Identity.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.ToTable("Tenant");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
               .HasColumnName(@"Name");

            builder.Property(x => x.Deleted)
               .HasColumnName(@"Deleted");
        }
    }
}
