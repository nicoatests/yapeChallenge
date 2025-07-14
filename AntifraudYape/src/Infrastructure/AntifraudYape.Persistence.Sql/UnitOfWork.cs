using AntifraudYape.Application.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace AntifraudYape.Persistence.Sql;
public class UnitOfWork : IUnitOfWork, IDisposable
{
    private IDbContextTransaction? _currentTransaction;
    private readonly TransactionDbContext dbContext;

    public UnitOfWork(TransactionDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        _currentTransaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync()
    {
        await dbContext.SaveChangesAsync();
        if (_currentTransaction != null)
            await _currentTransaction.CommitAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction != null)
            await _currentTransaction.RollbackAsync();
    }

    public void Dispose()
    {
        _currentTransaction?.Dispose();
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}
