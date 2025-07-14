using AntifraudYape.Domain.Transactions.Enums;

namespace AntifraudYape.Domain.Transactions.Entities;
public class Transaction
{
    private Transaction()
    {
    }

    public Guid Id { get; private set; }
    public Guid SourceAccountId { get; private set; }
    public Guid TargetAccountId { get; private set; }
    public decimal Value { get; private set; }
    public TransactionStatus Status { get; private set; }
    public DateOnly CreatedAt { get; private set; }


    private Transaction(
        Guid sourceAccountId,
        Guid targetAccountId,
        decimal value)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateOnly.FromDateTime(DateTime.Now);
        SourceAccountId = sourceAccountId;
        TargetAccountId = targetAccountId;
        Value = value;
        Status = TransactionStatus.Pending;
    }

    public static Transaction Create(
        Guid sourceAccountId,
        Guid targetAccountId,
        decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("Value must be greater than zero.");
        return new Transaction(sourceAccountId, targetAccountId, value);
    }

    public void UpdateStatus(Guid statusId)
    {
        Status = TransactionStatus.List.Single(ts => ts.Value == statusId);
    }
}
