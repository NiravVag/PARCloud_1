using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.TimeZones.Queries.GetAllTimeZones
{
    public class GetAllTimeZonesResponse
    {
        public List<TimeZoneModel> TimeZones { get; set; }
    }

    public class TimeZoneModel : IMap<TimeZone>
    {
        public short Id { get; internal set; }

        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TimeZone, TimeZoneModel>();

        }
    }
}
