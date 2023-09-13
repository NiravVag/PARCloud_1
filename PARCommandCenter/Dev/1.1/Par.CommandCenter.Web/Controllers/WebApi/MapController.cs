using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSwag.Annotations;
using Par.CommandCenter.Application.Handlers.Map.Queries.GetHealthCheckMapPoints;
using Par.CommandCenter.Application.Interfaces;
using System.Threading.Tasks;

namespace Par.CommandCenter.Web.Controllers.WebApi
{
    [Authorize]
    public class MapController : BaseController
    {
        private readonly ICurrentUserService _currentUserService;

        public readonly IConfiguration _configuration;

        public readonly IApplicationDbContext _dbContext;

        public MapController(IMediator mediator, IApplicationDbContext dbContext, ICurrentUserService currentUserService, IConfiguration configuration) : base(mediator)
        {
            _currentUserService = currentUserService;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpGet]
        [OpenApiOperation(nameof(Get), "Retreive health check map data for all tenant's")]
        public async Task<GetHealthCheckMapPointsResponse> Get([FromQuery] GetHealthCheckMapPointsQuery query)
        {
            var response = await Mediator.Send(query);

            return response;
        }
    }
}
