using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.Interfaces;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class JobQueueConfiguration : IEntityTypeConfiguration<JobQueueItem>
    {
        public void Configure(EntityTypeBuilder<JobQueueItem> builder)
        {
            builder.ToTable("JobQueue");

            builder.HasKey(x => x.Id);

            builder.Ignore(x => x.Job);

            builder.Ignore(x => x.JobType);
        }
    }
}
