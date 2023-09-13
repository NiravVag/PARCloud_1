using Microsoft.EntityFrameworkCore;
using Par.CommandCenter.Persistence.Configurations.AuditLog;
using Par.CommandCenter.Persistence.Configurations.HealthCheck;
using Par.CommandCenter.Persistence.Configurations.Product;
using Par.CommandCenter.Persistence.Configurations.Users;

namespace Par.CommandCenter.Persistence.Configurations
{
    public static class DBConfigurations
    {
        public static void ApplyConfiguration(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new TenantConfiguration());

            builder.ApplyConfiguration(new TimeZoneConfiguration());

            builder.ApplyConfiguration(new RouterConfiguration());

            builder.ApplyConfiguration(new FacilityConfiguration());

            builder.ApplyConfiguration(new StateConfiguration());

            builder.ApplyConfiguration(new ControllerConfiguration());

            builder.ApplyConfiguration(new ScaleConfiguration());

            builder.ApplyConfiguration(new BinConfiguration());

            builder.ApplyConfiguration(new LocationItemConfiguration());

            builder.ApplyConfiguration(new ItemConfiguration());

            builder.ApplyConfiguration(new LocationConfiguration());

            builder.ApplyConfiguration(new InventoryTrackingTypeConfiguration());

            builder.ApplyConfiguration(new ItemUnitConfiguration());


            //HealthCheck
            builder.ApplyConfiguration(new HealthCheckVPNConfiguration());

            builder.ApplyConfiguration(new HealthCheckOrderInterfaceConfiguration());

            builder.ApplyConfiguration(new HealthCheckInventoryInterfaceConfiguration());

            builder.ApplyConfiguration(new OrderEventOutputConfiguration());

            builder.ApplyConfiguration(new InventoryEventOutputConfiguration());

            builder.ApplyConfiguration(new ExternalSystemConfiguration());

            builder.ApplyConfiguration(new HealthCheckRouterConfiguration());

            builder.ApplyConfiguration(new HealthCheckControllerConfiguration());

            builder.ApplyConfiguration(new HealthCheckScaleConfiguration());

            builder.ApplyConfiguration(new InteractiveMapHealthCheckConfiguration());

            builder.ApplyConfiguration(new HealthCheckServerOperationConfiguration());

            // Audit

            builder.ApplyConfiguration(new HealthCheckVPNAudit_LastRecordConfiguration());

            builder.ApplyConfiguration(new HealthCheckRouterAudit_LastRecordConfiguration());

            // End HealthCheck

            builder.ApplyConfiguration(new UserApplicationTenantSettingConfiguration());

            builder.ApplyConfiguration(new OrderEventConfiguration());

            builder.ApplyConfiguration(new InventoryEventConfiguration());

            builder.ApplyConfiguration(new ScaleMeasureCountsConfiguration());

            builder.ApplyConfiguration(new TenantApplicationNotificationSettingConfiguration());

            builder.ApplyConfiguration(new Controller_AuditLogConfiguration());

            builder.ApplyConfiguration(new UserConfiguration());

            builder.ApplyConfiguration(new HL7CloudServerConfiguration());

            builder.ApplyConfiguration(new HL7ServerConfiguration());

            builder.ApplyConfiguration(new JobQueueConfiguration());

            builder.ApplyConfiguration(new JobConfiguration());

            builder.ApplyConfiguration(new JobTypeConfiguration());

            builder.ApplyConfiguration(new VirtualMachineConfiguration());

            builder.ApplyConfiguration(new InputBatchJobDataConfiguration());

            builder.ApplyConfiguration(new InventoryEventOutputTransactionConfiguration());

            builder.ApplyConfiguration(new OrderEventOutputOrderConfiguration());

            builder.ApplyConfiguration(new OutputBatchJobDataExternalSystemConfiguration());

            builder.ApplyConfiguration(new OutputBatchJobDataConfiguration());

            builder.ApplyConfiguration(new OrderConfiguration());

            builder.ApplyConfiguration(new OrderEventHandlerReplenishmentSourceConfiguration());

            builder.ApplyConfiguration(new OrderEventHandlerConfiguration());

            builder.ApplyConfiguration(new OrderEventTypeConfiguration());

            builder.ApplyConfiguration(new InventoryEventTypeConfiguration());

            builder.ApplyConfiguration(new InventoryEventHandlerLocationConfiguration());

            builder.ApplyConfiguration(new InventoryEventHandlerConfiguration());


            

            builder.ApplyConfiguration(new ProductConfiguration());            

            builder.ApplyConfiguration(new ProductCategoryConfiguration());

            builder.ApplyConfiguration(new ProductManufacturerConfiguration());

            builder.ApplyConfiguration(new ProductUnspscClassConfiguration());

            builder.ApplyConfiguration(new ProductUnspscCommodityConfiguration());

            builder.ApplyConfiguration(new ProductUnspscFamilyConfiguration());

            builder.ApplyConfiguration(new ProductUnspscSegmentConfiguration());
        }
    }
}
