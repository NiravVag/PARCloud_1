using MediatR;

namespace Par.CommandCenter.Application.Handlers.Controllers.Queries.GetController
{
    public class GetControllerQuery : IRequest<GetControllerResponse>
    {
        public int? ControllerId { get; set; }

        public int? routerId { get; set; }

        public bool IncludeScales { get; set; }
    }
}
