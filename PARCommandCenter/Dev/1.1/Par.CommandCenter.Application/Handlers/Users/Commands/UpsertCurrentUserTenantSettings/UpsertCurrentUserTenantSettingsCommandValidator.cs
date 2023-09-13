using FluentValidation;

namespace Par.CommandCenter.Application.Handlers.Users.Commands.UpsertCurrentUserTenantSettings
{
    public class UpsertCurrentUserTenantSettingsCommandValidator : AbstractValidator<UpsertCurrentUserTenantSettingsCommand>
    {
        public UpsertCurrentUserTenantSettingsCommandValidator()
        {
            RuleFor(f => f.TenantIds)
                .NotEmpty();

        }
    }
}
