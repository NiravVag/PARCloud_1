using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class TenantApplicationNotificationSettingConfiguration : IEntityTypeConfiguration<TenantApplicationNotificationSetting>
    {
        public void Configure(EntityTypeBuilder<TenantApplicationNotificationSetting> builder)
        {
            builder.ToTable("TenantApplicationNotificationSetting");

            builder.HasKey(x => x.Id);
        }
    }
}
