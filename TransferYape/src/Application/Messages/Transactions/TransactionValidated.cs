namespace TransferYape.Application.Messages.Transactions;

public record TransactionValidated(Guid TransactionExternalId, Guid StatusId);