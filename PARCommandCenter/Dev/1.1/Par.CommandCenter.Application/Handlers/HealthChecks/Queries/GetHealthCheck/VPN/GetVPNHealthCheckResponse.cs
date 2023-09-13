using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using System;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Queries.GetHealthCheck.VPN
{
    public class GetVPNHealthCheckResponse
    {
        public List<HealthCheckVPNModel> HealthChecks { get; set; }

        public HealthCheckStatisticModel Statistics { get; set; }
    }

    public class HealthCheckVPNModel : IMap<HealthCheckVPN>
    {
        public int Id { get; set; }

        public Tenant Tenant { get; set; }

        public IEnumerable<Facility> facilities { get; set; }

        public string ConnectionName { get; set; }

        public string Status { get; set; }

        public string PreviousStatus { get; set; }

        public DateTime? PreviousStatusDate { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<HealthCheckVPN, HealthCheckVPNModel>();
        }
    }
}
