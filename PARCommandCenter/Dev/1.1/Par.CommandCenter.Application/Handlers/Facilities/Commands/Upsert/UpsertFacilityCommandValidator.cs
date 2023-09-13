using FluentValidation;

namespace Par.CommandCenter.Application.Handlers.Facilities.Commands.Upsert
{
    public class UpsertFacilityCommandValidator : AbstractValidator<UpsertFacilityCommand>
    {
        public UpsertFacilityCommandValidator()
        {
            RuleFor(f => f.TenantId)
                .NotEmpty();
            RuleFor(t => t.Name)
                .NotEmpty();
            RuleFor(t => t.TimeZoneId)
                .NotEmpty();
        }
    }
}
