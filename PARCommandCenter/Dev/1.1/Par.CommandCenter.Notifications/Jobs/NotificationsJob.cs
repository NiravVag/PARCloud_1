using MediatR;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Handlers.HealthChecks.Commands.UpdateNotificationDate;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using Par.CommandCenter.Notifications.Services;
using Quartz;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Notifications.Jobs
{
    public class NotificationsJob : IJob
    {
        private readonly IVPNNotificationService _vpnNotificationService;

        ////private readonly IRouterNotificationService _routerNotificationService;

        private readonly IControllerNotificationService _controllerNotificationService;

        private readonly IServerOperationNotificationService _serverOperationNotificationService;

        private readonly IInterfaceNotificationService _interfaceNotificationService;

        private readonly ICommunicationService _communicationService;

        private readonly ILogger<NotificationsJob> _logger;

        private readonly IMediator _mediator;



        public NotificationsJob(
            IVPNNotificationService vpnNotificationService,
            ////IRouterNotificationService routerNotificationService,
            IControllerNotificationService controllerNotificationService,
            IServerOperationNotificationService serverOperationNotificationService,
            IInterfaceNotificationService interfaceNotificationService,
            ICommunicationService communicationService,
            ILogger<NotificationsJob> logger,
            IMediator Mediator)
        {
            _vpnNotificationService = vpnNotificationService;
            ////_routerNotificationService = routerNotificationService;
            _controllerNotificationService = controllerNotificationService;
            _serverOperationNotificationService = serverOperationNotificationService;
            _interfaceNotificationService = interfaceNotificationService;

            _communicationService = communicationService;

            _logger = logger;

            _mediator = Mediator;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var notifications = Enumerable.Empty<Notification>();


            var vpnNotifications = await _vpnNotificationService.GetVPNNotifications().ConfigureAwait(false);

            if (vpnNotifications?.Any() ?? false)
            {
                notifications = notifications.Concat(vpnNotifications);

                UpdateNotificationDateCommand command = new UpdateNotificationDateCommand()
                {
                    VPNHealthChecks = vpnNotifications.SelectMany(x => x.HealthCheckVPNs).Distinct()

                };

                await _mediator.Send(command).ConfigureAwait(false);
            }

            ////var routerNotifications = await _routerNotificationService.GetRouterNotifications(false).ConfigureAwait(false);

            ////if (routerNotifications?.Any() ?? false)
            ////{
            ////    notifications = notifications.Concat(routerNotifications);

            ////    UpdateNotificationDateCommand command = new UpdateNotificationDateCommand()
            ////    {
            ////        RouterHealthChecks = routerNotifications.SelectMany(x => x.HealthCheckRouters).Distinct()

            ////    };

            ////    var response = await _mediator.Send(command).ConfigureAwait(false);
            ////}


            var resolvedControllerNotification = await _controllerNotificationService.GetResolvedControllerNotifications().ConfigureAwait(false);

            if (resolvedControllerNotification?.Any() ?? false)
            {

                UpdateNotificationDateCommand command = new UpdateNotificationDateCommand()
                {
                    ControllerHealthChecks = resolvedControllerNotification.Distinct(),
                    SetNotificationDateNull = true,
                };

                await _mediator.Send(command).ConfigureAwait(false);
            }

            ////var controllerNotifications = await _controllerNotificationService.GetControllerNotifications().ConfigureAwait(false);

            ////if (controllerNotifications?.Any() ?? false)
            ////{
            ////    notifications = notifications.Concat(controllerNotifications);

            ////    UpdateNotificationDateCommand command = new UpdateNotificationDateCommand()
            ////    {
            ////        ControllerHealthChecks = controllerNotifications.SelectMany(x => x.HealthCheckControllers).Distinct()

            ////    };

            ////    await _mediator.Send(command).ConfigureAwait(false);
            ////}

            var interfaceNotifications = await _interfaceNotificationService.GetInterfaceNotifications().ConfigureAwait(false);

            if (interfaceNotifications?.Any() ?? false)
            {
                notifications = notifications.Concat(interfaceNotifications);

                var healthCheckInterfaces = interfaceNotifications.SelectMany(x => x.HealthCheckInterfaces).Distinct();

                var orderInterfaces = from oi in healthCheckInterfaces
                                      where oi.InterfaceType == "Order Interface"
                                      select new HealthCheckOrderInterface()
                                      {
                                          Id = oi.Id
                                      };

                var inventoryInterfaces = from ii in healthCheckInterfaces
                                          where ii.InterfaceType == "Inventory Interface"
                                          select new HealthCheckInventoryInterface()
                                          {
                                              Id = ii.Id
                                          };

                UpdateNotificationDateCommand command = new UpdateNotificationDateCommand()
                {
                    OrderInterfaceHealthChecks = orderInterfaces,
                    InvenotryInterfaceHealthChecks = inventoryInterfaces
                };

                await _mediator.Send(command).ConfigureAwait(false);
            }

            var serverOperationNotifications = await _serverOperationNotificationService.GetServerOperationNotification().ConfigureAwait(false);

            if (serverOperationNotifications != null)
            {
                notifications = notifications.Concat(new[] { serverOperationNotifications });

                UpdateNotificationDateCommand command = new UpdateNotificationDateCommand()
                {
                    ServerOperationHealthChecks = serverOperationNotifications.HealthCheckServerOperations
                };

                await _mediator.Send(command).ConfigureAwait(false);
            }


            if (notifications != null && notifications.Any())
            {
                //https://aka.ms/concurrent_sending
                // There is a max limit on the number of emails that can be sent concurantly for office 365 emails.
                // Max limit of concurrent emails is 3.
                var sendMessageTasksChunks = notifications
                    .Select(x => _communicationService.SendMessage(x))
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / 3)
                .Select(x => x.Select(v => v.Value));

                foreach (var chunk in sendMessageTasksChunks)
                {
                    var allTasks = Task.WhenAll(chunk);
                    allTasks.Wait();


                    if (allTasks.IsCompleted)
                    {
                        Thread.Sleep(5000);
                    }
                }
            }
        }
    }
}
