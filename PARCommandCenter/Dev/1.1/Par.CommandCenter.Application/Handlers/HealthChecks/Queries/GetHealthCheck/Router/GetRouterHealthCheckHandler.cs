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

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Queries.GetHealthCheck.Router
{
    public class GetRouterHealthCheckHandler : IRequestHandler<GetRouterHealthCheckQuery, GetRouterHealthCheckResponse>
    {
        private readonly ILogger<GetRouterHealthCheckHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;


        public GetRouterHealthCheckHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetRouterHealthCheckHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetRouterHealthCheckResponse> Handle(GetRouterHealthCheckQuery request, CancellationToken cancellationToken)
        {
            if (request.IncludeStatistics && request.StatisticsOnly)
            {
                throw new ArgumentException("The IncludeStatistics and StatisticsOnly can't both be a true value at the same time. Set only one to true at a time");
            }

            var response = new GetRouterHealthCheckResponse();
            HealthCheckStatisticModel statistics = null;

            IQueryable<HealthCheckRouter> query;

            if (request.HealthCheckIds?.Any() ?? false)
            {
                query = from hc in _dbContext.HealthCheckRouters
                        join r in _dbContext.Routers on hc.RouterId equals r.Id
                        join t in _dbContext.Tenants on r.TenantId equals t.Id
                        where request.HealthCheckIds.Contains(hc.Id)
                        select new HealthCheckRouter
                        {
                            Id = hc.Id,
                            RouterId = hc.RouterId,
                            RouterAdress = r.Address,
                            TenantName = t.Name,
                            Status = hc.Status,
                            PreviousStatus = hc.PreviousStatus,
                            PreviousStatusDate = hc.PreviousStatusDate,
                            LastCommunication = hc.LastCommunication,
                            LastReboot = hc.LastReboot,
                            Created = hc.Created,
                            Modified = hc.Modified,
                            TenantId = (int)r.TenantId,
                        };
            }
            else
            {
                query = from hc in _dbContext.HealthCheckRouters
                        join r in _dbContext.Routers on hc.RouterId equals r.Id
                        join t in _dbContext.Tenants on r.TenantId equals t.Id
                        where request.TenantIds.Contains(t.Id)
                        select new HealthCheckRouter
                        {
                            Id = hc.Id,
                            RouterId = hc.RouterId,
                            RouterAdress = r.Address,
                            TenantName = t.Name,
                            Status = hc.Status,
                            PreviousStatus = hc.PreviousStatus,
                            PreviousStatusDate = hc.PreviousStatusDate,
                            LastCommunication = hc.LastCommunication,
                            LastReboot = hc.LastReboot,
                            Created = hc.Created,
                            Modified = hc.Modified,
                            TenantId = (int)r.TenantId,
                        };

                switch (request.DateRangeFilter)
                {
                    case DateRangeFilterType.Past24Hours:
                        query = query.Where(x => x.LastCommunication >= DateTimeOffset.UtcNow.AddDays(-1));
                        break;
                    case DateRangeFilterType.Past3Days:
                        query = query.Where(x => x.LastCommunication >= DateTimeOffset.UtcNow.AddDays(-3));
                        break;
                    case DateRangeFilterType.Past7Days:
                        query = query.Where(x => x.LastCommunication >= DateTimeOffset.UtcNow.AddDays(-7));
                        break;
                    case DateRangeFilterType.Past30Days:
                        query = query.Where(x => x.LastCommunication >= DateTimeOffset.UtcNow.AddDays(-30));
                        break;
                    case DateRangeFilterType.CustomDate:
                        if (request.StartDate == null || request.EndDate == null)
                        {
                            throw new ArgumentNullException($"The {nameof(request.StartDate)} and the {nameof(request.EndDate)} can't be null");
                        }

                        query = query.Where(x => x.LastCommunication >= request.StartDate && x.LastCommunication <= request.EndDate);

                        break;
                    default:
                        query = query.Where(x => x.LastCommunication >= DateTimeOffset.UtcNow.AddDays(-1));
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

                    statistics = (await Task.FromResult(from hhr in new List<string> { "X" }
                    //join hhc in _dbContext.HealthCheckRouters on 1 equals 1 into hc
                    //join uhhc in _dbContext.HealthCheckRouters on 1 equals 1 into uhc
                                                        select new HealthCheckStatisticModel
                                                        {
                                                            Type = HealthCheckType.Routers,
                                                            TotalTenantsCount = tenantsCount,
                                                            HealthyTenantsCount = query.Where(x => x.Status == "Online").Select(x => x.TenantId).Distinct().Count(),
                                                            HealthyCount = query.Where(v => v.Status == "Online").Count(),
                                                            UnHealthyTenantsCount = query.Where(x => x.Status == "Offline").Select(x => x.TenantId).Distinct().Count(),
                                                            UnHealthyCount = query.Where(v => v.Status == "Offline").Count(),
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
                    query = query.Where(q => q.Status == "Offline");
                }
                else if (request.Filter == HealthCheckFilterType.Active)
                {
                    query = query.Where(q => q.Status == "Online");
                }
                else if (request.Filter == HealthCheckFilterType.Warnings)
                {
                    query = query.Where(q => q.Status == "Warning");
                }
            }

            var healthChecks = await query
                .OrderBy(x => x.TenantName)
                .ThenByDescending(x => x.LastCommunication)
                .ProjectTo<HealthCheckRouterModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            response.HealthChecks = healthChecks;

            return response;
        }
    }
}
