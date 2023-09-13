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

namespace Par.CommandCenter.Application.Handlers.Scales.Queries.GetScalesByTenant
{
    public class GetScalesByTenantHandler : IRequestHandler<GetScalesByTenantQuery, GetScalesByTenantResponse>
    {
        private readonly ILogger<GetScalesByTenantHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetScalesByTenantHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetScalesByTenantHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetScalesByTenantResponse> Handle(GetScalesByTenantQuery request, CancellationToken cancellationToken)
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
                            Address = s.Address,
                            ControllerId = s.ControllerId,
                            ScaleWeight = s.ScaleWeight,
                            LastCommunication = s.LastCommunication,
                            BinId = s.BinId,
                            Location = l,
                            Item = (i == null) ? null : new Item
                            {
                                Id = i.Id,
                                Name = i.Name,
                                Number = i.Number
                            },
                            Controller = new Controller
                            {
                                Id = c.Id,
                                IpAddress = c.IpAddress,
                            },
                        };


            var scales = await query
              .Where(s => s.TenantId == request.TenantId)
              .OrderBy(s => s.Location)
              .ThenBy(s => s.Address)
              .ThenBy(s => s.Item.Name)
              .ProjectTo<ScaleModel>(_mapper.ConfigurationProvider)
              .ToListAsync(cancellationToken)
              .ConfigureAwait(false);

            return new GetScalesByTenantResponse
            {
                Scales = scales
            };
        }
    }
}
