using FluentValidation;

namespace Par.CommandCenter.Application.Handlers.Interfaces.Commands.Delete
{
    public class DeleteInterfaceEventValidator : AbstractValidator<DeleteInterfaceEventCommand>
    {
        public DeleteInterfaceEventValidator()
        {
            RuleFor(f => f.Id)
                .NotEmpty();
        }
    }
}
