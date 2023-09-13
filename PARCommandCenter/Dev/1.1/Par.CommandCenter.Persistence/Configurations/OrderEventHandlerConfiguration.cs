using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Par.CommandCenter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class OrderEventHandlerConfiguration : IEntityTypeConfiguration<OrderEventHandler>
    {
        public void Configure(EntityTypeBuilder<OrderEventHandler> builder)
        {
            builder.ToTable("OrderEventHandler");

            builder.HasKey(x => x.Id);
        }
    }
}
