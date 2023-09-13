using System;
using System.ComponentModel;

namespace Par.CommandCenter.Domain.Entities.AuditLog
{
    public class Controller_AuditLog
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Type { get; set; }

        public string EntityType { get; set; }

        public int? UserId { get; set; }

        public string UserName { get; set; }

        public int ControllerId { get; set; }

        [DisplayName("Previous TenantId Value")]
        public int? PrevTenantId { get; set; }

        [DisplayName("New Tenant Id Value")]
        public int? TenantId { get; set; }

        [DisplayName("Previous Port Name Value")]
        public string PrevPortName { get; set; }

        [DisplayName("New Port Name Value")]
        public string PortName { get; set; }

        [DisplayName("Previous Router Id Value")]
        public int? PrevRouterId { get; set; }

        [DisplayName("New Router Id Value")]
        public int? RouterId { get; set; }

        [DisplayName("Previous Controller Type Id Value")]
        public byte? PrevControllerTypeId { get; set; }

        [DisplayName("New Controller Type Id")]
        public byte? ControllerTypeId { get; set; }

        [DisplayName("Previous IP Address Value")]
        public string PrevIpAddress { get; set; }

        [DisplayName("New IP Address Value")]
        public string IpAddress { get; set; }

        [DisplayName("Previous Network Port Value")]
        public int? PrevNetworkPort { get; set; }

        [DisplayName("New Network Port Value")]
        public int? NetworkPort { get; set; }

        [DisplayName("Previous MAC Address Value")]
        public string PrevMACAddress { get; set; }

        [DisplayName("New MAC Address Value")]
        public string MACAddress { get; set; }

        [DisplayName("Previous Firmware Version")]
        public string PrevFirmwareVersion { get; set; }

        [DisplayName("New Firmware Version Value")]
        public string FirmwareVersion { get; set; }

        [DisplayName("Previous PAR Charge Mode Value")]
        public bool? PrevPARChargeMode { get; set; }

        [DisplayName("New PAR Charge Mode Value")]
        public bool? PARChargeMode { get; set; }

        [DisplayName("Previous PAR Charge Batch Value")]
        public bool? PrevParChargeBatch { get; set; }

        [DisplayName("New PAR Charge Batch Value")]
        public bool? ParChargeBatch { get; set; }

        [DisplayName("Previous Active value")]
        public bool? PrevActive { get; set; }

        [DisplayName("New Active Value")]
        public bool? Active { get; set; }

        [DisplayName("Previous Creation Date")]
        public DateTimeOffset? PrevCreationDate { get; set; }

        [DisplayName("New Creation Date")]
        public DateTimeOffset? CreationDate { get; set; }

        [DisplayName("Previous Modified Date")]
        public DateTimeOffset? PrevModifiedDate { get; set; }

        [DisplayName("New Modified Date")]
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
