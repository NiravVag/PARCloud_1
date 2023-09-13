using AutoMapper;
using MediatR;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Application.Handlers.Facilities.Commands.Upsert
{
    public class UpsertFacilityCommand : IRequest<UpsertFacilityResponse>, IMap<Facility>
    {
        public int? FacilityId { get; set; }

        public int TenantId { get; set; }
        public string Name { get; set; }

        public short TimeZoneId { get; set; }

        public string VPNConnectionName { get; set; }

        public string AddressLine1 { get; set; }

        public string City { get; set; }

        public int? StateId { get; set; }

        public string PostalCode { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpsertFacilityCommand, Facility>();
        }
    }
}
