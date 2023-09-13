using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Facilities.Queries.GetFacilitiesByTenant
{
    public class GetFacilitiesByTenantHandler : IRequestHandler<GetFacilitiesByTenantQuery, GetFacilitiesByTenantResponse>
    {
        private readonly ILogger<GetFacilitiesByTenantHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetFacilitiesByTenantHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetFacilitiesByTenantHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetFacilitiesByTenantResponse> Handle(GetFacilitiesByTenantQuery request, CancellationToken cancellationToken)
        {
            var query = from f in _dbContext.Facilities
                        join tz in _dbContext.TimeZones on f.TimeZoneId equals tz.Id
                        join s in _dbContext.States on f.StateId equals s.Id into fx
                        from x in fx.DefaultIfEmpty()
                        select new Facility
                        {
                            Id = f.Id,
                            TenantId = f.TenantId,
                            Name = f.Name,
                            TimeZoneId = f.TimeZoneId,
                            TimeZone = tz,
                            VPNConnectionName = f.VPNConnectionName,
                            AddressLine1 = f.AddressLine1,
                            City = f.City,
                            StateId = f.StateId,
                            State = x,
                            PostalCode = f.PostalCode,
                            Deleted = f.Deleted,
                            Created = f.Created,
                            CreatedUserId = f.CreatedUserId,
                        };


            var facilities = await query
              .Where(r => r.TenantId == request.TenantId)
              .Where(r => !r.Deleted)
              .OrderBy(r => r.Name)
              .ProjectTo<FacilityModel>(_mapper.ConfigurationProvider)
              .ToListAsync(cancellationToken)
              .ConfigureAwait(false);


            return new GetFacilitiesByTenantResponse
            {
                Facilities = facilities
            };
        }
    }
}
