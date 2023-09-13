using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class HL7CloudServerConfiguration : IEntityTypeConfiguration<HL7CloudServer>
    {
        public void Configure(EntityTypeBuilder<HL7CloudServer> builder)
        {
            builder.ToTable("HL7CloudServer");

            builder.HasKey(x => x.Id);
        }
    }
}
