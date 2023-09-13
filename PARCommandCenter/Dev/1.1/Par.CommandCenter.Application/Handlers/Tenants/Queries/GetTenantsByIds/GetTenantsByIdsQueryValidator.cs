using FluentValidation;
using Par.CommandCenter.Application.Handlers.Tenants.Queries.GetTenantsByIds;
using System.Collections.Generic;
using System.Linq;

namespace Par.CommandCenter.Application.Handlers.Controllers.Commands.Upsert
{
    public class GetTenantsByIdsQueryValidator : AbstractValidator<GetTenantsByIdsQuery>
    {
        public GetTenantsByIdsQueryValidator()
        {

            RuleFor(x => x.TenantIds)
                //Stop on first failure to avoid exception in method with null value
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithMessage("Tenant Ids can't be empty or null")
                .Must(CustomRuleForTenantIds)
                .WithMessage("The tenants Ids list must have at least one tenant Id and all Ids must be greater than zero");



        }

        private bool CustomRuleForTenantIds(IEnumerable<int> tenantIds)
        {
            return tenantIds.Any() && tenantIds.All(id => id > 0);
        }
    }
}
