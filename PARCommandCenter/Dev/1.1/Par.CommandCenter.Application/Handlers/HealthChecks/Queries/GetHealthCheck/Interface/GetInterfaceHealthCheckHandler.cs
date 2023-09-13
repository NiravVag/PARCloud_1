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

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Queries.GetHealthCheck.Interface
{
    public class GetInterfaceHealthCheckHandler : IRequestHandler<GetInterfaceHealthCheckQuery, GetInterfaceHealthCheckResponse>
    {
        private readonly ILogger<GetInterfaceHealthCheckHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetInterfaceHealthCheckHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetInterfaceHealthCheckHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetInterfaceHealthCheckResponse> Handle(GetInterfaceHealthCheckQuery request, CancellationToken cancellationToken)
        {
            if (request.IncludeStatistics && request.StatisticsOnly)
            {
                throw new ArgumentException("The IncludeStatistics and StatisticsOnly can't both be a true value at the same time. Set only one to true at a time");
            }

            var response = new GetInterfaceHealthCheckResponse();
            HealthCheckStatisticModel statistics = null;

            var queryOrder = from hcoi in _dbContext.HealthCheckOrderInterfaces
                             join oeo in _dbContext.OrderEventOutputs on hcoi.EventOutputId equals oeo.Id
                             join t in _dbContext.Tenants on oeo.TenantId equals t.Id
                             join es in _dbContext.ExternalSystems on new { Id = oeo.ExternalSystemId, oeo.TenantId } equals new { es.Id, es.TenantId }
                             where request.TenantIds.Contains(t.Id)
                             select new HealthCheckInterface
                             {
                                 Id = hcoi.Id,
                                 EventOutputId = hcoi.EventOutputId,
                                 Status = hcoi.Status,
                                 Modified = hcoi.Modified,
                                 TenantName = t.Name,
                                 InterfaceType = "Order Interface",
                                 ExternalSystemName = es.Name,
                                 FileName = oeo.FileName,
                                 FileLocation = oeo.FileLocation,
                                 MimeType = oeo.MimeType,
                                 Sent = oeo.Sent,
                                 ErrorMessage = oeo.ErrorMessage,
                                 Published = oeo.Published,
                                 Started = oeo.Started,
                             };

            var queryInventory = from hcii in _dbContext.HealthCheckInventoryInterfaces
                                 join ieo in _dbContext.InventoryEventOutputs on hcii.EventOutputId equals ieo.Id
                                 join t in _dbContext.Tenants on ieo.TenantId equals t.Id
                                 join es in _dbContext.ExternalSystems on new { Id = ieo.ExternalSystemId, ieo.TenantId } equals new { es.Id, es.TenantId }
                                 where request.TenantIds.Contains(t.Id)
                                 select new HealthCheckInterface
                                 {
                                     Id = hcii.Id,
                                     EventOutputId = hcii.EventOutputId,
                                     Status = hcii.Status,
                                     Modified = hcii.Modified,
                                     TenantName = t.Name,
                                     InterfaceType = "Inventory Interface",
                                     ExternalSystemName = es.Name,
                                     FileName = ieo.FileName,
                                     FileLocation = ieo.FileLocation,
                                     MimeType = ieo.MimeType,
                                     Sent = ieo.Sent,
                                     ErrorMessage = ieo.ErrorMessage,
                                     Published = ieo.Published,
                                     Started = ieo.Started,
                                 };

            switch (request.DateRangeFilter)
            {
                case DateRangeFilterType.Past24Hours:
                    queryOrder = queryOrder.Where(x => x.Published >= DateTimeOffset.UtcNow.AddDays(-1));
                    queryInventory = queryInventory.Where(x => x.Published >= DateTimeOffset.UtcNow.AddDays(-1));
                    break;
                case DateRangeFilterType.Past3Days:
                    queryOrder = queryOrder.Where(x => x.Published >= DateTimeOffset.UtcNow.AddDays(-3));
                    queryInventory = queryInventory.Where(x => x.Published >= DateTimeOffset.UtcNow.AddDays(-3));
                    break;
                case DateRangeFilterType.Past7Days:
                    queryOrder = queryOrder.Where(x => x.Published >= DateTimeOffset.UtcNow.AddDays(-7));
                    queryInventory = queryInventory.Where(x => x.Published >= DateTimeOffset.UtcNow.AddDays(-7));
                    break;
                case DateRangeFilterType.Past30Days:
                    queryOrder = queryOrder.Where(x => x.Published >= DateTimeOffset.UtcNow.AddDays(-30));
                    queryInventory = queryInventory.Where(x => x.Published >= DateTimeOffset.UtcNow.AddDays(-30));
                    break;
                case DateRangeFilterType.CustomDate:
                    if (request.StartDate == null || request.EndDate == null)
                    {
                        throw new ArgumentNullException($"The {nameof(request.StartDate)} and the {nameof(request.EndDate)} can't be null");
                    }

                    queryOrder = queryOrder.Where(x => x.Published >= request.StartDate && x.Published <= request.EndDate);
                    queryInventory = queryInventory.Where(x => x.Published >= request.StartDate && x.Published <= request.EndDate);

                    break;
                default:
                    queryOrder = queryOrder.Where(x => x.Published >= DateTimeOffset.UtcNow.AddDays(-1));
                    queryInventory = queryInventory.Where(x => x.Published >= DateTimeOffset.UtcNow.AddDays(-1));
                    break;
            }

            var query = queryOrder.Union(queryInventory);

            if (request.StatisticsOnly || request.IncludeStatistics)
            {
                var tenantsCount = await (from t in _dbContext.Tenants
                                          where t.Deleted == false
                                          select new Tenant
                                          {
                                              Id = t.Id,
                                              Deleted = t.Deleted,
                                          }).CountAsync();

                statistics = new HealthCheckStatisticModel
                {
                    Type = HealthCheckType.Interface,
                    TotalTenantsCount = tenantsCount,
                    HealthyTenantsCount = await query.Where(x => x.Status == "Sent").Select(x => x.TenantName).Distinct().CountAsync(),
                    HealthyCount = await query.Where(x => x.Status == "Sent" /*&& x.Published >= DateTimeOffset.UtcNow.AddHours(-24)*/).CountAsync(),
                    UnHealthyTenantsCount = await query.Where(x => x.Status == "Error").Select(x => x.TenantName).Distinct().CountAsync(),
                    UnHealthyCount = await query.Where(x => x.Status == "Error").CountAsync(),
                    WarrningTenantsCount = 0,
                    WarrningCount = 0,
                };

                response.Statistics = statistics;

                if (request.StatisticsOnly)
                {
                    return response;
                }
            }

            if (request.Filter == HealthCheckFilterType.Errors)
            {
                query = query.Where(q => q.Status == "Error");
            }
            else if (request.Filter == HealthCheckFilterType.Active)
            {
                query = query.Where(q => q.Status == "Sent");
                //query = query.Where(q => q.Published >= DateTimeOffset.UtcNow.AddHours(-24));
            }
            else if (request.Filter == HealthCheckFilterType.Warnings)
            {
                query = query.Where(q => q.Status == "Warning");
            }

            var healthChecks = await query
                .OrderByDescending(t => t.Published)
                //.Take(2000)                         
                .ProjectTo<HealthCheckInterfaceModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            response.HealthChecks = healthChecks;

            return response;
        }
    }
}
