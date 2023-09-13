using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.HL7Servers.Queries.GetAllHL7Servers
{
    public class GetAllHL7ServersResponse
    {
        public List<HL7CloudServerModel> HL7CloudServers { get; set; }
    }

    public class HL7CloudServerModel : IMap<HL7Server>
    {
        public int Id { get; internal set; }

        public string TenantName { get; set; }

        public string FacilityName { get; set; }

        public int CloudServerId { get; set; }

        public string CloudServerAddress { get; set; }

        public int Port { get; set; }

        public int MaxPacketsPerMessage { get; set; }

        public bool IsActive { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<HL7Server, HL7CloudServerModel>();
        }
    }
}
