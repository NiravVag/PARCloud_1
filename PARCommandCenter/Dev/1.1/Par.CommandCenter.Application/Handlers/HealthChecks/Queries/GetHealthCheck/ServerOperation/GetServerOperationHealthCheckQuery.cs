using MediatR;

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Queries.GetHealthCheck.ServerOperation
{
    public class GetServerOperationHealthCheckQuery : IRequest<GetServerOperationHealthCheckResponse>
    {
    }
}
