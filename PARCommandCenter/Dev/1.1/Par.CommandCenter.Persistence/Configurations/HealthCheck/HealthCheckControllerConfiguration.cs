using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.HealthCheck;

namespace Par.CommandCenter.Persistence.Configurations.HealthCheck
{
    public class HealthCheckControllerConfiguration : IEntityTypeConfiguration<HealthCheckController>
    {
        public void Configure(EntityTypeBuilder<HealthCheckController> builder)
        {
            builder.ToTable("HealthCheckController");


            builder.Property(x => x.TCPTestStatus)
               .HasColumnName(@"Status");

            builder.HasKey(x => x.Id);

            builder.Ignore(x => x.Status);

            builder.Ignore(x => x.TenantId);

            builder.Ignore(x => x.TenantName);

            builder.Ignore(x => x.RouterAddress);
            builder.Ignore(x => x.RouterLastReboot);
            builder.Ignore(x => x.RouterStatus);

            builder.Ignore(x => x.ScalesLocations);

            builder.Ignore(x => x.Scales);
        }
    }
}
