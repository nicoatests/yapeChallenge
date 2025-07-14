using TransferYape.Domain.Transactions.Entities;

namespace TransferYape.Domain.Transactions.Repositories;
public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default);
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
