using MediatR;

namespace Par.CommandCenter.Application.Handlers.Routers.Queries.GetRoutersByTenant
{
    public class GetRoutersByTenantQuery : IRequest<GetRoutersByTenantResponse>
    {
        public int TenantId { get; set; }
    }
}
