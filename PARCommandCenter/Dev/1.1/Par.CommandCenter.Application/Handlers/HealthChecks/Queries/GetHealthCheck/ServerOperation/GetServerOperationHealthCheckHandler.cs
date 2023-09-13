using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Queries.GetHealthCheck.ServerOperation
{
    public class GetServerOperationHealthCheckHandler : IRequestHandler<GetServerOperationHealthCheckQuery, GetServerOperationHealthCheckResponse>
    {
        private readonly ILogger<GetServerOperationHealthCheckHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;


        public GetServerOperationHealthCheckHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetServerOperationHealthCheckHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetServerOperationHealthCheckResponse> Handle(GetServerOperationHealthCheckQuery request, CancellationToken cancellationToken)
        {
            var response = new GetServerOperationHealthCheckResponse();

            var query = from hc in _dbContext.HealthCheckServerOperations
                        select new HealthCheckServerOperation
                        {
                            Id = hc.Id,
                            ServerName = hc.ServerName,
                            Status = hc.Status,
                            HealthCheckMessage = hc.HealthCheckMessage,
                            PreviousStatus = hc.PreviousStatus,
                            PreviousStatusDate = hc.PreviousStatusDate,
                            Created = hc.Created,
                            Modified = hc.Modified,
                        };

            var healthChecks = await query
                .OrderBy(x => x.ServerName)
                .ProjectTo<HealthCheckServerOperationModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);


            response.HealthChecks = healthChecks ?? Enumerable.Empty<HealthCheckServerOperationModel>();

            return response;
        }
    }
}
