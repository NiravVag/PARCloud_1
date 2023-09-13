using MediatR;
using Microsoft.EntityFrameworkCore;
using Par.CommandCenter.Application.Handlers.Bins.Commands.MeasureBin;
using Par.CommandCenter.Application.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Controllers.Commands.Upsert
{
    public class MeasureBinCommandHandler : IRequestHandler<MeasureBinCommand, MeasureBinCommandResponse>
    {
        private readonly IAzureFunctionsClient _azureFunctionClient;
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicationDbContext _dbContext;

        public MeasureBinCommandHandler(IAzureFunctionsClient azureFunctionClient, ICurrentUserService currentUserService, IApplicationDbContext dbContext)
        {
            _azureFunctionClient = azureFunctionClient;
            _currentUserService = currentUserService;
            _dbContext = dbContext;
        }

        public async Task<MeasureBinCommandResponse> Handle(MeasureBinCommand request, CancellationToken cancellationToken)
        {
            if (request.BinId <= 0)
            {
                Exception exception = new Exception("Bin identifier must be greater than 0.");
                throw exception;
            }

            if (request.Timeout <= 0)
            {
                Exception exception = new Exception("Request timeout must be greater than 0.");
                throw exception;
            }

            // Get the bin
            var bin = await (from x in _dbContext.Bins
                             where x.Id == request.BinId
                             select x).SingleOrDefaultAsync(cancellationToken);

            if (bin == null)
            {
                Exception exception = new Exception($"Bin {request.BinId} not found.");
                throw exception;
            }

            // Call the azure function .           
            var (Weight, Quantity) = await _azureFunctionClient.RequestBinMeasureAsync(bin.Id, null, cancellationToken).ConfigureAwait(false);

            return new MeasureBinCommandResponse()
            {
                Quantity = Quantity,
                Weight = Weight,
            };
        }
    }
}
