using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class FacilityConfiguration : IEntityTypeConfiguration<Facility>
    {
        public void Configure(EntityTypeBuilder<Facility> builder)
        {
            builder.ToTable("Facility");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.TenantId)
               .HasColumnName(@"TenantId");

            builder.Property(x => x.Name)
               .HasColumnName(@"Name");

            builder.Property(x => x.TimeZoneId)
              .HasColumnName(@"TimeZoneId");

            builder.Property(x => x.VPNConnectionName)
              .HasColumnName(@"VPNConnectionName");

            builder.Property(x => x.AddressLine1)
              .HasColumnName(@"AddressLine1");

            builder.Property(x => x.City)
              .HasColumnName(@"City");

            builder.Property(x => x.StateId)
              .HasColumnName(@"StateId");


            builder.Property(x => x.PostalCode)
              .HasColumnName(@"PostalCode");

            builder.Property(x => x.CreatedUserId)
              .HasColumnName(@"CreatedUserId");

            builder.Property(x => x.Created)
              .HasColumnName(@"Created");

            builder.Property(x => x.ModifiedUserId)
              .HasColumnName(@"ModifiedUserId");

            builder.Property(x => x.Modified)
               .HasColumnName(@"Modified");

            builder.Property(x => x.Deleted)
              .HasColumnName(@"Deleted");
        }
    }
}
