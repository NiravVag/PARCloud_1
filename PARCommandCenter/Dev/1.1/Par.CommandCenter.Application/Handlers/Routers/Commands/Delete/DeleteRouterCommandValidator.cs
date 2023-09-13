using FluentValidation;

namespace Par.CommandCenter.Application.Handlers.Routers.Commands.Delete
{
    public class DeleteRouterCommandValidator : AbstractValidator<DeleteRouterCommand>
    {
        public DeleteRouterCommandValidator()
        {
            RuleFor(t => t.Address)
                .NotEmpty();

        }
    }
}
