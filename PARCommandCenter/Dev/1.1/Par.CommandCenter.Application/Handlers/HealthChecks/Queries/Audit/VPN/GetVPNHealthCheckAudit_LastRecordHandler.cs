using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Queries.Audit.VPN
{
    public class GetVPNHealthCheckAudit_LastRecordHandler : IRequestHandler<GetVPNHealthCheckAudit_LastRecordQuery, GetVPNHealthCheckAudit_LastRecordResponse>
    {
        private readonly ILogger<GetVPNHealthCheckAudit_LastRecordHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetVPNHealthCheckAudit_LastRecordHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetVPNHealthCheckAudit_LastRecordHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetVPNHealthCheckAudit_LastRecordResponse> Handle(GetVPNHealthCheckAudit_LastRecordQuery request, CancellationToken cancellationToken)
        {
            var query = from hc in _dbContext.HealthCheckVPNAudit_LastRecords
                        select hc;

            var healthChecks = await query
                .OrderBy(x => x.Audit_HealthCheckVPNId)
                .ThenBy(x => x.Status)
                .ProjectTo<GetVPNHealthCheckAudit_LastRecordResponseModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);


            return new GetVPNHealthCheckAudit_LastRecordResponse
            {
                HealthChecksLastRecords = healthChecks
            };
        }
    }
}
