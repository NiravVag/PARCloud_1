using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities.Users;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Users.Queries.GetCurrentUserApplicationTenants
{
    public class GetCurrentUserApplicationTenantsHandler : IRequestHandler<GetCurrentUserApplicationTenantsQuery, GetCurrentUserApplicationTenantsResponse>
    {
        private readonly ILogger<GetCurrentUserApplicationTenantsHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetCurrentUserApplicationTenantsHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IMapper mapper, ILogger<GetCurrentUserApplicationTenantsHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<GetCurrentUserApplicationTenantsResponse> Handle(GetCurrentUserApplicationTenantsQuery request, CancellationToken cancellationToken)
        {

            var query = from t in _dbContext.UserApplicationTenantSettings
                        where t.Deleted == false
                        where t.UserId == _currentUserService.UserId
                        select new UserApplicationTenantSetting
                        {
                            Id = t.Id,
                            TenantId = t.TenantId,
                            Deleted = t.Deleted,
                        };

            var userApplicationTenantSettings = await query
                .OrderBy(t => t.Id)
                .ProjectTo<UserApplicationTenantModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);


            return new GetCurrentUserApplicationTenantsResponse
            {
                UserApplicationTenants = userApplicationTenantSettings
            };
        }
    }
}
