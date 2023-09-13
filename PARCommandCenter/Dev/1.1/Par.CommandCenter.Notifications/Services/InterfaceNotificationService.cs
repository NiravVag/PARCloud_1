using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities.Email;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using Par.CommandCenter.Notifications.Services.Email;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Par.CommandCenter.Notifications.Services
{
    public interface IInterfaceNotificationService
    {
        Task<IEnumerable<Notification>> GetInterfaceNotifications();
    }

    public class InterfaceNotificationService : IInterfaceNotificationService
    {
        private readonly IEmailConfiguration _emailConfiguration;

        private readonly IApplicationDbContext _dbContext;

        private readonly ILogger<InterfaceNotificationService> _logger;

        private IHostingEnvironment _hostingEnv;

        public InterfaceNotificationService(IEmailConfiguration emailConfiguration, IApplicationDbContext dbContext, IHostingEnvironment hostingEnv,
            ILogger<InterfaceNotificationService> logger)
        {
            _emailConfiguration = emailConfiguration;

            _dbContext = dbContext;
            _logger = logger;
            _hostingEnv = hostingEnv;
        }

        public async Task<IEnumerable<Notification>> GetInterfaceNotifications()
        {
            _logger.LogDebug("START GetInterfaceNotifications");

            var queryOrder = from hc in _dbContext.HealthCheckOrderInterfaces
                             join oeo in _dbContext.OrderEventOutputs on hc.EventOutputId equals oeo.Id
                             join t in _dbContext.Tenants on oeo.TenantId equals t.Id
                             join es in _dbContext.ExternalSystems on new { Id = oeo.ExternalSystemId, oeo.TenantId } equals new { es.Id, es.TenantId }
                             join nt in _dbContext.TenantApplicationNotificationSettings on t.Id equals nt.TenantId
                             //where hc.Status == "Error" && (hc.NotificationDate == null || hc.NotificationDate < DateTime.UtcNow.AddHours(-24))
                             where !t.Deleted && !nt.Deleted && hc.Status == "Error" && hc.NotificationDate == null
                             select new HealthCheckInterface
                             {
                                 Id = hc.Id,
                                 EventOutputId = hc.EventOutputId,
                                 Status = hc.Status,
                                 Modified = hc.Modified,
                                 TenantId = t.Id,
                                 TenantName = t.Name,
                                 InterfaceType = "Order Interface",
                                 ExternalSystemName = es.Name,
                                 FileName = oeo.FileName,
                                 FileLocation = oeo.FileLocation,
                                 MimeType = oeo.MimeType,
                                 Sent = oeo.Sent,
                                 ErrorMessage = oeo.ErrorMessage,
                                 Published = oeo.Published,
                                 Started = oeo.Started,
                             };

            var queryInventory = from hc in _dbContext.HealthCheckInventoryInterfaces
                                 join ieo in _dbContext.InventoryEventOutputs on hc.EventOutputId equals ieo.Id
                                 join t in _dbContext.Tenants on ieo.TenantId equals t.Id
                                 join es in _dbContext.ExternalSystems on new { Id = ieo.ExternalSystemId, ieo.TenantId } equals new { es.Id, es.TenantId }
                                 join nt in _dbContext.TenantApplicationNotificationSettings on t.Id equals nt.TenantId
                                 //where hc.Status == "Error" && (hc.NotificationDate == null || hc.NotificationDate < DateTime.UtcNow.AddHours(-24))
                                 where !t.Deleted && !nt.Deleted && hc.Status == "Error" && hc.NotificationDate == null
                                 select new HealthCheckInterface
                                 {
                                     Id = hc.Id,
                                     EventOutputId = hc.EventOutputId,
                                     Status = hc.Status,
                                     Modified = hc.Modified,
                                     TenantId = t.Id,
                                     TenantName = t.Name,
                                     InterfaceType = "Inventory Interface",
                                     ExternalSystemName = es.Name,
                                     FileName = ieo.FileName,
                                     FileLocation = ieo.FileLocation,
                                     MimeType = ieo.MimeType,
                                     Sent = ieo.Sent,
                                     ErrorMessage = ieo.ErrorMessage,
                                     Published = ieo.Published,
                                     Started = ieo.Started,
                                 };

            var query = queryOrder.Union(queryInventory);

            var groupedNotifications = (await query
                .ToListAsync()
                .ConfigureAwait(false))
                .GroupBy(g => g.TenantId)
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
                            Address = _emailConfiguration.TechnicalSupportEmailAddress,
                            Name = "PAR Excellence Tech Support"
                        }
                    };
                }

                IList<Notification> notificationsList = new List<Notification>();
                foreach (var group in groupedNotifications)
                {
                    var notificationUrl = $"{_emailConfiguration.CommandCenterURL}/dashboard?type=interfaces&outagesonly=true&ids={new StringBuilder().AppendJoin(",", group.OrderBy(x => x.Id).Select(x => x.Id).Distinct())}";

                    if (notificationUrl.Length > 2000)
                    {
                        notificationUrl = $"{_emailConfiguration.CommandCenterURL}/dashboard?type=interfaces&outagesonly=true";
                    }

                    var notifications = group.Select(x => x);
                    if (notifications.Any())
                    {
                        var first = notifications.FirstOrDefault();
                        var notification = new Notification()
                        {
                            Emails = toAddresses,
                            TemplateName = "_InterfaceEmailTemplate",
                            EmailSubject = $"Par Command Customers Interface and Integration Errors Notification",
                            DisplayName = $"{first.TenantName} - Interface and Itegration Errors",
                            Description = $"The following customers interface transactions are having issues.",
                            ReferenceURL = notificationUrl,
                            HealthCheckInterfaces = notifications,
                            Tenant = new Domain.Model.Tenant()
                            {
                                Id = first.TenantId,
                                Name = first.TenantName,
                            }
                        };

                        notificationsList.Add(notification);
                    }


                }

                return notificationsList;
            }

            _logger.LogDebug("END GetInterfaceNotifications");

            return null;
        }
    }
}
