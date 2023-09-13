using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Tenants.Queries.GetAllTenants
{
    public class GetAllTenantsHandler : IRequestHandler<GetAllTenantsQuery, GetAllTenantsResponse>
    {
        private readonly ILogger<GetAllTenantsHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllTenantsHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetAllTenantsHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetAllTenantsResponse> Handle(GetAllTenantsQuery request, CancellationToken cancellationToken)
        {
            var query = from t in _dbContext.Tenants
                        join tz in _dbContext.TimeZones on t.DefaultTimeZoneId
                        equals tz.Id
                        select new Tenant
                        {
                            Id = t.Id,
                            Name = t.Name,
                            OrderBoxPercentage = t.OrderBoxPercentage,
                            ParMobileAllowRememberMe = t.ParMobileAllowRememberMe,
                            TimeZone = tz,
                            Deleted = t.Deleted,
                            IsTest = t.IsTest,
                        };



            var tenants = await query
                .Where(t => !t.Deleted)
                .OrderBy(t => t.Name)
                .ProjectTo<TenantModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);


            return new GetAllTenantsResponse
            {
                Tenants = tenants
            };
        }
    }
}
