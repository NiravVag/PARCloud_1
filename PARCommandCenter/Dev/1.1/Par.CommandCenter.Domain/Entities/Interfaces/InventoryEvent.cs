using System;
using System.Collections.Generic;
using System.Text;

namespace Par.CommandCenter.Domain.Entities.Interfaces
{
    /// <summary>
    /// An inventory event
    /// </summary>
    public class InventoryEvent
    {
        /// <summary>
        /// The id
        /// </summary>
        public Guid Id { get;  set; }

        public int TenantId { get; set; }

        /// <summary>
        /// The transaction id
        /// </summary>
        public long InventoryTransactionId { get; set; }   
        
        /// <summary>
        /// Date and time published to service bus
        /// </summary>
        public DateTimeOffset? Published { get; set; }

        /// <summary>
        /// Date and time formatter started
        /// </summary>
        public DateTimeOffset? Started { get; set; }

        /// <summary>
        /// Inventory event output id
        /// </summary>
        public int? InventoryEventOutputId { get; set; }

        /// <summary>
        /// Error message if any
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Id of the user who created the event
        /// </summary>
        public int CreatedUserId { get; internal set; }

        /// <summary>
        /// The date and time created
        /// </summary>
        public DateTimeOffset Created { get; set; }

        /// <summary>
        /// Id of the user who last modified the event
        /// </summary>
        public int ModifiedUserId { get;  set; }

        /// <summary>
        /// The date and time last modified
        /// </summary>
        public DateTimeOffset Modified { get; set; }
      
        public int InventoryEventTypeId { get; set; }

        /// <summary>
        /// The inventory event handler location id
        /// </summary>
        public int InventoryEventHandlerLocationId { get; set; }
    }
}
