using MediatR;

namespace Par.CommandCenter.Application.Handlers.Interfaces.Queries.GetOutputDataQueue
{
    public class GetOutputDataQueueQuery : IRequest<GetOutputDataQueueQueryResponse>
    {
        public int TenantId { get; set; }
    }
}
