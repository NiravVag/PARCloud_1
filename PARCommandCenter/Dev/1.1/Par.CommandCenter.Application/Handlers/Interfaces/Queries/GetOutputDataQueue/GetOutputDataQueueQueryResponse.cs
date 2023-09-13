using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities.Interfaces;
using Par.CommandCenter.Domain.Model;
using System;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.Interfaces.Queries.GetOutputDataQueue
{
    public class GetOutputDataQueueQueryResponse
    {
        public IEnumerable<GetOutputDataQueueQueryModel> OutputDataQueueItems { get; set; }
    }

    public class GetOutputDataQueueQueryModel : IMap<OutputDataQueueEntry>
    {
        public int Id { get; internal set; }

        /// <summary>
        /// The data type
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Date and time created
        /// </summary>
        public DateTimeOffset? Created { get; set; }

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


        public DateTimeOffset? Started { get; set; }

        public string ErrorMessage { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<OutputDataQueueEntry, GetOutputDataQueueQueryModel>();
        }
    }
}
