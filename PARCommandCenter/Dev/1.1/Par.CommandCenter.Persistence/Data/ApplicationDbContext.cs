using Microsoft.EntityFrameworkCore;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using Par.CommandCenter.Domain.Entities.AuditLog;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using Par.CommandCenter.Domain.Entities.Interfaces;
using Par.CommandCenter.Domain.Entities.Product;
using Par.CommandCenter.Domain.Entities.Users;
using Par.CommandCenter.Persistence.Configurations;
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Par.CommandCenter.Persistence.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            ////var connection = (SqlConnection)Database.GetDbConnection();
            ////var tokenProv = new AzureServiceTokenProvider();
            ////connection.AccessToken = tokenProv.GetAccessTokenAsync("https://database.windows.net/").Result;
            ////Console.WriteLine(connection.AccessToken);

            ////var connection = (SqlConnection)Database.GetDbConnection();
            ////var miAuthentication = new AzureManagedIdentityAuthentication("https://database.windows.net/");
            ////var tokenCredential = miAuthentication.GetAccessToken();
            ////connection.AccessToken = tokenCredential.Token;

            ////Console.WriteLine(connection.AccessToken);
        }

        public virtual DbSet<Tenant> Tenants { get; set; }

        public virtual DbSet<Domain.Entities.TimeZone> TimeZones { get; set; }

        public virtual DbSet<Router> Routers { get; set; }

        public virtual DbSet<Facility> Facilities { get; set; }

        public virtual DbSet<State> States { get; set; }

        public virtual DbSet<Controller> Controllers { get; set; }

        public virtual DbSet<Scale> Scales { get; set; }

        public virtual DbSet<Bin> Bins { get; set; }

        public virtual DbSet<LocationItem> LocationItems { get; set; }

        public virtual DbSet<Item> Items { get; set; }

        public virtual DbSet<Location> Locations { get; set; }

        public virtual DbSet<ItemUnit> ItemUnits { get; set; }

        public virtual DbSet<InventoryTrackingType> InventoryTrackingTypes { get; set; }

        public virtual DbSet<OrderEventOutput> OrderEventOutputs { get; set; }

        public virtual DbSet<InventoryEventOutput> InventoryEventOutputs { get; set; }

        public virtual DbSet<ExternalSystem> ExternalSystems { get; set; }


        // HealthCheck
        public virtual DbSet<HealthCheckVPN> HealthCheckVPNs { get; set; }

        public virtual DbSet<HealthCheckInventoryInterface> HealthCheckInventoryInterfaces { get; set; }

        public virtual DbSet<HealthCheckOrderInterface> HealthCheckOrderInterfaces { get; set; }

        public virtual DbSet<HealthCheckRouter> HealthCheckRouters { get; set; }

        public virtual DbSet<HealthCheckController> HealthCheckControllers { get; set; }

        public virtual DbSet<HealthCheckScale> HealthCheckScales { get; set; }

        public virtual DbSet<MapHealthCheckPoint> MapHealthCheckPoints { get; set; }

        public virtual DbSet<HealthCheckServerOperation> HealthCheckServerOperations { get; set; }

        // Audit
        public virtual DbSet<HealthCheckVPNAudit_LastRecord> HealthCheckVPNAudit_LastRecords { get; set; }

        public virtual DbSet<HealthCheckRouterAudit_LastRecord> HealthCheckRouterAudit_LastRecords { get; set; }

        // End HealthCheck

        public virtual DbSet<UserApplicationTenantSetting> UserApplicationTenantSettings { get; set; }

        public virtual DbSet<OrderEvent> OrderEvents { get; set; }

        public virtual DbSet<InventoryEvent> InventoryEvents { get; set; }

        public virtual DbSet<TenantScaleMeasureCounts> TenantScaleMeasureCounts { get; set; }

        public virtual DbSet<TenantApplicationNotificationSetting> TenantApplicationNotificationSettings { get; set; }

        public virtual DbSet<Controller_AuditLog> Controller_AuditLogs { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<HL7CloudServer> HL7CloudServers { get; set; }

        public virtual DbSet<HL7Server> HL7Servers { get; set; }

        public virtual DbSet<JobQueueItem> JobQueueItems { get; set; }

        public virtual DbSet<Job> Jobs { get; set; }

        public virtual DbSet<JobType> JobTypes { get; set; }

        public virtual DbSet<VirtualMachine> VirtualMachines { get; set; }

        public virtual DbSet<InputBatchJobData> InputBatchJobData { get; set; }

        public virtual DbSet<OrderEventOutputOrder> OrderEventOutputOrders { get; set; }

        public virtual DbSet<InventoryEventOutputTransaction> InventoryEventOutputTransactions { get; set; }

        public virtual DbSet<OutputBatchJobDataExternalSystem> OutputBatchJobDataExternalSystems { get; set; }

        public virtual DbSet<OutputBatchJobData> OutputBatchJobData { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderEventHandlerReplenishmentSource> OrderEventHandlerReplenishmentSources { get; set; }

        public virtual DbSet<OrderEventHandler> OrderEventHandlers { get; set; }

        public virtual DbSet<OrderEventType> OrderEventTypes { get; set; }

        public virtual DbSet<InventoryEventType> InventoryEventTypes { get; set; }

        public virtual DbSet<InventoryEventHandlerLocation> InventoryEventHandlerLocations { get; set; }


        public virtual DbSet<InventoryEventHandler> InventoryEventHandlers { get; set; }        

        public DbSet<Product> Products => Set<Product>();


        public DbSet<ProductManufacturer> ProductManufacturers => Set<ProductManufacturer>();

        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();

        public DbSet<ProductUNSPSCCommodity> ProductUNSPSCCommodities => Set<ProductUNSPSCCommodity>();

        public DbSet<ProductUNSPSCSegment> ProductUNSPSCSegments => Set<ProductUNSPSCSegment>();

        public DbSet<ProductUNSPSCFamily> ProductUNSPSCFamilies => Set<ProductUNSPSCFamily>();

        public DbSet<ProductUNSPSCClass> ProductUNSPSCClasses => Set<ProductUNSPSCClass>();

        public async Task<int> ExecuteSqlRawAsync([NotNull] string sql, [NotNull] params object[] parameters)
        {
            return await Database.ExecuteSqlRawAsync(sql, parameters).ConfigureAwait(false);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("dbo");

            DBConfigurations.ApplyConfiguration(modelBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Test");
        //}

        /// <summary>
        /// Set the current user information in the database SESSION_CONTEXT
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <returns>A SessionUser instance containing the user's name, user id and tenant id</returns>
        public async Task<SessionUser> SetSessionUserAsync(string userName)
        {
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            try
            {
                if (string.IsNullOrEmpty(userName))
                    throw new ArgumentNullException("userName");

                var connection = Database.GetDbConnection();

                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                SessionUser sessionUser = new SessionUser
                {
                    UserName = userName.Trim()
                };

                using (DbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"EXEC app.SetSessionUser '{sessionUser.UserName}';SELECT app.GetUserId(),app.GetTenantId();";

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        await reader.ReadAsync();

                        sessionUser.UserId = reader.GetInt32(0);

                        if (!await reader.IsDBNullAsync(1))
                            sessionUser.TenantId = reader.GetInt32(1);
                    }
                }

                return sessionUser;

            }
            catch (Exception ex)
            {

                throw;
            }
#pragma warning restore CS0168 // The variable 'ex' is declared but never used

        }
    }
}
