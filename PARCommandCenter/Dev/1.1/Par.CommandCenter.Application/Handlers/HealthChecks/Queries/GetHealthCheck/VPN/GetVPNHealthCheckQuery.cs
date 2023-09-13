using MediatR;
using Newtonsoft.Json;
using Par.CommandCenter.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Queries.GetHealthCheck.VPN
{
    public class GetVPNHealthCheckQuery : IRequest<GetVPNHealthCheckResponse>
    {
        [Required]
        public HealthCheckFilterType Filter { get; set; }


        public IEnumerable<int> TenantIds { get; set; }


        public IEnumerable<int> HealthCheckIds { get; set; }


        public bool IncludeStatistics { get; set; }


        public bool StatisticsOnly { get; set; }

        [Required]
        public DateRangeFilterType DateRangeFilter { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
