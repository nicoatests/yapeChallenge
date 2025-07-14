using AntifraudYape.Domain.Transactions.Entities;
using Microsoft.EntityFrameworkCore;

namespace AntifraudYape.Persistence.Sql.Interfaces;
public interface ITransactionDbContext
{
    DbSet<Transaction> Transactions { get; set; }
}
