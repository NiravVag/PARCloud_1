using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Handlers.Controllers.Queries.GetControllersByTenant;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Controllers.Queries.GetController
{
    public class GetControllerHandler : IRequestHandler<GetControllerQuery, GetControllerResponse>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        private readonly ILogger<GetControllerHandler> _logger;

        public GetControllerHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetControllerHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<GetControllerResponse> Handle(GetControllerQuery request, CancellationToken cancellationToken)
        {
            if (request.ControllerId <= 0)
            {
                throw new ArgumentNullException($"You must provide the {request.ControllerId}");
            }

            
            return await GetControllers(request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<GetControllerResponse> GetControllers(GetControllerQuery request, CancellationToken cancellationToken)
        {
            if(request.ControllerId > 0 && request.routerId > 0)
            {
                throw new ArgumentException("Both controller Id and router Id can't be greater than zero at the same time");
            }

            IQueryable<Scale> query1 = null;
            if(request.IncludeScales)
            {
                query1 = from s in _dbContext.Scales
                         join c in _dbContext.Controllers on s.ControllerId equals c.Id
                         join t in _dbContext.Tenants on s.TenantId equals t.Id
                         join r in _dbContext.Routers on c.RouterId equals r.Id
                         join b in _dbContext.Bins on s.BinId equals b.Id into bx
                         from b in bx.DefaultIfEmpty()
                         join li in _dbContext.LocationItems on b.LocationItemId equals li.Id into lix
                         from li in lix.DefaultIfEmpty()
                         join i in _dbContext.Items on li.ItemId equals i.Id into ix
                         from i in ix.DefaultIfEmpty()
                         join l in _dbContext.Locations on li.LocationId equals l.Id into lx
                         from l in lx.DefaultIfEmpty()
                         join f in _dbContext.Facilities on l.FacilityId equals f.Id into fx
                         from f in fx.DefaultIfEmpty()
                         where !s.Deleted && (c.ControllerTypeId == 3 || c.ControllerTypeId == 4 || c.ControllerTypeId == 5)
                         select new Scale
                         {
                             Id = s.Id,
                             ControllerId = s.ControllerId,

                             Controller = c,
                             LastCommunication = s.LastCommunication,
                             Location = l,
                         };

                if (request.ControllerId.HasValue && request.ControllerId > 0)
                {
                    query1 = from i in query1
                             where i.Id == request.ControllerId.Value
                             select i;
                  
                }

                if (request.routerId.HasValue && request.routerId > 0)
                {
                    query1 = from i in query1
                             where i.Controller.RouterId == request.routerId.Value
                             select i;
                }

            }            

            var query2 = from c in _dbContext.Controllers
                         select c;
            if(request.ControllerId.HasValue && request.ControllerId > 0)
            {
                query2 = from c in query2
                         where c.Id == request.ControllerId.Value
                         select c;
            }

            if (request.routerId.HasValue && request.routerId > 0)
            {
                query2 = from c in query2
                         where c.RouterId == request.routerId.Value
                         select c;
            }


            var query = from c in query2
                        join r in _dbContext.Routers on c.RouterId equals r.Id
                        where !r.Deleted //&& c.Id == request.ControllerId
                        select new Controller
                        {
                            Id = c.Id,
                            TenantId = c.TenantId,
                            PortName = c.PortName,
                            RouterId = c.RouterId,
                            ControllerTypeId = c.ControllerTypeId,
                            IpAddress = c.IpAddress,
                            NetworkPort = c.NetworkPort,
                            MACAddress = c.MACAddress,
                            FirmwareVersion = c.FirmwareVersion,
                            ParChargeMode = c.ParChargeMode,
                            ParChargeBatch = c.ParChargeBatch,
                            Created = c.Created,
                            CreatedUserId = c.CreatedUserId,
                            Router = new Router
                            {
                                Id = r.Id,
                                Address = r.Address,
                                LastCommunication = r.LastCommunication,
                            },
                            Scales = (query1 == null)? null : query1.Where(s => s.ControllerId == c.Id).ToList(),
                            Active = c.Active,
                        };

            var controllers = await query
              .ProjectTo<ControllerModel>(_mapper.ConfigurationProvider)
              .OrderBy(x => x.Id)
              .ThenBy(x => x.RouterId)
              .ThenBy(x => x.IpAddress)
              .ToListAsync(cancellationToken)
              .ConfigureAwait(false);



            return new GetControllerResponse
            {
                Controller = (request.ControllerId.HasValue) ? controllers.FirstOrDefault() : null,
                Controllers = (request.routerId.HasValue) ? controllers : null
            };
        }
    }
}
