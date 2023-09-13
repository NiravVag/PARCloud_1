using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.States.Queries.GetAllStates
{
    public class GetAllStatesHandler : IRequestHandler<GetAllStatesQuery, GetAllStatesResponse>
    {
        private readonly ILogger<GetAllStatesHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllStatesHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetAllStatesHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetAllStatesResponse> Handle(GetAllStatesQuery request, CancellationToken cancellationToken)
        {
            var states = await _dbContext.States
                .OrderBy(t => t.Name)
                .ProjectTo<StateModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);


            return new GetAllStatesResponse
            {
                States = states
            };
        }
    }
}
