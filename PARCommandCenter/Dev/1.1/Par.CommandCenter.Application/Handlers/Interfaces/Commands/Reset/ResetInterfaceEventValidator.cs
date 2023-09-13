using FluentValidation;

namespace Par.CommandCenter.Application.Handlers.Interfaces.Commands.Reset
{
    public class ResetInterfaceEventValidator : AbstractValidator<ResetInterfaceEventCommand>
    {
        public ResetInterfaceEventValidator()
        {
            RuleFor(f => f.Id)
                .NotEmpty();
        }
    }
}
