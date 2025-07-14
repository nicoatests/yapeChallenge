using AntifraudYape.Domain.Transactions.Entities;
using AntifraudYape.Persistence.Sql.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AntifraudYape.Persistence.Sql;
public class TransactionDbContext : DbContext,
    ITransactionDbContext
{
    public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
