using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities.Interfaces;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class JobTypeConfiguration : IEntityTypeConfiguration<JobType>
    {
        public void Configure(EntityTypeBuilder<JobType> builder)
        {
            builder.ToTable("JobType");

            builder.HasKey(x => x.Id);
        }
    }
}
