using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Commands.UpdateNotificationDate
{
    public class UpdateNotificationDateCommandHandler : IRequestHandler<UpdateNotificationDateCommand, UpdateNotificationDateResponse>
    {
        private readonly ILogger<UpdateNotificationDateCommandHandler> _logger;
        private readonly IApplicationDbContext _dbContext;

        public UpdateNotificationDateCommandHandler(IApplicationDbContext dbContext, ILogger<UpdateNotificationDateCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<UpdateNotificationDateResponse> Handle(UpdateNotificationDateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<int> healthCheckIds = Enumerable.Empty<int>();
                DateTime? newDate = DateTime.UtcNow;
                if (request.SetNotificationDateNull)
                {
                    newDate = null;
                }

                if (request.VPNHealthChecks?.Any() ?? false)
                {
                    healthCheckIds = request.VPNHealthChecks.Select(x => x.Id).Distinct();

                    var vpnHealthChecks = _dbContext.HealthCheckVPNs.Where(hc => healthCheckIds.Contains(hc.Id)).ToList();                                     

                    vpnHealthChecks.ForEach(hc => hc.NotificationDate = newDate);
                }

                if (request.RouterHealthChecks?.Any() ?? false)
                {
                    healthCheckIds = request.RouterHealthChecks.Select(x => x.Id).Distinct();

                    var routerHealthChecks = _dbContext.HealthCheckRouters.Where(hc => healthCheckIds.Contains(hc.Id)).ToList();

                    routerHealthChecks.ForEach(hc => hc.NotificationDate = newDate);
                }

                if (request.ControllerHealthChecks?.Any() ?? false)
                {
                    healthCheckIds = request.ControllerHealthChecks.Select(x => x.Id).Distinct();

                    var controllerHealthChecks = _dbContext.HealthCheckControllers.Where(hc => healthCheckIds.Contains(hc.Id)).ToList();

                    controllerHealthChecks.ForEach(hc => hc.NotificationDate = newDate);
                }

                if (request.ServerOperationHealthChecks?.Any() ?? false)
                {
                    healthCheckIds = request.ServerOperationHealthChecks.Select(x => x.Id).Distinct();

                    var serverOperationHealthChecks = _dbContext.HealthCheckServerOperations.Where(hc => healthCheckIds.Contains(hc.Id)).ToList();

                    serverOperationHealthChecks.ForEach(hc => hc.NotificationDate = newDate);
                }

                if (request.InvenotryInterfaceHealthChecks?.Any() ?? false)
                {
                    healthCheckIds = request.InvenotryInterfaceHealthChecks.Select(x => x.Id).Distinct();

                    var inventoryInterfaceHealthChecks = _dbContext.HealthCheckInventoryInterfaces.Where(hc => healthCheckIds.Contains(hc.Id)).ToList();

                    inventoryInterfaceHealthChecks.ForEach(hc => hc.NotificationDate = newDate);
                }

                if (request.OrderInterfaceHealthChecks?.Any() ?? false)
                {
                    healthCheckIds = request.OrderInterfaceHealthChecks.Select(x => x.Id).Distinct();

                    var orderInterfaceHealthChecks = _dbContext.HealthCheckOrderInterfaces.Where(hc => healthCheckIds.Contains(hc.Id)).ToList();

                    orderInterfaceHealthChecks.ForEach(hc => hc.NotificationDate = newDate);
                }

                var result = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);


                return new UpdateNotificationDateResponse()
                {
                    Success = result > 0,
                    UpdateResult = result
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
