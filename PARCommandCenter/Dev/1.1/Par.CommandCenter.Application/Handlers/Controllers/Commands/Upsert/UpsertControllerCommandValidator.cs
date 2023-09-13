using FluentValidation;

namespace Par.CommandCenter.Application.Handlers.Controllers.Commands.Upsert
{
    public class UpsertControllerCommandValidator : AbstractValidator<UpsertControllerCommand>
    {
        public UpsertControllerCommandValidator()
        {
            RuleFor(f => f.TenantId)
                .NotEmpty();
            RuleFor(f => f.PortName)
                .NotEmpty();
            RuleFor(t => t.RouterId)
                .NotEmpty();
            RuleFor(t => t.ControllerTypeId)
                .NotEmpty();
        }
    }
}
