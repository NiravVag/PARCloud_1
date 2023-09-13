using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Par.CommandCenter.Web.Controllers.WebApi
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class BaseController : ControllerBase
    {
        private readonly IMediator _mediator;

        protected BaseController(IMediator mediator) => _mediator = mediator;

        protected IMediator Mediator => _mediator;
    }
}