using Microsoft.EntityFrameworkCore;


namespace Par.CommandCenter.Identity.Configurations
{
    public static class DBConfigurations
    {
        public static void ApplyConfiguration(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());

            builder.ApplyConfiguration(new TenantConfiguration());

            builder.ApplyConfiguration(new UserApplicationTenantSettingConfiguration());
        }
    }
}
