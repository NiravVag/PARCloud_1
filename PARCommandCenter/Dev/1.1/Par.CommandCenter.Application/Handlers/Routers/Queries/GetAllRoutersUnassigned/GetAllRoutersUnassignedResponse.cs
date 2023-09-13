using AutoMapper;
using Newtonsoft.Json;
using Par.CommandCenter.Application.Common;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.Routers.Queries.GetAllRoutersUnassigned
{
    public class GetAllRoutersUnassignedResponse
    {
        public List<RouterModel> Routers { get; set; }
    }

    public class RouterModel : IMap<Router>
    {
        public int Id { get; internal set; }

        public int TenantId { get; set; }

        public string Address { get; set; }

        public string FirmwareVersion { get; set; }

        [JsonConverter(typeof(JsonDateTimeOffsetConverter))]
        public DateTimeOffset LastCommunication { get; set; }

        [JsonConverter(typeof(JsonDateTimeOffsetConverter))]
        public DateTimeOffset LastReboot { get; set; }

        public bool IsRunning => this.LastCommunication.DateTime > DateTime.Now.AddHours(-1);


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Router, RouterModel>();
        }
    }
}
