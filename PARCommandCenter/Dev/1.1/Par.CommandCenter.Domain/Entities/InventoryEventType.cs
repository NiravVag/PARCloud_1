using System;
using System.Collections.Generic;
using System.Text;

namespace Par.CommandCenter.Domain.Entities
{
    /// <summary>
    /// An inventory event type
    /// </summary>
    public class InventoryEventType
    {
        /// <summary>
        /// The id
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// The name
        /// </summary>
        public string Name { get; internal set; }
    }
}
