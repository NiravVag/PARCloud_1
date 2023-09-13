using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.HealthCheck;

namespace Par.CommandCenter.Persistence.Configurations.HealthCheck
{
    public class HealthCheckServerOperationConfiguration : IEntityTypeConfiguration<HealthCheckServerOperation>
    {
        public void Configure(EntityTypeBuilder<HealthCheckServerOperation> builder)
        {
            builder.ToTable("HealthCheckServerOperation");

            builder.HasKey(x => x.Id);
        }
    }
}
