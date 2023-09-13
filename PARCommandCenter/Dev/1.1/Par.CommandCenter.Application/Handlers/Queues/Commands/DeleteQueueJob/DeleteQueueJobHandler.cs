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

namespace Par.CommandCenter.Application.Handlers.Queues.Commands.DeleteQueueJob
{
    public class DeleteQueueJobHandler : IRequestHandler<DeleteQueueJobCommand, DeleteQueueJobResponse>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicationDbContext _dbContext;

        private readonly ILogger<DeleteQueueJobHandler> _logger;

        public DeleteQueueJobHandler(ICurrentUserService currentUserService,
            IApplicationDbContext dbContext, IMediator Mediator,
            ILogger<DeleteQueueJobHandler> logger)
        {   
            _currentUserService = currentUserService;
            _dbContext = dbContext;

            _logger = logger;
        }

        public async Task<DeleteQueueJobResponse> Handle(DeleteQueueJobCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                Exception exception = new Exception("Identifier must be not emapty.");
                throw exception;
            }

            _logger.LogDebug("Deleting job queue entry {id}", request.Id);            

            var jobQueue = await _dbContext.JobQueueItems
                .Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);

            if (jobQueue == null)
            {
                string message = string.Format("Job queue entry {0} not found", request.Id);
                _logger.LogDebug(message);
                throw new ArgumentException(message);
            }

            _dbContext.JobQueueItems.Remove(jobQueue);

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogDebug("Job queue entry {id} deleted", request.Id);

            return new DeleteQueueJobResponse()
            {
                Succeed = true,
            };            
        }
    }
}
