using MediatR;
using TransferYape.Application.Transactions.Models;

namespace TransferYape.Application.Transactions.Commands.CreateTransaction;

public record CreateTranstactionCommand(
    Guid SourceAccountId,
    Guid TargetAccountId,
    decimal Value
) : IRequest<TransactionCreatedResponse>;