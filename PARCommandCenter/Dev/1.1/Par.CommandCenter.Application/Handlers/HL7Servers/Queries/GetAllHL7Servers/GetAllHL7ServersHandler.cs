using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.HL7Servers.Queries.GetAllHL7Servers
{
    public class GetAllHL7ServersHandler : IRequestHandler<GetAllHL7ServersQuery, GetAllHL7ServersResponse>
    {
        private readonly ILogger<GetAllHL7ServersHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllHL7ServersHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetAllHL7ServersHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetAllHL7ServersResponse> Handle(GetAllHL7ServersQuery request, CancellationToken cancellationToken)
        {
            var query = from s in _dbContext.HL7Servers
                        join cs in _dbContext.HL7CloudServers on s.HL7CloudServerId equals cs.Id
                        join t in _dbContext.Tenants on s.TenantId equals t.Id
                        join f in _dbContext.Facilities on s.FacilityId equals f.Id into fx
                        from x in fx.DefaultIfEmpty()
                        select new HL7CloudServerModel
                        {
                            Id = s.Id,
                            TenantName = t.Name,
                            FacilityName = x.Name,
                            CloudServerId = cs.Id,
                            CloudServerAddress = cs.Address,
                            Port = s.Port,
                            MaxPacketsPerMessage = s.MaxPacketsPerMessage,
                            IsActive = s.IsActive ?? false,
                        };

            var hl7CloudServers = await query
              .OrderBy(x => x.Id)
              .ThenBy(x => x.TenantName)
              .ThenBy(x => x.FacilityName)
              .ThenBy(x => x.CloudServerAddress)
              .ToListAsync(cancellationToken)
              .ConfigureAwait(false);


            return new GetAllHL7ServersResponse
            {
                HL7CloudServers = hl7CloudServers
            };
        }
    }
}
