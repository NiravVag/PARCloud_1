using System.Collections.Generic;

namespace Par.CommandCenter.Application.Interfaces
{
    public interface ICurrentUserService
    {
        int UserId { get; }

        string ObjectGuid { get; }

        string Name { get; }

        string UPN { get; }

        string PreferredUsername { get; }

        bool IsAuthenticated { get; }

        IEnumerable<int> TenantIds { get; }
    }
}
