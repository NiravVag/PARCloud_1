using MediatR;

namespace Par.CommandCenter.Application.Handlers.Scales.Queries.GetScalesByIpAddress
{
    public class GetScalesByControllerIdQuery : IRequest<GetScalesByControllerIdResponse>
    {
        public int ControllerId { get; set; }

        public bool RegisteredScalesOnly { get; set; }

        public bool OnlineScalesOnly { get; set; }

        public bool OfflineScalesOnly { get; set; }
    }
}
