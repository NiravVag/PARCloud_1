using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Par.CommandCenter.Application.Handlers.Interfaces.Queries.GetOutputDataQueue;
using System.Threading.Tasks;

namespace Par.CommandCenter.Web.Controllers.WebApi
{
    [Authorize]
    public class OutputDataQueueController : BaseController
    {
        public OutputDataQueueController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        [OpenApiOperation(nameof(GetByTenantId), "Retreive All input data Queue for a tenant by Tenant Id")]
        [Route("{tenantId}")]
        public async Task<IActionResult> GetByTenantId(int tenantId)
        {
            var response = await Mediator.Send(new GetOutputDataQueueQuery() { TenantId = tenantId });

            return Ok(response);
        }       
    }
}
