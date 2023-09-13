using System;

namespace Par.CommandCenter.Domain.Entities.HealthCheck
{
    public class HealthCheckRouterAudit_LastRecord
    {
        public int Audit_HealthCheckRouterId { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAction { get; set; }

        public int HealthCheckRouterId { get; set; }

        public int RouterId { get; set; }

        public string Status { get; set; }

        public DateTimeOffset? LastCommunication { get; set; }

        public DateTimeOffset? LastReboot { get; set; }

        public DateTime? NotificationDate { get; set; }
    }
}
