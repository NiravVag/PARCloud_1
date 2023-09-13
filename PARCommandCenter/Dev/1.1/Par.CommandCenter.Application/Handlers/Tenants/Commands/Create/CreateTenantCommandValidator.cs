using FluentValidation;

namespace Par.CommandCenter.Application.Handlers.Tenants.Commands.Create
{
    public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
    {
        public CreateTenantCommandValidator()
        {
            RuleFor(t => t.Name)
                .NotEmpty();
            RuleFor(t => t.Acronym)
                .NotEmpty();
            RuleFor(t => t.DefaultTimeZoneId)
                .NotEmpty();

        }
    }
}
