using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.Interfaces.Queries.GetInputDataQueue
{
    public class GetInputDataQueueQueryResponse
    {
        public IEnumerable<GetInputDataQueueQueryModel> InputDataQueueItems { get; set; }
    }

    public class GetInputDataQueueQueryModel : IMap<InputBatchJobData>
    {
        public int Id { get; internal set; }

        
        public string FileName { get; set; }

        public DateTimeOffset? Received { get; set; }

        public Job Job { get; set; }

        public JobType JobType { get; set; }        

        public DateTimeOffset? Started { get; set; }

        public string ErrorMessage { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<InputBatchJobData, GetInputDataQueueQueryModel>();
        }
    }
}
