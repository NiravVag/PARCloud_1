using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Par.CommandCenter.Application.Handlers.HL7Servers.Commands.Upsert;
using Par.CommandCenter.Application.Handlers.HL7Servers.Queries.GetAllHL7Servers;
using System.Threading.Tasks;

namespace Par.CommandCenter.Web.Controllers.WebApi
{
    [Authorize]
    public class IntegrationController : BaseController
    {
        public IntegrationController(IMediator mediator)
           : base(mediator)
        {
        }

        [HttpGet]
        [OpenApiOperation(nameof(GetHL7CloudServers), "Retreive All HL7 Cloud Servers", "Retreive All HL7 Cloud Servers")]
        public async Task<IActionResult> GetHL7CloudServers()
        {
            var response = await Mediator.Send(new GetAllHL7ServersQuery());

            return Ok(response);
        }

        [HttpPost]
        [OpenApiOperation(nameof(Upsert), "Insert and update HL7 Server information in the backend data store")]
        public async Task<IActionResult> Upsert([FromBody] UpsertHL7ServerCommand model)
        {
            var response = await Mediator.Send(model);

            return Ok(response);
        }
    }
}
