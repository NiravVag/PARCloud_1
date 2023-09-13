using FluentValidation;
using Par.CommandCenter.Application.Handlers.Bins.Commands.MeasureBin;

namespace Par.CommandCenter.Application.Handlers.Controllers.Commands.Upsert
{
    public class MeasureBinCommandValidator : AbstractValidator<MeasureBinCommand>
    {
        public MeasureBinCommandValidator()
        {
            RuleFor(f => f.BinId)
                .NotEmpty();
        }
    }
}
