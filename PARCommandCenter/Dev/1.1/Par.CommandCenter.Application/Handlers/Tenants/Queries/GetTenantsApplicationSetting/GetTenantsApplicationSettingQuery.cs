using MediatR;

namespace Par.CommandCenter.Application.Handlers.Tenants.Queries.GetTenantsApplicationSetting
{
    public class GetTenantsApplicationSettingQuery : IRequest<GetTenantsApplicationSettingResponse>
    {
        public bool ActiveOnly { get; set; }
    }
}
