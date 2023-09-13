using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.ToTable("Tenant");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
               .HasColumnName(@"Name");

            builder.Property(x => x.Acronym)
              .HasColumnName(@"Acronym");

            builder.Property(x => x.DefaultTimeZoneId)
              .HasColumnName(@"DefaultTimeZoneId");

            builder.Property(x => x.OrderBoxPercentage)
             .HasColumnName(@"OrderBoxPercentage");

            builder.Property(x => x.EmployeeSecurityTypeId)
             .HasColumnName(@"EmployeeSecurityTypeId");

            builder.Property(x => x.IssueAdjustments)
             .HasColumnName(@"IssueAdjustments");

            builder.Property(x => x.ParMobileAllowRememberMe)
             .HasColumnName(@"ParMobileAllowRememberMe");

            builder.Property(x => x.CreatedUserId)
              .HasColumnName(@"CreatedUserId");

            builder.Property(x => x.Deleted)
               .HasColumnName(@"Deleted");

            builder.Property(x => x.IsTest)
               .HasColumnName(@"IsTest");
        }
    }
}
