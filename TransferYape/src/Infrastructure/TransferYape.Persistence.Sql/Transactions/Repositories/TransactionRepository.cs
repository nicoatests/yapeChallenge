using Microsoft.EntityFrameworkCore;
using TransferYape.Domain.Transactions.Entities;
using TransferYape.Domain.Transactions.Repositories;
using TransferYape.Persistence.Sql.Interfaces;

namespace TransferYape.Persistence.Sql.Transactions.Repositories;
public sealed class TransactionRepository : ITransactionRepository
{
    private readonly ITransactionDbContext _dbContext;
    public TransactionRepository(ITransactionDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default)
    => await _dbContext.Transactions.AddAsync(transaction, cancellationToken);

    public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Transactions.SingleOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
}
