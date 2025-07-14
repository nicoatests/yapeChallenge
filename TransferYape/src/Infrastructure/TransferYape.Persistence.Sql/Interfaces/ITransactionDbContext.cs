using Microsoft.EntityFrameworkCore;
using TransferYape.Domain.Transactions.Entities;

namespace TransferYape.Persistence.Sql.Interfaces;
public interface ITransactionDbContext
{
    DbSet<Transaction> Transactions { get; set; }
}
