using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using Par.CommandCenter.Domain.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CCAuditLog = Par.CommandCenter.Domain.Model.AuditLog.AuditLog;

namespace Par.CommandCenter.Application.Handlers.AuditLog.Queries
{
    public class GetAuditLogHandler : IRequestHandler<GetAuditLogQuery, GetAuditLogResponse>
    {
        private readonly ILogger<GetAuditLogHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;


        public GetAuditLogHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetAuditLogHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetAuditLogResponse> Handle(GetAuditLogQuery request, CancellationToken cancellationToken)
        {
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            try
            {
                var query = from l in _dbContext.Controller_AuditLogs
                            select l;

                if (!request.AllUsers)
                {
                    query = from l in query
                            where l.UserId == request.UserId
                            select l;
                }

                if (request.AuditLogTypes != AuditLogType.All)
                {
                    query = from l in query
                            where l.Type == request.AuditLogTypes.ToString()
                            select l;
                }

                if (request.AuditLogEntityTypes != AuditLogEntityType.All)
                {
                    query = from l in query
                            where l.EntityType == request.AuditLogEntityTypes.ToString()
                            select l;
                }

                switch (request.DateRangeFilter)
                {
                    case DateRangeFilterType.Past24Hours:
                        query = query.Where(x => x.Date >= DateTime.UtcNow.AddDays(-1));
                        break;
                    case DateRangeFilterType.Past3Days:
                        query = query.Where(x => x.Date >= DateTime.UtcNow.AddDays(-3));
                        break;
                    case DateRangeFilterType.Past7Days:
                        query = query.Where(x => x.Date >= DateTime.UtcNow.AddDays(-7));
                        break;
                    case DateRangeFilterType.Past30Days:
                        query = query.Where(x => x.Date >= DateTime.UtcNow.AddDays(-30));
                        break;
                    case DateRangeFilterType.CustomDate:
                        if (request.StartDate == null || request.EndDate == null)
                        {
                            throw new ArgumentNullException($"The {nameof(request.StartDate)} and the {nameof(request.EndDate)} can't be null");
                        }

                        query = query.Where(x => x.Date >= request.StartDate && x.Date <= request.EndDate);
                        break;
                    default:
                        query = query.Where(x => x.Date >= DateTime.UtcNow.AddDays(-1));
                        break;
                }

                if (!request.AllTenants)
                {
                    query = from l in query
                            join c in _dbContext.Controllers on l.ControllerId equals c.Id
                            join t in _dbContext.Tenants on c.TenantId equals t.Id
                            where t.Id == request.TenantId
                            select l;
                }

                var result = from l in query
                             join c in _dbContext.Controllers on l.ControllerId equals c.Id
                             join r in _dbContext.Routers on c.RouterId equals r.Id
                             join t in _dbContext.Tenants on c.TenantId equals t.Id
                             orderby l.Date descending
                             select new CCAuditLog(l, new Router() { Id = r.Id, Address = r.Address }, new Controller() { Id = c.Id, IpAddress = c.IpAddress }, new Par.CommandCenter.Domain.Entities.Tenant() { Id = t.Id, Name = t.Name });


                var auditLogs = await result
                  .ProjectTo<AuditLogModel>(_mapper.ConfigurationProvider)
                  .ToListAsync(cancellationToken)
                  .ConfigureAwait(false);


                return new GetAuditLogResponse
                {
                    AuditLogs = auditLogs
                };
            }
            catch (Exception ex)
            {
                throw;
            }
#pragma warning restore CS0168 // The variable 'ex' is declared but never used

        }
    }
}
