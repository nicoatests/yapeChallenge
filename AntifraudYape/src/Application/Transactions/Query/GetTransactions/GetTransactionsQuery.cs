using MediatR;

namespace AntifraudYape.Application.Transactions.Query.GetTransactions;
public record GetTransactionsQuery(Guid Id, DateOnly CreatedAt, Guid SourceAccountId) : IRequest;
