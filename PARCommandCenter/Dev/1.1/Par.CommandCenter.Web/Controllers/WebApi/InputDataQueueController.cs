using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Par.CommandCenter.Application.Handlers.Interfaces.Queries.GetInputDataQueue;
using Par.CommandCenter.Application.Handlers.Queues.Commands.DeleteQueueJob;
using Par.CommandCenter.Application.Handlers.Queues.Commands.ResetQueueJob;
using System;
using System.Threading.Tasks;

namespace Par.CommandCenter.Web.Controllers.WebApi
{
    [Authorize]
    public class InputDataQueueController : BaseController
    {
        public InputDataQueueController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        [OpenApiOperation(nameof(GetByTenantId), "Retreive All input data Queue for a tenant by Tenant Id")]
        [Route("{tenantId}")]
        public async Task<IActionResult> GetByTenantId(int tenantId)
        {
            var response = await Mediator.Send(new GetInputDataQueueQuery() { TenantId = tenantId });

            return Ok(response);
        }       
    }
}
