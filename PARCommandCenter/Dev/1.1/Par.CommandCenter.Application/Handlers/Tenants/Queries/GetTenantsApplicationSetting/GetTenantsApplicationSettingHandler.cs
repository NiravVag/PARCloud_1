using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Tenants.Queries.GetTenantsApplicationSetting
{
    public class GetTenantsApplicationSettingHandler : IRequestHandler<GetTenantsApplicationSettingQuery, GetTenantsApplicationSettingResponse>
    {
        private readonly ILogger<GetTenantsApplicationSettingHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetTenantsApplicationSettingHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetTenantsApplicationSettingHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetTenantsApplicationSettingResponse> Handle(GetTenantsApplicationSettingQuery request, CancellationToken cancellationToken)
        {
            var query = from s in _dbContext.TenantApplicationNotificationSettings
                        select s;

            if (request.ActiveOnly)
            {
                query = query.Where(s => !s.Deleted);
            }

            var tenantNotificationSettings = await query
                .ProjectTo<TenantApplicationNotificationSettingModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);


            return new GetTenantsApplicationSettingResponse
            {
                TenantNotificationSettings = tenantNotificationSettings
            };
        }
    }
}
