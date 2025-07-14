using Microsoft.EntityFrameworkCore;
using TransferYape.Domain.Transactions.Entities;
using TransferYape.Persistence.Sql.Interfaces;

namespace TransferYape.Persistence.Sql;
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
