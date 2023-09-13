using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Queries.FacilityVPNConnections
{
    public class GetFacilityVPNConnectionsResponse
    {
        public List<FacilityVPNConnectionsModel> VPNConnections { get; set; }
    }

    public class FacilityVPNConnectionsModel : IMap<HealthCheckVPN>
    {
        public int Id { get; set; }

        public string ConnectionName { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<HealthCheckVPN, FacilityVPNConnectionsModel>();
        }
    }
}
