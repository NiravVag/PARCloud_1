using MediatR;

namespace Par.CommandCenter.Application.Handlers.Facilities.Queries.GetFacilitiesByTenant
{
    public class GetFacilitiesByTenantQuery : IRequest<GetFacilitiesByTenantResponse>
    {
        public int TenantId { get; set; }
    }
}
