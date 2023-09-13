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

namespace Par.CommandCenter.Application.Handlers.Users.Queries.GetAllCommandCenterUsers
{
    public class GetAllCommandCenterUsersHandler : IRequestHandler<GetAllCommandCenterUsersQuery, GetAllCommandCenterUsersResponse>
    {
        private readonly ILogger<GetAllCommandCenterUsersHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetAllCommandCenterUsersHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IMapper mapper, ILogger<GetAllCommandCenterUsersHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<GetAllCommandCenterUsersResponse> Handle(GetAllCommandCenterUsersQuery request, CancellationToken cancellationToken)
        {

            var query = from u in _dbContext.Users
                        where u.Deleted == false
                        where u.AzureAdObjectId != null
                        select new User
                        {
                            Id = u.Id,
                            UserName = u.UserName,
                            FirstName = u.FirstName,
                            LastName = u.LastName
                        };

            var ccUsers = await query
                .OrderBy(t => t.Id)
                .ProjectTo<UserModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);


            return new GetAllCommandCenterUsersResponse
            {
                Users = ccUsers
            };
        }
    }
}
