using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using Par.CommandCenter.Domain.Enums;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Routers.Queries.GetRoutersByTenant
{
    public class GetRoutersByTenantHandler : IRequestHandler<GetRoutersByTenantQuery, GetRoutersByTenantResponse>
    {
        private readonly ILogger<GetRoutersByTenantHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetRoutersByTenantHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetRoutersByTenantHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetRoutersByTenantResponse> Handle(GetRoutersByTenantQuery request, CancellationToken cancellationToken)
        {
            var query = from r in _dbContext.Routers
                        join c in _dbContext.Controllers on r.Id equals c.RouterId into gx
                        from c in gx.DefaultIfEmpty()
                        group c by new { r.Id, r.DeviceTypeId, r.TenantId, r.Address, r.ComputerName, r.ServiceName, r.ServiceDisplayName, r.FirmwareVersion, r.LastCommunication, r.LastReboot, r.Deleted } into routerGrouped
                        select new Router()
                        {
                            Id = routerGrouped.Key.Id,
                            TenantId = routerGrouped.Key.TenantId,
                            Address = routerGrouped.Key.Address,
                            FirmwareVersion = routerGrouped.Key.FirmwareVersion,
                            LastCommunication = routerGrouped.Key.LastCommunication,
                            LastReboot = routerGrouped.Key.LastReboot,
                            Deleted = routerGrouped.Key.Deleted,
                            RegisteredControllerCount = routerGrouped.Count(x => x != null),
                            DeviceType = (DeviceType)routerGrouped.Key.DeviceTypeId,
                            ComputerName = routerGrouped.Key.ComputerName,
                            ServiceName = routerGrouped.Key.ServiceName,
                            ServiceDisplayName = routerGrouped.Key.ServiceDisplayName,
                        };

            var routers = await query
               .Where(r => r.TenantId == request.TenantId)
               .Where(r => !r.Deleted)
               .OrderBy(r => r.Address)
               .ProjectTo<RouterModel>(_mapper.ConfigurationProvider)
               .ToListAsync(cancellationToken)
               .ConfigureAwait(false);

            return new GetRoutersByTenantResponse
            {
                Routers = routers
            };
        }
    }
}
