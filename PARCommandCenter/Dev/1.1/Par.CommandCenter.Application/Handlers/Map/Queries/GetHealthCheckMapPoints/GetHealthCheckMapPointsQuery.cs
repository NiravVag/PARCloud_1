using MediatR;
using Par.CommandCenter.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Par.CommandCenter.Application.Handlers.Map.Queries.GetHealthCheckMapPoints
{
    public class GetHealthCheckMapPointsQuery : IRequest<GetHealthCheckMapPointsResponse>
    {

        public bool ErrorsOnly { get; set; }

        public bool HealthyOnly { get; set; }

        [Required]
        public DateRangeFilterType DateRangeFilter { get; set; }

        public DateTimeOffset? StartDate { get; set; }

        public DateTimeOffset? EndDate { get; set; }

        public bool IncludeInterfaces { get; set; }

        public bool IncludeRouters { get; set; }

        public bool IncludeControllers { get; set; }

        public bool IncludeScales { get; set; }
    }
}
