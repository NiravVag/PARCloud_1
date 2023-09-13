using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities.HealthCheck;

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Queries.GetHealthCheck
{
    public class HealthCheckStatisticModel : IMap<HealthCheckStatistic>
    {
        public HealthCheckType Type { get; set; }

        public int TotalTenantsCount { get; set; }

        public int HealthyTenantsCount { get; set; }
        public int HealthyCount { get; set; }

        public int UnHealthyTenantsCount { get; set; }
        public int UnHealthyCount { get; set; }

        public int WarrningTenantsCount { get; set; }
        public int WarrningCount { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<HealthCheckStatistic, HealthCheckStatisticModel>();
        }
    }
}
