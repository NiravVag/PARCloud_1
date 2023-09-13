using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using System;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Queries.GetHealthCheck.Interface
{
    public class GetInterfaceHealthCheckResponse
    {
        public List<HealthCheckInterfaceModel> HealthChecks { get; set; }

        public HealthCheckStatisticModel Statistics { get; set; }
    }

    public class HealthCheckInterfaceModel : IMap<HealthCheckInterface>
    {
        public int Id { get; set; }

        public string TenantName { get; set; }

        public string InterfaceType { get; set; }

        public string Status { get; set; }

        public string ExternalSystemName { get; set; }

        public string FileName { get; set; }

        public string FileLocation { get; set; }

        public string MimeType { get; set; }

        public DateTimeOffset? Sent { get; set; }

        public string ErrorMessage { get; set; }

        public DateTimeOffset? Published { get; set; }

        public DateTimeOffset? Started { get; set; }

        public DateTime? Modified { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<HealthCheckInterface, HealthCheckInterfaceModel>();
        }
    }
}
