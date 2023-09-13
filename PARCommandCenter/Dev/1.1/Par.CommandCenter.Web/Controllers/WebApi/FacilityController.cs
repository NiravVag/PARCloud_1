using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Par.CommandCenter.Application.Handlers.Facilities.Commands.Upsert;
using Par.CommandCenter.Application.Handlers.Facilities.Queries.GetFacilitiesByTenant;
using System.Threading.Tasks;

namespace Par.CommandCenter.Web.Controllers.WebApi
{
    [Authorize]
    public class FacilityController : BaseController
    {
        public FacilityController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        [OpenApiOperation(nameof(GetByTenantId), "Retreive All Facilities for a tenant by Tenant Id", "Retreive All Facilities for a tenant by Tenant Id")]
        [Route("{tenantId}")]
        public async Task<IActionResult> GetByTenantId(int tenantId)
        {
            var response = await Mediator.Send(new GetFacilitiesByTenantQuery() { TenantId = tenantId });

            return Ok(response);
        }

        [HttpPost]
        [OpenApiOperation(nameof(Upsert), "Insert and update facilities information in the backend data store")]
        public async Task<IActionResult> Upsert([FromBody] UpsertFacilityCommand model)
        {
            var response = await Mediator.Send(model);

            return Ok(response);
        }
    }
}
