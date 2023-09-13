using FluentValidation;

namespace Par.CommandCenter.Application.Handlers.Routers.Commands.Create
{
    public class CreateRouterCommandValidator : AbstractValidator<CreateRouterCommand>
    {
        public CreateRouterCommandValidator()
        {
            RuleFor(t => t.Address)
                .NotEmpty();

        }
    }
}
