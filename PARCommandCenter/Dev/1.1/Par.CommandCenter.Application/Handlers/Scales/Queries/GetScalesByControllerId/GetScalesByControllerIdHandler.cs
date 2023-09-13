﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Scales.Queries.GetScalesByIpAddress
{
    public class GetScalesByControllerIdHandler : IRequestHandler<GetScalesByControllerIdQuery, GetScalesByControllerIdResponse>
    {
        private readonly ILogger<GetScalesByControllerIdHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetScalesByControllerIdHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetScalesByControllerIdHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetScalesByControllerIdResponse> Handle(GetScalesByControllerIdQuery request, CancellationToken cancellationToken)
        {
            if (request.ControllerId <= 0)
            {
                throw new ArgumentNullException($"You must provide {nameof(request.ControllerId)}");
            }


            var query = from s in _dbContext.Scales
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
                        where c.Id == request.ControllerId && s.Deleted == false
                        select new Scale
                        {
                            Id = s.Id,
                            TenantId = s.TenantId,
                            Address = s.Address,
                            ControllerId = s.ControllerId,
                            ScaleWeight = s.ScaleWeight,
                            LastCommunication = s.LastCommunication,
                            BinId = s.BinId,
                            Location = l,
                            Item = (i == null) ? null : new Item
                            {
                                Id = i.Id,
                                Name = i.Name,
                                Number = i.Number
                            },
                            Controller = new Controller
                            {
                                Id = c.Id,
                                IpAddress = c.IpAddress,
                            },
                        };

            if (request.RegisteredScalesOnly)
            {
                if (request.OnlineScalesOnly || request.OfflineScalesOnly)
                {
                    throw new ArgumentException("Only one boolean value can be true");
                }

                query = query.Where(x => x.BinId != null);
            }
            else if (request.OnlineScalesOnly)
            {
                if (request.RegisteredScalesOnly || request.OfflineScalesOnly)
                {
                    throw new ArgumentException("Only one boolean value can be true");
                }

                query = query.Where(x => x.LastCommunication > System.DateTimeOffset.UtcNow.AddHours(-1));
            }
            else if (request.OfflineScalesOnly)
            {
                if (request.RegisteredScalesOnly || request.OnlineScalesOnly)
                {
                    throw new ArgumentException("Only one boolean value can be true");
                }

                query = query.Where(x => x.LastCommunication <= System.DateTimeOffset.UtcNow.AddHours(-1));
            }

            var scales = await query
              //.Where(s => s.con == request.TenantId)
              .OrderBy(s => s.Location)
              .ThenBy(s => s.Address)
              .ThenBy(s => s.Item.Name)
              .ProjectTo<ScaleModel>(_mapper.ConfigurationProvider)
              .ToListAsync(cancellationToken)
              .ConfigureAwait(false);

            return new GetScalesByControllerIdResponse
            {
                Scales = scales
            };
        }
    }
}
