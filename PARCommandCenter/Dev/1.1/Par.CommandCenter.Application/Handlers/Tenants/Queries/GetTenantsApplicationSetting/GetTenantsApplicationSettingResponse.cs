using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.Tenants.Queries.GetTenantsApplicationSetting
{
    public class GetTenantsApplicationSettingResponse
    {
        public List<TenantApplicationNotificationSettingModel> TenantNotificationSettings { get; set; }
    }

    public class TenantApplicationNotificationSettingModel : IMap<TenantApplicationNotificationSetting>
    {
        public int Id { get; set; }

        public short ApplicationId { get; set; }

        public int TenantId { get; set; }

        public bool Deleted { get; set; }

        public int CreatedUserId { get; set; }

        public DateTime Created { get; set; }

        public int? ModifiedUserId { get; set; }

        public DateTime? Modified { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<TenantApplicationNotificationSetting, TenantApplicationNotificationSettingModel>();
        }
    }
}
