using MediatR;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.Tenants.Queries.GetTenantsByIds
{
    public class GetTenantsByIdsQuery : IRequest<GetTenantsByIdsResponse>
    {
        public IEnumerable<int> TenantIds { get; set; }

        public OptionalField OptionalField { get; set; }
    }

    public enum OptionalField
    {
        None = 0,
        ScalesCount = 1,
    }
}
