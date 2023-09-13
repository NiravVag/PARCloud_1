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

namespace Par.CommandCenter.Application.Handlers.Interfaces.Queries.GetJobQueue
{
    public class GetJobQueueHandler : IRequestHandler<GetInputDataQueueQuery, GetJobQueueResponse>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        private readonly ILogger<GetJobQueueHandler> _logger;

        public GetJobQueueHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetJobQueueHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetJobQueueResponse> Handle(GetInputDataQueueQuery request, CancellationToken cancellationToken)
        {
            var query = from jqi in _dbContext.JobQueueItems
                        join j in _dbContext.Jobs on jqi.JobId equals j.Id
                        join jt in _dbContext.JobTypes on j.JobTypeId equals jt.Id
                        select new JobQueueItem
                        {
                            Id = jqi.Id,
                            Submitted = jqi.Submitted,
                            TenantId = jqi.TenantId,
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
                            RunOnceDate = jqi.RunOnceDate,
                            Started = jqi.Started,
                            ErrorMessage = jqi.ErrorMessage,
                        };

            var jobQueueItems = await query
                .Where(jqi => jqi.TenantId == request.TenantId)
                .OrderBy(r => r.Submitted)
                .ProjectTo<JobQueueItemModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);


            return new GetJobQueueResponse
            {
                JobQueueItems = jobQueueItems
            };
        }
    }
}
