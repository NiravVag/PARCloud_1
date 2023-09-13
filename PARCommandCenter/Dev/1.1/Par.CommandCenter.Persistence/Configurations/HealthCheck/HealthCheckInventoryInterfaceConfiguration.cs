using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.HealthCheck;

namespace Par.CommandCenter.Persistence.Configurations.HealthCheck
{
    public class HealthCheckInventoryInterfaceConfiguration : IEntityTypeConfiguration<HealthCheckInventoryInterface>
    {
        public void Configure(EntityTypeBuilder<HealthCheckInventoryInterface> builder)
        {
            builder.ToTable("HealthCheckInventoryInterface");

            builder.HasKey(x => x.Id);

            builder.Ignore(x => x.TenantId);

            builder.Ignore(x => x.TenantName);

            builder.Ignore(x => x.InterfaceType);

            builder.Ignore(x => x.ExternalSystemName);

            builder.Ignore(x => x.ErrorMessage);

            builder.Ignore(x => x.FileLocation);

            builder.Ignore(x => x.FileName);

            builder.Ignore(x => x.MimeType);

            builder.Ignore(x => x.Published);

            builder.Ignore(x => x.Sent);

            builder.Ignore(x => x.Started);
        }
    }
}
