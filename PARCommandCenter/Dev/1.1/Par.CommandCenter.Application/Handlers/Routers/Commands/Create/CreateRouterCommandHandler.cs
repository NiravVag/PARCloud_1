using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Routers.Commands.Create
{
    public class CreateRouterCommandHandler : IRequestHandler<CreateRouterCommand, CreateRouterResponse>
    {
        private readonly ILogger<CreateRouterCommandHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAzureIoTService _azureIoTService;

        public CreateRouterCommandHandler(IApplicationDbContext dbContext, IAzureIoTService azureIoTService, IMapper mapper, ILogger<CreateRouterCommandHandler> logger)
        {
            _dbContext = dbContext;
            _azureIoTService = azureIoTService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CreateRouterResponse> Handle(CreateRouterCommand request, CancellationToken cancellationToken)
        {
            var deviceId = await _azureIoTService.AddDeviceAsync(request.Address, cancellationToken).ConfigureAwait(false);

            return new CreateRouterResponse()
            {
                DeviceId = deviceId,
            };
        }
    }
}
