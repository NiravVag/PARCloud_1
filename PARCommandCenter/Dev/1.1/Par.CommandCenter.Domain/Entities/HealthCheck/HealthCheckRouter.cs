using System;
using System.ComponentModel;

namespace Par.CommandCenter.Domain.Entities.HealthCheck
{
    public class HealthCheckRouter
    {
        public int Id { get; set; }

        public int RouterId { get; set; }

        public int TenantId { get; set; }

        [DisplayName("Tenant Name")]
        public string TenantName { get; set; }

        [DisplayName("Router Address")]
        public string RouterAdress { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        public string PreviousStatus { get; set; }

        public DateTime? PreviousStatusDate { get; set; }

        [DisplayName("Last Communication")]
        public DateTimeOffset? LastCommunication { get; set; }

        [DisplayName("Last Reboot")]
        public DateTimeOffset? LastReboot { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        public DateTime? NotificationDate { get; set; }
    }
}
