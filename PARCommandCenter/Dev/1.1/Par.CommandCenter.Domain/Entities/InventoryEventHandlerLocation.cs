using System;
using System.Collections.Generic;
using System.Text;

namespace Par.CommandCenter.Domain.Entities
{
    /// <summary>
    /// Location configuration for an event handler
    /// </summary>
    public class InventoryEventHandlerLocation
    {
        /// <summary>
        /// The id
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// The inventory event handler id
        /// </summary>
        public int InventoryEventHandlerId { get; set; }

        /// <summary>
        /// The location id
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// The Replenishment Source id
        /// </summary>
        public int? ReplenishmentSourceId { get; set; }

        /// <summary>
        /// The type of inventory event
        /// </summary>
        public int InventoryEventTypeId { get; set; }

        /// <summary>
        /// The external system id
        /// </summary>
        public int ExternalSystemId { get; set; }

        /// <summary>
        /// Path/location to write output files
        /// </summary>
        public string FileLocation { get; set; }

        /// <summary>
        /// True if deleted
        /// </summary>
        public bool Deleted { get; set; }
    }
}
