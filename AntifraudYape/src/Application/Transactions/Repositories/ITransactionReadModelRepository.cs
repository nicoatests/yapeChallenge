using AntifraudYape.Domain.Transactions.Entities;

namespace AntifraudYape.Application.Transactions.Repositories;
public interface ITransactionReadModelRepository
{
    Task<IReadOnlyList<Transaction>> GetByDateAsync(DateOnly date, Guid sourceAccountId, CancellationToken cancellationToken = default);
}
