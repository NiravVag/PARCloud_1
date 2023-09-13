using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities.Interfaces;
using Par.CommandCenter.Domain.Model;
using System;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.Interfaces.Queries.GetInventoryEventQueue
{
    public class GetInventoryEventQueueQueryResponse
    {
        public IEnumerable<InventoryEventQueueQueryModel> InventoryEventQueueItems { get; set; }
    }

    public class InventoryEventQueueQueryModel : IMap<InventoryEvent>
    {
        public Guid Id { get; internal set; }
       
        public long InventoryTransactionId { get; set; }        
        
        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Published { get; set; }

        public int InventoryEventHandlerId { get; set; }

        public string InventoryEventHandlerName { get; set; }

        public int InventoryEventTypeId { get; set; }

        public string InventoryEventTypeName { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public DateTimeOffset? Started { get; set; }

        public string ErrorMessage { get; set; }
        
        public void Mapping(Profile profile)
        {
            //profile.miss
            profile.CreateMap<InventoryEvent, InventoryEventQueueQueryModel>();
        }
    }
}
