using MediatR;

namespace Par.CommandCenter.Application.Handlers.Controllers.Queries.GetControllerPortName
{
    public class GetControllerPortNameQuery : IRequest<GetControllerPortNameResponse>
    {
        public int RouterId { get; set; }

        public int ControllerTypeId { get; set; }
    }
}
