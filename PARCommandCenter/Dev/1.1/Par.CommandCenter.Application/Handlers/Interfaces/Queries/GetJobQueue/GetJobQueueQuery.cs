using MediatR;

namespace Par.CommandCenter.Application.Handlers.Interfaces.Queries.GetJobQueue
{
    public class GetInputDataQueueQuery : IRequest<GetJobQueueResponse>
    {
        public int TenantId { get; set; }
    }
}
