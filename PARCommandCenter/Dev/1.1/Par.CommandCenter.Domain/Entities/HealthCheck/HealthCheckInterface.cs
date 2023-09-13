using System;
using System.ComponentModel;

namespace Par.CommandCenter.Domain.Entities.HealthCheck
{
    public class HealthCheckInterface
    {
        [DisplayName("Health Check Id")]
        public int Id { get; set; }

        public int EventOutputId { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        public int TenantId { get; set; }

        public string TenantName { get; set; }

        [DisplayName("Interface Type")]
        public string InterfaceType { get; set; }

        [DisplayName("External System")]
        public string ExternalSystemName { get; set; }

        [DisplayName("File Name")]
        public string FileName { get; set; }

        public string FileLocation { get; set; }

        public string MimeType { get; set; }

        public DateTimeOffset? Sent { get; set; }

        [DisplayName("Description")]
        public string ErrorMessage { get; set; }

        [DisplayName("Sent Date")]
        public DateTimeOffset? Published { get; set; }

        public DateTimeOffset? Started { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        public DateTime? NotificationDate { get; set; }
    }
}
