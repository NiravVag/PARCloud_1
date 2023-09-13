using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Par.CommandCenter.Application.Handlers.AuditLog.Queries;
using System.Threading.Tasks;

namespace Par.CommandCenter.Web.Controllers.WebApi
{
    [Authorize]
    public class AuditLogController : BaseController
    {
        public AuditLogController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [OpenApiOperation(nameof(Get), "Retreive audit logs data")]
        public async Task<GetAuditLogResponse> Get([FromQuery] GetAuditLogQuery query)
        {
            var response = await Mediator.Send(query);

            return response;
        }
    }
}
