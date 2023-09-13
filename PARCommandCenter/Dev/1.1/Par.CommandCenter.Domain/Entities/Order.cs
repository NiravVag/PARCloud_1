using System;
using System.Collections.Generic;
using System.Text;

namespace Par.CommandCenter.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public int LocationId { get; set; }

        public int ReplenishmentSourceId { get; set; }

        public int Number { get; set; }
    }
}
