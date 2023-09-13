using Microsoft.AspNetCore.Http;
using Par.CommandCenter.Application.Common.Claims;
using Par.CommandCenter.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Par.CommandCenter.Web.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            var user = httpContextAccessor.HttpContext?.User;

            var userId = user?.FindFirstValue(ParClaimTypes.USER_ID);
            if (!string.IsNullOrWhiteSpace(userId))
            {
                UserId = Convert.ToInt32(userId);
            }

            ObjectGuid = user?.FindFirstValue(ParClaimTypes.OBJECT_GUID);
            Name = user?.FindFirstValue(ParClaimTypes.NAME);
            UPN = user?.FindFirstValue(ParClaimTypes.UPN);

            PreferredUsername = user?.FindFirstValue(ParClaimTypes.PREFERRED_USERNAME);
            IsAuthenticated = !string.IsNullOrWhiteSpace(ObjectGuid);

            var tenantIdsClaim = user?.FindFirstValue(ParClaimTypes.TENANT_IDS);
            if (tenantIdsClaim != null && tenantIdsClaim.Length > 0)
            {
                TenantIds = tenantIdsClaim.Split(',')
                    .Where(m => int.TryParse(m, out _))
                    .Select(m => int.Parse(m))
                    .ToList();
            }

            Roles = user.Claims.Where(x => x.Type == ParClaimTypes.ROLE).Select(x => x.Value);
        }

        public int UserId { get; }

        public string ObjectGuid { get; }

        public string Name { get; }

        public string UPN { get; }

        public string PreferredUsername { get; }

        public bool IsAuthenticated { get; }

        public IEnumerable<int> TenantIds { get; }

        public IEnumerable<string> Roles { get; }
    }
}
