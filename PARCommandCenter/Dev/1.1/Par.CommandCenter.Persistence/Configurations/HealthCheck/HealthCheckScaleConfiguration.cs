using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.HealthCheck;

namespace Par.CommandCenter.Persistence.Configurations.HealthCheck
{
    public class HealthCheckScaleConfiguration : IEntityTypeConfiguration<HealthCheckScale>
    {
        public void Configure(EntityTypeBuilder<HealthCheckScale> builder)
        {
            builder.ToTable("HealthCheckScale");

            builder.HasKey(x => x.Id);

            builder.Ignore(x => x.ScaleAdress);

            builder.Ignore(x => x.TenantId);

            builder.Ignore(x => x.TenantName);

            builder.Ignore(x => x.BinId);

            builder.Ignore(x => x.Location);

            builder.Ignore(x => x.Item);
        }
    }
}
