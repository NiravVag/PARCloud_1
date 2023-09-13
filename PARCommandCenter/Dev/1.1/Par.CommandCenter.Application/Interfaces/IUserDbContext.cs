using Microsoft.EntityFrameworkCore;
using Par.CommandCenter.Domain.Entities;
using Par.CommandCenter.Domain.Entities.Users;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Interfaces
{
    public interface IUserDbContext
    {
        DbSet<User> Users { get; set; }

        DbSet<Tenant> Tenants { get; set; }

        DbSet<UserApplicationTenantSetting> UserApplicationTenantSettings { get; set; }

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
