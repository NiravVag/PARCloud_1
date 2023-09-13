using Microsoft.EntityFrameworkCore;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using Par.CommandCenter.Domain.Entities.Users;
using Par.CommandCenter.Identity.Configurations;



namespace Par.CommandCenter.Identity.Data
{
    public class UserDbContext : DbContext, IUserDbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
            //var connection = (SqlConnection)Database.GetDbConnection();
            //var tokenProv = new AzureServiceTokenProvider();
            //connection.AccessToken = tokenProv.GetAccessTokenAsync("https://database.windows.net/").Result;
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Tenant> Tenants { get; set; }


        public virtual DbSet<UserApplicationTenantSetting> UserApplicationTenantSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("dbo");
            DBConfigurations.ApplyConfiguration(modelBuilder);
        }
    }
}
