using Par.CommandCenter.Domain.Entities;
using Par.CommandCenter.Domain.Entities.AuditLog;
using System;


namespace Par.CommandCenter.Domain.Model.AuditLog
{
    public class AuditLog
    {
        public AuditLog()
        {
        }

        public AuditLog(Controller_AuditLog log, Router router, Controller controller, Entities.Tenant tenant)
        {
            if (tenant != null)
            {
                this.Tenant = tenant;
            }

            if (log != null)
            {
                this.Id = log.Id;
                this.Date = log.Date;
                this.Type = log.Type;
                this.EntityType = log.EntityType;
                this.UserName = log.UserName;

                this.Description = $"Controller Id:{log.ControllerId} ";

                if (!string.IsNullOrWhiteSpace(router.Address))
                {
                    this.Description += $"| Router Address:{router.Address} ";
                }

                if (!string.IsNullOrWhiteSpace(controller.IpAddress))
                {
                    this.Description += $"| Controller IP Address:{controller.IpAddress} ";
                }

                var type = log.GetType();

                if (log.Type == "Created")
                {
                    this.Description += "Created";
                }
                else if (log.Type == "Edited")
                {
                    this.Description += "Modified.";
                    if (log.TenantId.HasValue && log.TenantId != null)
                    {
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "PrevTenantId")} : {log.PrevTenantId}, ";
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "TenantId")} : {log.TenantId}";
                    }

                    if (!string.IsNullOrWhiteSpace(log.PortName))
                    {
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "PrevPortName")} : {log.PrevPortName}, ";
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "PortName")} : {log.PortName}";
                    }

                    if (log.RouterId.HasValue && log.RouterId != null)
                    {
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "PrevRouterId")} : {log.PrevRouterId}, ";
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "RouterId")} : {log.RouterId}";
                    }

                    if (log.ControllerTypeId.HasValue && log.ControllerTypeId != null)
                    {
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "PrevControllerTypeId")} : {log.PrevControllerTypeId}, ";
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "ControllerTypeId")} : {log.ControllerTypeId}";
                    }

                    if (!string.IsNullOrWhiteSpace(log.IpAddress))
                    {
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "PrevIpAddress")} : {log.PrevIpAddress}, ";
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "IpAddress")} : {log.IpAddress}";
                    }

                    if (log.NetworkPort.HasValue && log.NetworkPort != null)
                    {
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "PrevNetworkPort")} : {log.PrevNetworkPort}, ";
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "NetworkPort")} : {log.NetworkPort}";
                    }

                    if (!string.IsNullOrWhiteSpace(log.MACAddress))
                    {
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "PrevMACAddress")} : {log.PrevMACAddress}, ";
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "MACAddress")} : {log.MACAddress}";
                    }

                    if (!string.IsNullOrWhiteSpace(log.FirmwareVersion))
                    {
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "PrevFirmwareVersion")} : {log.PrevFirmwareVersion}, ";
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "FirmwareVersion")} : {log.FirmwareVersion}";
                    }

                    if (log.PARChargeMode.HasValue && log.PARChargeMode != null)
                    {
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "PrevPARChargeMode")} : {log.PrevPARChargeMode}, ";
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "PARChargeMode")} : {log.PARChargeMode}";
                    }

                    if (log.ParChargeBatch.HasValue && log.ParChargeBatch != null)
                    {
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "PrevParChargeBatch")} : {log.PrevParChargeBatch}, ";
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "ParChargeBatch")} : {log.ParChargeBatch}";
                    }

                    if (log.Active.HasValue && log.Active != null)
                    {
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "PrevActive")} : {log.PrevActive}, ";
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "Active")} : {log.Active}";
                    }

                    if (log.CreationDate.HasValue && log.CreationDate != null)
                    {
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "PrevCreationDate")} : {log.PrevCreationDate.Value.ToOffset(TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time").BaseUtcOffset).ToString("MM/dd/yyyy h:mm tt")}, ";
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "CreationDate")} : {log.CreationDate.Value.ToOffset(TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time").BaseUtcOffset).ToString("MM/dd/yyyy h:mm tt")}";
                    }

                    if (log.ModifiedDate.HasValue && log.ModifiedDate != null)
                    {
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "PrevModifiedDate")} : {log.PrevModifiedDate.Value.ToOffset(TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time").BaseUtcOffset).ToString("MM/dd/yyyy h:mm tt")}, ";
                        this.Description += $" {AttributeHelper.GetPropertyDisPlayName(type, "ModifiedDate")} : {log.ModifiedDate.Value.ToOffset(TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time").BaseUtcOffset).ToString("MM/dd/yyyy h:mm tt")}";
                    }
                }
                else if (log.Type == "Deleted")
                {
                    this.Description += "Deleted.";
                }
            }
        }

        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Type { get; set; }

        public string EntityType { get; set; }

        public string Description { get; set; }

        public string UserName { get; set; }

        public Entities.Tenant Tenant { get; set; }
    }
}
