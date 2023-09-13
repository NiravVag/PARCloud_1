////using Microsoft.Extensions.Logging;
////using Par.CommandCenter.Notifications.Services;
////using Quartz;
////using System;
////using System.Linq;
////using System.Threading;
////using System.Threading.Tasks;

////namespace Par.CommandCenter.Notifications.Jobs
////{
////    public class ReminderNotificationsJob : IJob
////    {

////        private readonly IVPNNotificationService _vpnNotificationService;

////        ////private readonly IRouterNotificationService _routerNotificationService;

////        private readonly ICommunicationService _communicationService;

////        private readonly ILogger<ReminderNotificationsJob> _logger;


////        public ReminderNotificationsJob(
////            IVPNNotificationService vpnNotificationService,
////            ////IRouterNotificationService routerNotificationService,
////             ICommunicationService communicationService,
////              ILogger<ReminderNotificationsJob> logger)
////        {

////            _vpnNotificationService = vpnNotificationService;
////            ////_routerNotificationService = routerNotificationService;

////            _communicationService = communicationService;

////            _logger = logger;
////        }

////        public async Task Execute(IJobExecutionContext context)
////        {
////#pragma warning disable CS0168 // The variable 'ex' is declared but never used
////            try
////            {
////                var notifications = Enumerable.Empty<Notification>();
////                var vpnNotifications = await _vpnNotificationService.GetReminderVPNNotifications().ConfigureAwait(false);
////                if (vpnNotifications?.Any() ?? false)
////                {
////                    notifications = notifications.Concat(vpnNotifications);
////                }

////                ////var routerNotifications = await _routerNotificationService.GetRouterNotifications(true).ConfigureAwait(false);

////                ////if (routerNotifications?.Any() ?? false)
////                ////{
////                ////    notifications = notifications.Concat(routerNotifications);
////                ////}

////                if (notifications != null && notifications.Any())
////                {
////                    //https://aka.ms/concurrent_sending
////                    // There is a max limit on the number of emails that can be sent concurantly for office 365 emails.
////                    // Max limit of concurrent emails is 3.
////                    var sendMessageTasksChunks = notifications
////                        .Select(x => _communicationService.SendMessage(x))
////                    .Select((x, i) => new { Index = i, Value = x })
////                    .GroupBy(x => x.Index / 3)
////                    .Select(x => x.Select(v => v.Value));

////                    foreach (var chunk in sendMessageTasksChunks)
////                    {
////                        var allTasks = Task.WhenAll(chunk);
////                        allTasks.Wait();


////                        if (allTasks.IsCompleted)
////                        {
////                            Thread.Sleep(5000);
////                        }
////                    }

////                }

////                _logger.LogDebug("END Send Notifications");

////            }
////            catch (Exception ex)
////            {

////                throw;
////            }
////#pragma warning restore CS0168 // The variable 'ex' is declared but never used
////        }
////    }
////}
