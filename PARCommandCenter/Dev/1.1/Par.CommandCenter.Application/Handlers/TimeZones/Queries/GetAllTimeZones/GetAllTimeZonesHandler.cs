using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.TimeZones.Queries.GetAllTimeZones
{
    public class GetAllTimeZonesHandler : IRequestHandler<GetAllTimeZonesQuery, GetAllTimeZonesResponse>
    {
        private readonly ILogger<GetAllTimeZonesHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllTimeZonesHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetAllTimeZonesHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetAllTimeZonesResponse> Handle(GetAllTimeZonesQuery request, CancellationToken cancellationToken)
        {
            var TimeZones = await _dbContext.TimeZones
                .OrderBy(t => t.Name)
                .ProjectTo<TimeZoneModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);


            return new GetAllTimeZonesResponse
            {
                TimeZones = TimeZones
            };
        }
    }
}
