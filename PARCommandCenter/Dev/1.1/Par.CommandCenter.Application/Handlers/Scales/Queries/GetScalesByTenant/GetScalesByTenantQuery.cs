using MediatR;

namespace Par.CommandCenter.Application.Handlers.Scales.Queries.GetScalesByTenant
{
    public class GetScalesByTenantQuery : IRequest<GetScalesByTenantResponse>
    {
        public int TenantId { get; set; }
    }
}
