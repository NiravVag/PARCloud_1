using FluentValidation;
using System.Diagnostics;
using System.Linq;

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Commands.UpdateNotificationDate
{
    public class UpdateNotificationDateCommandValidator : AbstractValidator<UpdateNotificationDateCommand>
    {
        public UpdateNotificationDateCommandValidator()
        {
            this.RuleFor(x => x.VPNHealthChecks)
                .Must((criteria, id) => HasFilterSet(criteria))
                .WithMessage("Must provide one or more health check records.");

            this.RuleFor(x => x.RouterHealthChecks)
               .Must((criteria, id) => HasFilterSet(criteria))
               .WithMessage("Must provide one or more health check records.");

            this.RuleFor(x => x.ControllerHealthChecks)
               .Must((criteria, id) => HasFilterSet(criteria))
               .WithMessage("Must provide one or more health check records.");

            this.RuleFor(x => x.InvenotryInterfaceHealthChecks)
               .Must((criteria, id) => HasFilterSet(criteria))
               .WithMessage("Must provide one or more health check records.");

            this.RuleFor(x => x.OrderInterfaceHealthChecks)
               .Must((criteria, id) => HasFilterSet(criteria))
               .WithMessage("Must provide one or more health check records.");

            this.RuleFor(x => x.ServerOperationHealthChecks)
               .Must((criteria, id) => HasFilterSet(criteria))
               .WithMessage("Must provide one or more health check records.");
        }

        private static bool HasFilterSet(UpdateNotificationDateCommand criteria)
        {
            Debug.Assert(criteria != null, "criteria != null");

            return (criteria.VPNHealthChecks?.Any() ?? false)
                || (criteria.RouterHealthChecks?.Any() ?? false)
                || (criteria.ControllerHealthChecks?.Any() ?? false)
                || (criteria.InvenotryInterfaceHealthChecks?.Any() ?? false)
                || (criteria.OrderInterfaceHealthChecks?.Any() ?? false)
                || (criteria.ServerOperationHealthChecks?.Any() ?? false);
        }
    }
}
