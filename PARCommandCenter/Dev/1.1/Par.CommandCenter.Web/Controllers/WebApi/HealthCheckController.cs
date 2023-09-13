using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Par.CommandCenter.Application.Handlers.HealthChecks.Queries.FacilityVPNConnections;
using Par.CommandCenter.Application.Handlers.HealthChecks.Queries.GetHealthCheck.Controller;
using Par.CommandCenter.Application.Handlers.HealthChecks.Queries.GetHealthCheck.Interface;
using Par.CommandCenter.Application.Handlers.HealthChecks.Queries.GetHealthCheck.Router;
using Par.CommandCenter.Application.Handlers.HealthChecks.Queries.GetHealthCheck.Scale;
using Par.CommandCenter.Application.Handlers.HealthChecks.Queries.GetHealthCheck.ServerOperation;
using Par.CommandCenter.Application.Handlers.HealthChecks.Queries.GetHealthCheck.VPN;
using Par.CommandCenter.Application.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Par.CommandCenter.Web.Controllers.WebApi
{

    [Authorize]
    public class HealthCheckController : BaseController
    {
        private readonly ICurrentUserService _currentUserService;


        public readonly IApplicationDbContext _dbContext;

        public HealthCheckController(IMediator mediator, IApplicationDbContext dbContext, ICurrentUserService currentUserService)
            : base(mediator)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
        }

        [HttpPost]
        [OpenApiOperation(nameof(VPNs), "Retrieve VPNs Health Check information for the supplied filter", "Retrieve Order Interfaces Health Check information for the supplied filter")]
        public async Task<IActionResult> VPNs([FromBody] GetVPNHealthCheckQuery query)
        {
            query.TenantIds = _currentUserService.TenantIds;

            var response = await Mediator.Send(query);

            return Ok(response);
        }

        [HttpPost]
        [OpenApiOperation(nameof(Interfaces), "Retrieve Interfaces Health Check information for the supplied filter", "Retrieve Order Interfaces Health Check information for the supplied filter")]
        public async Task<IActionResult> Interfaces([FromBody] GetInterfaceHealthCheckQuery query)
        {
            query.TenantIds = _currentUserService.TenantIds;

            var response = await Mediator.Send(query);

            return Ok(response);
        }

        [HttpPost]
        [OpenApiOperation(nameof(Routers), "Retrieve Routers Health Check information for the supplied filter", "Retrieve Routers Health Check information for the supplied filter")]
        public async Task<IActionResult> Routers([FromBody] GetRouterHealthCheckQuery query)
        {
            query.TenantIds = _currentUserService.TenantIds;

            var response = await Mediator.Send(query);

            return Ok(response);
        }

        [HttpPost]
        [OpenApiOperation(nameof(Controllers), "Retrieve Controllers Health Check information for the supplied filter", "Retrieve Controllers Health Check information for the supplied filter")]
        public async Task<IActionResult> Controllers([FromBody] GetControllerHealthCheckQuery query)
        {
            query.TenantIds = _currentUserService.TenantIds;

            var response = await Mediator.Send(query);

            return Ok(response);
        }

        [HttpPost]
        [OpenApiOperation(nameof(Scales), "Retrieve Scales Health Check information for the supplied filter", "Retrieve Scales Health Check information for the supplied filter")]
        public async Task<IActionResult> Scales([FromBody] GetScaleHealthCheckQuery query)
        {
            query.TenantIds = _currentUserService.TenantIds;

            var response = await Mediator.Send(query);

            return Ok(response);
        }

        [HttpGet]
        [OpenApiOperation(nameof(FTPServer), "Retrieve FTP Server Health Check information for the supplied filter")]
        public async Task<IActionResult> FTPServer([FromQuery] GetServerOperationHealthCheckQuery query)
        {
            var response = await Mediator.Send(query);

            if (response.HealthChecks.Any(x => x.ServerName.Contains("FTP Server") && x.Status == "Connected"))
            {
                return Ok(new { ftpServerStatus = true });
            }

            return Ok(new { ftpServerStatus = false });
        }

        [HttpGet]
        [OpenApiOperation(nameof(GetFacilityVPNConnections), "Retrieve a list of VPN connections that are avaiable to associate with a given facility Id")]
        public async Task<IActionResult> GetFacilityVPNConnections([FromQuery] GetFacilityVPNConnectionsQuery query)
        {
            var response = await Mediator.Send(query);

            return Ok(response);
        }
    }
}