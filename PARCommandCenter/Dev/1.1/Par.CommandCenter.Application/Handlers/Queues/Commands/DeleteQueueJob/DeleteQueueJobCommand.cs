using MediatR;
using System;

namespace Par.CommandCenter.Application.Handlers.Queues.Commands.DeleteQueueJob
{
    public class DeleteQueueJobCommand : IRequest<DeleteQueueJobResponse>
    {
        public Guid Id { get; set; }
    }
}
