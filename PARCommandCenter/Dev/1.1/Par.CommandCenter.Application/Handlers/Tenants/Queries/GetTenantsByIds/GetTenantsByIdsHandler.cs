using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Handlers.Scales.Queries.GetScalesCountByTenantIds;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Tenants.Queries.GetTenantsByIds
{
    public class GetTenantsByIdsHandler : IRequestHandler<GetTenantsByIdsQuery, GetTenantsByIdsResponse>
    {
        private readonly ILogger<GetTenantsByIdsHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        private readonly IMediator _mediator;

        public GetTenantsByIdsHandler(IApplicationDbContext dbContext,  IMapper mapper, IMediator Mediator, ILogger<GetTenantsByIdsHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _mediator = Mediator;
            _logger = logger;
        }

        public async Task<GetTenantsByIdsResponse> Handle(GetTenantsByIdsQuery request, CancellationToken cancellationToken)
        {
            var query = from t in _dbContext.Tenants
                        join tz in _dbContext.TimeZones on t.DefaultTimeZoneId
                        equals tz.Id
                        where request.TenantIds.Contains(t.Id)
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

            if(request.OptionalField == OptionalField.ScalesCount)
            {
                var response = await _mediator.Send(new GetScalesCountByTenantIdsQuery() { TenantIds = request.TenantIds} );

                foreach (var item in tenants)
                {
                    item.ScalesCount = response.TenantScalesCount.FirstOrDefault(x => x.TenantId == item.Id)?.ScalesCount ?? 0;
                }
            }

            return new GetTenantsByIdsResponse
            {
                Tenants = tenants
            };
        }
    }
}
