using MediatR;

namespace TransferYape.Application.Transactions.Commands.UpdateTransactionStatus;

public record UpdateTransactionStatusCommand(Guid Id, Guid StatusId) : IRequest;
