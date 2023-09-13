using System;
using System.Collections.Generic;
using System.Text;

namespace Par.CommandCenter.Domain.Entities.Interfaces
{
    public class OutputBatchJobDataExternalSystem 
    {
        /// <summary>
        /// The id
        /// </summary>
        public int Id { get; set; }

        public int TenantId { get; set; }

        /// <summary>
        /// The output data id
        /// </summary>
        public int OutputBatchJobDataId { get; set; }

        /// <summary>
        /// The external system id
        /// </summary>
        public int ExternalSystemId { get; set; }

        /// <summary>
        /// Name of the output file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Location of the output file
        /// </summary>
        public string FileLocation { get; set; }

        /// <summary>
        /// Date and time message was published to service bus
        /// </summary>
        public DateTimeOffset? Published { get; set; }

        /// <summary>
        /// Date and time processing was started
        /// </summary>
        public DateTimeOffset? Started { get; set; }

        /// <summary>
        /// Date and time data was sent
        /// </summary>
        public DateTimeOffset? Sent { get; set; }

        /// <summary>
        /// Error message if processing failed
        /// </summary>
        public string ErrorMessage { get; set; }


    }
}
