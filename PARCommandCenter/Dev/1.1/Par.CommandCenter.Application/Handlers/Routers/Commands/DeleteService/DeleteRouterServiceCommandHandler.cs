using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Application.Handlers.Routers.Commands.DeleteService
{
    public class DeleteRouterServiceCommandHandler : IRequestHandler<DeleteRouterServiceCommand, DeleteRouterServiceResponse>
    {
        private readonly ILogger<DeleteRouterServiceCommandHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAzureIoTService _azureIoTService;
        private readonly IAzureServiceBusService _azureServiceBusService;

        public DeleteRouterServiceCommandHandler(IApplicationDbContext dbContext, IAzureServiceBusService azureServiceBusService,
            IAzureIoTService azureIoTService, ICurrentUserService currentUserService, ILogger<DeleteRouterServiceCommandHandler> logger)
        {
            _dbContext = dbContext;
            _azureIoTService = azureIoTService;
            _currentUserService = currentUserService;
            _logger = logger;
            _azureServiceBusService = azureServiceBusService;
        }

        public async Task<DeleteRouterServiceResponse> Handle(DeleteRouterServiceCommand request, CancellationToken cancellationToken)
        {
            //1. find the router in the database, and it must be marked as deleted.
            var query = from r in _dbContext.Routers
                        join vm in _dbContext.VirtualMachines on r.VirtualMachineId equals vm.Id
                        where r.Address.ToUpper() == request.Address.ToUpper().Trim()
                        select new Par.CommandCenter.Domain.Entities.Router()
                        { 
                           Id = r.Id,
                           Address = r.Address,
                           VirtualMachineId = r.VirtualMachineId,
                           VirtualMachine = new VirtualMachine() { 
                               Id = r.Id,
                               ComputerName = vm.ComputerName,
                           }                            
                        };

            var router = await query.SingleOrDefaultAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

            //2. Remove the service from the VM by calling the delete cloud router azure function
            //* Stop the service.
            //* Delete the service
            if (router != null && router.Id > 0)
            {
                var funcResult = await _azureServiceBusService.DeleteCloudRouterAsync(router.Address, router.VirtualMachine.ComputerName, cancellationToken).ConfigureAwait(false);

                return await Task.FromResult(new DeleteRouterServiceResponse()
                {
                    Success = funcResult,
                });
            }

            return await Task.FromResult(new DeleteRouterServiceResponse()
            {
                Success = false,
            });
        }
    }
}
