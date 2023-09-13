using FluentValidation;

namespace Par.CommandCenter.Application.Handlers.Routers.Commands.Restart
{
    public class RestartRouterCommandValidator : AbstractValidator<RestartRouterCommand>
    {
        public RestartRouterCommandValidator()
        {
            RuleFor(t => t.Address)
                .NotEmpty();
        }
    }
}
