using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Queries.Audit.Router
{
    public class GetRouterHealthCheckAudit_LastRecordHandler : IRequestHandler<GetRouterHealthCheckAudit_LastRecordQuery, GetRouterHealthCheckAudit_LastRecordResponse>
    {
        private readonly ILogger<GetRouterHealthCheckAudit_LastRecordHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetRouterHealthCheckAudit_LastRecordHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetRouterHealthCheckAudit_LastRecordHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetRouterHealthCheckAudit_LastRecordResponse> Handle(GetRouterHealthCheckAudit_LastRecordQuery request, CancellationToken cancellationToken)
        {
            var query = from hc in _dbContext.HealthCheckRouterAudit_LastRecords
                        select hc;

            var healthChecks = await query
                .OrderBy(x => x.Audit_HealthCheckRouterId)
                .ThenBy(x => x.Status)
                .ProjectTo<GetRouterHealthCheckAudit_LastRecordResponseModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);


            return new GetRouterHealthCheckAudit_LastRecordResponse
            {
                HealthChecksLastRecords = healthChecks
            };
        }
    }
}
