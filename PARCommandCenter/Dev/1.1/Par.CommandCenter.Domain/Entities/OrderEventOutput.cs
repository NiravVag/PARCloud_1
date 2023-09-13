using System;

namespace Par.CommandCenter.Domain.Entities
{
    public class OrderEventOutput : AuditableEntity
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public int ExternalSystemId { get; set; }

        public string FileName { get; set; }

        public string FileLocation { get; set; }

        public string MimeType { get; set; }

        public DateTimeOffset? Sent { get; set; }

        public string ErrorMessage { get; set; }

        public DateTimeOffset? Published { get; set; }

        public DateTimeOffset? Started { get; set; }

    }
}
