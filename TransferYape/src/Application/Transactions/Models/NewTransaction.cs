namespace TransferYape.Application.Transactions.Models;

public record NewTransaction(
    Guid SourceAccountId,
    Guid TargetAccountId,
    decimal Value
);
