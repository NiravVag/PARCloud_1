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

namespace Par.CommandCenter.Application.Handlers.Interfaces.Queries.GetOutputDataQueue
{
    public class GetOutputDataQueueQueryHandler : IRequestHandler<GetOutputDataQueueQuery, GetOutputDataQueueQueryResponse>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        private readonly ILogger<GetOutputDataQueueQueryHandler> _logger;

        public GetOutputDataQueueQueryHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetOutputDataQueueQueryHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetOutputDataQueueQueryResponse> Handle(GetOutputDataQueueQuery request, CancellationToken cancellationToken)
        {
            var query1 = (from oeoo in _dbContext.OrderEventOutputOrders
                          join oeo in _dbContext.OrderEventOutputs on oeoo.OrderEventOutputId equals oeo.Id
                          join t in _dbContext.Tenants on oeo.TenantId equals t.Id
                          join es in _dbContext.ExternalSystems on new { Id = oeo.ExternalSystemId, oeo.TenantId } equals new { es.Id, es.TenantId }
                          where oeo.TenantId == request.TenantId && oeo.Sent == null
                          select new OutputDataQueueEntry()
                          {
                              Id = oeo.Id,
                              DataType = "Order Event Output",
                              Created = oeo.Created.Value,
                              Published = oeo.Published,
                              ExternalSystemId = oeo.ExternalSystemId,
                              ExternalSystemName = es.Name,
                              Started = oeo.Started,
                              ErrorMessage = oeo.ErrorMessage
                          })
                         .Distinct();

            var query2 = (from ieot in _dbContext.InventoryEventOutputTransactions
                          join ieo in _dbContext.InventoryEventOutputs on ieot.InventoryEventOutputId equals ieo.Id
                          join t in _dbContext.Tenants on ieo.TenantId equals t.Id
                          join es in _dbContext.ExternalSystems on new { Id = ieo.ExternalSystemId, ieo.TenantId } equals new { es.Id, es.TenantId }
                          where ieo.TenantId == request.TenantId && ieo.Sent == null
                          select new OutputDataQueueEntry()
                          {
                              Id = ieo.Id,
                              DataType = "Inventory Event Output",
                              Created = ieo.Created.Value,
                              Published = ieo.Published,
                              ExternalSystemId = ieo.ExternalSystemId,
                              ExternalSystemName = es.Name,
                              Started = ieo.Started,
                              ErrorMessage = ieo.ErrorMessage
                          })
                         .Distinct();


            var query3 = (from objdes in _dbContext.OutputBatchJobDataExternalSystems
                          join objd in _dbContext.OutputBatchJobData on objdes.OutputBatchJobDataId equals objd.Id
                          join t in _dbContext.Tenants on objdes.TenantId equals t.Id
                          join es in _dbContext.ExternalSystems on new { Id = objdes.ExternalSystemId, objdes.TenantId } equals new { es.Id, es.TenantId }
                          where objdes.TenantId == request.TenantId && objdes.Sent == null
                          select new OutputDataQueueEntry()
                          {
                              Id = objdes.Id,
                              DataType = "Batch Job Output",
                              Created = objd.Created,
                              Published = objdes.Published,
                              ExternalSystemId = objdes.ExternalSystemId,
                              ExternalSystemName = es.Name,
                              Started = objdes.Started,
                              ErrorMessage = objdes.ErrorMessage
                          })
                        .Distinct();

            var query = query1
                .Union(
                (from q2 in query2
                 select new OutputDataQueueEntry()
                 {
                     Id = q2.Id,
                     DataType = q2.DataType,
                     Created = q2.Created,
                     Published = q2.Published,
                     ExternalSystemId = q2.ExternalSystemId,
                     ExternalSystemName = q2.ExternalSystemName,
                     Started = q2.Started,
                     ErrorMessage = q2.ErrorMessage
                 })
                )
                .Union(
                 (from q3 in query3
                  select new OutputDataQueueEntry()
                  {
                      Id = q3.Id,
                      DataType = q3.DataType,
                      Created = q3.Created,
                      Published = q3.Published,
                      ExternalSystemId = q3.ExternalSystemId,
                      ExternalSystemName = q3.ExternalSystemName,
                      Started = q3.Started,
                      ErrorMessage = q3.ErrorMessage
                  })
                );


            var outputDataQueueItems = await query
            .ProjectTo<GetOutputDataQueueQueryModel>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);


            return new GetOutputDataQueueQueryResponse
            {
                OutputDataQueueItems = outputDataQueueItems
            };
        }
    }
}
