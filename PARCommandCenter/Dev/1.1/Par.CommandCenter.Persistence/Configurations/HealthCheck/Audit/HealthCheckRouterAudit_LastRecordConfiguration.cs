using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.HealthCheck;

namespace Par.CommandCenter.Persistence.Configurations.HealthCheck
{
    public class HealthCheckRouterAudit_LastRecordConfiguration : IEntityTypeConfiguration<HealthCheckRouterAudit_LastRecord>
    {
        public void Configure(EntityTypeBuilder<HealthCheckRouterAudit_LastRecord> builder)
        {
            builder.ToTable("HealthCheckRouter_Audit_LastRecord");

            builder.HasKey(x => x.Audit_HealthCheckRouterId);
        }
    }
}
