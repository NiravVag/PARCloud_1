using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using System;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Queries.GetHealthCheck.Scale
{
    public class GetScaleHealthCheckResponse
    {
        public List<HealthCheckScaleModel> HealthChecks { get; set; }

        public ScaleHealthCheckStatisticModel Statistics { get; set; }
    }

    public class HealthCheckScaleModel : IMap<HealthCheckScale>
    {
        public int Id { get; set; }

        public int ScaleId { get; set; }

        public string TenantName { get; set; }

        public string ScaleAdress { get; set; }

        public string Status { get; set; }

        public string PreviousStatus { get; set; }

        public DateTime? PreviousStatusDate { get; set; }

        public DateTimeOffset? LastCommunication { get; set; }

        public DateTimeOffset? LastReboot { get; set; }

        public DateTime? Modified { get; set; }

        public int? BinId { get; set; }

        public virtual Location Location { get; set; }

        public virtual Item Item { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<HealthCheckScale, HealthCheckScaleModel>();
        }
    }

    public class ScaleHealthCheckStatisticModel
    {
        public HealthCheckType Type { get; set; }

        public int TotalTenantsCount { get; set; }

        public int HealthyTenantsCount { get; set; }
        public int HealthyCount { get; set; }

        public int UnHealthyTenantsCount { get; set; }
        public int UnHealthyCount { get; set; }

        public int WarrningTenantsCount { get; set; }
        public int WarrningCount { get; set; }

        public int ScalesMissingCalibrationCount { get; set; }
    }
}
