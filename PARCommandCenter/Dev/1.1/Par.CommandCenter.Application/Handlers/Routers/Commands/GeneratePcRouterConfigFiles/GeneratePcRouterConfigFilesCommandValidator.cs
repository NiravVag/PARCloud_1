using FluentValidation;
using System.Diagnostics;

namespace Par.CommandCenter.Application.Handlers.Routers.Commands.GeneratePcRouterConfigFiles
{
    public class GeneratePcRouterConfigFilesCommandValidator : AbstractValidator<GeneratePcRouterConfigFilesCommand>
    {
        public GeneratePcRouterConfigFilesCommandValidator()
        {
            RuleFor(x => x.ControllerIds)
             .NotNull()
             .NotEmpty()             
             .WithMessage("At least one or more controllers Ids is required");
        }
    }
}
