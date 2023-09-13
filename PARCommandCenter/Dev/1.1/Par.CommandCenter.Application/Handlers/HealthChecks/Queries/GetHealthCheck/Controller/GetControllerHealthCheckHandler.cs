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

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Queries.GetHealthCheck.Controller
{
    public class GetControllerHealthCheckHandler : IRequestHandler<GetControllerHealthCheckQuery, GetControllerHealthCheckResponse>
    {
        private readonly ILogger<GetControllerHealthCheckHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;


        public GetControllerHealthCheckHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetControllerHealthCheckHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetControllerHealthCheckResponse> Handle(GetControllerHealthCheckQuery request, CancellationToken cancellationToken)
        {
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            try
            {
                if (request.IncludeStatistics && request.StatisticsOnly)
                {
                    throw new ArgumentException("The IncludeStatistics and StatisticsOnly can't both be a true value at the same time. Set only one to true at a time");
                }

                var response = new GetControllerHealthCheckResponse();
                HealthCheckStatisticModel statistics = null;

                IQueryable<HealthCheckController> query;

                if (request.HealthCheckIds?.Any() ?? false)
                {
                    query = from hc in _dbContext.HealthCheckControllers
                            join c in _dbContext.Controllers on hc.ControllerId equals c.Id
                            join t in _dbContext.Tenants on c.TenantId equals t.Id
                            where request.HealthCheckIds.Contains(hc.Id) && c.Active == true
                            select new HealthCheckController
                            {
                                Id = hc.Id,
                                ControllerId = hc.ControllerId,
                                RemoteIpAddress = hc.RemoteIpAddress,
                                RemoteNetworkPort = hc.RemoteNetworkPort,
                                TenantName = t.Name,
                                TCPTestStatus = hc.TCPTestStatus,
                                PreviousStatus = hc.PreviousStatus,
                                PreviousStatusDate = hc.PreviousStatusDate,
                                RouterLastCommunication = hc.RouterLastCommunication,
                                RegisteredScaleCount = hc.RegisteredScaleCount,
                                OnlineScaleCount = hc.OnlineScaleCount,
                                OfflineScaleCount = hc.OfflineScaleCount,
                                Created = hc.Created,
                                Modified = hc.Modified,
                                TenantId = c.TenantId,
                            };

                }
                else
                {

                    query = from hc in _dbContext.HealthCheckControllers
                            join c in _dbContext.Controllers on hc.ControllerId equals c.Id
                            join t in _dbContext.Tenants on c.TenantId equals t.Id
                            where request.TenantIds.Contains(t.Id) && c.Active == true
                            select new HealthCheckController
                            {
                                Id = hc.Id,
                                ControllerId = hc.ControllerId,
                                RemoteIpAddress = hc.RemoteIpAddress,
                                RemoteNetworkPort = hc.RemoteNetworkPort,
                                TenantName = t.Name,
                                TCPTestStatus = hc.TCPTestStatus,
                                PreviousStatus = hc.PreviousStatus,
                                PreviousStatusDate = hc.PreviousStatusDate,
                                RouterLastCommunication = hc.RouterLastCommunication,
                                RegisteredScaleCount = hc.RegisteredScaleCount,
                                OnlineScaleCount = hc.OnlineScaleCount,
                                OfflineScaleCount = hc.OfflineScaleCount,
                                Created = hc.Created,
                                Modified = hc.Modified,
                                TenantId = c.TenantId,
                            };

                    switch (request.DateRangeFilter)
                    {
                        case DateRangeFilterType.Past24Hours:
                            query = query.Where(x => x.RouterLastCommunication >= DateTimeOffset.UtcNow.AddDays(-1));
                            break;
                        case DateRangeFilterType.Past3Days:
                            query = query.Where(x => x.RouterLastCommunication >= DateTimeOffset.UtcNow.AddDays(-3));
                            break;
                        case DateRangeFilterType.Past7Days:
                            query = query.Where(x => x.RouterLastCommunication >= DateTimeOffset.UtcNow.AddDays(-7));
                            break;
                        case DateRangeFilterType.Past30Days:
                            query = query.Where(x => x.RouterLastCommunication >= DateTimeOffset.UtcNow.AddDays(-30));
                            break;
                        case DateRangeFilterType.CustomDate:
                            if (request.StartDate == null || request.EndDate == null)
                            {
                                throw new ArgumentNullException($"The {nameof(request.StartDate)} and the {nameof(request.EndDate)} can't be null");
                            }

                            query = query.Where(x => x.RouterLastCommunication >= request.StartDate && x.RouterLastCommunication <= request.EndDate);
                            break;
                        default:
                            query = query.Where(x => x.RouterLastCommunication >= DateTimeOffset.UtcNow.AddDays(-1));
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
                        //join hhc in _dbContext.HealthCheckControllers on 1 equals 1 into hc
                        //join uhhc in _dbContext.HealthCheckControllers on 1 equals 1 into uhc
                                                            select new HealthCheckStatisticModel
                                                            {
                                                                Type = HealthCheckType.Controllers,
                                                                TotalTenantsCount = tenantsCount,
                                                                HealthyTenantsCount = query.Where(x => x.TCPTestStatus == "Established").Select(x => x.TenantId).Distinct().Count(),
                                                                HealthyCount = query.Where(v => v.TCPTestStatus == "Established").Count(),
                                                                UnHealthyTenantsCount = query.Where(x => x.TCPTestStatus != "Established").Select(x => x.TenantId).Distinct().Count(),
                                                                UnHealthyCount = query.Where(v => v.TCPTestStatus != "Established").Count(),
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
                        query = query.Where(q => q.TCPTestStatus != "Established");
                    }
                    else if (request.Filter == HealthCheckFilterType.Active)
                    {
                        query = query.Where(q => q.TCPTestStatus == "Established");
                    }
                    else if (request.Filter == HealthCheckFilterType.Warnings)
                    {
                        query = query.Where(q => q.TCPTestStatus == "Warning");
                    }
                }

                var healthChecks = await query
                    .OrderBy(x => x.TenantName)
                    .ThenByDescending(x => x.RouterLastCommunication)
                    .ProjectTo<HealthCheckControllerModel>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);

                response.HealthChecks = healthChecks;

                return response;

            }
            catch (Exception ex)
            {

                throw;
            }
#pragma warning restore CS0168 // The variable 'ex' is declared but never used

        }
    }
}
