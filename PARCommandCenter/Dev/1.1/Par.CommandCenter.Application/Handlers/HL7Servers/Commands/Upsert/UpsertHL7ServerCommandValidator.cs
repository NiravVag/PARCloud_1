using FluentValidation;

namespace Par.CommandCenter.Application.Handlers.HL7Servers.Commands.Upsert
{
    public class UpsertHL7ServerCommandValidator : AbstractValidator<UpsertHL7ServerCommand>
    {
        public UpsertHL7ServerCommandValidator()
        {
        }
    }
}
