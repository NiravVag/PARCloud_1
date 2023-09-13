using Par.CommandCenter.Domain.Enums;
using System;

namespace Par.CommandCenter.Domain.Entities
{
    public class Router : AuditableEntity
    {
        public int Id { get; set; }

        public int? TenantId { get; set; }

        public byte? DeviceTypeId { get; set; }

        public string Address { get; set; }

        public string FirmwareVersion { get; set; }

        public bool Deleted { get; set; }

        public DateTimeOffset LastCommunication { get; set; }

        public DateTimeOffset LastReboot { get; set; }

        public bool IsRunning { get; set; }

        public int RegisteredControllerCount { get; set; }

        public int? VirtualMachineId { get; set; }

        public DeviceType DeviceType { get; set; }


        public VirtualMachine VirtualMachine { get; set; }

        public string? ServiceName { get; set; }

        public string? ServiceDisplayName { get; set; }

        public string? ComputerName { get; set; }
    }
}
