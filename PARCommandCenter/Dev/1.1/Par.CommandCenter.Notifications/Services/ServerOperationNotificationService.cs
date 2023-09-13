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
using System.Threading.Tasks;

namespace Par.CommandCenter.Notifications.Services
{
    public interface IServerOperationNotificationService
    {
        Task<Notification> GetServerOperationNotification();
    }

    public class ServerOperationNotificationService : IServerOperationNotificationService
    {
        private readonly IEmailConfiguration _emailConfiguration;

        private readonly IApplicationDbContext _dbContext;

        private readonly ILogger<ServerOperationNotificationService> _logger;

        private IHostingEnvironment _hostingEnv;

        public ServerOperationNotificationService(IEmailConfiguration emailConfiguration, IApplicationDbContext dbContext, IHostingEnvironment hostingEnv, ILogger<ServerOperationNotificationService> logger)
        {
            _emailConfiguration = emailConfiguration;
            _dbContext = dbContext;
            _logger = logger;
            _hostingEnv = hostingEnv;
        }

        public async Task<Notification> GetServerOperationNotification()
        {
            _logger.LogDebug("START GetServerOperationNotifications");

            var timeUtc = DateTime.UtcNow;
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);

            var query = from hc in _dbContext.HealthCheckServerOperations
                        where
                        hc.Status != "Connected"
                        && (hc.NotificationDate == null || (hc.NotificationDate < DateTime.UtcNow.AddHours(-24) && easternTime >= new DateTime(easternTime.Year, easternTime.Month, easternTime.Day, 10, 0, 0)))
                        select new HealthCheckServerOperation
                        {
                            Id = hc.Id,
                            ServerName = hc.ServerName,
                            Status = hc.Status,
                            HealthCheckMessage = hc.HealthCheckMessage,
                            PreviousStatus = hc.PreviousStatus,
                            PreviousStatusDate = hc.PreviousStatusDate,
                            Created = hc.Created,
                            Modified = hc.Modified,
                        };

            var notifications = await query.ToListAsync();

            if (notifications.Count > 0)
            {
                var notificationUrl = $"{_emailConfiguration.CommandCenterURL}/dashboard?type=serversoperation&outagesonly=true";

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

                var notification =
                        new Notification()
                        {
                            Emails = toAddresses,
                            TemplateName = "_ServerOperationEmailTemplate",
                            EmailSubject = $"Par Command Cloud Services Operation Outage Notification",
                            DisplayName = $"Customer Cloud Services Outage",
                            Description = $"The following cloud services are having issues.",
                            ReferenceURL = notificationUrl,
                            HealthCheckServerOperations = notifications,
                        };


                return notification;
            }

            _logger.LogDebug("END GetServerOperationNotifications");

            return null;
        }
    }
}
