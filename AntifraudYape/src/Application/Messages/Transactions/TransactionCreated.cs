namespace AntifraudYape.Application.Messages.Transactions;

public record TransactionCreated(
    Guid Id,
    DateOnly CreatedAt,
    Guid SourceAccountId
);
