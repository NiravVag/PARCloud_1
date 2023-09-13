using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities.Interfaces;
using Par.CommandCenter.Domain.Model;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Interfaces.Queries.GetInventoryEventQueue
{
    public class GetInventoryEventQueueQueryHandler : IRequestHandler<GetInventoryEventQueueQuery, GetInventoryEventQueueQueryResponse>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        private readonly ILogger<GetInventoryEventQueueQueryHandler> _logger;

        public GetInventoryEventQueueQueryHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetInventoryEventQueueQueryHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetInventoryEventQueueQueryResponse> Handle(GetInventoryEventQueueQuery request, CancellationToken cancellationToken)
        {
            var query = from ie in _dbContext.InventoryEvents
                        join iehl in _dbContext.InventoryEventHandlerLocations on ie.InventoryEventHandlerLocationId equals iehl.Id
                        join ieh in _dbContext.InventoryEventHandlers on iehl.InventoryEventHandlerId equals ieh.Id
                        join iet in _dbContext.InventoryEventTypes on ie.InventoryEventTypeId equals iet.Id                       
                        join u in _dbContext.Users on ie.CreatedUserId equals u.Id
                        where ie.InventoryEventOutputId == null && ie.TenantId == request.TenantId
                        select new InventoryEventQueueQueryModel
                        {
                            Id = ie.Id,
                            InventoryTransactionId = ie.InventoryTransactionId,
                            Created = ie.Created,
                            Published = ie.Published,
                            InventoryEventHandlerId = iehl.InventoryEventHandlerId,
                            InventoryEventHandlerName = ieh.Name,
                            InventoryEventTypeId = ie.InventoryEventTypeId,
                            InventoryEventTypeName = iet.Name,                            
                            UserId = ie.CreatedUserId,
                            UserName = u.UserName,
                            Started = ie.Started,
                            ErrorMessage = ie.ErrorMessage
                        };


            var inventoryEventQueueItems = await query
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);


            return new GetInventoryEventQueueQueryResponse
            {
                InventoryEventQueueItems = inventoryEventQueueItems
            };
        }
    }
}
