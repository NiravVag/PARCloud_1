using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Par.CommandCenter.Application.Handlers.Controllers.Commands.Ping;
using Par.CommandCenter.Application.Handlers.Controllers.Commands.Upsert;
using Par.CommandCenter.Application.Handlers.Controllers.Queries.GetController;
using Par.CommandCenter.Application.Handlers.Controllers.Queries.GetControllerPortName;
using Par.CommandCenter.Application.Handlers.Controllers.Queries.GetControllersByTenant;
using System.Threading.Tasks;

namespace Par.CommandCenter.Web.Controllers.WebApi
{
    [Authorize]
    public class ControllerController : BaseController
    {
        public ControllerController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        [OpenApiOperation(nameof(GetByTenantId), "Retreive All Controllers for a tenant by Tenant Id", "Retreive All Controllers for a tenant by Tenant Id")]
        [Route("{tenantId}")]
        public async Task<IActionResult> GetByTenantId(int tenantId)
        {
            var response = await Mediator.Send(new GetControllersByTenantQuery() { TenantId = tenantId });

            return Ok(response);
        }

        [HttpPost]
        [OpenApiOperation(nameof(Upsert), "Insert and update Controllers information in the backend data store")]
        public async Task<IActionResult> Upsert([FromBody] UpsertControllerCommand model)
        {
            var response = await Mediator.Send(model);

            return Ok(response);
        }

        [HttpGet]
        [OpenApiOperation(nameof(GetPortName), "Retreive controller port name")]
        [Route("{routerId}/{controllerTypeId}")]
        public async Task<IActionResult> GetPortName(int routerId, int controllerTypeId)
        {
            var response = await Mediator.Send(new GetControllerPortNameQuery() { RouterId = routerId, ControllerTypeId = controllerTypeId });

            return Ok(response);
        }

        [HttpPost]
        [OpenApiOperation(nameof(PingCloudController), "Ping cloud controller and retrieve the ping result.")]
        public async Task<IActionResult> PingCloudController([FromBody] PingControllerCommand model)
        {
            var response = await Mediator.Send(model);

            return Ok(response);
        }

        [HttpGet]
        [OpenApiOperation(nameof(GetById), "Retreive a Controller by Id")]
        [Route("{controllerId}")]
        [Route("{controllerId}/{includeScales}")]
        public async Task<IActionResult> GetById(int controllerId, bool includeScales = false)
        {
            var response = await Mediator.Send(new GetControllerQuery() { ControllerId = controllerId, IncludeScales = includeScales });

            return Ok(response);
        }

        [HttpGet]
        [OpenApiOperation(nameof(GetByRouterId), "Retreive all controllers by Router Id")]
        [Route("{routerId}")]
        [Route("{routerId}/{includeScales}")]
        public async Task<IActionResult> GetByRouterId(int routerId, bool includeScales = false)
        {
            var response = await Mediator.Send(new GetControllerQuery() { routerId = routerId, IncludeScales = includeScales });

            return Ok(response);
        }
    }
}
