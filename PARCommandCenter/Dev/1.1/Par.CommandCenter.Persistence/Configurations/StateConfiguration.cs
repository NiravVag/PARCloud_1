using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class StateConfiguration : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.ToTable("State");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
               .HasColumnName(@"Name");

            builder.Property(x => x.StateCode)
               .HasColumnName(@"StateCode");
        }
    }
}
