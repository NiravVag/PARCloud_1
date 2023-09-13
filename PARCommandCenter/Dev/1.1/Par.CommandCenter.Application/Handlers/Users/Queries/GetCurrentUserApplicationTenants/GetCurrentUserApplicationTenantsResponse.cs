using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities.Users;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.Users.Queries.GetCurrentUserApplicationTenants
{
    public class GetCurrentUserApplicationTenantsResponse
    {
        public IEnumerable<UserApplicationTenantModel> UserApplicationTenants { get; set; }
    }

    public class UserApplicationTenantModel : IMap<UserApplicationTenantSetting>
    {
        public int Id { get; internal set; }

        public int TenantId { get; internal set; }


        public bool Deleted { get; internal set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserApplicationTenantSetting, UserApplicationTenantModel>();
        }
    }
}
