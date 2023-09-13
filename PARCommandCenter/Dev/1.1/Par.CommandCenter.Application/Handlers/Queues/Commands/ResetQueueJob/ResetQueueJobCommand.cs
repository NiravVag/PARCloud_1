using MediatR;
using System;

namespace Par.CommandCenter.Application.Handlers.Queues.Commands.ResetQueueJob
{
    public class ResetQueueJobCommand : IRequest<ResetQueueJobResponse>
    {
        public Guid Id { get; set; }
    }
}
