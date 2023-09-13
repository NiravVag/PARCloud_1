using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Application.Handlers.Routers.Commands.Restart
{
    public class RestartRouterCommandHandler : IRequestHandler<RestartRouterCommand, RestartRouterResponse>
    {
        private readonly ILogger<RestartRouterCommandHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IAzureServiceBusService _azureServiceBusService;

        public RestartRouterCommandHandler(IApplicationDbContext dbContext, IAzureServiceBusService azureServiceBusService, ILogger<RestartRouterCommandHandler> logger)
        {
            _dbContext = dbContext;
            _azureServiceBusService = azureServiceBusService;
            _logger = logger;            
        }

        public async Task<RestartRouterResponse> Handle(RestartRouterCommand request, CancellationToken cancellationToken)
        {

            var query = from r in _dbContext.Routers
                        join vm in _dbContext.VirtualMachines on r.VirtualMachineId equals vm.Id into rx
                        from vm in rx.DefaultIfEmpty()
                        where !r.Deleted
                        select new Router()
                        {
                            Id = r.Id,
                            Address = r.Address,
                            VirtualMachine = vm,
                        };


            var entity = await query.SingleOrDefaultAsync(r => r.Address.ToUpper() == request.Address.ToUpper()).ConfigureAwait(false);

            if (entity == null)
            {
                return new RestartRouterResponse()
                {
                    Success = false,
                };
            }

            var response = await _azureServiceBusService.RestartCloudRouterAsync(entity.Address, entity.VirtualMachine?.ComputerName, cancellationToken).ConfigureAwait(false);
            var funcResult = false;

            if (response != null && response.Contains($"\"result\":\"ok\""))
            {
                funcResult = true;
            }

            if (funcResult)
            {
                return new RestartRouterResponse()
                {
                    Success = funcResult,
                };
            }

            return new RestartRouterResponse()
            {
                Success = false,
            };
        }
    }
}
