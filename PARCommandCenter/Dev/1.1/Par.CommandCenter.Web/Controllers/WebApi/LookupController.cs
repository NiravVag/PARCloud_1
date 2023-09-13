using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Par.CommandCenter.Application.Handlers.States.Queries.GetAllStates;
using Par.CommandCenter.Application.Handlers.TimeZones.Queries.GetAllTimeZones;
using System.Threading.Tasks;

namespace Par.CommandCenter.Web.Controllers.WebApi
{
    [Authorize]
    public class LookupController : BaseController
    {
        public LookupController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [OpenApiOperation(nameof(GetTimeZones), "Retreive All Time Zones")]
        public async Task<IActionResult> GetTimeZones()
        {
            var response = await Mediator.Send(new GetAllTimeZonesQuery());

            return Ok(response);
        }

        [HttpGet]
        [OpenApiOperation(nameof(GetStates), "Retreive All States")]
        public async Task<IActionResult> GetStates()
        {
            var response = await Mediator.Send(new GetAllStatesQuery());

            return Ok(response);
        }


    }
}
