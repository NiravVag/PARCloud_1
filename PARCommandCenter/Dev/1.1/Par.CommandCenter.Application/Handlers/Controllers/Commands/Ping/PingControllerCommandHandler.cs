using MediatR;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace Par.CommandCenter.Application.Handlers.Controllers.Commands.Ping
{
    public class PingControllerCommandHandler : IRequestHandler<PingControllerCommand, PingControllerResponse>
    {
        private readonly ILogger<PingControllerCommandHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IAzureServiceBusService _azureServiceBusService;

        public PingControllerCommandHandler(IApplicationDbContext dbContext, IAzureServiceBusService azureServiceBusService,
            ILogger<PingControllerCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            _azureServiceBusService = azureServiceBusService;
        }

        public async Task<PingControllerResponse> Handle(PingControllerCommand request, CancellationToken cancellationToken)
        {
            var cleanIpAddress = request.Address.Trim();
            try
            {
                IPAddress address = IPAddress.Parse(cleanIpAddress);
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (FormatException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

            var query = from c in _dbContext.Controllers
                        join r in _dbContext.Routers on c.RouterId equals r.Id
                        join vm in _dbContext.VirtualMachines on r.VirtualMachineId equals vm.Id
                        where c.IpAddress == cleanIpAddress && c.TenantId == request.TenantId
                        select new
                        {
                            c.Id,
                            c.NetworkPort,
                            VirtualMachine = new
                            {
                                Id = vm.Id,
                                ComputerName = vm.ComputerName,
                            }
                        };

            if (request.NetworkPort.HasValue && request.NetworkPort > 0)
            {
                query = from c in query
                        where c.NetworkPort == request.NetworkPort
                        select c;

            }

            var entity = await query.SingleOrDefaultAsync().ConfigureAwait(false);            
            if (entity == null)
            {
                return new PingControllerResponse()
                {
                    Success = false,
                    Message = $"The controller record for IP Address {cleanIpAddress} is not found",
                };
            }

            var funcResult = await _azureServiceBusService.PingControllerAsync(cleanIpAddress, entity.VirtualMachine.ComputerName.Trim(), 
                cancellationToken, request.NetworkPort).ConfigureAwait(false);

            return new PingControllerResponse()
            {
                Success = funcResult.PingSucceeded,
                PingResponse = funcResult,
            };
        }
    }
}
