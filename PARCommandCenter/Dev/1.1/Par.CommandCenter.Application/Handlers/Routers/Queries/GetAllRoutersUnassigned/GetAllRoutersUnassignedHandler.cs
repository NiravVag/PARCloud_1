using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Routers.Queries.GetAllRoutersUnassigned
{
    public class GetAllRoutersUnassignedHandler : IRequestHandler<GetAllRoutersUnassignedQuery, GetAllRoutersUnassignedResponse>
    {
        private readonly ILogger<GetAllRoutersUnassignedHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllRoutersUnassignedHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetAllRoutersUnassignedHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetAllRoutersUnassignedResponse> Handle(GetAllRoutersUnassignedQuery request, CancellationToken cancellationToken)
        {
            var routers = await _dbContext.Routers
               .Where(r => r.TenantId == null)
               .Where(r => r.DeviceTypeId == 4)
               .Where(r => !r.Deleted)
               .OrderBy(r => r.Address)
               .ProjectTo<RouterModel>(_mapper.ConfigurationProvider)
               .ToListAsync(cancellationToken)
               .ConfigureAwait(false);


            return new GetAllRoutersUnassignedResponse
            {
                Routers = routers
            };
        }
    }
}
