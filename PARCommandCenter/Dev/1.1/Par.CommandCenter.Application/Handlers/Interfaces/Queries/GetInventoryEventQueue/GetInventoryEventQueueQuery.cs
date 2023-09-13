using MediatR;

namespace Par.CommandCenter.Application.Handlers.Interfaces.Queries.GetInventoryEventQueue
{
    public class GetInventoryEventQueueQuery : IRequest<GetInventoryEventQueueQueryResponse>
    {
        public int TenantId { get; set; }
    }
}
