using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.Interfaces;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class OutputBatchJobDataExternalSystemConfiguration : IEntityTypeConfiguration<OutputBatchJobDataExternalSystem>
    {
        public void Configure(EntityTypeBuilder<OutputBatchJobDataExternalSystem> builder)
        {
            builder.ToTable("OutputBatchJobDataExternalSystem");

            builder.HasKey(x => x.Id);
        }
    }
}
