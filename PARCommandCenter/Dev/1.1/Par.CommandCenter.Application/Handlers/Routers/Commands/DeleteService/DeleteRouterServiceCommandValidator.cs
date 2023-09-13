using FluentValidation;

namespace Par.CommandCenter.Application.Handlers.Routers.Commands.DeleteService
{
    public class DeleteRouterServiceCommandValidator : AbstractValidator<DeleteRouterServiceCommand>
    {
        public DeleteRouterServiceCommandValidator()
        {
            RuleFor(t => t.Address)
                .NotEmpty();

        }
    }
}
