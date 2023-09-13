using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.Users;

namespace Par.CommandCenter.Persistence.Configurations.Users
{
    public class UserApplicationTenantSettingConfiguration : IEntityTypeConfiguration<UserApplicationTenantSetting>
    {
        public void Configure(EntityTypeBuilder<UserApplicationTenantSetting> builder)
        {
            builder.ToTable("UserApplicationTenantSetting");

            builder.HasKey(x => x.Id);
        }
    }
}
