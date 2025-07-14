using Ardalis.SmartEnum;

namespace AntifraudYape.Domain.Transactions.Enums;

public sealed class TransactionStatus : SmartEnum<TransactionStatus, Guid>
{
    public static readonly TransactionStatus Pending = new(nameof(Pending), new Guid("7FE75DBA-A44F-4FA9-9218-C5C189470DCF"));
    public static readonly TransactionStatus Approved = new(nameof(Approved), new Guid("7FE75DBA-A44F-4FA9-9218-C5C189470DC1"));
    public static readonly TransactionStatus Rejected = new(nameof(Rejected), new Guid("7FE75DBA-A44F-4FA9-9218-C5C189470DC2"));

    private TransactionStatus(string name, Guid value) : base(name, value) { }
}
