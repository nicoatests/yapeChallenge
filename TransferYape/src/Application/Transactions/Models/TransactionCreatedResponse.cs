namespace TransferYape.Application.Transactions.Models;

public record TransactionCreatedResponse(
    Guid TransactionExternalId,
    DateOnly CreatedAt
);