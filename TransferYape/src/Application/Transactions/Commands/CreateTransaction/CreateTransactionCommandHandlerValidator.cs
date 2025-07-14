using FluentValidation;

namespace TransferYape.Application.Transactions.Commands.CreateTransaction;
public sealed class CreateTranstactionCommandHandlerValidator : AbstractValidator<CreateTranstactionCommand>
{
    public CreateTranstactionCommandHandlerValidator()
    {
        RuleFor(x => x.SourceAccountId).NotEmpty().WithMessage("Source Accound Id must not be empty.");
        RuleFor(x => x.TargetAccountId).NotEmpty().WithMessage("Target Account Id must not be empty.");
        RuleFor(x => x.Value).GreaterThan(0).WithMessage("Value must be greater than zero.");
    }
}
