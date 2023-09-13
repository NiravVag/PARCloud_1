using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.HealthCheck;

namespace Par.CommandCenter.Persistence.Configurations.HealthCheck
{
    public class HealthCheckRouterConfiguration : IEntityTypeConfiguration<HealthCheckRouter>
    {
        public void Configure(EntityTypeBuilder<HealthCheckRouter> builder)
        {
            builder.ToTable("HealthCheckRouter");

            builder.HasKey(x => x.Id);

            builder.Ignore(x => x.RouterAdress);

            builder.Ignore(x => x.TenantId);

            builder.Ignore(x => x.TenantName);
        }
    }
}
