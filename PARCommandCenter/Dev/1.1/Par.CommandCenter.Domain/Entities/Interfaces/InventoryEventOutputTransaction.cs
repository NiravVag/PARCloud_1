using System;
using System.Collections.Generic;
using System.Text;

namespace Par.CommandCenter.Domain.Entities.Interfaces
{
    public class InventoryEventOutputTransaction
    {
        /// <summary>
        /// The id
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// The inventory event output id
        /// </summary>
        public int InventoryEventOutputId { get; set; }

        /// <summary>
        /// The transaction id
        /// </summary>
        public long InventoryTransactionId { get; set; }

        /// <summary>
        /// Id of the user who created the manufacturer
        /// </summary>
        public int CreatedUserId { get; internal set; }

        /// <summary>
        /// The date and time created
        /// </summary>
        public DateTimeOffset Created { get; internal set; }

        /// <summary>
        /// Id of the user who last modified the manufacturer
        /// </summary>
        public int ModifiedUserId { get; internal set; }

        /// <summary>
        /// The date and time last modified
        /// </summary>
        public DateTimeOffset Modified { get; internal set; }
    }
}
