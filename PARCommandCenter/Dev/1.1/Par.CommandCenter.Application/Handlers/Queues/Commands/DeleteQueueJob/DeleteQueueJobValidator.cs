using FluentValidation;

namespace Par.CommandCenter.Application.Handlers.Queues.Commands.DeleteQueueJob
{
    public class DeleteQueueJobValidator : AbstractValidator<DeleteQueueJobCommand>
    {
        public DeleteQueueJobValidator()
        {
            this.RuleFor(f => f.Id)
                .NotEmpty();
        }
    }
}
