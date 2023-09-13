using System;
using System.Collections.Generic;
using System.Text;

namespace Par.CommandCenter.Domain.Enums
{
    /// <summary>
    /// Possible event types used by order processing
    /// </summary>
    public enum OrderEventTypeId
    {
        /// <summary>
        /// Order created
        /// </summary>
        OrderCreated = 1,

        /// <summary>
        /// Order edited
        /// </summary>
        OrderEdited = 2,

        /// <summary>
        /// Order picked
        /// </summary>
        OrderPicked = 3,

        /// <summary>
        /// Order received
        /// </summary>
        OrderReceived = 4,

        /// <summary>
        /// Order closed
        /// </summary>
        OrderClosed = 5,

        /// <summary>
        /// Order deleted
        /// </summary>
        OrderDeleted = 6,

        /// <summary>
        /// Order reopened
        /// </summary>
        OrderReopened = 7,

        /// <summary>
        /// Order undeleted
        /// </summary>
        OrderUndeleted = 8
    }
}
