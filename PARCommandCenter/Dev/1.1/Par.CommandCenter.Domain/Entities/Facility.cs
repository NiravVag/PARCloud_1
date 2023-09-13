namespace Par.CommandCenter.Domain.Entities
{
    public class Facility : AuditableEntity
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public string Name { get; set; }

        public short TimeZoneId { get; set; }

        public TimeZone TimeZone { get; set; }

        public string VPNConnectionName { get; set; }

        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public int? StateId { get; set; }

        public State State { get; set; }

        public string PostalCode { get; set; }


        public bool Deleted { get; set; }
    }
}
