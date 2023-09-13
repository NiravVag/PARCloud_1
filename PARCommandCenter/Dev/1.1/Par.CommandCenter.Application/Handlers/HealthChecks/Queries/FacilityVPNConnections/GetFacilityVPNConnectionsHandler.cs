using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Queries.FacilityVPNConnections
{
    public class GetFacilityVPNConnectionsHandler : IRequestHandler<GetFacilityVPNConnectionsQuery, GetFacilityVPNConnectionsResponse>
    {
        private readonly ILogger<GetFacilityVPNConnectionsHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;


        public GetFacilityVPNConnectionsHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetFacilityVPNConnectionsHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetFacilityVPNConnectionsResponse> Handle(GetFacilityVPNConnectionsQuery request, CancellationToken cancellationToken)
        {
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            try
            {
                var tenant = await (from t in _dbContext.Tenants
                                    where t.Id == request.TenantId
                                    select new Tenant
                                    {
                                        Id = t.Id,
                                        Name = t.Name,
                                    }).FirstOrDefaultAsync().ConfigureAwait(false);

                var exclude = (from f in _dbContext.Facilities
                               where f.TenantId != tenant.Id && !string.IsNullOrEmpty(f.VPNConnectionName)
                               select f.VPNConnectionName.Trim().ToLower()).Distinct();


                var query = from hc in _dbContext.HealthCheckVPNs
                            where !exclude.Contains(hc.ConnectionName.Trim().ToLower())
                            select new HealthCheckVPN
                            {
                                Id = hc.Id,
                                ConnectionName = hc.ConnectionName,
                            };

                var vpnConnections = await query
                    .OrderBy(t => t.ConnectionName)
                    .ProjectTo<FacilityVPNConnectionsModel>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);

                return new GetFacilityVPNConnectionsResponse
                {
                    VPNConnections = vpnConnections
                };
            }
            catch (Exception ex)
            {

                throw;
            }
#pragma warning restore CS0168 // The variable 'ex' is declared but never used

        }
    }
}
