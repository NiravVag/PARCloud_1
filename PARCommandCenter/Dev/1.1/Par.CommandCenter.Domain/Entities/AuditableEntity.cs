using System;

namespace Par.CommandCenter.Domain.Entities
{
    public class AuditableEntity
    {
        public int? CreatedUserId { get; set; }

        public DateTimeOffset? Created { get; set; }

        public int? ModifiedUserId { get; set; }

        public DateTimeOffset? Modified { get; set; }
    }
}
