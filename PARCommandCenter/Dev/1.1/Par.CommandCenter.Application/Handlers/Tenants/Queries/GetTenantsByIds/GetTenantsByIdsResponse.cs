using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Application.Handlers.Scales.Queries.GetScalesCountByTenantIds;
using Par.CommandCenter.Domain.Entities;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.Tenants.Queries.GetTenantsByIds
{
    public class GetTenantsByIdsResponse
    {
        public List<TenantModel> Tenants { get; set; }
    }

    public class TenantModel : IMap<Tenant>
    {
        public int Id { get; internal set; }

        public string Name { get; set; }

        public byte OrderBoxPercentage { get; set; }

        public bool ParMobileAllowRememberMe { get; set; }

        public bool IsTest { get; set; }

        public bool Deleted { get; set; }

        public virtual TimeZone TimeZone { get; set; }

        public string SearchField => this.Name.ToLower();

        public int ScalesCount { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Tenant, TenantModel>()
               .ForMember(t => t.TimeZone, opts => opts.MapFrom(m => m.TimeZone));
        }
    }
}
