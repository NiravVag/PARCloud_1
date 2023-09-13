using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using System;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Queries.GetHealthCheck.Router
{
    public class GetRouterHealthCheckResponse
    {
        public List<HealthCheckRouterModel> HealthChecks { get; set; }

        public HealthCheckStatisticModel Statistics { get; set; }
    }

    public class HealthCheckRouterModel : IMap<HealthCheckRouter>
    {
        public int Id { get; set; }

        public int RouterId { get; set; }

        public string TenantName { get; set; }

        public string RouterAdress { get; set; }

        public string Status { get; set; }

        public string PreviousStatus { get; set; }

        public DateTime? PreviousStatusDate { get; set; }

        public DateTimeOffset? LastCommunication { get; set; }

        public DateTimeOffset? LastReboot { get; set; }

        public DateTime? Modified { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<HealthCheckRouter, HealthCheckRouterModel>();
        }
    }
}
