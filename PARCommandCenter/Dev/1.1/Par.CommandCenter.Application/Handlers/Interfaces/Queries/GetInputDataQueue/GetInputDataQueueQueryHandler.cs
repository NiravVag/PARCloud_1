using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Interfaces.Queries.GetInputDataQueue
{
    public class GetInputDataQueueQueryHandler : IRequestHandler<GetInputDataQueueQuery, GetInputDataQueueQueryResponse>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        private readonly ILogger<GetInputDataQueueQueryHandler> _logger;

        public GetInputDataQueueQueryHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetInputDataQueueQueryHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetInputDataQueueQueryResponse> Handle(GetInputDataQueueQuery request, CancellationToken cancellationToken)
        {
            var query = from ibj in _dbContext.InputBatchJobData
                        join j in _dbContext.Jobs on ibj.JobId equals j.Id
                        join jt in _dbContext.JobTypes on j.JobTypeId equals jt.Id
                        where ibj.Completed == null
                        select new InputBatchJobData
                        {
                            Id = ibj.Id,                            
                            TenantId = ibj.TenantId,
                            FileName = ibj.FileName,
                            FileLocation = ibj.FileLocation,
                            Job = new Job
                            {
                                Id = j.Id,
                                Name = j.Name,
                                TenantId = j.TenantId,
                                JobTypeId = j.JobTypeId,
                            },
                            JobType = new JobType
                            {
                                Id = jt.Id,
                                Name = jt.Name
                            },
                            Received = ibj.Received,
                            Published = ibj.Published,
                            Started = ibj.Started,
                            Completed = ibj.Completed,                            
                            ErrorMessage = ibj.ErrorMessage,
                        };

            var inputDataQueueItems = await query
                .Where(jqi => jqi.TenantId == request.TenantId)
                .OrderBy(r => r.Received)
                .ProjectTo<GetInputDataQueueQueryModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);


            return new GetInputDataQueueQueryResponse
            {
                InputDataQueueItems = inputDataQueueItems
            };
        }
    }
}
