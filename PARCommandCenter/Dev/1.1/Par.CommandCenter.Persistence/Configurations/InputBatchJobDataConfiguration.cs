using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.Interfaces;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class InputBatchJobDataConfiguration : IEntityTypeConfiguration<InputBatchJobData>
    {
        public void Configure(EntityTypeBuilder<InputBatchJobData> builder)
        {
            builder.ToTable("InputBatchJobData");

            builder.HasKey(x => x.Id);

            builder.Ignore(x => x.Job);

            builder.Ignore(x => x.JobType);
        }
    }
}
