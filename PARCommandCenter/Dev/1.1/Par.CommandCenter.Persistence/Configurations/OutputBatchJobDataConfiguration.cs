using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.Interfaces;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class OutputBatchJobDataConfiguration : IEntityTypeConfiguration<OutputBatchJobData>
    {
        public void Configure(EntityTypeBuilder<OutputBatchJobData> builder)
        {
            builder.ToTable("OutputBatchJobData");

            builder.HasKey(x => x.Id);
        }
    }
}
