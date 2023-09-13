using MediatR;
using Microsoft.EntityFrameworkCore;
using Par.Command.Request;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using Par.CommandCenter.Domain.Entities.Interfaces;
using Par.Data.Context;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Interfaces.Commands.Delete
{
    public class DeleteInterfaceEventHandler : IRequestHandler<DeleteInterfaceEventCommand, DeleteInterfaceEventResponse>
    {
        private readonly IAzureFunctionsClient _azureFunctionClient;
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicationDbContext _dbContext;

        private readonly FullContext _parDataFullContext;
        private readonly IMediator _mediator;

        public DeleteInterfaceEventHandler(IAzureFunctionsClient azureFunctionClient, ICurrentUserService currentUserService, IApplicationDbContext dbContext, FullContext parDataFullContext, IMediator Mediator)
        {
            _azureFunctionClient = azureFunctionClient;
            _currentUserService = currentUserService;
            _dbContext = dbContext;

            _parDataFullContext = parDataFullContext;
            _mediator = Mediator;
        }

        public async Task<DeleteInterfaceEventResponse> Handle(DeleteInterfaceEventCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                Exception exception = new Exception("Identifier must be greater than 0.");
                throw exception;
            }

            HealthCheckInterface healthCheck = null;
            OrderEvent orderEvent = null;
            InventoryEvent inventoryEvent = null;

            if (request.InterfaceType.ToLower().Contains("order"))
            {
                var query = from hcoi in _dbContext.HealthCheckOrderInterfaces
                            where hcoi.Id == request.Id
                            select new HealthCheckInterface
                            {
                                Id = hcoi.Id,
                                EventOutputId = hcoi.EventOutputId,
                                Status = hcoi.Status,
                                Modified = hcoi.Modified,
                                InterfaceType = "Order Interface",
                            };


                healthCheck = await query.FirstOrDefaultAsync().ConfigureAwait(false);

                if (healthCheck != null)
                {
                    orderEvent = await _dbContext.OrderEvents
                        .Select(x => new OrderEvent
                        {
                            Id = x.Id,
                            TenantId = x.TenantId,
                            OrderEventOutputId = x.OrderEventOutputId,
                            OrderId = x.OrderId,
                        })
                    .FirstOrDefaultAsync(x => x.OrderEventOutputId == healthCheck.EventOutputId).ConfigureAwait(false);
                }

            }
            else
            {

                var query = from hcoi in _dbContext.HealthCheckInventoryInterfaces
                            where hcoi.Id == request.Id
                            select new HealthCheckInterface
                            {
                                Id = hcoi.Id,
                                EventOutputId = hcoi.EventOutputId,
                                Status = hcoi.Status,
                                Modified = hcoi.Modified,
                                InterfaceType = "Invenotry Interface",
                            };


                healthCheck = await query.FirstOrDefaultAsync().ConfigureAwait(false);

                if (healthCheck != null)
                {
                    inventoryEvent = await _dbContext.InventoryEvents
                        .Select(x => new InventoryEvent
                        {
                            Id = x.Id,
                            TenantId = x.TenantId,
                            InventoryEventOutputId = x.InventoryEventOutputId,
                            InventoryTransactionId = x.InventoryTransactionId
                        })
                        .FirstOrDefaultAsync(x => x.InventoryEventOutputId == healthCheck.EventOutputId).ConfigureAwait(false);
                }
            }

            if (healthCheck == null)
            {
                throw new ArgumentException($"Can't find the {request.InterfaceType} health check with Id {request.Id}");
            }

            if (orderEvent == null && inventoryEvent == null)
            {
                throw new ArgumentNullException($"We couldn't find the interface event record");
            }

            int tenantId = 0;

            tenantId = orderEvent?.TenantId ?? inventoryEvent.TenantId;

            try
            {
                var userName = string.IsNullOrWhiteSpace(_currentUserService.UPN) ? _currentUserService.PreferredUsername : _currentUserService.UPN;
                using var context = _parDataFullContext;
                
                var sessionUser = await context.SetSessionUserAsync(userName, (int)tenantId).ConfigureAwait(false);


                if (request.InterfaceType.ToLower().Contains("order"))
                {
                    var response = await _mediator.Send(new DeleteOrderEventQueueEntryRequest() { Id = orderEvent.Id, Context = context }, cancellationToken);
                }
                else
                {
                    var response = await _mediator.Send(new DeleteInventoryEventQueueEntryRequest() { Id = orderEvent.Id, Context = context }, cancellationToken);
                }

            }
            catch (Exception)
            {

                throw;
            }


            return new DeleteInterfaceEventResponse()
            {
                Id = request.Id
            };
        }
    }
}
