using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Par.CommandCenter.Application.Handlers.Interfaces.Queries.GetJobQueue;
using Par.CommandCenter.Application.Handlers.Queues.Commands.DeleteQueueJob;
using Par.CommandCenter.Application.Handlers.Queues.Commands.ResetQueueJob;
using System;
using System.Threading.Tasks;

namespace Par.CommandCenter.Web.Controllers.WebApi
{
    [Authorize]
    public class JobQueueController : BaseController
    {
        public JobQueueController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        [OpenApiOperation(nameof(GetByTenantId), "Retreive All Job Queue for a tenant by Tenant Id")]
        [Route("{tenantId}")]
        public async Task<IActionResult> GetByTenantId(int tenantId)
        {
            var response = await Mediator.Send(new GetInputDataQueueQuery() { TenantId = tenantId });

            return Ok(response);
        }

        [HttpPost]
        [OpenApiOperation(nameof(Reset), "Rest job queue entry")]
        public async Task<IActionResult> Reset(ResetQueueJobCommand resetJobRequest)
        {
            var result = await Mediator.Send(resetJobRequest);

            return Ok(result);
        }

        [HttpDelete]
        [OpenApiOperation(nameof(Reset), "Delete job queue entry")]
        public async Task<IActionResult> Delete(DeleteQueueJobCommand deleteJobRequest)
        {
            var result = await Mediator.Send(deleteJobRequest);

            return Ok(result);
        }
    }
}
