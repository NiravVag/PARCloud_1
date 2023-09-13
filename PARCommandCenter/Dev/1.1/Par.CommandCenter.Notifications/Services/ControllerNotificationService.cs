using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using Par.CommandCenter.Domain.Entities.Email;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using Par.CommandCenter.Notifications.Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Par.CommandCenter.Notifications.Services
{
    public interface IControllerNotificationService
    {
        Task<IEnumerable<HealthCheckController>> GetResolvedControllerNotifications();

        ////Task<IEnumerable<Notification>> GetControllerNotifications();
    }

    public class ControllerNotificationService : IControllerNotificationService
    {
        private readonly IEmailConfiguration _emailConfiguration;

        private readonly IApplicationDbContext _dbContext;

        private readonly ILogger<ControllerNotificationService> _logger;

        private IHostingEnvironment _hostingEnv;

        public ControllerNotificationService(IEmailConfiguration emailConfiguration, IApplicationDbContext dbContext, IHostingEnvironment hostingEnv, ILogger<ControllerNotificationService> logger)
        {
            _emailConfiguration = emailConfiguration;

            _dbContext = dbContext;

            _logger = logger;
            _hostingEnv = hostingEnv;
        }

        public async Task<IEnumerable<HealthCheckController>> GetResolvedControllerNotifications()
        {
            var query = from hc in _dbContext.HealthCheckControllers
                        join c in _dbContext.Controllers on hc.ControllerId equals c.Id
                        join r in _dbContext.Routers on c.RouterId equals r.Id
                        join t in _dbContext.Tenants on c.TenantId equals t.Id
                        join nt in _dbContext.TenantApplicationNotificationSettings on t.Id equals nt.TenantId
                        where
                           !r.Deleted
                           && !t.Deleted
                           && !nt.Deleted
                           && c.Active == true
                           && hc.TCPTestStatus == "Established"
                           && hc.NotificationDate != null
                        select hc;

            return await query.ToListAsync().ConfigureAwait(false);
        }

        ////public async Task<IEnumerable<Notification>> GetControllerNotifications()
        ////{
        ////    _logger.LogDebug("START GetControllerNotifications");

        ////    ////var timeUtc = DateTime.UtcNow;
        ////    ////TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        ////    ////DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);

        ////    var query1 = from s in _dbContext.Scales
        ////                 join c in _dbContext.Controllers on s.ControllerId equals c.Id
        ////                 join t in _dbContext.Tenants on s.TenantId equals t.Id
        ////                 join r in _dbContext.Routers on c.RouterId equals r.Id
        ////                 join b in _dbContext.Bins on s.BinId equals b.Id into bx
        ////                 from b in bx.DefaultIfEmpty()
        ////                 join li in _dbContext.LocationItems on b.LocationItemId equals li.Id into lix
        ////                 from li in lix.DefaultIfEmpty()
        ////                 join i in _dbContext.Items on li.ItemId equals i.Id into ix
        ////                 from i in ix.DefaultIfEmpty()
        ////                 join l in _dbContext.Locations on li.LocationId equals l.Id into lx
        ////                 from l in lx.DefaultIfEmpty()
        ////                 join f in _dbContext.Facilities on l.FacilityId equals f.Id into fx
        ////                 from f in fx.DefaultIfEmpty()
        ////                 where !s.Deleted && (c.ControllerTypeId == 3 || c.ControllerTypeId == 4)
        ////                 select new Scale
        ////                 {
        ////                     Id = s.Id,
        ////                     ControllerId = s.ControllerId,
        ////                     LastCommunication = s.LastCommunication,
        ////                     Location = l,
        ////                 };


        ////    var query = from hc in _dbContext.HealthCheckControllers
        ////                join c in _dbContext.Controllers on hc.ControllerId equals c.Id
        ////                join r in _dbContext.Routers on c.RouterId equals r.Id
        ////                join t in _dbContext.Tenants on c.TenantId equals t.Id
        ////                join nt in _dbContext.TenantApplicationNotificationSettings on t.Id equals nt.TenantId
        ////                where
        ////                !r.Deleted
        ////                && !t.Deleted
        ////                && !nt.Deleted
        ////                && c.Active == true
        ////                && hc.TCPTestStatus != "Established"
        ////                && hc.NotificationDate == null
        ////                //&& (
        ////                //hc.NotificationDate == null
        ////                //|| (hc.NotificationDate < DateTime.UtcNow.AddHours(-24) && easternTime >= new DateTime(easternTime.Year, easternTime.Month, easternTime.Day, 10, 0, 0))
        ////                //)
        ////                select new HealthCheckController
        ////                {
        ////                    Id = hc.Id,
        ////                    ControllerId = hc.ControllerId,
        ////                    RouterAddress = r.Address,
        ////                    RouterLastReboot = r.LastReboot,
        ////                    RouterLastCommunication = r.LastCommunication,
        ////                    RouterStatus = (r.LastCommunication.DateTime > DateTime.Now.AddHours(-1)) ? "Online" : "Offline",
        ////                    RemoteIpAddress = hc.RemoteIpAddress,
        ////                    RemoteNetworkPort = hc.RemoteNetworkPort,
        ////                    TenantId = t.Id,
        ////                    TenantName = t.Name,
        ////                    TCPTestStatus = hc.TCPTestStatus,
        ////                    PreviousStatus = hc.PreviousStatus,
        ////                    PreviousStatusDate = hc.PreviousStatusDate,
        ////                    RegisteredScaleCount = hc.RegisteredScaleCount,
        ////                    OnlineScaleCount = hc.OnlineScaleCount,
        ////                    OfflineScaleCount = hc.OfflineScaleCount,
        ////                    Created = hc.Created,
        ////                    Modified = hc.Modified,
        ////                    Scales = query1.Where(s => s.ControllerId == c.Id).ToList(),
        ////                };


        ////    var groupedNotifications = (await query
        ////        .ToListAsync()
        ////        .ConfigureAwait(false))
        ////        .GroupBy(g => g.TenantId ?? 0)
        ////        .OrderBy(g => g.Key);

        ////    if (groupedNotifications.Any())
        ////    {
        ////        List<EmailAddress> toAddresses = null;
        ////        if (_hostingEnv.IsDevelopment() || _hostingEnv.IsEnvironment("LocalDevelopment"))
        ////        {
        ////            toAddresses = _emailConfiguration
        ////               .DevelopmentEmailAddressses
        ////               .Split(';')
        ////               .Where(x => x != null && !string.IsNullOrWhiteSpace(x))
        ////               .Select(x => new EmailAddress() { Address = x.Trim(), Name = "Development" })
        ////               .ToList();
        ////        }
        ////        else
        ////        {
        ////            toAddresses = new List<EmailAddress>()
        ////            {
        ////                new EmailAddress()
        ////                {
        ////                    Address = _emailConfiguration.TechnicalSupportEmailAddress,
        ////                    Name = "PAR Excellence Tech Support"
        ////                }
        ////            };
        ////        }

        ////        IList<Notification> notificationsList = new List<Notification>();
        ////        foreach (var group in groupedNotifications)
        ////        {
        ////            var notificationUrl = $"{_emailConfiguration.CommandCenterURL}/dashboard?type=controllers&outagesonly=true&ids={new StringBuilder().AppendJoin(",", group.OrderBy(x => x.Id).Select(x => x.Id).Distinct())}";

        ////            if (notificationUrl.Length > 2000)
        ////            {
        ////                notificationUrl = $"{_emailConfiguration.CommandCenterURL}/dashboard?type=controllers&outagesonly=true";
        ////            }


        ////            var notifications = group.Select(x => x);
        ////            if (notifications.Any())
        ////            {
        ////                var first = notifications.FirstOrDefault();

        ////                var notification = new Notification()
        ////                {
        ////                    Emails = toAddresses,
        ////                    TemplateName = "_CustomerControllerOfflineEmailTemplate",
        ////                    EmailSubject = $"Par Command Customers Controller Offline Notification",
        ////                    DisplayName = $"{first.TenantName} - Customer Controller Offline",
        ////                    Description = $"The following Customers Controllers are offline.",
        ////                    ReferenceURL = notificationUrl,
        ////                    HealthCheckControllers = notifications,
        ////                    Tenant = new Domain.Model.Tenant()
        ////                    {
        ////                        Id = first.TenantId.Value,
        ////                        Name = first.TenantName,
        ////                    }
        ////                };

        ////                notificationsList.Add(notification);
        ////            }

        ////        }

        ////        return notificationsList;
        ////    }


        ////    _logger.LogDebug("END GetControllerNotifications");

        ////    return null;
        ////}
    }
}
