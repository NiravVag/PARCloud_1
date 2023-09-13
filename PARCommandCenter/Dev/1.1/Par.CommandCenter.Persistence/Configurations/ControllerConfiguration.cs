using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class ControllerConfiguration : IEntityTypeConfiguration<Controller>
    {
        public void Configure(EntityTypeBuilder<Controller> builder)
        {
            builder.ToTable("Controller");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.TenantId)
               .HasColumnName(@"TenantId");

            builder.Property(x => x.PortName)
               .HasColumnName(@"PortName");

            builder.Property(x => x.RouterId)
              .HasColumnName(@"RouterId");

            builder.Property(x => x.ControllerTypeId)
              .HasColumnName(@"ControllerTypeId");

            builder.Property(x => x.IpAddress)
              .HasColumnName(@"IpAddress");

            builder.Property(x => x.NetworkPort)
              .HasColumnName(@"NetworkPort");


            builder.Property(x => x.MACAddress)
              .HasColumnName(@"MACAddress");

            builder.Property(x => x.FirmwareVersion)
              .HasColumnName(@"FirmwareVersion");

            builder.Property(x => x.ParChargeMode)
              .HasColumnName(@"PARChargeMode");

            builder.Property(x => x.ParChargeBatch)
              .HasColumnName(@"ParChargeBatch");



            builder.Property(x => x.CreatedUserId)
                .HasColumnName(@"CreatedUserId");

            builder.Property(x => x.Created)
              .HasColumnName(@"Created");

            builder.Property(x => x.ModifiedUserId)
              .HasColumnName(@"ModifiedUserId");

            builder.Property(x => x.Modified)
               .HasColumnName(@"Modified");


            builder.Property(x => x.Active)
               .HasColumnName(@"Active");


            builder.Ignore(x => x.Router);

            builder.Ignore(x => x.VirtualMachine);

            
        }
    }
}
