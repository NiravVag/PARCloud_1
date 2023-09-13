using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities.Email;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using Par.CommandCenter.Notifications.Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Par.CommandCenter.Notifications.Services
{
    public interface IVPNNotificationService
    {
        Task<IEnumerable<Notification>> GetReminderVPNNotifications();

        Task<IEnumerable<Notification>> GetVPNNotifications();
    }

    public class VPNNotificationService : IVPNNotificationService
    {
        private readonly IEmailConfiguration _emailConfiguration;

        private readonly IApplicationDbContext _dbContext;


        private readonly ILogger<VPNNotificationService> _logger;

        private IHostingEnvironment _hostingEnv;

        public VPNNotificationService(IEmailConfiguration emailConfiguration, IApplicationDbContext dbContext, IHostingEnvironment hostingEnv, ILogger<VPNNotificationService> logger)
        {
            _emailConfiguration = emailConfiguration;

            _dbContext = dbContext;
            _logger = logger;
            _hostingEnv = hostingEnv;
        }



        public async Task<IEnumerable<Notification>> GetReminderVPNNotifications()
        {
            try
            {
                _logger.LogDebug("START GetReminderVPNNotifications");

                var query = from hcl in _dbContext.HealthCheckVPNAudit_LastRecords
                            join hc in _dbContext.HealthCheckVPNs on hcl.HealthCheckVPNId equals hc.Id
                            join s in _dbContext.TenantApplicationNotificationSettings on hcl.TenantId equals s.TenantId into tsx
                            from s in tsx.DefaultIfEmpty()
                            join t in _dbContext.Tenants on hc.TenantId equals t.Id into tx
                            from t in tx.DefaultIfEmpty()
                            where
                            (
                                hc.TenantId == null
                                && hcl.Status == "NotConnected"
                                && (hc.NotificationDate != null && hc.NotificationDate < DateTime.UtcNow.AddHours(-12))

                            )
                            ||
                            (
                                !s.Deleted
                                && hcl.Status == "NotConnected"
                                && (hc.NotificationDate != null && hc.NotificationDate < DateTime.UtcNow.AddHours(-12))
                            )
                            select new HealthCheckVPN()
                            {
                                Id = hc.Id,
                                ConnectionName = hc.ConnectionName,
                                Created = hc.Created,
                                Modified = hc.Modified,
                                NotificationDate = hc.NotificationDate,
                                PreviousStatus = hc.PreviousStatus,
                                PreviousStatusDate = hc.PreviousStatusDate,
                                Status = hc.Status,
                                TenantId = hc.TenantId,
                                Tenant = (t == null) ? null : new Domain.Entities.Tenant()
                                {
                                    Id = t.Id,
                                    Name = t.Name,
                                }
                            };



                return await GroupTenantNotifications(query).ConfigureAwait(false);

            }
            finally
            {

                _logger.LogDebug("END GetReminderVPNNotifications");
            }
        }

        public async Task<IEnumerable<Notification>> GetVPNNotifications()
        {

            try
            {
                _logger.LogDebug("START GetVPNNotifications");                

                var query = from hcl in _dbContext.HealthCheckVPNAudit_LastRecords
                            join hc in _dbContext.HealthCheckVPNs on hcl.HealthCheckVPNId equals hc.Id
                            join s in _dbContext.TenantApplicationNotificationSettings on hcl.TenantId equals s.TenantId into tsx
                            from s in tsx.DefaultIfEmpty()
                            join t in _dbContext.Tenants on hc.TenantId equals t.Id into tx
                            from t in tx.DefaultIfEmpty()
                            where
                            (
                                hc.TenantId == null
                                && hcl.Status == "NotConnected"
                                && hcl.NotificationDate == null
                            )
                            ||
                            (
                                !s.Deleted
                                && hcl.Status == "NotConnected"
                                && hcl.NotificationDate == null
                            )
                            select new HealthCheckVPN()
                            {
                                Id = hc.Id,
                                ConnectionName = hc.ConnectionName,
                                Created = hc.Created,
                                Modified = hc.Modified,
                                NotificationDate = hc.NotificationDate,
                                PreviousStatus = hc.PreviousStatus,
                                PreviousStatusDate = hc.PreviousStatusDate,
                                Status = hc.Status,
                                TenantId = hc.TenantId,
                                Tenant = (t == null) ? null : new Domain.Entities.Tenant()
                                {
                                    Id = t.Id,
                                    Name = t.Name,
                                }
                            };


                return await GroupTenantNotifications(query).ConfigureAwait(false);
            }
            finally
            {

                _logger.LogDebug("END GetVPNNotifications");
            }
        }

        private async Task<IEnumerable<Notification>> GroupTenantNotifications(IQueryable<HealthCheckVPN> query)
        {
            var groupedNotifications = (await query
                .ToListAsync()
                .ConfigureAwait(false))
                .GroupBy(g => g.TenantId ?? 0)
                .OrderBy(g => g.Key);


            if (groupedNotifications.Any())
            {
                List<EmailAddress> toAddresses = null;
                if (_hostingEnv.IsDevelopment() || _hostingEnv.IsEnvironment("LocalDevelopment"))
                {
                    toAddresses = _emailConfiguration
                       .DevelopmentEmailAddressses
                       .Split(';')
                       .Where(x => x != null && !string.IsNullOrWhiteSpace(x))
                       .Select(x => new EmailAddress() { Address = x.Trim(), Name = "Development" })
                       .ToList();
                }
                else
                {
                    toAddresses = new List<EmailAddress>()
                    {
                        new EmailAddress()
                        {
                            Address = _emailConfiguration.ParITEmailAddress,
                            Name = "PAR Excellence IT"
                        },
                        new EmailAddress()
                        {
                            Address = _emailConfiguration.TechnicalSupportEmailAddress,
                            Name = "PAR Excellence Tech Support"
                        }
                    };
                }


                IList<Notification> notificationsList = new List<Notification>();
                foreach (var group in groupedNotifications)
                {
                    var notificationUrl = $"{_emailConfiguration.CommandCenterURL}/dashboard?type=vpns&outagesonly=true&ids={new StringBuilder().AppendJoin(",", group.OrderBy(x => x.Id).Select(x => x.Id).Distinct())}";
                    if (notificationUrl.Length > 2000)
                    {
                        notificationUrl = $"{_emailConfiguration.CommandCenterURL}/dashboard?type=vpns&outagesonly=true";
                    }

                    var notifications = group.Select(x => x);
                    if (notifications.Any())
                    {
                        var tenant = notifications.FirstOrDefault().Tenant;
                        var notification = new Notification()
                        {
                            Emails = toAddresses,
                            TemplateName = "_CustomerVPNConnectionDown",
                            EmailSubject = $"Par Command Customers VPN Connections Down Notification",
                            DisplayName = (tenant == null) ? "Customer VPN Connections Down" : $"{tenant.Name} VPN Connections Down",
                            Description = $"The following Customers VPN Connections are down.",
                            ReferenceURL = notificationUrl,
                            HealthCheckVPNs = notifications,
                            Tenant = (tenant == null) ? null : new Domain.Model.Tenant()
                            {
                                Id = tenant.Id,
                                Name = tenant.Name,
                            }
                        };

                        notificationsList.Add(notification);

                    }
                }

                return notificationsList;
            }

            return null;
        }
    }
}

