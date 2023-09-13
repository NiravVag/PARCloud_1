using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using System;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Queries.GetHealthCheck.Controller
{
    public class GetControllerHealthCheckResponse
    {
        public List<HealthCheckControllerModel> HealthChecks { get; set; }

        public HealthCheckStatisticModel Statistics { get; set; }
    }

    public class HealthCheckControllerModel : IMap<HealthCheckController>
    {
        public int Id { get; set; }

        public int ControllerId { get; set; }

        public int TenantId { get; set; }

        public string TenantName { get; set; }

        public string RemoteIpAddress { get; set; }

        public int RemoteNetworkPort { get; set; }

        public string TCPTestStatus { get; set; }

        public string Status { get; set; }

        public string PreviousStatus { get; set; }

        public DateTime? PreviousStatusDate { get; set; }

        public DateTimeOffset? RouterLastCommunication { get; set; }

        public DateTime? Modified { get; set; }

        public int? RegisteredScaleCount { get; set; }

        public int? OnlineScaleCount { get; set; }

        public int? OfflineScaleCount { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<HealthCheckController, HealthCheckControllerModel>();
            //.ForMember(t => t.TCPTestStatus, opts => opts.MapFrom(m => m.Status));
        }
    }
}
