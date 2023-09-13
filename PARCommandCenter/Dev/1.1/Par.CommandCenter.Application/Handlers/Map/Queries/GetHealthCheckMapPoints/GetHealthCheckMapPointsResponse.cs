using GeoJSON.Net.Feature;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.Map.Queries.GetHealthCheckMapPoints
{
    public class GetHealthCheckMapPointsResponse
    {
        public string Type { get; set; }

        public object Metadata { get; set; }

        public IEnumerable<Feature> Features { get; set; }
    }

    //public class TimeZoneModel : IMap<Feature>
    //{       
    //    public short Id { get; internal set; }

    //    public string Name { get; set; } 

    //    public void Mapping(Profile profile)
    //    {
    //        profile.CreateMap<TimeZone, TimeZoneModel>();

    //    }
    //}
}
