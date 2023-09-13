using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class OrderEventHandlerReplenishmentSourceConfiguration : IEntityTypeConfiguration<OrderEventHandlerReplenishmentSource>
    {
        public void Configure(EntityTypeBuilder<OrderEventHandlerReplenishmentSource> builder)
        {
            builder.ToTable("OrderEventHandlerReplenishmentSource");

            builder.HasKey(x => x.Id);
        }
    }
}
