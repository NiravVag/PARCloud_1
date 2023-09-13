using MediatR;

namespace Par.CommandCenter.Application.Handlers.Items.Queries.GetItemsByTenant
{
    public class GetItemsByTenantQuery : IRequest<GetItemsByTenantResponse>
    {
        public int TenantId { get; set; }
    }
}
