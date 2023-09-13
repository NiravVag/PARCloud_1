using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Par.Command.Request.CloudRouter;
using Par.CommandCenter.Application.Interfaces;
using Par.Data.Context;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.CloudRouters.Commands.Upsert
{
    public class UpsertCloudRouterCommandHandler : IRequestHandler<UpsertCloudRouterCommand, UpsertCloudRouterResponse>
    {
        private readonly ILogger<UpsertCloudRouterCommandHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        private readonly IMediator _mediator;

        private readonly FullContext _parDataFullContext;

        private readonly ICurrentUserService _currentUserService;

        public UpsertCloudRouterCommandHandler(IApplicationDbContext dbContext, FullContext parDataFullContext, IMediator Mediator, IMapper mapper, ICurrentUserService currentUserService, ILogger<UpsertCloudRouterCommandHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _mediator = Mediator;
            _parDataFullContext = parDataFullContext;
            _currentUserService = currentUserService;
        }

        public async Task<UpsertCloudRouterResponse> Handle(UpsertCloudRouterCommand request, CancellationToken cancellationToken)
        {
            var router = await _dbContext.Routers.FirstOrDefaultAsync(x => x.Address.Trim() == request.RouterAddress.Trim() && !x.Deleted).ConfigureAwait(false);

            if (router == null)
            {
                throw new ArgumentException($"Can't find the router with address {request.RouterAddress}");
            }
            if (router.DeviceTypeId != 4)
            {
                throw new ArgumentException($"Router is not registered as a CDC {request.RouterAddress}");
            }


            using var context = _parDataFullContext;
            var tenantId = request.TenantId;

            //// var tenantIdCheck = _dbContext.Routers.FirstOrDefault(x => x.Address.Trim() == request.RouterAddress.Trim() && !x.Deleted).TenantId;


            //// var entity = await _dbContext.Routers.FirstOrDefaultAsync(f => f.Address == request.RouterAddress);

            if (router.TenantId == null)
            {
                _mapper.Map(request, router);

                router.TenantId = tenantId;


                _dbContext.Routers.Update(router);
                await _dbContext.SaveChangesAsync();
            }

            //CY HERE !!!!!!!!!!!!!!!!!!!!!!           

            var userName = string.IsNullOrWhiteSpace(_currentUserService.UPN) ? _currentUserService.PreferredUsername : _currentUserService.UPN;

            await context.SetSessionUserAsync(userName, tenantId).ConfigureAwait(false);


            await _mediator.Send(new CreateUpdateCloudRouterRequest() { RouterAddress = request.RouterAddress, Ports = request.Ports, Context = context }, cancellationToken);


            return new UpsertCloudRouterResponse()
            {
                RouterId = router.Id,
            };
        }
    }
}
