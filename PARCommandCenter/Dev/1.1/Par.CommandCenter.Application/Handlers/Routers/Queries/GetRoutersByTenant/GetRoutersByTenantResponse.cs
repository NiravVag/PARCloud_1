using AutoMapper;
using Newtonsoft.Json;
using Par.CommandCenter.Application.Common;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;
using Par.CommandCenter.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.Routers.Queries.GetRoutersByTenant
{
    public class GetRoutersByTenantResponse
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

        public int RegisteredControllerCount { get; set; }

        public DeviceType DeviceType { get; set; }

        public string? ServiceName { get; set; }

        public string? ServiceDisplayName { get; set; }

        public string? ComputerName { get; set; }

        public string DeviceTypeDisplayName {
            get
            {
                var deviceType = string.Empty;
                switch (this.DeviceType)
                {
                    case DeviceType.Scale:
                        deviceType = "Scale";
                        break;
                    case DeviceType.CloudRouter:
                        deviceType = "Cloud Router";
                        break;
                    case DeviceType.Controller:
                        deviceType = "Controller";
                        break;
                    case DeviceType.CDC:
                        deviceType = "CDC";
                        break;
                    case DeviceType.CloudRouterOnPC:
                        deviceType = "Cloud Router On-PC";
                        break;
                    default:
                        deviceType = "Undefined";
                        break;
                }

                return deviceType;
            }
        }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Router, RouterModel>();
        }
    }
}
