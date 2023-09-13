using MediatR;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.Scales.Queries.GetScalesCountByTenantIds
{
    public class GetScalesCountByTenantIdsQuery : IRequest<GetScalesCountByTenantIdsResponse>
    {
        public IEnumerable<int> TenantIds { get; set; }
    }
}
