using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class ScaleMeasureCountsConfiguration : IEntityTypeConfiguration<TenantScaleMeasureCounts>
    {
        public void Configure(EntityTypeBuilder<TenantScaleMeasureCounts> builder)
        {
            builder.ToView("ScaleMeasureCounts");

            builder.HasKey(x => x.TenantId);

            builder.Property(x => x.TenantId)
               .HasColumnName(@"TenantId");

            builder.Property(x => x.TenantName)
               .HasColumnName(@"TenantName");

            builder.Property(x => x.Hour1Total)
               .HasColumnName(@"0");

            builder.Property(x => x.Hour2Total)
               .HasColumnName(@"1");

            builder.Property(x => x.Hour3Total)
              .HasColumnName(@"2");

            builder.Property(x => x.Hour4Total)
              .HasColumnName(@"3");

            builder.Property(x => x.Hour5Total)
              .HasColumnName(@"4");

            builder.Property(x => x.Hour6Total)
               .HasColumnName(@"5");

            builder.Property(x => x.Hour7Total)
               .HasColumnName(@"6");

            builder.Property(x => x.Hour8Total)
              .HasColumnName(@"7");

            builder.Property(x => x.Hour9Total)
              .HasColumnName(@"8");

            builder.Property(x => x.Hour10Total)
              .HasColumnName(@"9");

            builder.Property(x => x.Hour11Total)
               .HasColumnName(@"10");

            builder.Property(x => x.Hour12Total)
               .HasColumnName(@"11");

            builder.Property(x => x.Hour13Total)
              .HasColumnName(@"12");

            builder.Property(x => x.Hour14Total)
              .HasColumnName(@"13");

            builder.Property(x => x.Hour15Total)
              .HasColumnName(@"14");

            builder.Property(x => x.Hour16Total)
               .HasColumnName(@"15");

            builder.Property(x => x.Hour17Total)
               .HasColumnName(@"16");

            builder.Property(x => x.Hour18Total)
              .HasColumnName(@"17");

            builder.Property(x => x.Hour19Total)
              .HasColumnName(@"18");

            builder.Property(x => x.Hour20Total)
              .HasColumnName(@"19");

            builder.Property(x => x.Hour21Total)
               .HasColumnName(@"20");

            builder.Property(x => x.Hour22Total)
               .HasColumnName(@"21");

            builder.Property(x => x.Hour23Total)
              .HasColumnName(@"22");

            builder.Property(x => x.Hour24Total)
              .HasColumnName(@"23");
        }
    }
}
