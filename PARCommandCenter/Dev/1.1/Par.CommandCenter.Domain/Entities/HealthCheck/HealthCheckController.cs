using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Par.CommandCenter.Domain.Entities.HealthCheck
{
    public class HealthCheckController
    {
        public int Id { get; set; }

        public int? ControllerId { get; set; }

        public int? TenantId { get; set; }

        ////public Router Router { get; set; }

        [DisplayName("Router Address")]
        public string RouterAddress { get; set; }

        [DisplayName("Router Last Communication")]
        public DateTimeOffset? RouterLastCommunication { get; set; }

        [DisplayName("Router Last Reboot")]
        public DateTimeOffset? RouterLastReboot { get; set; }

        [DisplayName("Router Status")]
        public string RouterStatus { get; set; }

        public string TenantName { get; set; }

        [DisplayName("IP Address")]
        public string RemoteIpAddress { get; set; }

        [DisplayName("Network Port")]
        public int RemoteNetworkPort { get; set; }

        [DisplayName("TCP Test Status")]
        public string TCPTestStatus { get; set; }

        [DisplayName("Status")]
        public string Status
        {
            get { return (OnlineScaleCount > 0) ? "Online" : "Offline"; }
        }

        public string PreviousStatus { get; set; }

        public DateTime? PreviousStatusDate { get; set; }

       

        [DisplayName("Registered Scale Count")]
        public int? RegisteredScaleCount { get; set; }

        [DisplayName("Online Scale Count")]
        public int? OnlineScaleCount { get; set; }

        [DisplayName("Offline Scale Count")]
        public int? OfflineScaleCount { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        public DateTime? NotificationDate { get; set; }


        [DisplayName("Scales Locations")]
        public string ScalesLocations => (this.Scales == null || !this.Scales.Any()) ? string.Empty : string.Join(
            " | ",
            this.Scales?
            .Where(s => s != null)
            .Select(s => s.Location?.Name)
            .Where(s => s != null && !string.IsNullOrWhiteSpace(s))
            .Distinct()
            );

        public IEnumerable<Scale> Scales {get; set; }

    }
}
