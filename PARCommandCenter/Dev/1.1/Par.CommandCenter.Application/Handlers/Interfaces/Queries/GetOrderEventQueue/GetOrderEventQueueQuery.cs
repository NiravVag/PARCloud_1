using MediatR;

namespace Par.CommandCenter.Application.Handlers.Interfaces.Queries.GetOrderEventQueue
{
    public class GetOrderEventQueueQuery : IRequest<GetOrderEventQueueQueryResponse>
    {
        public int TenantId { get; set; }
    }
}
