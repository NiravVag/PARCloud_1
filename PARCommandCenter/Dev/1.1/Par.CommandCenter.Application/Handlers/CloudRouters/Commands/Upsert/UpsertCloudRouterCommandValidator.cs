using FluentValidation;

namespace Par.CommandCenter.Application.Handlers.CloudRouters.Commands.Upsert
{
    public class UpsertCloudRouterCommandValidator : AbstractValidator<UpsertCloudRouterCommand>
    {
        public UpsertCloudRouterCommandValidator()
        {
            RuleFor(t => t.RouterAddress)
                .NotEmpty();

        }
    }
}
