using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Routers.Commands.Install
{
    public class InstallCloudRouterCommandHandler : IRequestHandler<InstallCloudRouterCommand, InstallCloudRouterResponse>
    {
        private readonly ILogger<InstallCloudRouterCommandHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IAzureServiceBusService _azureServiceBusService;

        public InstallCloudRouterCommandHandler(IApplicationDbContext dbContext, IAzureServiceBusService azureServiceBusService, ILogger<InstallCloudRouterCommandHandler> logger)
        {
            _dbContext = dbContext;
            _azureServiceBusService = azureServiceBusService;
            _logger = logger;
        }

        public async Task<InstallCloudRouterResponse> Handle(InstallCloudRouterCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Install router ({0}).", request.Address);
            string responseMesage = null;

            // check if the router address in the database and not deleted.
            var router = await _dbContext.Routers
              .Where(r => r.Address.Trim().ToLower() == request.Address.Trim().ToLower())
              .Where(r => !r.Deleted)
              .SingleOrDefaultAsync()
              .ConfigureAwait(false);

            if (router == null)
            {
                Exception exception = new Exception($"Router with address '{request.Address}' not found.");
                throw exception;
            }

            var lastVirtualMachine = await (from vm in _dbContext.VirtualMachines
                                     orderby vm.Id descending
                                     select new VirtualMachine()
                                     {
                                         Id = vm.Id,
                                         ComputerName = vm.ComputerName,
                                     })
                                     .FirstOrDefaultAsync(cancellationToken)
                                     .ConfigureAwait(false) ?? throw new InvalidOperationException("No Virtual Machine informatoin found in the DB.");

            // Call the azure function to install the windows service.           
            responseMesage = await _azureServiceBusService.InstallCloudRouterAsync(router.Address, request.ServiceName, request.ServiceDisplayName, lastVirtualMachine.ComputerName.Trim(), cancellationToken).ConfigureAwait(false);

            return new InstallCloudRouterResponse()
            {
                ResponseMesssage = responseMesage,
            };
        }
    }
}
