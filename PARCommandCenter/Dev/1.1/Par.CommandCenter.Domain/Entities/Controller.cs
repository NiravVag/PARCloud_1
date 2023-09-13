using System.Collections.Generic;

namespace Par.CommandCenter.Domain.Entities
{
    public class Controller : AuditableEntity
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public string PortName { get; set; }

        public int RouterId { get; set; }

        public byte ControllerTypeId { get; set; }

        public string IpAddress { get; set; }

        public int? NetworkPort { get; set; }

        public string MACAddress { get; set; }

        public string FirmwareVersion { get; set; }

        public bool? ParChargeMode { get; set; }

        public bool? ParChargeBatch { get; set; }

        //// public int OnlineScaleCount => (this.Scales?.Any() ?? false) ? Scales.Where(s => s.LastCommunication > DateTimeOffset.UtcNow.AddHours(-1)).Count() : 0;

        //// public bool Deleted { get; set; }

        public virtual Router Router { get; set; }

        public virtual VirtualMachine VirtualMachine { get; set; }

        public IEnumerable<Scale> Scales { get; set; }

        public bool? Active { get; set; }
    }
}
