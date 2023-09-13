using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Controllers.Queries.GetControllersByTenant
{
    public class GetControllersByTenantHandler : IRequestHandler<GetControllersByTenantQuery, GetControllersByTenantResponse>
    {

        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetControllersByTenantHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<GetControllersByTenantResponse> Handle(GetControllersByTenantQuery request, CancellationToken cancellationToken)
        {
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            try
            {
                var query1 = from s in _dbContext.Scales
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
                             where c.TenantId == request.TenantId && s.Deleted == false && (c.ControllerTypeId == 3 || c.ControllerTypeId == 4)
                             select new Scale
                             {
                                 Id = s.Id,
                                 ControllerId = s.ControllerId,
                                 LastCommunication = s.LastCommunication,
                                 Location = l,

                             };

                //var result1 = await query1.ToListAsync().ConfigureAwait(false);


                var query2 = from c in _dbContext.Controllers
                             join r in _dbContext.Routers on c.RouterId equals r.Id
                             where r.Deleted == false
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
                                 Scales = query1.Where(s => s.ControllerId == c.Id).ToList(),
                                 Active = c.Active,
                             };


                var controllers = await query2
                  .Where(c => c.TenantId == request.TenantId && (c.ControllerTypeId == 3 || c.ControllerTypeId == 4))
                  .OrderBy(c => c.Router.Address)
                  .ThenBy(c => c.IpAddress)
                  .ProjectTo<ControllerModel>(_mapper.ConfigurationProvider)
                  .ToListAsync(cancellationToken)
                  .ConfigureAwait(false);


                return new GetControllersByTenantResponse
                {
                    Controllers = controllers
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
