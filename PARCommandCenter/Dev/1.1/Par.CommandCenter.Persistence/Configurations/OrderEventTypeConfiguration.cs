using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class OrderEventTypeConfiguration : IEntityTypeConfiguration<OrderEventType>
    {
        public void Configure(EntityTypeBuilder<OrderEventType> builder)
        {
            builder.ToTable("OrderEventType");

            builder.HasKey(x => x.Id);
        }
    }
}
