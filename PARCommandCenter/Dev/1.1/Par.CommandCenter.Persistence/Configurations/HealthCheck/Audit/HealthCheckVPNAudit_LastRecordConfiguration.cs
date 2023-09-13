using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.HealthCheck;

namespace Par.CommandCenter.Persistence.Configurations.HealthCheck
{
    public class HealthCheckVPNAudit_LastRecordConfiguration : IEntityTypeConfiguration<HealthCheckVPNAudit_LastRecord>
    {
        public void Configure(EntityTypeBuilder<HealthCheckVPNAudit_LastRecord> builder)
        {
            builder.ToTable("HealthCheckVPN_Audit_LastRecord");

            builder.HasKey(x => x.Audit_HealthCheckVPNId);
        }
    }
}
