using System;
using System.ComponentModel;

namespace Par.CommandCenter.Domain.Entities.HealthCheck
{
    public class HealthCheckServerOperation
    {
        public int Id { get; set; }

        [DisplayName("Service Name")]
        public string ServerName { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Log Message")]
        public string HealthCheckMessage { get; set; }

        public string PreviousStatus { get; set; }

        public DateTime? PreviousStatusDate { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        public DateTime? NotificationDate { get; set; }
    }
}
