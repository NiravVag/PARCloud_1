using AutoMapper;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using Par.CommandCenter.Domain.Enums;
using Par.CommandCenter.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Map.Queries.GetHealthCheckMapPoints
{
    public class GetHealthCheckMapPointsHandler : IRequestHandler<GetHealthCheckMapPointsQuery, GetHealthCheckMapPointsResponse>
    {
        private readonly ILogger<GetHealthCheckMapPointsHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAzureMapAPIWebClient _azureMapAPIWebClient;

        private readonly ICurrentUserService _currentUserService;

        public GetHealthCheckMapPointsHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IAzureMapAPIWebClient azureMapAPIWebClient, IMapper mapper, ILogger<GetHealthCheckMapPointsHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _azureMapAPIWebClient = azureMapAPIWebClient;
            _currentUserService = currentUserService;
        }

        public async Task<GetHealthCheckMapPointsResponse> Handle(GetHealthCheckMapPointsQuery request, CancellationToken cancellationToken)
        {
            if (request.ErrorsOnly && request.HealthyOnly)
            {
                throw new ArgumentException("Error only and healthy only can't be both true");
            }

            var query = from hc in _dbContext.MapHealthCheckPoints
                        select hc;

            if (request.ErrorsOnly)
            {
                query = from hc in _dbContext.MapHealthCheckPoints
                        where hc.PointStatus.ToLower() == "error"
                        select hc;
            }

            if (request.HealthyOnly)
            {
                query = from hc in _dbContext.MapHealthCheckPoints
                        where hc.PointStatus.ToLower() == "healthy"
                        select hc;
            }



            var predicate = PredicateBuilder.New<MapHealthCheckPoint>();

            if (request.IncludeRouters)
            {
                predicate = predicate.Or(u => u.HealthCheckType.Contains("router"));
            }

            if (request.IncludeControllers)
            {
                predicate = predicate.Or(u => u.HealthCheckType.Contains("controller"));
            }

            if (request.IncludeScales)
            {
                predicate = predicate.Or(u => u.HealthCheckType.Contains("scale"));
            }

            if (request.IncludeInterfaces)
            {
                predicate = predicate.Or(u => u.HealthCheckType.Contains("interface"));
            }

            //if (request.IncludeControllers || request.IncludeInterfaces || request.IncludeRouters || request.IncludeScales)
                query = query.Where(predicate);

            query = query.Where(x => _currentUserService.TenantIds.Contains(x.TenantId));

            switch (request.DateRangeFilter)
            {
                case DateRangeFilterType.Past24Hours:
                    query = query.Where(x => x.StatusDate >= DateTimeOffset.UtcNow.AddDays(-1));
                    break;
                case DateRangeFilterType.Past3Days:
                    query = query.Where(x => x.StatusDate >= DateTimeOffset.UtcNow.AddDays(-3));
                    break;
                case DateRangeFilterType.Past7Days:
                    query = query.Where(x => x.StatusDate >= DateTimeOffset.UtcNow.AddDays(-7));
                    break;
                case DateRangeFilterType.Past30Days:
                    query = query.Where(x => x.StatusDate >= DateTimeOffset.UtcNow.AddDays(-30));
                    break;
                case DateRangeFilterType.CustomDate:
                    if (request.StartDate == null || request.EndDate == null)
                    {
                        throw new ArgumentNullException($"The {nameof(request.StartDate)} and the {nameof(request.EndDate)} can't be null");
                    }

                    query = query.Where(x => x.StatusDate >= request.StartDate.Value && x.StatusDate <= request.EndDate.Value.Add(DateTime.MaxValue.TimeOfDay));
                    break;
                default:
                    query = query.Where(x => x.StatusDate >= DateTimeOffset.UtcNow.AddDays(-1));
                    break;
            }

            var mapHealthCheckPoints = await query.ToListAsync();

            var addresses = (from point in mapHealthCheckPoints
                             .Where(x => !string.IsNullOrWhiteSpace(x.AddressLine1) && !string.IsNullOrWhiteSpace(x.City))
                             select new
                             {
                                 AddressLine1 = point.AddressLine1,
                                 City = point.City,
                                 PostalCode = point.PostalCode,
                             })
                            .Distinct()
                            .Select(a => new Address
                            {
                                AddressLine1 = a.AddressLine1,
                                City = a.City,
                                PostalCode = a.PostalCode,
                            });


            var geoLocationTask = addresses.Select(address => _azureMapAPIWebClient.GetAddressCoordinates(address, cancellationToken));

            var geoCoordinates = (await Task.WhenAll(geoLocationTask)).Where(x => x != null).ToList();

            var features = new List<Feature>();
            foreach (var mapHealthCheckPoint in mapHealthCheckPoints)
            {
                var mapGeoLocation = geoCoordinates.FirstOrDefault(l => l.Address?.AddressLine1 == mapHealthCheckPoint.AddressLine1 && l.Address?.PostalCode == mapHealthCheckPoint.PostalCode);

                if (mapGeoLocation != null)
                {
                    Position position = new Position(mapGeoLocation.Latitude, mapGeoLocation.Longitude);

                    Point point = new Point(position);

                    Feature feature = null;

                    var properties = new Dictionary<string, object>();
                    properties.Add("PointType", mapHealthCheckPoint.PointStatus);
                    properties.Add("TenantName", mapHealthCheckPoint.TenantName);
                    properties.Add("FacilityName", mapHealthCheckPoint.FacilityName);
                    properties.Add("HealthCheckType", mapHealthCheckPoint.HealthCheckType);

                    feature = new Feature(point, properties, mapHealthCheckPoint.Id.ToString());

                    features.Add(feature);
                }
            }


            return new GetHealthCheckMapPointsResponse
            {
                Type = "FeatureCollection",

                Metadata = new
                {
                    Generated = DateTime.Now,
                    Title = "Par Cloud Tenants Health Check",
                    features.Count,
                },

                Features = features
            };
        }
    }
}
