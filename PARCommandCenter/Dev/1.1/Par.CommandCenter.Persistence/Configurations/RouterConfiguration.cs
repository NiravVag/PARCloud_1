using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Persistence.Configurations
{
    class RouterConfiguration : IEntityTypeConfiguration<Router>
    {
        public void Configure(EntityTypeBuilder<Router> builder)
        {
            builder.ToTable("Router");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.TenantId)
               .HasColumnName(@"TenantId");


            builder.Property(x => x.DeviceTypeId)
               .HasColumnName(@"DeviceTypeId");

            builder.Property(x => x.Address)
              .HasColumnName(@"Address");

            builder.Property(x => x.FirmwareVersion)
              .HasColumnName(@"FirmwareVersion");

            builder.Property(x => x.Deleted)
             .HasColumnName(@"Deleted");

            builder.Property(x => x.LastCommunication)
             .HasColumnName(@"LastCommunication");

            builder.Property(x => x.LastReboot)
             .HasColumnName(@"LastReboot");

            builder.Property(x => x.IsRunning)
               .HasColumnName(@"IsRunning");

            builder.Property(x => x.Created)
                .HasColumnName(@"Created");

            builder.Property(x => x.CreatedUserId)
                .HasColumnName(@"CreatedUserId");

            builder.Property(x => x.Deleted)
                .HasColumnName(@"Deleted");

            builder.Ignore(x => x.RegisteredControllerCount);


            builder.Ignore(x => x.VirtualMachine);

            builder.Ignore(x => x.DeviceType);
            
        }
    }
}
