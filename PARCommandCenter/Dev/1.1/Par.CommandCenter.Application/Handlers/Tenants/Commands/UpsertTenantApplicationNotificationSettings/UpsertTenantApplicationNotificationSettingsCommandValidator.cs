using FluentValidation;

namespace Par.CommandCenter.Application.Handlers.Tenants.Commands.UpsertTenantApplicationNotificationSettings
{
    public class UpsertTenantApplicationNotificationSettingsCommandValidator : AbstractValidator<UpsertTenantApplicationNotificationSettingsCommand>
    {
        public UpsertTenantApplicationNotificationSettingsCommandValidator()
        {
            RuleFor(f => f.TenantIds)
                .NotEmpty();

        }
    }
}
