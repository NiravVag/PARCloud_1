using FluentValidation;
using System.Diagnostics;

namespace Par.CommandCenter.Application.Handlers.Routers.Commands.Register
{
    public class RegisterRouterCommandValidator : AbstractValidator<RegisterRouterCommand>
    {
        public RegisterRouterCommandValidator()
        {
            RuleFor(t => t.TenantId)
                .NotEmpty();

            RuleFor(x => x.Address)
               .MaximumLength(14)
               .WithMessage("Router address must not exceed 14 Character.");

            RuleFor(x => x.ServiceName)
                .MaximumLength(100);

            RuleFor(x => x.ServiceDisplayName)
                .MaximumLength(100);

            this.RuleFor(x => x.Address)
                .Must((criteria, id) => HasFilterSet(criteria))
                .WithMessage("When you supply a router address, service name and service display name are required.");

            this.RuleFor(x => x.ServiceName)
                .Must((criteria, id) => HasFilterSet(criteria))
                .WithMessage("When you supply a router address, service name and service display name are required.");

            this.RuleFor(x => x.ServiceDisplayName)
                .Must((criteria, id) => HasFilterSet(criteria))
                .WithMessage("When you supply router address, service name and service display name are required.");
        }

        /// <summary>
        /// Validates that a least one Filter has been set for the criteria
        /// </summary>
        /// <returns>boolean</returns>
        private static bool HasFilterSet(RegisterRouterCommand criteria)
        {
            Debug.Assert(criteria != null, "criteria != null");

            if (string.IsNullOrWhiteSpace(criteria.Address))
            {
                return true;
            }

            var filter = criteria.Address.Length > 0 && criteria.ServiceName.Length > 0 && criteria.ServiceDisplayName.Length > 0;

            return filter;
        }
    }
}
