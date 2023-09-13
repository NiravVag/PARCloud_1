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

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Queries.GetHealthCheck.Scale
{
    public class GetScaleHealthCheckHandler : IRequestHandler<GetScaleHealthCheckQuery, GetScaleHealthCheckResponse>
    {
        private readonly ILogger<GetScaleHealthCheckHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetScaleHealthCheckHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetScaleHealthCheckHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetScaleHealthCheckResponse> Handle(GetScaleHealthCheckQuery request, CancellationToken cancellationToken)
        {
            if (request.IncludeStatistics && request.StatisticsOnly)
            {
                throw new ArgumentException("The IncludeStatistics and StatisticsOnly can't both be a true value at the same time. Set only one to true at a time");
            }

            var response = new GetScaleHealthCheckResponse();

            ScaleHealthCheckStatisticModel statistics = null;

            IQueryable<HealthCheckScale> query;

            if (request.HealthCheckIds?.Any() ?? false)
            {
                query = from hc in _dbContext.HealthCheckScales
                        join s in _dbContext.Scales on hc.ScaleId equals s.Id
                        join c in _dbContext.Controllers on s.ControllerId equals c.Id
                        join t in _dbContext.Tenants on s.TenantId equals t.Id
                        join r in _dbContext.Routers on c.RouterId equals r.Id
                        join b in _dbContext.Bins on s.BinId equals b.Id into bx
                        from b in bx.DefaultIfEmpty()
                        join li in _dbContext.LocationItems on b.LocationItemId equals li.Id into lix
                        from li in lix.DefaultIfEmpty()
                        join i in _dbContext.Items on li.ItemId equals i.Id into ix
                        from i in ix.DefaultIfEmpty()
                        join l in _dbContext.Locations on li.LocationId equals l.Id into lx
                        from l in lx.DefaultIfEmpty()
                        join f in _dbContext.Facilities on l.FacilityId equals f.Id into fx
                        from f in fx.DefaultIfEmpty()
                        where request.HealthCheckIds.Contains(hc.Id) && c.Active == true
                        select new HealthCheckScale
                        {
                            Id = hc.Id,
                            ScaleId = hc.ScaleId,
                            ScaleAdress = s.Address,
                            TenantName = t.Name,
                            Status = hc.Status,
                            PreviousStatus = hc.PreviousStatus,
                            PreviousStatusDate = hc.PreviousStatusDate,
                            LastCommunication = hc.LastCommunication,
                            LastReboot = hc.LastReboot,
                            Created = hc.Created,
                            Modified = hc.Modified,
                            TenantId = (int)r.TenantId,
                            BinId = s.BinId,
                            Location = l,
                            Item = (i == null) ? null : new Item
                            {
                                Id = i.Id,
                                Name = i.Name,
                                Number = i.Number
                            },
                        };

            }
            else
            {
                query = from hc in _dbContext.HealthCheckScales
                        join s in _dbContext.Scales on hc.ScaleId equals s.Id
                        join c in _dbContext.Controllers on s.ControllerId equals c.Id
                        join t in _dbContext.Tenants on s.TenantId equals t.Id
                        join r in _dbContext.Routers on c.RouterId equals r.Id
                        join b in _dbContext.Bins on s.BinId equals b.Id into bx
                        from b in bx.DefaultIfEmpty()
                        join li in _dbContext.LocationItems on b.LocationItemId equals li.Id into lix
                        from li in lix.DefaultIfEmpty()
                        join i in _dbContext.Items on li.ItemId equals i.Id into ix
                        from i in ix.DefaultIfEmpty()
                        join l in _dbContext.Locations on li.LocationId equals l.Id into lx
                        from l in lx.DefaultIfEmpty()
                        join f in _dbContext.Facilities on l.FacilityId equals f.Id into fx
                        from f in fx.DefaultIfEmpty()
                        where request.TenantIds.Contains(t.Id) && c.Active == true
                        select new HealthCheckScale
                        {
                            Id = hc.Id,
                            ScaleId = hc.ScaleId,
                            ScaleAdress = s.Address,
                            TenantName = t.Name,
                            Status = hc.Status,
                            PreviousStatus = hc.PreviousStatus,
                            PreviousStatusDate = hc.PreviousStatusDate,
                            LastCommunication = hc.LastCommunication,
                            LastReboot = hc.LastReboot,
                            Created = hc.Created,
                            Modified = hc.Modified,
                            TenantId = (int)r.TenantId,
                            BinId = s.BinId,
                            Location = l,
                            Item = (i == null) ? null : new Item
                            {
                                Id = i.Id,
                                Name = i.Name,
                                Number = i.Number
                            },
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

                    //var count = query.Where(v => v.Status == "Need Calibration");

                    statistics = (await Task.FromResult(from hhr in new List<string> { "X" }
                    //join hhc in _dbContext.HealthCheckScales on 1 equals 1 into hc
                    //join uhhc in _dbContext.HealthCheckScales on 1 equals 1 into uhc
                                                        select new ScaleHealthCheckStatisticModel
                                                        {
                                                            Type = HealthCheckType.Scales,
                                                            TotalTenantsCount = tenantsCount,
                                                            HealthyTenantsCount = query.Where(x => x.Status == "Online").Select(x => x.TenantId).Distinct().Count(),
                                                            HealthyCount = query.Where(v => v.Status == "Online").Count(),
                                                            UnHealthyTenantsCount = query.Where(x => x.Status != "Online").Select(x => x.TenantId).Distinct().Count(),
                                                            UnHealthyCount = query.Where(v => v.Status != "Online").Count(),
                                                            WarrningTenantsCount = 0,
                                                            WarrningCount = 0,
                                                            ScalesMissingCalibrationCount = query.Where(v => v.Status == "Need Calibration").Count(),
                                                        })).FirstOrDefault();

                    response.Statistics = statistics;

                    if (request.StatisticsOnly)
                    {
                        return response;
                    }
                }


                if (request.Filter == ScaleHealthCheckFilterType.Errors)
                {
                    query = query.Where(q => q.Status != "Online");
                }
                else if (request.Filter == ScaleHealthCheckFilterType.Active)
                {
                    query = query.Where(q => q.Status == "Online");
                }
                else if (request.Filter == ScaleHealthCheckFilterType.Warnings)
                {
                    query = query.Where(q => q.Status == "Warning");
                }
                else if (request.Filter == ScaleHealthCheckFilterType.NeedCalibration)
                {
                    query = query.Where(q => q.Status == "Need Calibration");
                }
            }

            var healthChecks = await query
                .OrderBy(x => x.TenantName)
                .ThenByDescending(x => x.LastCommunication)
                .ProjectTo<HealthCheckScaleModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            response.HealthChecks = healthChecks;

            return response;
        }
    }
}
