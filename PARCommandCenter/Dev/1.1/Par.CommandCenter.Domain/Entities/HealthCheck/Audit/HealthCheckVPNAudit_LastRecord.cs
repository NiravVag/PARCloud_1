using System;

namespace Par.CommandCenter.Domain.Entities.HealthCheck
{
    public class HealthCheckVPNAudit_LastRecord
    {
        public int Audit_HealthCheckVPNId { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAction { get; set; }

        public int HealthCheckVPNId { get; set; }

        public int? TenantId { get; set; }

        public string Status { get; set; }

        public DateTime? NotificationDate { get; set; }
    }
}
