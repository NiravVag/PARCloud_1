using System;

namespace Par.CommandCenter.Domain.Entities.Interfaces
{
    public class JobQueueItem
    {
        /// <summary>
        /// The id
        /// </summary>
        public Guid Id { get; set; }

        public int TenantId { get; set; }

        /// <summary>
        /// Date and time job was added to queue
        /// </summary>
        public DateTimeOffset? Submitted { get; set; }

        /// <summary>
        /// Date and time message was published
        /// </summary>
        public DateTimeOffset? Published { get; set; }

        /// <summary>
        /// The job id
        /// </summary>
        public int JobId { get; set; }

        /// <summary>
        /// The job schedule id or null if not scheduled
        /// </summary>
        public int? JobScheduleId { get; set; }

        /// <summary>
        /// The date and time to run or null if immediate
        /// </summary>
        public DateTimeOffset? RunOnceDate { get; set; }

        /// <summary>
        /// The user who submitted the job or null if scheduled
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Date and time the job was started
        /// </summary>
        public DateTimeOffset? Started { get; set; }

        /// <summary>
        /// Error message if job ended in error
        /// </summary>
        public string ErrorMessage { get; set; }

        public Job Job { get; set; }

        public JobType JobType { get; set; }
    }
}
