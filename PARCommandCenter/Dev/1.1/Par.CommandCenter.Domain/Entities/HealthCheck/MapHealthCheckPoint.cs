using System;

namespace Par.CommandCenter.Domain.Entities.HealthCheck
{
    public class MapHealthCheckPoint
    {
        public long Id { get; set; }

        public int TenantId { get; set; }

        public string TenantName { get; set; }

        public int FacilityId { get; set; }

        public string FacilityName { get; set; }

        public string AddressLine1 { get; set; }

        public string City { get; set; }

        public int? StateId { get; set; }

        public string PostalCode { get; set; }

        public int HealthCheckId { get; set; }

        public string HealthCheckType { get; set; }

        public string PointStatus { get; set; }

        public DateTimeOffset StatusDate { get; set; }
    }
}
