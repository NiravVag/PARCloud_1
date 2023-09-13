using System;
using System.Collections.Generic;
using System.Text;

namespace Par.CommandCenter.Domain.Model
{
    public class OutputDataQueueEntry
    {
        /// <summary>
        /// The id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The data type
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Date and time created
        /// </summary>
        public DateTimeOffset Created { get; set; }

        /// <summary>
        /// Date and time published
        /// </summary>
        public DateTimeOffset? Published { get; set; }

        /// <summary>
        /// External system id
        /// </summary>
        public int ExternalSystemId { get; set; }

        /// <summary>
        /// External system name
        /// </summary>
        public string ExternalSystemName { get; set; }

        /// <summary>
        /// Date and time started
        /// </summary>
        public DateTimeOffset? Started { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
