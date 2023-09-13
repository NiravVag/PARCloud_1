using System;
using System.Collections.Generic;
using System.Text;

namespace Par.CommandCenter.Domain.Entities.Interfaces
{
    public class OutputBatchJobData
    {

        /// <summary>
        /// The id
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// The id of the job
        /// </summary>
        public int JobId { get; set; }

        /// <summary>
        /// Date and time data was created
        /// </summary>
        public DateTimeOffset Created { get; internal set; }

        /// <summary>
        /// The MIME type
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// The data
        /// </summary>
        public byte[] OutputData { get; set; }

    }
}
