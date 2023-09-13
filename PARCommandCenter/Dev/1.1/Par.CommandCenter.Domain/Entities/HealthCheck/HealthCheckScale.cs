using System;


namespace Par.CommandCenter.Domain.Entities.HealthCheck
{
    public class HealthCheckScale
    {
        public int Id { get; set; }

        public int ScaleId { get; set; }

        public int TenantId { get; set; }

        public string TenantName { get; set; }

        public string ScaleAdress { get; set; }

        public string Status { get; set; }

        public string PreviousStatus { get; set; }

        public DateTime? PreviousStatusDate { get; set; }

        public DateTimeOffset? LastCommunication { get; set; }

        public DateTimeOffset? LastReboot { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }


        public int? BinId { get; set; }

        public virtual Location Location { get; set; }

        public virtual Item Item { get; set; }
    }
}
