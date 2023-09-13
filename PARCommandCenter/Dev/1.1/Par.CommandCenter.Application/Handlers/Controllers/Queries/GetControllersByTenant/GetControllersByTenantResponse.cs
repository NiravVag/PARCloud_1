using AutoMapper;
using Newtonsoft.Json;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Application.Handlers.Routers.Queries.GetRoutersByTenant;
using Par.CommandCenter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Par.CommandCenter.Application.Handlers.Controllers.Queries.GetControllersByTenant
{
    public class GetControllersByTenantResponse
    {
        public List<ControllerModel> Controllers { get; set; }
    }

    public class ControllerModel : IMap<Controller>
    {
        public int Id { get; internal set; }

        public int TenantId { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string PortName { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int RouterId { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public byte ControllerTypeId { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string IpAddress { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? NetworkPort { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string MACAddress { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FirmwareVersion { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? PARChargeMode { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? ParChargeBatch { get; set; }


        public int OnlineScaleCount => (this.Scales != null && this.Scales.Any()) ? this.Scales.Where(s => s.LastCommunication > DateTimeOffset.UtcNow.AddHours(-1)).Count() : 0;


        public bool IsRunning
        {
            get
            {
                return OnlineScaleCount > 0;
            }
        }

        public string ScalesLocations => (this.Scales == null || !this.Scales.Any()) ? string.Empty : string.Join(
            " | ",
            this.Scales?
            .Where(s => s != null)
            .Select(s => s.Location?.Name)
            .Where(s => s != null && !string.IsNullOrWhiteSpace(s))
            .Distinct()
            );


        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public virtual RouterModel Router { get; set; }

        public virtual IEnumerable<Scale> Scales { get; set; }

        public bool Active { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Controller, ControllerModel>()
                 .ForMember(t => t.Router, opts => opts.MapFrom(m => m.Router))
            .ForMember(t => t.Scales, opts => opts.MapFrom(m => m.Scales));
        }
    }
}
