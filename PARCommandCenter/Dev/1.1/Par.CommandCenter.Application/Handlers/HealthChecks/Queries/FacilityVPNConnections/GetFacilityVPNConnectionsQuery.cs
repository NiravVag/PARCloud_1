using MediatR;

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Queries.FacilityVPNConnections
{
    public class GetFacilityVPNConnectionsQuery : IRequest<GetFacilityVPNConnectionsResponse>
    {
        public int TenantId { get; set; }

        public int? FacilityId { get; set; }
    }
}
