using MediatR;

namespace Par.CommandCenter.Application.Handlers.Controllers.Queries.GetControllersByTenant
{
    public class GetControllersByTenantQuery : IRequest<GetControllersByTenantResponse>
    {
        public int TenantId { get; set; }
    }
}
