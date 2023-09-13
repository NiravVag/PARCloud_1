using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Par.CommandCenter.Application.Handlers.Interfaces.Queries.GetInventoryEventQueue;
using System.Threading.Tasks;

namespace Par.CommandCenter.Web.Controllers.WebApi
{
    [Authorize]
    public class InventoryEventQueueController : BaseController
    {
        public InventoryEventQueueController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        [OpenApiOperation(nameof(GetByTenantId), "Retreive all inventory event queue items for a tenant")]
        [Route("{tenantId}")]
        public async Task<IActionResult> GetByTenantId(int tenantId)
        {
            var response = await Mediator.Send(new GetInventoryEventQueueQuery() { TenantId = tenantId });

            return Ok(response);
        }
    }
}
