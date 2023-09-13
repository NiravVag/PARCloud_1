using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.HealthCheck;

namespace Par.CommandCenter.Persistence.Configurations.HealthCheck
{
    public class InteractiveMapHealthCheckConfiguration : IEntityTypeConfiguration<MapHealthCheckPoint>
    {
        public void Configure(EntityTypeBuilder<MapHealthCheckPoint> builder)
        {
            builder.ToTable("InteractiveMapHealthCheckData");

            builder.HasKey(x => x.Id);
        }
    }
}
