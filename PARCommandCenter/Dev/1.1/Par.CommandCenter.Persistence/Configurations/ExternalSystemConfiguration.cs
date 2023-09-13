using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class ExternalSystemConfiguration : IEntityTypeConfiguration<ExternalSystem>
    {
        public void Configure(EntityTypeBuilder<ExternalSystem> builder)
        {
            builder.ToTable("ExternalSystem");

            builder.HasKey(x => x.Id);
        }
    }
}
