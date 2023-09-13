using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.Command.Request;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using Par.Data.Context;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Queues.Commands.ResetQueueJob
{
    public class ResetQueueJobHandler : IRequestHandler<ResetQueueJobCommand, ResetQueueJobResponse>
    {
        private readonly IAzureServiceBusService _azureServiceBusService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicationDbContext _dbContext;

        
        private readonly IMediator _mediator;

        private readonly ILogger<ResetQueueJobHandler> _logger;

        public ResetQueueJobHandler(IAzureServiceBusService azureServiceBusService, ICurrentUserService currentUserService, 
            IApplicationDbContext dbContext, IMediator Mediator,
            ILogger<ResetQueueJobHandler> logger)
        {
            _azureServiceBusService = azureServiceBusService;
            _currentUserService = currentUserService;
            _dbContext = dbContext;
            
            _mediator = Mediator;
            _logger = logger;
        }

        public async Task<ResetQueueJobResponse> Handle(ResetQueueJobCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                Exception exception = new Exception("Identifier must be not emapty.");
                throw exception;
            }

            var jobQueue = await _dbContext.JobQueueItems
                .Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync();

            if (jobQueue == null)
            {
                string message = string.Format("Job queue entry {0} not found", request.Id);
                _logger.LogDebug(message);
                throw new ArgumentException(message);
            }

            jobQueue.Started = null;
            jobQueue.ErrorMessage = null;
            jobQueue.Published = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            
            // publish a message about a job queue entry

            var result = await _azureServiceBusService.PublishJobQueueEntry(jobQueue.Id, jobQueue.JobId, jobQueue.TenantId, cancellationToken);

            
            return new ResetQueueJobResponse()
            {
                Succeed = result,
            };
            
        }
    }
}
