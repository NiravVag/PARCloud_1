using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.AuditLog;

namespace Par.CommandCenter.Persistence.Configurations.AuditLog
{
    public class Controller_AuditLogConfiguration : IEntityTypeConfiguration<Controller_AuditLog>
    {
        public void Configure(EntityTypeBuilder<Controller_AuditLog> builder)
        {
            builder.ToTable("Controller_AuditLog");

            builder.HasKey(x => x.Id);
        }
    }
}
