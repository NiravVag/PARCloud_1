using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Par.CommandCenter.Domain.Entities.HealthCheck
{
    public class HealthCheckVPN
    {
        public int Id { get; set; }

        [DisplayName("Tenant Id")]
        public int? TenantId { get; set; }

        public Tenant Tenant { get; set; }

        public IEnumerable<Facility> facilities { get; set; }

        [DisplayName("Connection Name")]
        public string ConnectionName { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        public string PreviousStatus { get; set; }

        public DateTime? PreviousStatusDate { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        public DateTime? NotificationDate { get; set; }
    }
}
