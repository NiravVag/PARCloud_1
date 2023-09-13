using MediatR;

namespace Par.CommandCenter.Application.Handlers.Interfaces.Queries.GetInputDataQueue
{
    public class GetInputDataQueueQuery : IRequest<GetInputDataQueueQueryResponse>
    {
        public int TenantId { get; set; }
    }
}
