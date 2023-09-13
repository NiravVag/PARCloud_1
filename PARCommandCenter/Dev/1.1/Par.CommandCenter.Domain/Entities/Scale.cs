using System;

namespace Par.CommandCenter.Domain.Entities
{
    public class Scale
    {
        public int Id { get; set; }

        public int? BinId { get; set; }

        public int TenantId { get; set; }

        public string Address { get; set; }

        public decimal? ScaleWeight { get; set; }


        public DateTimeOffset? LastCommunication { get; set; }


        public int? ControllerId { get; set; }

        public bool Deleted { get; set; }


        public virtual Location Location { get; set; }

        public virtual Item Item { get; set; }

        public Controller Controller { get; set; }
    }
}
