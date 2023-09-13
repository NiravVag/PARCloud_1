using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using Par.CommandCenter.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Queries.GetHealthCheck.VPN
{
    public class GetVPNHealthCheckHandler : IRequestHandler<GetVPNHealthCheckQuery, GetVPNHealthCheckResponse>
    {
        private readonly ILogger<GetVPNHealthCheckHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;


        public GetVPNHealthCheckHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetVPNHealthCheckHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetVPNHealthCheckResponse> Handle(GetVPNHealthCheckQuery request, CancellationToken cancellationToken)
        {
            if (request.IncludeStatistics && request.StatisticsOnly)
            {
                throw new ArgumentException("The IncludeStatistics and StatisticsOnly can't both be a true value at the same time. Set only one to true at a time");
            }

            var response = new GetVPNHealthCheckResponse();
            HealthCheckStatisticModel statistics = null;

            IQueryable<HealthCheckVPN> query;

            if (request.HealthCheckIds?.Any() ?? false)
            {
                query = from hc in _dbContext.HealthCheckVPNs
                        join t in _dbContext.Tenants on hc.TenantId equals t.Id into tx
                        from t in tx.DefaultIfEmpty<Tenant>()
                        where request.HealthCheckIds.Contains(hc.Id)
                        select new HealthCheckVPN
                        {
                            Id = hc.Id,
                            Tenant = (t == null) ? null : new Tenant()
                            {
                                Id = t.Id,
                                Name = t.Name,
                            },
                            facilities = (from f in _dbContext.Facilities
                                          where f.TenantId == t.Id && f.VPNConnectionName == hc.ConnectionName
                                          select f),
                            ConnectionName = hc.ConnectionName,
                            Status = hc.Status,
                            PreviousStatus = hc.PreviousStatus,
                            PreviousStatusDate = hc.PreviousStatusDate,
                            Modified = hc.Modified,
                            Created = hc.Created,
                        };

            }
            else
            {

                query = from hc in _dbContext.HealthCheckVPNs
                        join t in _dbContext.Tenants on hc.TenantId equals t.Id into tx
                        from t in tx.DefaultIfEmpty<Tenant>()
                        select new HealthCheckVPN
                        {
                            Id = hc.Id,
                            Tenant = (t == null) ? null : new Tenant()
                            {
                                Id = t.Id,
                                Name = t.Name,
                            },
                            facilities = (from f in _dbContext.Facilities
                                          where f.TenantId == t.Id && f.VPNConnectionName == hc.ConnectionName
                                          select f),
                            ConnectionName = hc.ConnectionName,
                            Status = hc.Status,
                            PreviousStatus = hc.PreviousStatus,
                            PreviousStatusDate = hc.PreviousStatusDate,
                            Modified = hc.Modified,
                            Created = hc.Created,
                        };

                switch (request.DateRangeFilter)
                {
                    case DateRangeFilterType.Past24Hours:
                        query = query.Where(x => x.Modified >= DateTimeOffset.UtcNow.AddDays(-1));
                        break;
                    case DateRangeFilterType.Past3Days:
                        query = query.Where(x => x.Modified >= DateTimeOffset.UtcNow.AddDays(-3));
                        break;
                    case DateRangeFilterType.Past7Days:
                        query = query.Where(x => x.Modified >= DateTimeOffset.UtcNow.AddDays(-7));
                        break;
                    case DateRangeFilterType.Past30Days:
                        query = query.Where(x => x.Modified >= DateTimeOffset.UtcNow.AddDays(-30));
                        break;
                    case DateRangeFilterType.CustomDate:
                        if (request.StartDate == null || request.EndDate == null)
                        {
                            throw new ArgumentNullException($"The {nameof(request.StartDate)} and the {nameof(request.EndDate)} can't be null");
                        }

                        query = query.Where(x => x.Modified >= request.StartDate.Value.ToUniversalTime() && x.Modified <= request.EndDate.Value.ToUniversalTime().Add(DateTime.MaxValue.TimeOfDay));
                        break;
                    default:
                        query = query.Where(x => x.Modified >= DateTimeOffset.UtcNow.AddDays(-1));
                        break;
                }

                if (request.StatisticsOnly || request.IncludeStatistics)
                {
                    var tenantsCount = await (from t in _dbContext.Tenants
                                              where t.Deleted == false
                                              select new Tenant
                                              {
                                                  Id = t.Id,
                                                  Deleted = t.Deleted,
                                              }).CountAsync();


                    statistics = (await Task.FromResult(from dummyRow in new List<string> { "X" }
                                                        join hhc in query on 1 equals 1 into hc
                                                        //join uhhc in _dbContext.HealthCheckVPNs on 1 equals 1 into uhc                             
                                                        select new HealthCheckStatisticModel
                                                        {
                                                            Type = HealthCheckType.VPN,
                                                            TotalTenantsCount = tenantsCount,
                                                            HealthyTenantsCount = tenantsCount,
                                                            HealthyCount = hc.Where(v => v.Status == "Connected").Count(),
                                                            UnHealthyTenantsCount = tenantsCount,
                                                            UnHealthyCount = hc.Where(v => v.Status != "Connected").Count(),
                                                            WarrningTenantsCount = 0,
                                                            WarrningCount = 0,
                                                        })).FirstOrDefault();

                    response.Statistics = statistics;

                    if (request.StatisticsOnly)
                    {
                        return response;
                    }
                }

                if (request.Filter == HealthCheckFilterType.Errors)
                {
                    query = query.Where(q => q.Status != "Connected");
                }
                else if (request.Filter == HealthCheckFilterType.Active)
                {
                    query = query.Where(q => q.Status == "Connected");
                }
                else if (request.Filter == HealthCheckFilterType.Warnings)
                {
                    query = query.Where(q => q.Status == "Warning");
                }
            }

            var healthChecks = await query
                .OrderBy(t => t.ConnectionName)
                .ThenByDescending(t => t.Modified)
                .ProjectTo<HealthCheckVPNModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            response.HealthChecks = healthChecks;

            return response;
        }
    }
}
