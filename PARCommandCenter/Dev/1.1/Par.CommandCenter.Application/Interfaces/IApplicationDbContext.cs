using Microsoft.EntityFrameworkCore;
using Par.CommandCenter.Domain.Entities;
using Par.CommandCenter.Domain.Entities.AuditLog;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using Par.CommandCenter.Domain.Entities.Interfaces;
using Par.CommandCenter.Domain.Entities.Product;
using Par.CommandCenter.Domain.Entities.Users;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Tenant> Tenants { get; set; }

        DbSet<TimeZone> TimeZones { get; set; }

        DbSet<Router> Routers { get; set; }

        DbSet<Facility> Facilities { get; set; }

        DbSet<State> States { get; set; }

        DbSet<Controller> Controllers { get; set; }

        DbSet<Scale> Scales { get; set; }

        DbSet<Bin> Bins { get; set; }

        DbSet<LocationItem> LocationItems { get; set; }

        DbSet<Item> Items { get; set; }

        DbSet<Location> Locations { get; set; }

        DbSet<ItemUnit> ItemUnits { get; set; }

        DbSet<InventoryTrackingType> InventoryTrackingTypes { get; set; }

        DbSet<OrderEventOutput> OrderEventOutputs { get; set; }

        DbSet<InventoryEventOutput> InventoryEventOutputs { get; set; }

        DbSet<ExternalSystem> ExternalSystems { get; set; }

        // HealthCheck
        DbSet<HealthCheckVPN> HealthCheckVPNs { get; set; }

        DbSet<HealthCheckInventoryInterface> HealthCheckInventoryInterfaces { get; set; }

        DbSet<HealthCheckOrderInterface> HealthCheckOrderInterfaces { get; set; }

        DbSet<HealthCheckRouter> HealthCheckRouters { get; set; }

        DbSet<HealthCheckController> HealthCheckControllers { get; set; }

        DbSet<HealthCheckScale> HealthCheckScales { get; set; }

        DbSet<MapHealthCheckPoint> MapHealthCheckPoints { get; set; }

        DbSet<HealthCheckServerOperation> HealthCheckServerOperations { get; set; }

        // Audit
        DbSet<HealthCheckVPNAudit_LastRecord> HealthCheckVPNAudit_LastRecords { get; set; }

        DbSet<HealthCheckRouterAudit_LastRecord> HealthCheckRouterAudit_LastRecords { get; set; }

        // End HealthCheck


        DbSet<UserApplicationTenantSetting> UserApplicationTenantSettings { get; set; }

        DbSet<OrderEvent> OrderEvents { get; set; }


        DbSet<InventoryEvent> InventoryEvents { get; set; }

        DbSet<TenantScaleMeasureCounts> TenantScaleMeasureCounts { get; set; }

        DbSet<TenantApplicationNotificationSetting> TenantApplicationNotificationSettings { get; set; }

        DbSet<Controller_AuditLog> Controller_AuditLogs { get; set; }


        DbSet<User> Users { get; set; }

        DbSet<HL7CloudServer> HL7CloudServers { get; set; }

        DbSet<HL7Server> HL7Servers { get; set; }

        DbSet<JobQueueItem> JobQueueItems { get; set; }

        DbSet<Job> Jobs { get; set; }

        DbSet<JobType> JobTypes { get; set; }

        DbSet<VirtualMachine> VirtualMachines { get; set; }

        DbSet<InputBatchJobData> InputBatchJobData { get; set; }

        DbSet<OrderEventOutputOrder> OrderEventOutputOrders { get; set; }

        DbSet<InventoryEventOutputTransaction> InventoryEventOutputTransactions { get; set; }

        DbSet<OutputBatchJobDataExternalSystem> OutputBatchJobDataExternalSystems { get; set; }

        DbSet<OutputBatchJobData> OutputBatchJobData { get; set; }

        DbSet<Order> Orders { get; set; }

        DbSet<OrderEventHandlerReplenishmentSource> OrderEventHandlerReplenishmentSources { get; set; }

        DbSet<OrderEventHandler> OrderEventHandlers { get; set; }

        DbSet<OrderEventType> OrderEventTypes { get; set; }

        DbSet<InventoryEventType> InventoryEventTypes { get; set; }

        DbSet<InventoryEventHandlerLocation> InventoryEventHandlerLocations { get; set; }

        DbSet<InventoryEventHandler> InventoryEventHandlers { get; set; }        

        DbSet<Product> Products { get; }


        DbSet<ProductManufacturer> ProductManufacturers { get; }



        DbSet<ProductCategory> ProductCategories { get; }



        DbSet<ProductUNSPSCCommodity> ProductUNSPSCCommodities { get; }



        DbSet<ProductUNSPSCSegment> ProductUNSPSCSegments { get; }



        DbSet<ProductUNSPSCFamily> ProductUNSPSCFamilies { get; }



        DbSet<ProductUNSPSCClass> ProductUNSPSCClasses { get; }


        Task<int> ExecuteSqlRawAsync([NotNull] string sql, [NotNull] params object[] parameters);

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Set the current user information in the database SESSION_CONTEXT
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <returns>A SessionUser instance containing the user's name, user id and tenant id</returns>
        Task<SessionUser> SetSessionUserAsync(string userName);
    }
}