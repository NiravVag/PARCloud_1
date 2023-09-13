using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.HealthCheck;

namespace Par.CommandCenter.Persistence.Configurations.HealthCheck
{
    public class HealthCheckVPNConfiguration : IEntityTypeConfiguration<HealthCheckVPN>
    {
        public void Configure(EntityTypeBuilder<HealthCheckVPN> builder)
        {
            builder.ToTable("HealthCheckVPN");

            builder.HasKey(x => x.Id);

            builder.Ignore(x => x.Tenant);

            builder.Ignore(x => x.facilities);
        }
    }
}
