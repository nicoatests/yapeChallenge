using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TransferYape.Domain.Transactions.Entities;
using TransferYape.Domain.Transactions.Enums;

namespace TransferYape.Persistence.Sql.Transactions.Configurations;
public class TransactionConfig : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");
        builder.HasKey(s => s.Id);
        builder.Property(m => m.Id).ValueGeneratedNever();

        builder.Property(x => x.SourceAccountId).IsRequired();
        builder.Property(x => x.TargetAccountId).IsRequired();
        builder.Property(x => x.Value).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.Property(t => t.Status)
               .HasConversion(new ValueConverter<TransactionStatus, string>(
                   d => d.Name,
                   d => TransactionStatus.FromName(d, false)));

    }
}
