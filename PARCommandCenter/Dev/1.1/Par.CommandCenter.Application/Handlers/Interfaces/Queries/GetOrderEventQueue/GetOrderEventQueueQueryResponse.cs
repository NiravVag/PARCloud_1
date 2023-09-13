using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities.Interfaces;
using Par.CommandCenter.Domain.Model;
using System;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.Interfaces.Queries.GetOrderEventQueue
{
    public class GetOrderEventQueueQueryResponse
    {
        public IEnumerable<OrderEventQueueQueryModel> OrderEventQueueItems { get; set; }
    }

    public class OrderEventQueueQueryModel : IMap<OrderEvent>
    {
        public Guid Id { get; internal set; }
       
        public int OrderId { get; set; }

        public int OrderNumber { get; set; }
        
        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Published { get; set; }

        public int OrderEventHandlerId { get; set; }

        public string OrderEventHandlerName { get; set; }

        public int OrderEventTypeId { get; set; }

        public string OrderEventTypeName { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public DateTimeOffset? Started { get; set; }

        public string ErrorMessage { get; set; }
        
        public void Mapping(Profile profile)
        {
            //profile.miss
            profile.CreateMap<OrderEvent, OrderEventQueueQueryModel>();
        }
    }
}
