using MediatR;
using Microsoft.Azure.Devices.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Routers.Commands.Delete
{
    public class DeleteRouterCommandHandler : IRequestHandler<DeleteRouterCommand, DeleteRouterResponse>
    {
        private readonly ILogger<DeleteRouterCommandHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAzureIoTService _azureIoTService;
        private readonly IAzureFunctionsClient _azureFunctionsClient;

        public DeleteRouterCommandHandler(IApplicationDbContext dbContext, IAzureFunctionsClient azureFunctionsClient, IAzureIoTService azureIoTService, ICurrentUserService currentUserService, ILogger<DeleteRouterCommandHandler> logger)
        {
            _dbContext = dbContext;
            _azureIoTService = azureIoTService;
            _currentUserService = currentUserService;
            _logger = logger;
            _azureFunctionsClient = azureFunctionsClient;
        }

        public async Task<DeleteRouterResponse> Handle(DeleteRouterCommand request, CancellationToken cancellationToken)
        {
            var result = -1;
            //1. find the router in the database.
            var entity = await _dbContext.Routers.SingleOrDefaultAsync(r => r.Address.ToUpper() == request.Address.ToUpper());

            try
            {
                //2. Remove the IoT device.
                await _azureIoTService.RemoveDeviceAsync(entity.Address.Trim(), cancellationToken).ConfigureAwait(false);
            }
            catch (DeviceNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
           

            //3. Update the DB router record delete value in the database to 1.
            if (entity.Id > 0)
            {
                entity.Deleted = true;

                entity.Modified = DateTime.Now;
                entity.ModifiedUserId = _currentUserService.UserId;

                _dbContext.Routers.Update(entity);
            }

            result = await _dbContext.SaveChangesAsync(cancellationToken);

            // Step 4 you need to call Delete router service api
            //2. Remove the service from the VM by calling the delete cloud router azure function
            //* Stop the service.
            //* Delete the service
            //var funcResult = await _azureFunctionsClient.DeleteCloudRouterAsync(request.Address, cancellationToken).ConfigureAwait(false);

            return new DeleteRouterResponse()
            {
                Success = result > 0,
            };
        }
    }
}
