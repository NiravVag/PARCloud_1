using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Tenants.Queries.GetTenantsSummary
{
    public class GetTenantsSummaryHandler : IRequestHandler<GetTenantsSummaryQuery, GetTenantsSummaryResponse>
    {
        private readonly ILogger<GetTenantsSummaryHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        readonly IServiceScopeFactory _serviceScopeFactory;

        public GetTenantsSummaryHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetTenantsSummaryHandler> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<GetTenantsSummaryResponse> Handle(GetTenantsSummaryQuery request, CancellationToken cancellationToken)
        {
            object locker = new object();
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            IEnumerable<TenantSummaryModel> result = new List<TenantSummaryModel>();

            var query = from t in _dbContext.Tenants
                        where !t.IsTest && !t.Deleted
                        orderby t.Id
                        select new Tenant
                        {
                            Id = t.Id,
                            Name = t.Name,                          
                        };

            var tenants = await query.ToListAsync();

            var tasks = tenants.Select(async tenant =>
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopeContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                    var vmsSummary = await (from r in scopeContext.Routers
                                    join vm in scopeContext.VirtualMachines on r.VirtualMachineId equals vm.Id
                                    where r.TenantId == tenant.Id
                                    group vm by new { vm.Id, vm.ComputerName } into grouped
                                       select new TenantVmSummaryModel()
                                       {
                                           Id= grouped.Key.Id,
                                           ComputerName = grouped.Key.ComputerName,
                                           TotalRouters = grouped.Count()
                                       }).ToListAsync();

                    var summary = await scopeContext.Tenants.DefaultIfEmpty().Select(t => new TenantSummaryModel()
                    {
                        Id = tenant.Id,
                        Name = tenant.Name,
                        TotalFacilities = scopeContext.Facilities.Count(l => l.TenantId == tenant.Id),
                        TotalLocations = scopeContext.Locations.Count(l => l.TenantId == tenant.Id),
                        TotalRouters = scopeContext.Routers.Count(r => r.TenantId == tenant.Id),
                        TotalControllers = scopeContext.Controllers.Count(c => c.TenantId == tenant.Id),
                        ReplenishControllers = scopeContext.Controllers.Count(c => c.TenantId == tenant.Id && c.ParChargeMode == false),
                        ChargeControllers = scopeContext.Controllers.Count(c => c.TenantId == tenant.Id && c.ParChargeMode == true),
                        TotalScales = scopeContext.Scales.Count(c => c.TenantId == tenant.Id),
                        OfflineRouters = scopeContext.Routers.Count(r => r.TenantId == tenant.Id && r.LastCommunication < DateTimeOffset.UtcNow.AddHours(-1)),
                        OfflineControllers = scopeContext.Controllers.Count(c => c.TenantId == tenant.Id && c.Active == false),
                        OfflineScales = scopeContext.Scales.Count(s => s.TenantId == tenant.Id && s.LastCommunication < DateTimeOffset.UtcNow.AddHours(-1)),
                        AzureVmsSummary = vmsSummary
                    }).FirstAsync();                    

                    lock (locker)
                        result = result.Append(summary);
                }
            });


            await Task.WhenAll(tasks);

            watch.Stop();

            Console.WriteLine($"Execution time for GetTenantsSummaryHandler:{watch.ElapsedMilliseconds} milliseconds");           


            return new GetTenantsSummaryResponse
            {
                TenantsSummary = result.OrderByDescending(t => t.TotalScales),
            };
        }
    }
}
