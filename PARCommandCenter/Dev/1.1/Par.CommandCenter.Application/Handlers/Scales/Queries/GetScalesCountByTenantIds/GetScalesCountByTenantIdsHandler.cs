using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Scales.Queries.GetScalesCountByTenantIds
{
    public class GetScalesCountByTenantIdsHandler : IRequestHandler<GetScalesCountByTenantIdsQuery, GetScalesCountByTenantIdsResponse>
    {
        private readonly ILogger<GetScalesCountByTenantIdsHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetScalesCountByTenantIdsHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetScalesCountByTenantIdsHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetScalesCountByTenantIdsResponse> Handle(GetScalesCountByTenantIdsQuery request, CancellationToken cancellationToken)
        {
            var query = from s in _dbContext.Scales
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
                        select new Scale
                        {
                            Id = s.Id,
                            TenantId = s.TenantId,                           
                        };                       


            var scales = await query
              .Distinct()
              .Where(s => request.TenantIds.Contains(s.TenantId))                            
              .ToListAsync(cancellationToken)
              .ConfigureAwait(false);

            var scaleCounts = scales.GroupBy(x => x.TenantId).Select(x => new TenantScalesCountModel() { TenantId = x.Key, ScalesCount = x.Count() });           

            return new GetScalesCountByTenantIdsResponse
            {
                TenantScalesCount = scaleCounts
            };
        }
    }
}
