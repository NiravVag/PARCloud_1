using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.Interfaces.Queries.GetJobQueue
{
    public class GetJobQueueResponse
    {
        public List<JobQueueItemModel> JobQueueItems { get; set; }
    }

    public class JobQueueItemModel : IMap<JobQueueItem>
    {
        public Guid Id { get; internal set; }

        public DateTimeOffset? Submitted { get; internal set; }

        public Job Job { get; set; }

        public JobType JobType { get; set; }

        public DateTimeOffset? RunOnceDate { get; set; }

        public DateTimeOffset? Started { get; set; }

        public string ErrorMessage { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<JobQueueItem, JobQueueItemModel>();
        }
    }
}
