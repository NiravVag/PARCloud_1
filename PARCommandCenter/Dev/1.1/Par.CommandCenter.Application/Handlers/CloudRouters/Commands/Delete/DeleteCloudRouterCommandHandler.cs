using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Par.Command.Request.CloudRouter;
using Par.CommandCenter.Application.Interfaces;
using Par.Data.Context;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Par.CommandCenter.Application.Handlers.CloudRouters.Commands.Delete
{
    public class DeleteCloudRouterCommandHandler : IRequestHandler<DeleteCloudRouterCommand, DeleteCloudRouterResponse>
    {
        private readonly ILogger<DeleteCloudRouterCommandHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        private readonly IMediator _mediator;

        private readonly FullContext _parDataFullContext;

        private readonly ICurrentUserService _currentUserService;


        public DeleteCloudRouterCommandHandler(IApplicationDbContext dbContext, FullContext parDataFullContext, IMediator Mediator, IMapper mapper, ICurrentUserService currentUserService, ILogger<DeleteCloudRouterCommandHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _mediator = Mediator;
            _parDataFullContext = parDataFullContext;
            _currentUserService = currentUserService;
        }

        public async Task<DeleteCloudRouterResponse> Handle(DeleteCloudRouterCommand request, CancellationToken cancellationToken)
        {
            var router = _dbContext.Routers.FirstOrDefault(x => x.Address.Trim() == request.RouterAddress.Trim() && !x.Deleted);

            if (router == null)
            {
                throw new ArgumentException($"Can't find the router with address {request.RouterAddress}");
            }

            var tenantId = _dbContext.Routers.FirstOrDefault(x => x.Address.Trim() == request.RouterAddress.Trim() && !x.Deleted).TenantId;

            using var context = _parDataFullContext;
            var userName = string.IsNullOrWhiteSpace(_currentUserService.UPN) ? _currentUserService.PreferredUsername : _currentUserService.UPN;

            var sessionUser = await context.SetSessionUserAsync(userName, (int)tenantId).ConfigureAwait(false);


            var response = await _mediator.Send(new DeleteCloudRouterRequest() { RouterAddress = request.RouterAddress, Context = context }, cancellationToken);


            return new DeleteCloudRouterResponse()
            {
                RouterId = (int)tenantId,
            };
        }
    }
}
