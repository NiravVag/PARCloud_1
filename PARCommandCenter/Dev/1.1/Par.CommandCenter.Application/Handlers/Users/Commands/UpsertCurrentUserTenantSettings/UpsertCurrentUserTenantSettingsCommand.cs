using AutoMapper;
using MediatR;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities.Users;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.Users.Commands.UpsertCurrentUserTenantSettings
{
    public class UpsertCurrentUserTenantSettingsCommand : IRequest<UpsertCurrentUserTenantSettingsResponse>, IMap<UserApplicationTenantSetting>
    {
        public IEnumerable<int> TenantIds { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpsertCurrentUserTenantSettingsCommand, UserApplicationTenantSetting>();
        }
    }
}
