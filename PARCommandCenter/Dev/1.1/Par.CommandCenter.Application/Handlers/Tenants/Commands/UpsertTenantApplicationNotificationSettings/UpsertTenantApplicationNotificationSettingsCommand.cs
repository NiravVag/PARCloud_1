using AutoMapper;
using MediatR;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.Tenants.Commands.UpsertTenantApplicationNotificationSettings
{
    public class UpsertTenantApplicationNotificationSettingsCommand : IRequest<UpsertTenantApplicationNotificationSettingsResponse>, IMap<TenantApplicationNotificationSetting>
    {
        public IEnumerable<int> TenantIds { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpsertTenantApplicationNotificationSettingsCommand, TenantApplicationNotificationSetting>();
        }
    }
}
