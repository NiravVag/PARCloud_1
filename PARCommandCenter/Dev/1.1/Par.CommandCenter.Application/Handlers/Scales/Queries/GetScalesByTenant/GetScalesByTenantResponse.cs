using AutoMapper;
using Newtonsoft.Json;
using Par.CommandCenter.Application.Common;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.Scales.Queries.GetScalesByTenant
{
    public class GetScalesByTenantResponse
    {
        public List<ScaleModel> Scales { get; set; }
    }

    public class ScaleModel : IMap<Scale>
    {
        public int Id { get; internal set; }

        public int TenantId { get; set; }

        public string Address { get; set; }


        public decimal? ScaleWeight { get; set; }


        [JsonConverter(typeof(JsonDateTimeOffsetConverter))]
        public DateTimeOffset? LastCommunication { get; set; }

        public bool IsRunning
        {
            get
            {
                if (this.LastCommunication.HasValue)
                {
                    return this.LastCommunication.Value.DateTime > DateTime.Now.AddHours(-1);
                }
                return false;
            }
        }

        public string LocationName { get; set; }

        public string ItemName { get; set; }
        public string ItemNumber { get; set; }

        public string ControllerIp { get; set; }

        public int? BinId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Scale, ScaleModel>()
                .ForMember(s => s.LocationName, opts => opts.MapFrom(m => m.Location.Name))
                .ForMember(s => s.ItemName, opts => opts.MapFrom(m => m.Item.Name))
                .ForMember(s => s.ItemNumber, opts => opts.MapFrom(m => m.Item.Number))
                .ForMember(s => s.ControllerIp, opts => opts.MapFrom(m => m.Controller.IpAddress))
                .ForMember(s => s.BinId, opts => opts.MapFrom(m => m.BinId));
        }
    }
}
