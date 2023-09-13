using FluentValidation;

namespace Par.CommandCenter.Application.Handlers.Queues.Commands.ResetQueueJob
{
    public class ResetQueueJobValidator : AbstractValidator<ResetQueueJobCommand>
    {
        public ResetQueueJobValidator()
        {
            this.RuleFor(f => f.Id)
                .NotEmpty();
        }
    }
}
