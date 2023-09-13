using System;
using System.Collections.Generic;
using System.Text;

namespace Par.CommandCenter.Domain.Entities.Interfaces
{
    public class InputBatchJobData
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        /// <summary>
        /// Original location of source file
        /// </summary>
        public string FileLocation { get; set; }

        /// <summary>
        /// Original name of source file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The id of the job
        /// </summary>
        public int JobId { get; set; }

        /// <summary>
        /// Date and time file was received
        /// </summary>
        public DateTimeOffset Received { get; set; }

        /// <summary>
        /// Date and time message was published to service bus
        /// </summary>
        public DateTimeOffset? Published { get; set; }

        /// <summary>
        /// Date and time processing was started
        /// </summary>
        public DateTimeOffset? Started { get; set; }

        /// <summary>
        /// Date and time processing was completed
        /// </summary>
        public DateTimeOffset? Completed { get; set; }

        /// <summary>
        /// Error message if processing failed
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// The data
        /// </summary>
        public byte[] InputData { get; set; }

        public Job Job { get; set; }

        public JobType JobType { get; set; }
    }
}
