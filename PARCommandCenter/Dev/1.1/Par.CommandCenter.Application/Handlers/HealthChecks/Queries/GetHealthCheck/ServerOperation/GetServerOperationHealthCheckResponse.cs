using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Queries.GetHealthCheck.ServerOperation
{
    public class GetServerOperationHealthCheckResponse
    {
        public IEnumerable<HealthCheckServerOperationModel> HealthChecks { get; set; }

    }

    public class HealthCheckServerOperationModel : IMap<HealthCheckServerOperation>
    {
        public int Id { get; set; }

        public string ServerName { get; set; }

        public string Status { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<HealthCheckServerOperation, HealthCheckServerOperationModel>();
        }
    }
}
