using AntifraudYape.Application.Transactions.Repositories;
using AntifraudYape.Domain.Transactions.Entities;
using AntifraudYape.Persistence.Sql.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AntifraudYape.Persistence.Sql.Transactions.Repositories;
public sealed class TransactionRepository : ITransactionReadModelRepository
{
    private readonly ITransactionDbContext _dbContext;
    public TransactionRepository(ITransactionDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Transaction>> GetByDateAsync(DateOnly date, Guid sourceAccountId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Transactions.AsNoTracking().Where(t => t.CreatedAt == date && t.SourceAccountId == sourceAccountId)
            .ToListAsync(cancellationToken);
    }
}
