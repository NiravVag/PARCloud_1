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

namespace Par.CommandCenter.Application.Handlers.Interfaces.Queries.GetOrderEventQueue
{
    public class GetOrderEventQueueQueryHandler : IRequestHandler<GetOrderEventQueueQuery, GetOrderEventQueueQueryResponse>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        private readonly ILogger<GetOrderEventQueueQueryHandler> _logger;

        public GetOrderEventQueueQueryHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetOrderEventQueueQueryHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetOrderEventQueueQueryResponse> Handle(GetOrderEventQueueQuery request, CancellationToken cancellationToken)
        {
            var query = from oe in _dbContext.OrderEvents
                        join o in _dbContext.Orders on oe.OrderId equals o.Id
                        join oehrs in _dbContext.OrderEventHandlerReplenishmentSources on oe.OrderEventHandlerReplenishmentSourceId equals oehrs.Id
                        join oeh in _dbContext.OrderEventHandlers on oehrs.OrderEventHandlerId equals oeh.Id
                        join oet in _dbContext.OrderEventTypes on oe.OrderEventTypeId equals oet.Id
                        join u in _dbContext.Users on oe.CreatedUserId equals u.Id
                        where oe.OrderEventOutputId == null && oe.TenantId == request.TenantId
                        select new OrderEventQueueQueryModel
                        {
                            Id = oe.Id,
                            OrderId = oe.OrderId,
                            OrderNumber = o.Number,
                            Created = oe.Created,
                            Published = oe.Published,
                            OrderEventHandlerId = oehrs.OrderEventHandlerId,
                            OrderEventHandlerName = oeh.Name,
                            OrderEventTypeId = oe.OrderEventTypeId,
                            OrderEventTypeName = oet.Name,
                            UserId = oe.CreatedUserId,
                            UserName = u.UserName,
                            Started = oe.Started,
                            ErrorMessage = oe.ErrorMessage
                        };


            var orderEventQueueItems = await query
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);


            return new GetOrderEventQueueQueryResponse
            {
                OrderEventQueueItems = orderEventQueueItems
            };
        }
    }
}
