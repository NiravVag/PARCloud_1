using System;
using System.Collections.Generic;
using System.Text;

namespace Par.CommandCenter.Domain.Entities.Interfaces
{
    /// <summary>
    /// An order event
    /// </summary>
    public class OrderEvent
    {
        /// <summary>
        /// The id
        /// </summary>
        public Guid Id { get; set; }

        public int TenantId { get; set; }   

        /// <summary>
        /// The order id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Date and time published to service bus
        /// </summary>
        public DateTimeOffset? Published { get; set; }

        /// <summary>
        /// Date and time formatter started
        /// </summary>
        public DateTimeOffset? Started { get; set; }

        /// <summary>
        /// Order event output id
        /// </summary>
        public int? OrderEventOutputId { get; set; }

        /// <summary>
        /// Error message if any
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Id of the user who created the manufacturer
        /// </summary>
        public int CreatedUserId { get; set; }

        /// <summary>
        /// The date and time created
        /// </summary>
        public DateTimeOffset Created { get; set; }

        /// <summary>
        /// Id of the user who last modified the manufacturer
        /// </summary>
        public int ModifiedUserId { get; set; }

        /// <summary>
        /// The date and time last modified
        /// </summary>
        public DateTimeOffset Modified { get; set; }

       
        public int OrderEventTypeId { get; set; }

        /// <summary>
        /// The order event handler replenishment source id
        /// </summary>
        public int OrderEventHandlerReplenishmentSourceId { get; set; }        
    }
}
