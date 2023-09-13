using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.Users;

namespace Par.CommandCenter.Identity.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.TenantId)
               .HasColumnName(@"TenantId");

            builder.Property(x => x.NormalizedUserName)
              .HasColumnName(@"NormalizedUserName");

            builder.Property(x => x.UserName)
               .HasColumnName(@"UserName");

            builder.Property(x => x.AzureAdObjectId)
               .HasColumnName(@"AzureAdObjectId");

            builder.Property(x => x.Deleted)
               .HasColumnName(@"Deleted");

        }
    }
}
