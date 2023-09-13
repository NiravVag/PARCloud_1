using System;
using System.Collections.Generic;
using System.Text;

namespace Par.CommandCenter.Domain.Entities
{
    /// <summary>
    /// Configuration data on how an inventory event should be handled
    /// </summary>
    public class InventoryEventHandler
    {
        /// <summary>
        /// The id
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// The handler name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The facility id or null if global
        /// </summary>
        public int FacilityId { get; set; }

        /// <summary>
        /// The inventory event formatter id
        /// </summary>
        public int InventoryEventFormatterId { get; set; }

        /// <summary>
        /// The event schedule id if scheduled
        /// </summary>
        public int? EventScheduleId { get; set; }

        /// <summary>
        /// True if this handler currently on hold
        /// </summary>
        public bool Hold { get; set; }

        /// <summary>
        /// True if this handler has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Id of the user who created the handler
        /// </summary>
        public int CreatedUserId { get; internal set; }

        /// <summary>
        /// The date and time created
        /// </summary>
        public DateTimeOffset Created { get; internal set; }
    }
}
