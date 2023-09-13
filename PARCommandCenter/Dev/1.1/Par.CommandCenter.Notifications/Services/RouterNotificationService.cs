//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using Par.CommandCenter.Application.Interfaces;
//using Par.CommandCenter.Domain.Entities.Email;
//using Par.CommandCenter.Domain.Entities.HealthCheck;
//using Par.CommandCenter.Notifications.Services.Email;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Par.CommandCenter.Notifications.Services
//{
//    public interface IRouterNotificationService
//    {
//        Task<IEnumerable<Notification>> GetRouterNotifications(bool isReminder);
//    }

//    public class RouterNotificationService : IRouterNotificationService
//    {
//        private readonly IEmailConfiguration _emailConfiguration;

//        private readonly IApplicationDbContext _dbContext;

//        private readonly ILogger<RouterNotificationService> _logger;

//        private IHostingEnvironment _hostingEnv;

//        public RouterNotificationService(IEmailConfiguration emailConfiguration, IApplicationDbContext dbContext, IHostingEnvironment hostingEnv, ILogger<RouterNotificationService> logger)
//        {
//            _emailConfiguration = emailConfiguration;
//            _dbContext = dbContext;
//            _logger = logger;
//            _hostingEnv = hostingEnv;
//        }


//        public async Task<IEnumerable<Notification>> GetRouterNotifications(bool isReminder)
//        {
//            _logger.LogDebug("START GetRouterNotifications");

//            var timeUtc = DateTime.UtcNow;
//            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
//            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);

//            var query = from hca in _dbContext.HealthCheckRouterAudit_LastRecords
//                        join hc in _dbContext.HealthCheckRouters on hca.HealthCheckRouterId equals hc.Id
//                        join r in _dbContext.Routers on hc.RouterId equals r.Id
//                        join t in _dbContext.Tenants on r.TenantId equals t.Id
//                        join nt in _dbContext.TenantApplicationNotificationSettings on t.Id equals nt.TenantId
//                        where
//                        !r.Deleted
//                        &&
//                        !t.Deleted
//                        &&
//                        !nt.Deleted
//                        &&
//                        hc.Status == "Offline"
//                        //&& /*(*/hc.NotificationDate == null //|| (hc.NotificationDate < DateTime.UtcNow.AddHours(-24) && easternTime >= new DateTime(easternTime.Year, easternTime.Month, easternTime.Day, 10, 0, 0))) 
//                        select new HealthCheckRouter
//                        {
//                            Id = hc.Id,
//                            RouterId = hc.RouterId,
//                            RouterAdress = r.Address,
//                            TenantId = t.Id,
//                            TenantName = t.Name,
//                            Status = hc.Status,
//                            PreviousStatus = hc.PreviousStatus,
//                            PreviousStatusDate = hc.PreviousStatusDate,
//                            LastCommunication = hc.LastCommunication,
//                            LastReboot = hc.LastReboot,
//                            Created = hc.Created,
//                            Modified = hc.Modified,
//                            NotificationDate = hc.NotificationDate,
//                        };

//            if (isReminder)
//            {
//                query = from hc in query
//                        where (hc.NotificationDate != null && hc.NotificationDate < DateTime.UtcNow.AddHours(-12))
//                        select hc;
//            }
//            else
//            {
//                query = from hc in query
//                        where hc.NotificationDate == null
//                        select hc;
//            }


//            var groupedNotifications = (await query
//                .ToListAsync()
//                .ConfigureAwait(false))
//                .GroupBy(g => g.TenantId)
//                .OrderBy(g => g.Key);



//            if (groupedNotifications.Any())
//            {
//                List<EmailAddress> toAddresses = null;
//                if (_hostingEnv.IsDevelopment() || _hostingEnv.IsEnvironment("LocalDevelopment"))
//                {
//                    toAddresses = _emailConfiguration
//                       .DevelopmentEmailAddressses
//                       .Split(';')
//                       .Where(x => x != null && !string.IsNullOrWhiteSpace(x))
//                       .Select(x => new EmailAddress() { Address = x.Trim(), Name = "Development" })
//                       .ToList();
//                }
//                else
//                {
//                    toAddresses = new List<EmailAddress>()
//                    {
//                        new EmailAddress()
//                        {
//                            Address = _emailConfiguration.TechnicalSupportEmailAddress,
//                            Name = "PAR Excellence Tech Support"
//                        }
//                    };
//                }

//                IList<Notification> notificationsList = new List<Notification>();
//                foreach (var group in groupedNotifications)
//                {
//                    var notificationUrl = $"{_emailConfiguration.CommandCenterURL}/dashboard?type=routers&outagesonly=true&ids={new StringBuilder().AppendJoin(",", group.OrderBy(x => x.Id).Select(x => x.Id).Distinct())}";

//                    if (notificationUrl.Length > 2000)
//                    {
//                        notificationUrl = $"{_emailConfiguration.CommandCenterURL}/dashboard?type=routers&outagesonly=true";
//                    }

//                    var notifications = group.Select(x => x);
//                    if (notifications.Any())
//                    {
//                        var first = notifications.FirstOrDefault();

//                        var notification = new Notification()
//                        {
//                            Emails = toAddresses,
//                            TemplateName = "_CustomerRouterOfflineEmailTemplate",
//                            EmailSubject = $"Par Command Customers Router Offline Notification",
//                            DisplayName = $"{first.TenantName} - Customer Router Offline",
//                            Description = $"The following Customers Routers are offline.",
//                            ReferenceURL = notificationUrl,
//                            HealthCheckRouters = notifications,
//                            Tenant = new Domain.Model.Tenant()
//                            {
//                                Id = first.TenantId,
//                                Name = first.TenantName,
//                            }
//                        };

//                        notificationsList.Add(notification);
//                    }
//                }

//                return notificationsList;
//            }

//            _logger.LogDebug("END GetRouterNotifications");

//            return null;
//        }
//    }
//}
