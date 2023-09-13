using FluentValidation;

namespace Par.CommandCenter.Application.Handlers.CloudRouters.Commands.Delete
{
    public class DeleteCloudRouterCommandValidator : AbstractValidator<DeleteCloudRouterCommand>
    {
        public DeleteCloudRouterCommandValidator()
        {
            RuleFor(t => t.RouterAddress)
                .NotEmpty();

        }
    }
}
