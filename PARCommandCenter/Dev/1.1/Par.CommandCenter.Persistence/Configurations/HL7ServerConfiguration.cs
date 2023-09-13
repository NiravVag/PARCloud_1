using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class HL7ServerConfiguration : IEntityTypeConfiguration<HL7Server>
    {
        public void Configure(EntityTypeBuilder<HL7Server> builder)
        {
            builder.ToTable("HL7Server");

            builder.HasKey(x => x.Id);
        }
    }
}
