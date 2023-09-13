using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.Facilities.Queries.GetFacilitiesByTenant
{
    public class GetFacilitiesByTenantResponse
    {
        public List<FacilityModel> Facilities { get; set; }
    }

    public class FacilityModel : IMap<Facility>
    {
        public int Id { get; internal set; }

        public int TenantId { get; set; }

        public string Name { get; set; }

        public short TimeZoneId { get; set; }

        public string VPNConnectionName { get; set; }

        public string AddressLine1 { get; set; }
        public string City { get; set; }

        public string PostalCode { get; set; }

        public bool Deleted { get; set; }

        public virtual State State { get; set; }

        public virtual TimeZone TimeZone { get; set; }

        public string SearchField => this.Name.ToLower();


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Facility, FacilityModel>()
               .ForMember(t => t.TimeZone, opts => opts.MapFrom(m => m.TimeZone))
                .ForMember(t => t.State, opts => opts.MapFrom(m => m.State));
        }
    }
}
