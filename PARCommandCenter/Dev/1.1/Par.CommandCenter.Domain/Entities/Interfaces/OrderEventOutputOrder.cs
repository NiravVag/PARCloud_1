using System;
using System.Collections.Generic;
using System.Text;

namespace Par.CommandCenter.Domain.Entities.Interfaces
{
    public  class OrderEventOutputOrder
    {
        /// <summary>
        /// The id
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// The order event output id
        /// </summary>
        public int OrderEventOutputId { get; set; }

        /// <summary>
        /// The order id
        /// </summary>
        public int OrderId { get; set; }

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
