using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Par.CommandCenter.Application.Handlers.Interfaces.Queries.GetOrderEventQueue;
using System.Threading.Tasks;

namespace Par.CommandCenter.Web.Controllers.WebApi
{
    [Authorize]
    public class OrderEventQueueController : BaseController
    {
        public OrderEventQueueController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        [OpenApiOperation(nameof(GetByTenantId), "Retreive all order event queue for a tenant by Tenant Id")]
        [Route("{tenantId}")]
        public async Task<IActionResult> GetByTenantId(int tenantId)
        {
            var response = await Mediator.Send(new GetOrderEventQueueQuery() { TenantId = tenantId });

            return Ok(response);
        }       
    }
}
