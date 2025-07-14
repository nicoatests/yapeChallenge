using AntifraudYape.Domain.Transactions.Entities;

namespace AntifraudYape.Domain.Transactions.Repositories;
public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default);
}
