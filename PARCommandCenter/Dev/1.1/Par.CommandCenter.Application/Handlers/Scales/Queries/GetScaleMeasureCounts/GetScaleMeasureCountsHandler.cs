using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Scales.Queries.GetScaleMeasureCounts
{
    public class GetScaleMeasureCountsHandler : IRequestHandler<GetScaleMeasureCountsQuery, GetScaleMeasureCountsResponse>
    {
        private readonly ILogger<GetScaleMeasureCountsHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        private readonly ICurrentUserService _currentUserService;

        public GetScaleMeasureCountsHandler(IApplicationDbContext dbContext, IMapper mapper, ICurrentUserService currentUserService, ILogger<GetScaleMeasureCountsHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<GetScaleMeasureCountsResponse> Handle(GetScaleMeasureCountsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = from sc in _dbContext.TenantScaleMeasureCounts
                            where _currentUserService.TenantIds.Contains(sc.TenantId)
                            select sc;

                var scaleCounts = await query
                  .OrderBy(s => s.TenantName)
                  .ProjectTo<TenantScaleMeasureCountsModel>(_mapper.ConfigurationProvider)
                  .ToListAsync(cancellationToken)
                  .ConfigureAwait(false);

                if (!scaleCounts.Any())
                {
                    return new GetScaleMeasureCountsResponse
                    {
                        TenantScaleMeasureCounts = null
                    };
                }

                // Ignore last hour scale counts                
                var timeUtc = DateTime.UtcNow;
                TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);

                var currrentHour = easternTime.AddHours(1).Hour;

                var propertyName = string.Empty;
                foreach (PropertyInfo propInfo in scaleCounts.FirstOrDefault().GetType().GetProperties())
                {
                    if (propInfo.Name.Contains(currrentHour.ToString()))
                    {
                        propertyName = propInfo.Name;
                        break;
                    }
                }

                foreach (var scaleCount in scaleCounts)
                {
                    PropertyInfo prop = scaleCount.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                    if (null != prop && prop.CanWrite)
                    {
                        prop.SetValue(scaleCount, Convert.ChangeType(0, prop.PropertyType), null);
                    }
                }




                return new GetScaleMeasureCountsResponse
                {
                    TenantScaleMeasureCounts = scaleCounts
                };
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
