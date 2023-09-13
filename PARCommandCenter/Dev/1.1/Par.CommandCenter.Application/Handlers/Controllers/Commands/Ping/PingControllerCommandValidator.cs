using FluentValidation;

namespace Par.CommandCenter.Application.Handlers.Controllers.Commands.Ping
{
    public class PingControllerCommandValidator : AbstractValidator<PingControllerCommand>
    {
        public PingControllerCommandValidator()
        {
            RuleFor(f => f.Address)
                .NotEmpty();

            RuleFor(f => f.TenantId)
               .NotEmpty();
        }
    }
}
