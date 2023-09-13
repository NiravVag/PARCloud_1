using System;
using System.Collections.Generic;
using System.Text;

namespace Par.CommandCenter.Domain.Entities
{
    /// <summary>
    /// Replenishment source configuration for an event handler
    /// </summary>
    public class OrderEventHandlerReplenishmentSource
    {
        /// <summary>
        /// The id
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// The order event handler id
        /// </summary>
        public int OrderEventHandlerId { get; set; }

        /// <summary>
        /// The replenishment source id
        /// </summary>
        public int ReplenishmentSourceId { get; set; }       

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
