using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Par.CommandCenter.Application.Handlers.Interfaces.Commands.Delete;
using Par.CommandCenter.Application.Handlers.Interfaces.Commands.Reset;
using System.Threading.Tasks;

namespace Par.CommandCenter.Web.Controllers.WebApi
{
    [Authorize]
    public class InterfaceController : BaseController
    {
        public InterfaceController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpPost]
        [OpenApiOperation(nameof(ResetInterfaceEvent))]
        [Route("{interfaceType}/{Id}")]
        public async Task<IActionResult> ResetInterfaceEvent(string interfaceType, int Id)
        {
            var response = await Mediator.Send(new ResetInterfaceEventCommand() { InterfaceType = interfaceType, Id = Id });

            await Task.Delay(3000);

            return Ok(response);
        }

        [HttpDelete]
        [OpenApiOperation(nameof(DeleteInterfaceEvent))]
        [Route("{interfaceType}/{Id}")]
        public async Task<IActionResult> DeleteInterfaceEvent(string interfaceType, int Id)
        {
            var response = await Mediator.Send(new DeleteInterfaceEventCommand() { InterfaceType = interfaceType, Id = Id });

            await Task.Delay(3000);

            return Ok(response);
        }
    }
}
