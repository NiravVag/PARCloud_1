using System;

namespace Par.CommandCenter.Domain.Entities
{
    public class TenantApplicationNotificationSetting
    {
        public int Id { get; set; }

        public short ApplicationId { get; set; }

        public int TenantId { get; set; }

        public bool Deleted { get; set; }

        public int CreatedUserId { get; set; }

        public DateTime Created { get; set; }

        public int? ModifiedUserId { get; set; }

        public DateTime? Modified { get; set; }
    }
}
