using AntifraudYape.Domain.Transactions.Entities;
using AntifraudYape.Domain.Transactions.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AntifraudYape.Persistence.Sql.Transactions.Configurations;
public class TransactionConfig : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");
        builder.HasKey(s => s.Id);
        builder.Property(m => m.Id).ValueGeneratedNever();

        builder.Property(t => t.Status)
               .HasConversion(new ValueConverter<TransactionStatus, string>(
                   d => d.Name,
                   d => TransactionStatus.FromName(d, false)));
    }
}
