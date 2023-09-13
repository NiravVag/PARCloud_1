namespace Par.CommandCenter.Domain.Entities
{
    public class HL7Server : AuditableEntity
    {
        public int Id { get; set; }

        public int HL7CloudServerId { get; set; }

        public int Port { get; set; }

        public int TenantId { get; set; }

        public int? FacilityId { get; set; }

        public int MaxPacketsPerMessage { get; set; }

        public bool? IsActive { get; set; }
    }
}
